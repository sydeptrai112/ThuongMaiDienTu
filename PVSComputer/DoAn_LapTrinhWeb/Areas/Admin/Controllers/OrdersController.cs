using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //List view đơn hàng
        public ActionResult OrderIndex(int?page,int?size, string search, string show,string sortOrder)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                ViewBag.PhoneNumberSortParm = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.WaitingSortParm = sortOrder == "waiting" ? "waiting" : "waiting";
                ViewBag.ProcessingSortParm = sortOrder == "processing" ? "processing" : "processing";
                ViewBag.CompletegSortParm = sortOrder == "complete" ? "complete" : "complete";
                ViewBag.countTrash = db.Orders.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                var list = from a in db.Oder_Detail
                           join b in db.Orders on a.order_id equals b.order_id
                           group a by new { a.order_id, b } into g
                           where g.Key.b.status != "0"
                           orderby g.Key.b.order_id descending
                           select new OrderDTOs
                           {
                               order_id = g.Key.order_id,
                               total_price = g.Key.b.total,
                               status = g.Key.b.status,
                               order_date = g.Key.b.oder_date,
                               update_at = g.Key.b.update_at,
                               Name = g.Key.b.Account.Name,
                               Phone = g.Key.b.Account.Phone
                           };
                switch (sortOrder)
                {
                    case "name_asc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.Account.Name ascending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "name_desc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.Account.Name descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "price_asc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.total ascending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "price_desc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.total descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "waiting":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status == "1"
                               orderby g.Key.b.order_id descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "processing":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status == "2"
                               orderby g.Key.b.order_id descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "complete":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status == "3"
                               orderby g.Key.b.order_id descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "phone_asc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.Account.Phone ascending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "phone_desc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.Account.Phone descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "date_asc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.oder_date ascending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    case "date_desc":
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.oder_date descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                    default:
                        list = from a in db.Oder_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.order_id descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                        break;
                }
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.order_id.ToString().Contains(search) ||s.Name.ToString().Trim().Contains(search)|| s.status.Trim().Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = list.Where(s => s.order_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên khách hàng
                        list = list.Where(s => s.Name.ToString().Contains(search));
                    else if (show.Equals("4"))//theo số điện thoại
                        list = list.Where(s => s.Phone.ToString().Contains(search));
                    else if (show.Equals("5"))// trạng thái
                        list = list.Where(s => s.status.Contains(search));
                    return View("OrderIndex", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //List view trash đơn hàng
        public ActionResult OrderTrash(int? page, int? size, string search, string show)
        {
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var list = from a in db.Oder_Detail
                              join b in db.Orders on a.order_id equals b.order_id
                              group a by new { a.order_id, b } into g
                              where g.Key.b.status == "0"
                              orderby g.Key.b.update_at descending
                              select new OrderDTOs
                              {
                                  order_id = g.Key.order_id,
                                  total_price = g.Key.b.total,
                                  status = g.Key.b.status,
                                  update_at=g.Key.b.update_at,
                                  Name = g.Key.b.Account.Name,
                                  Phone = g.Key.b.Account.Phone
                              };
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.order_id.ToString().Contains(search) || s.Name.ToString().Trim().Contains(search) || s.Phone.ToString().Contains(search) || s.status.Trim().Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = list.Where(s => s.order_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên khách hàng
                        list = list.Where(s => s.Name.ToString().Contains(search));
                    else if (show.Equals("4"))//theo số điện thoại
                        list = list.Where(s => s.Phone.ToString().Contains(search));
                    else if (show.Equals("5"))// trạng thái
                        list = list.Where(s => s.status.Contains(search));
                    return View("OrderTrash", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //gợi ý tìm kiếm
        [HttpPost]
        public JsonResult GetOrderSearch(string Prefix)
        {
            var search = (from c in db.Orders
                          where c.status!="0"&& c.order_id.ToString().StartsWith(Prefix)
                          orderby c.order_id descending
                          select new { c.order_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Thông tin đơn hàng
        public ActionResult OrderDetails(int? id)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                // var order = db.Orders.Find(id);
               
                var order = (from a in db.Oder_Detail
                             join b in db.Orders on a.order_id equals id
                             join p in db.Products on a.product_id equals p.product_id
                             group a by new { a.order_id, b, a.product_id, p } into g
                             where g.Key.b.order_id==id
                             select new OrderDTOs
                             {
                                 discount_price =g.Sum(m => m.price * m.quantity),
                                 account_id=g.Key.b.account_id,
                                 order_id = g.Key.order_id,
                                 status = g.Key.b.status,
                                 total_price = g.Key.b.total,
                                 create_at = g.Key.b.create_at,
                                 order_date=g.Key.b.oder_date,
                                 create_by = g.Key.b.create_by,
                                 payment_name = g.Key.b.Payment.payment_name,
                                 delivery_name = g.Key.b.Delivery.delivery_name,
                                 product_name = g.Key.p.product_name,
                                 Email = g.Key.b.Account.Email,
                                 order_note = g.Key.b.order_note,
                                 update_at = g.Key.b.update_at,
                                 update_by = g.Key.b.update_by,
                                 Name = g.Key.b.Account.Name,
                                 Phone = g.Key.b.Account.Phone,
                                 Address = g.Key.b.Account.Addres_1
                             }).FirstOrDefault();
                if (order == null || id == null)
                {
                    Notification.set_flash("Không tồn tại đơn hàng: " + "PVS" + id + ")", "warning");
                    return RedirectToAction("OrderIndex");
                }
                ViewBag.orderDetails = db.Oder_Detail.Where(m => m.order_id == id).ToList();
                ViewBag.orderProduct = db.Products.ToList();
                return View(order);
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        [HttpPost]
        public JsonResult CancleOrder(int id, string Roles)
        {
            Boolean result;
            var order = db.Orders.Where(m => m.order_id == id).FirstOrDefault();
            if (order.status == "3")
            {
                result = false;
            }
            else
            {
                order.status = "0";
                order.update_by = User.Identity.GetEmail();
                order.update_at = DateTime.Now;
                if (User.Identity.GetRole() == "0")
                {
                    Roles = "Quản trị viên";
                }
                else if (User.Identity.GetRole() == "2")
                {
                    Roles = "Biên tập viên";
                }
                string Update_by = User.Identity.GetName();
                string emailID = order.Account.Email;
                string OrderID = order.order_id.ToString();
                string OrderDate = order.oder_date.ToString("dd-MM-yyyy HH:mm");
                string OrderNote = order.order_note.ToString();
                string OrderTotal = order.total.ToString("#,###₫");
                string OrderPhone = order.Account.Phone.ToString();
                string OrderName = order.Account.Name;
                string OrderAddress = order.Account.Addres_1;
                string OrderDelivery = order.Delivery.delivery_name;
                SendEmailOrders(emailID, OrderID, OrderDate, OrderNote, OrderTotal, OrderPhone, OrderName, OrderAddress, OrderDelivery, Update_by, Roles, "CancleOrders");
                db.SaveChanges();
                result = true;
                return Json(new { result, Message = "Huỷ đơn hàng '#" + order.order_id + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Chuyển trang thái chờ
        public ActionResult ChangeWaitting(int? id)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var order = db.Orders.SingleOrDefault(pro => pro.order_id == id && pro.status != "3");
                if (order != null)
                {
                    order.status = "1";
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái đơn hàng: " + "PVS" + id + " sang chờ xử lý!", "success");
                }
                else
                {
                    Notification.set_flash("Đơn hàng: " + "PVS" + id + " đã được hoàn thành, không thể thay đổi trạng thái khác!", "warning");
                }
                return RedirectToAction("OrderIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Chuyển trạng thái sang đang xử lý
        public ActionResult ChangeProcessing(int? id,string Roles)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var order = db.Orders.SingleOrDefault(pro => pro.order_id == id && pro.status != "3");
                if (order != null)
                {
                    order.status = "2";
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    if (User.Identity.GetRole() == "0")
                    {
                        Roles = "Quản trị viên";
                    }
                    else if (User.Identity.GetRole() == "2")
                    {
                        Roles = "Biên tập viên";
                    }
                    db.Entry(order).State = EntityState.Modified;
                    string Update_by = User.Identity.GetName();
                    string emailID = order.Account.Email;
                    string OrderID = order.order_id.ToString();
                    string OrderDate = order.oder_date.ToString("dd-MM-yyyy");
                    string OrderNote = order.order_note.ToString();
                    string OrderTotal = order.total.ToString("#,###₫");
                    string OrderPhone = order.Account.Phone.ToString();
                    string OrderName = order.Account.Name;
                    string OrderAddress = order.Account.Addres_1;
                    string OrderDelivery = order.Delivery.delivery_name;
                    SendEmailOrders(emailID, OrderID, OrderDate, OrderNote, OrderTotal, OrderPhone, OrderName, OrderAddress, OrderDelivery, Roles, Update_by, "ChangeProcessing");
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái đơn hàng: "+"PVS"+id+" sang đang xử lý!", "success");
                }
                else
                {
                    Notification.set_flash("Đơn hàng: "+"PVS"+id+" đã được hoàn thành, không thể thay đổi trạng thái khác!", "warning");
                }

                return RedirectToAction("OrderIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Chuyển trạng thái đơn hàng sang hoàn thành
        public ActionResult ChangeComplete(int? id,string Roles)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
                if (order != null)
                {
                    order.status = "3";
                    order.update_at = DateTime.Now;
                    order.update_by = User.Identity.GetEmail();
                    db.Entry(order).State = EntityState.Modified;
                }
                order.update_by = User.Identity.GetName();
                if (User.Identity.GetRole() == "0")
                {
                    Roles = "Quản trị viên";
                }
                else if (User.Identity.GetRole() == "2")
                {
                    Roles = "Biên tập viên";
                }
                string Update_by = User.Identity.GetName();
                string emailID = order.Account.Email;
                string OrderID = order.order_id.ToString();
                string OrderDate = order.oder_date.ToString("dd-MM-yyyy");
                string OrderNote = order.order_note.ToString();
                string OrderTotal = order.total.ToString("#,###₫");
                string OrderPhone = order.Account.Phone.ToString();
                string OrderName = order.Account.Name;
                string OrderAddress = order.Account.Addres_1;
                string OrderDelivery = order.Delivery.delivery_name;
                SendEmailOrders(emailID, OrderID, OrderDate, OrderNote, OrderTotal, OrderPhone, OrderName, OrderAddress, OrderDelivery, Roles, Update_by, "ChangeComplete");
                db.SaveChanges();
                Notification.set_flash("Đã chuyển trạng thái đơn hàng: "+"PVS"+id+" sang Hoàn thành!", "success");
                return RedirectToAction("OrderIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Gửi Email trạng thái sản phẩm
        public void SendEmailOrders(string emailID,string OrderID,string OrderDate,string OrderNote,string OrderTotal,
        string OrderPhone,string OrderName,string OrderAddress,string OrderDelivery,string Roles, string Update_by, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmail, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailOrderStatus" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"

             if (emailFor == "ChangeProcessing")
            {
                subject = "Đơn hàng "+"#"+ OrderID + " đang được xử lý và giao đến bạn";
                body = body.Replace("{{OrderId}}", "#"+ OrderID);
                body = body.Replace("{{OrderDate}}", OrderDate);
                body = body.Replace("{{OrderStatus}}", "Đang xử lý");
                body = body.Replace("{{OrderNote}}", OrderNote);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{OrderPhone}}", OrderPhone);
                body = body.Replace("{{OrderName}}", OrderName);
                body = body.Replace("{{OrderAddress}}", OrderAddress);
                body = body.Replace("{{OrderDelivery}}", OrderDelivery);
                body = body.Replace("{{RolesName}}", Update_by);
                body = body.Replace("{{Roles}}", Roles);
            }
            else if (emailFor == "ChangeComplete")
            {
                subject = "Đơn hàng " + "#" + OrderID + " đã hoàn thành";
                body = body.Replace("{{OrderId}}", "#" + OrderID);
                body = body.Replace("{{OrderDate}}", OrderDate);
                body = body.Replace("{{OrderStatus}}", "Giao thành công");
                body = body.Replace("{{OrderNote}}", OrderNote);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{OrderPhone}}", OrderPhone);
                body = body.Replace("{{OrderName}}", OrderName);
                body = body.Replace("{{OrderAddress}}", OrderAddress);
                body = body.Replace("{{OrderDelivery}}", OrderDelivery);
                body = body.Replace("{{RolesName}}", Update_by);
                body = body.Replace("{{Roles}}", Roles);
            }
            else if (emailFor == "CancleOrders")
            {
                subject = "Đơn hàng " + "#" + OrderID + " đã bị hủy";
                body = body.Replace("{{OrderId}}", "#" + OrderID);
                body = body.Replace("{{OrderDate}}", OrderDate);
                body = body.Replace("{{OrderStatus}}", "Đã bị hủy");
                body = body.Replace("{{OrderNote}}", OrderNote);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{OrderPhone}}", OrderPhone);
                body = body.Replace("{{OrderName}}", OrderName);
                body = body.Replace("{{OrderAddress}}", OrderAddress);
                body = body.Replace("{{OrderDelivery}}", OrderDelivery);
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
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}