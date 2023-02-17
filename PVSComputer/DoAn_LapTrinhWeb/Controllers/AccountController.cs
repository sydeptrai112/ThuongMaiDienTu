using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class AccountController : Controller
    {
        //gọi DbContext để sử dụng các Model
        private readonly DbContext db = new DbContext();
        //View Đăng nhập
        public ActionResult SignIn(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("SignIn", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code xử lý đăng nhập
        [HttpPost]
        //script xử lý SignIn | path: Scripts/my_js/checkuseraccount.js"
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AccountsDTO model,string returnUrl)
        {
            //Mã hóa password dùng sha 256
            model.signin_password = Crypto.Hash(model.signin_password);
            //nếu trùng username,password và status ="1 tức là đang hoạt dộng và 0 là vô hiệu hóa" thì đăng nhập thành công
            var DataItem = db.Accounts.Where(m => m.status == "1" && m.Email.ToLower() == model.Email && m.password == model.signin_password).SingleOrDefault();
            var checkdisable = db.Accounts.Where(m => m.status == "2" && m.Email.ToLower() == model.Email && m.password == model.signin_password).SingleOrDefault();
            var checkactivate = db.Accounts.Where(m => m.status == "0" && m.Email.ToLower() == model.Email && m.password == model.signin_password).SingleOrDefault();
            if (checkdisable !=null)
            {
                Notification.set_flash("Tài khoản đã bị vô hiệu hoá", "danger");
            }
            else
            if (checkactivate != null)
            {
                Notification.set_flash("Tài khoản chưa được kích hoạt", "warning");
            }
            else
            if (DataItem != null)
            {
                //lưu thông tin khi sau khi đăng nhập
                var userData = new LoggedUserData
                {
                    UserId = DataItem.account_id,
                    Name = DataItem.Name,
                    Email = DataItem.Email,
                    RoleCode = DataItem.Role,
                    Avatar = DataItem.Avatar,
                };            
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(userData), false);
                Notification.set_flash("Đăng nhập thành công", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //fail thì thông báo cho người dùng 
                Notification.set_flash("Sai tài khoản hoặc mật khẩu", "danger");
            }
            return View(model);
        }
        //Đăng xuất
        public ActionResult Logout(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Logout", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            FormsAuthentication.SignOut();//đăng xuất
            Notification.set_flash("Đăng xuất thành công", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        //View Đăng ký
        public ActionResult Register()
        {
            //Nếu đã đăng nhập thì không vô được trang đăng ký
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //Code Xử lý đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "status,Requestcode")] Account model)
        {
            string fail = "";
            string success = "";
            //check email đã có trong hệ database chưa
            var checkemail = db.Accounts.Any(m => m.Email == model.Email && m.status == "1");
            //check email tài khoản có bị vô hiệu hóa hay không 
            var checmailkdisable = db.Accounts.Any(m => m.Email == model.Email && m.status == "2");
            //check email tài khoản đã kích hoạt chưa 
            var checkmailactivated = db.Accounts.Any(m => m.Email == model.Email && m.status == "0");
            //check số điện thoại đã có trong hệ database chưa
            var checkphone = db.Accounts.Any(m => m.Phone == model.Phone && (m.status == "1"|| m.status == "2"));
            //check phone đã có trong hệ database chưa
            var checkphoneacivated = db.Accounts.Any(m => m.Phone == model.Phone && m.status == "1");
            if (checkemail)
            {
                fail = "Email đã được sử dụng";
            }
            else if (checmailkdisable)
            {
                fail = "Email đăng ký tài khoản này đã bị vô hiệu hóa, vui lòng dùng email khác";
            }
            else if (checkmailactivated)
            {
                fail = "Email đã được sử dụng nhưng chưa kích hoạt tài khoản," +
                    " vui lòng vào email đăng ký tài khoản này và kích hoạt";
            }
            else if (checkphone)
            {
                fail = "SĐT đã được sử dụng";
            }
            else if (checkphoneacivated)
            {
                fail = "SĐT đã được sử dụng nhưng chưa kích hoạt tài khoản," +
                    " vui lòng vào email đăng ký tài khoản này và kích hoạt";
            }
            else
            {
                model.Role = Const.ROLE_MEMBER_CODE; //admin quyền là 0: thành viên quyền là 1,biên tập viên là 2, người kiểm duyệt là 3             
                model.Email = model.Email;
                model.create_by = model.Email;
                model.update_by = model.Email;
                model.Name = model.Name;
                model.Gender = "3";
                model.Avatar = "/Images/svg/avatars/001-boy.svg";
                model.Phone = model.Phone;
                model.update_at = DateTime.Now;
                model.Dateofbirth = DateTime.Now;
                //tạo chuỗi code kích hoạt tài khoản
                model.status = "0";
                model.Requestcode = Guid.NewGuid().ToString();
                //Gửi request code đến email bạn đăng ký tài khoản, nếu không muốn gửi request code thì chuyển status ="1", comment  model.Requestcode = Guid.NewGuid().ToString(); và SendVerificationLinkEmail(model.Email, model.Requestcode, "VerifyAccount");
                SendVerificationLinkEmail(model.Email, model.Requestcode, "VerifyAccount");
                //do password có nhiều ràng buộc "validdation nên phải thêm" không thêm sẽ báo lõi "Validation failed for one or more entities" 
                db.Configuration.ValidateOnSaveEnabled = false; 
                model.Addres_1 = model.Addres_1;
                //hash password và không cho khoảng trắng
                model.password = Crypto.Hash(model.password.Trim());
                model.create_at = DateTime.Now;
                db.Accounts.Add(model);
                //add dữ liệu vào database
                db.SaveChanges(); 
               
                success = "Đăng ký thành công. Đường dẫn kích hoạt tài khoản đã được gửi đến Email của bạn: " + model.Email;
            }
            ViewBag.Success = success; 
            ViewBag.Fail = fail;
            return View(model);
        }
        //Code xử lý kích hoạt tài khoản sau khi đăng ký
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                bool activate = false;
                {
                    db.Configuration.ValidateOnSaveEnabled = false; 
                    var model = db.Accounts.Where(a => a.Requestcode == new Guid(id).ToString()).FirstOrDefault();
                    if (model != null)
                    {
                        model.update_at = DateTime.Now;
                        model.status = "1";
                        model.Requestcode = "";
                        db.SaveChanges();
                        activate = true;
                    }
                    else
                    {
                        ViewBag.Message = "Yêu cầu không hợp lệ";
                    }
                }
                ViewBag.Status = activate;
            }
            return View();
        }
        //View quên mật khẩu
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)//nếu dã đăng nhập thì không thể gọi action "ForgotPassword"
            {
                return RedirectToAction("Index", "Home");//quay về trang chủ
            }
            return View();
        }
        //Code xử lý quên mật khẩu
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string fail = "";
            string success = "";
            var account = db.Accounts.Where(m => m.Email == EmailID && m.status=="1").FirstOrDefault(); // kiểm tra email đã trùng với email đăng ký tài khoản chưa, nếu chưa đăng ký sẽ trả về fail
            if (account != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                //Gửi code reset đến mail đã nhập ở form quên mật khẩu 
                SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword"); 
                string sendmail = account.Email;
                account.Requestcode = resetCode; //request code phải giống reset code
                //khi chạy action "ForgotPassword" và bị báo lỗi "Validation failed for one or more entities. See 'EntityValidationErrors'" thì thêm dòng này vô. Vì có quá nhiều validation trong một funtion nên báo lỗi.
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                success = "Đường dẫn reset password đã được gửi đến "+EmailID+" vui lòng kiểm tra email";
            }
            else
            {
                fail = "Email chưa tồn tại trong hệ thống hoặc tài khoản đã bị vô hiệu hóa"; // tài khoản không có trong hệ thống sẽ báo fail
            }
            
            ViewBag.Message1 = success;//truyền viewbag qua view của "ForgotPassword"
            ViewBag.Message2 = fail;
            return View();
        }
        //View ResetPassword
        public ActionResult Resetpassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            var user = db.Accounts.Where(a => a.Requestcode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        //Code xử lý resetpassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.Where(m => m.Requestcode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.password = Crypto.Hash(model.NewPassword);
                    //sau khi đổi mật khẩu thành công khi quay lại link cũ thì sẽ không đôi được mật khẩu nữa 
                    user.Requestcode = ""; 
                    user.update_by = user.Email;
                    user.update_at = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    Notification.set_flash("Cập nhật mật khẩu thành công", "success");
                    return RedirectToAction("SignIn");
                }
            }
            else
            {
                return HttpNotFound();
            }
            return View(model);
        }
        //Gửi Email xác nhận đăng ký, quên mật khẩu
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmailSupport, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password; 
            string subject = "";
            //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailConfirm" + ".cshtml");
            if (emailFor == "VerifyAccount")
            {
                subject = "Xác thực tài khoản " + emailID;            
                body = body.Replace("{{ViewBag.Sendmail}}", "Xác thực tài khoản");
                body = body.Replace("{{ViewBag.confirmtext}}", "Kích hoạt tài khoản");
                body = body.Replace("{{ViewBag.bodytext}}", "Nhấn vào nút bên dưới để xác thực hoàn tất đăng ký cho tài khoản của bạn");
                body = body.Replace("{{viewBag.Confirmlink}}", link); //hiển thị nội dung lên form html
                body = body.Replace("{{viewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Khôi phục mật khẩu cho "+emailID;
                body = body.Replace("{{ViewBag.Sendmail}}", "Khôi phục mật khẩu");
                body = body.Replace("{{ViewBag.confirmtext}}", "Thiết lập mật khẩu mới");
                body = body.Replace("{{ViewBag.bodytext}}", "Nhấn vào nút bên dưới để đặt lại mật khẩu tài khoản của bạn. Nếu bạn không yêu cầu đặt lại mật khẩu mới, vui lòng bỏ qua email này");
                body = body.Replace("{{viewBag.Confirmlink}}", link); //hiển thị nội dung lên form html
                body = body.Replace("{{viewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
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
        //View chỉnh sửa thông tin cá nhân
        [Authorize]
        public ActionResult Editprofile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Accounts.Where(u => u.account_id == userId).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        // Đăng nhập mới có thể truy cập
        [ValidateAntiForgeryToken]
        public ActionResult Editprofile(Account model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var account = db.Accounts.Where(m => m.account_id == userId).SingleOrDefault();
                if (model.ImageUpload == null)
                {
                    account.Avatar = account.Avatar;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                    string extension = Path.GetExtension(model.ImageUpload.FileName);
                    fileName=  SlugGenerator.SlugGenerator.GenerateSlug(fileName) + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + extension;
                    account.Avatar = "/Images/ImagesAvatar/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/ImagesAvatar/"), fileName);
                    model.ImageUpload.SaveAs(fileName);
                }
                account.account_id = userId;
                account.Name = model.Name;
                account.Phone = model.Phone;
                account.Addres_1 = model.Addres_1;
                account.Dateofbirth = model.Dateofbirth;
                account.Gender = model.Gender;
                account.status = "1";
                account.update_by = User.Identity.GetEmail();
                account.update_at = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công", "success");
                return RedirectToAction("Editprofile");
            }
            catch { 
             Notification.set_flash("Lỗi", "danger");
            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // thay đổi mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            var userId = User.Identity.GetUserId();
            string oldpass = Crypto.Hash(model.OldPassword); 
            var account = db.Accounts.SingleOrDefault(m => m.account_id == userId);
            var checkpassword = db.Accounts.Any(m => m.password == oldpass && m.account_id == userId);
            if (checkpassword)
            {
                if (account != null)
                {
                    if (model.NewPassword == model.OldPassword)
                    {
                        Notification.set_flash("Mật khẩu mới và mật khẩu cũ không được trùng", "danger");
                    }
                    else
                    {
                        account.password = Crypto.Hash(model.NewPassword);
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    account.update_by = User.Identity.GetEmail();
                    account.update_at = DateTime.Now;
                    db.SaveChanges();
                    Notification.set_flash("Cập nhật mật khẩu thành công", "success");
                    return RedirectToAction("Logout");
                }
                else
                {
                    Notification.set_flash("Lỗi", "danger");
                }
            }
            else
            {
                Notification.set_flash("Mật khẩu cũ không đúng", "danger");
            }
            return View();
        }
        //kiểm tra đơn hàng
        public ActionResult TrackingOrder(int? page,int? size,string search,string sortOrder)//kiểm tra đơn hàng đã đặt và có phân trang cho phần đơn hàng dã đặt
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 2;
                var pageNumber = page ?? 1;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                ViewBag.WaitingSortParm = sortOrder == "order_waiting" ? "order_waiting" : "order_waiting";
                ViewBag.ProcessingortParm = sortOrder == "order_processing" ? "order_processing" : "order_processing";
                ViewBag.CompleteSortParm = sortOrder == "order_complete" ? "order_complete" : "order_complete";
                ViewBag.CancleSortParm = sortOrder == "order_cancle" ? "order_cancle" : "order_cancle";
                //truyền viewbag của Deliveries qua view "TrackingOrder"
                ViewBag.Deli = db.Deliveries;
                //truyền view bag của payment qua view "TrackingOrder"
                ViewBag.Payment = db.Payments;
                ViewBag.itemOrder = db.Oder_Detail.OrderByDescending(m => m.order_id);
                ViewBag.productOrder = db.Products;
                int userid= User.Identity.GetUserId();
                var list = from a in db.Orders
                           where (a.account_id== userid)
                           orderby a.order_id descending
                           select a;
                if (!string.IsNullOrEmpty(search))
                {
                    list = (IOrderedQueryable<Order>)list.Where(s => s.order_id.ToString().Contains(search));
                    return View("TrackingOrder", list.ToPagedList(pageNumber, pageSize));
                }
                switch (sortOrder)
                {
                    case "date_asc":
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.order_id ascending
                               select a;
                        break;
                    case "price_asc":
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.total ascending
                               select a;
                        break;
                    case "price_desc":
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.total descending
                               select a;
                        break;
                    case "order_waiting":
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status=="1")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_processing":
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "2")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_complete":
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "3")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_cancle":
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "0")
                               orderby a.order_id descending
                               select a;
                        break;
                    default:
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.order_id descending
                               select a;
                        break;
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        //gợi ý search
        [HttpPost]
        public JsonResult GetOrderSearch(string Prefix)
        {
            //tìm sản phẩm theo tên
            var userid = User.Identity.GetUserId();
            var search = (from c in db.Orders
                          where c.account_id== userid && c.order_id.ToString().Contains(Prefix)
                          orderby c.order_id ascending
                          select new { c.order_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        //phân trang cho phần đơn hàng cá nhân
        public ActionResult UserLogged()
        {
            // Được gọi khi nhấn [Thanh toán]
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
    }
}