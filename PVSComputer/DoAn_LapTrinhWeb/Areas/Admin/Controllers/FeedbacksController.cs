using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly DbContext db = new DbContext();
        //View list đánh giá
        public ActionResult FeedbackIndex(int?size,int?page,string search,string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.countTrash = db.Feedbacks.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                var list = from fb in db.Feedbacks
                           join p in db.Products on fb.product_id equals p.product_id
                           join a in db.Accounts on fb.account_id equals a.account_id
                           join fbimg in db.Feedback_Image on fb.feedback_id equals fbimg.feedback_id
                           where fb.status == "1" || fb.status == "2"
                           orderby fb.feedback_id descending
                           select new FeedbackDTOs
                           {
                               product_name=p.product_name,
                               product_slug=p.slug,
                               feedback_id = fb.feedback_id,
                               genre_id = p.genre_id,
                               discount_id = p.disscount_id,
                               description = fb.description,
                               rating_star = fb.rate_star,
                               status = fb.status,
                               create_at = fb.create_at,
                               Image = fbimg.image,
                               User_Email = a.Email,
                               account_id = a.account_id,
                           };
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.feedback_id.ToString().Contains(search) || s.account_id.ToString().Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = list.Where(s => s.feedback_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên thể loại
                        list = list.Where(s => s.account_id.ToString().Contains(search));
                    return View("FeedbackIndex", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View list trash đánh giá
        public ActionResult FeedbackTrash(int? size, int? page, string search, string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                var list = from fb in db.Feedbacks
                           join p in db.Products on fb.product_id equals p.product_id
                           join a in db.Accounts on fb.account_id equals a.account_id
                           join fbimg in db.Feedback_Image on fb.feedback_id equals fbimg.feedback_id
                           where fb.status == "0" 
                           orderby fb.feedback_id descending
                           select new FeedbackDTOs
                           {
                               product_name = p.product_name,
                               feedback_id = fb.feedback_id,
                               genre_id = p.genre_id,
                               discount_id = p.disscount_id,
                               description = fb.description,
                               rating_star = fb.rate_star,
                               status = fb.status,
                               create_at = fb.create_at,
                               Image = fbimg.image,
                               User_Email = a.Email,
                               update_at=fb.update_at,
                               account_id = a.account_id,
                           };
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận duyệt đánh giá
        public ActionResult ChangeComplete(int? id, string Roles)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var feedback = db.Feedbacks.SingleOrDefault(fb=>fb.feedback_id == id);
                var feedbackimage = db.Feedback_Image.Where(m => m.feedback_id == feedback.feedback_id).FirstOrDefault();
                if (feedback != null)
                {
                    if (User.Identity.GetRole() == "0")
                    {
                        Roles = "Quản trị viên";
                    }
                    else if (User.Identity.GetRole() == "2")
                    {
                        Roles = "Biên tập viên";
                    }
                    feedback.status = "2";
                    feedback.update_by = User.Identity.GetName();
                    feedback.update_at = DateTime.Now;
                    string Update_by = User.Identity.GetName();
                    string emailID = feedback.Account.Email;
                    string Feedbackid = feedback.feedback_id.ToString();
                    string productname = feedback.Product.product_name;
                    string Create_at = feedback.create_at.ToString("dd-MM-yyyy HH:mm");
                    string feedbackcontent = feedback.description;
                    string productslug = feedback.Product.slug;

                    SendEmailFeedback(emailID, Feedbackid, productname, Create_at, feedbackcontent, productslug, Roles, Update_by, "FeedbackAccept");
                }
                db.SaveChanges();
                Notification.set_flash("Đã xét duyệt đánh giá: "  + feedback.feedback_id+"", "success");
                return RedirectToAction("FeedbackIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Huỷ đánh giá
        [HttpPost]
        public JsonResult CancleFeedback(int id, string Roles)
        {
            Boolean result;
            var feedback = db.Feedbacks.Where(m => m.feedback_id == id).FirstOrDefault();
            var feedbackimage = db.Feedback_Image.Where(m => m.feedback_id == feedback.feedback_id).FirstOrDefault();
            if (feedback.status == "2")
            {
                result = false;
            }
            else
            {
                if (User.Identity.GetRole() == "0")
                {
                    Roles = "Quản trị viên";
                }
                else if (User.Identity.GetRole() == "2")
                {
                    Roles = "Biên tập viên";
                }
                feedback.status = "0";
                feedback.update_by = User.Identity.GetName();
                feedback.update_at = DateTime.Now;
                string Update_by = User.Identity.GetName();
                string emailID = feedback.Account.Email;
                string Feedbackid = feedback.feedback_id.ToString();
                string productname = feedback.Product.product_name;
                string Create_at = feedback.create_at.ToString("dd-MM-yyyy HH:mm");
                string feedbackcontent = feedback.description;
                string productslug = feedback.Product.slug;
                SendEmailFeedback(emailID, Feedbackid, productname, Create_at, feedbackcontent, productslug, Roles, Update_by, "FeedbackCancled");
                db.SaveChanges();
                result = true;
                return Json(new { result, Message = "Huỷ đánh giá ID'" + feedback.feedback_id + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //send mail khi thay đổi trạng thái
        public void SendEmailFeedback(string emailID,string Feedbackid,string productname, string Create_at,  string feedbackcontent, string productslug, string Roles, string Update_by, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmail, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailFeedback" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            if (emailFor == "FeedbackAccept")
            {
                subject = "Đánh giá của bạn về sản phẩm '"+ productname + "' Đã được duyệt";
                body = body.Replace("{{FeedbackId}}", Feedbackid);
                body = body.Replace("{{Productname}}", productname);
                body = body.Replace("{{productslug}}", "https://pvscomputer.tech/san-pham/" + productslug);
                body = body.Replace("{{CreateAt}}", Create_at);
                body = body.Replace("{{Feedbackcontent}}", feedbackcontent);
                body = body.Replace("{{RolesName}}", Update_by);
                body = body.Replace("{{Roles}}", Roles);
            }
            else if (emailFor == "FeedbackCancled")
            {
                subject = "Đánh giá của bạn về sản phẩm '" + productname + "' Đã bị huỷ";
                body = body.Replace("{{FeedbackId}}", Feedbackid);
                body = body.Replace("{{Productname}}", productname);
                body = body.Replace("{{productslug}}", "https://pvscomputer.tech/san-pham/" + productslug);
                body = body.Replace("{{CreateAt}}", Create_at);
                body = body.Replace("{{Feedbackcontent}}", feedbackcontent);
                body = body.Replace("{{Feedbackdisable}}", "- Lý do huỷ: Vi phạm 1 trong các quy tăc đánh giá của PVSComputer");
                body = body.Replace("{{RolesName}}", Update_by);
                body = body.Replace("{{Roles}}", Roles);
            }
            var smtp = new SmtpClient
            {
                Host = AccountEmail.Host, //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"
                Port = 587,
                EnableSsl = true, //bật ssl
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
