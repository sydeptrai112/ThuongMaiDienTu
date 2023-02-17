using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class CartController : Controller
    {
        private DbContext db = new DbContext();
        //View Giỏ hàng
        public ActionResult ViewCart(string returnUrl)
        {
            var cart = this.GetCart();
            ViewBag.Quans = cart.Item2;
            var listProduct = cart.Item1.OrderByDescending(x => x.product_id).ToList();
            double discount =0d;
            if (Session["Discount"] != null)
            {
                discount = Convert.ToDouble(Session["Discount"].ToString());
            }
            ViewBag.Discount = discount;
            return View(listProduct);
        }
        //Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveProduct(int id)
        {
            List<Cart> lst = Session["Cart"] as List<Cart>;
            Cart item = lst.Find(n => n.Product_ID == id);
            lst.Remove(item);
            return RedirectToAction("ViewCart");
        }
        //Cập nhật giỏ hàng
        [HttpPost]
        public ActionResult UpdateCart(int id, FormCollection frm)
        {
            List<Cart> lst = Session["Cart"] as List<Cart>;
            Cart item = lst.Find(n => n.Product_ID == id);
            int soluong = int.Parse(frm["txtSoluong"].ToString());
            item.Quantity = soluong;//cap nhat so luong moi
            return RedirectToAction("ViewCart");
        }
        // => Phải đăng nhập mới được phép vào action bên dưới
        // => Có thể đặt trước Controller để áp dụng cho toàn bộ action trong controller đó
        [Authorize]
        public ActionResult Checkout()
        {
            int userId = User.Identity.GetUserId();
            var user = db.Accounts.SingleOrDefault(u => u.account_id == userId);
            var cart = this.GetCart();

            if (cart.Item2.Count < 1)
            {
                Notification.set_flash("Bạn chưa có sản phẩm trong giỏ hàng", "Warning");
                return RedirectToAction(nameof(ViewCart));
            }
            var products = cart.Item1;
            double total = 0d;
            double discount = 0d;
            double productPrice = 0d;
            for (int i = 0; i < products.Count; i++)
            {
                var item = products[i];
                productPrice = item.price;
                if (item.Discount != null)
                {
                    if (item.Discount.discount_start < DateTime.Now && item.Discount.discount_end > DateTime.Now)
                    {
                        productPrice = item.price - item.Discount.discount_price;
                    }
                }
                total += productPrice * cart.Item2[i];
            }
            //Áp dụng mã giảm giá
            if (Session["Discount"] != null)
            {
                discount = Convert.ToDouble(Session["Discount"].ToString());
                total -= discount;
            }
            ViewBag.Total = total;
            ViewBag.Discount = discount;
            return View(user);
        }
        //Lưu giỏ hàng
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SaveOrder()
        {
            try
            {
                var cart = this.GetCart();
                var listProduct = new List<Product>();
                var order = new Order()
                {
                    account_id = User.Identity.GetUserId(),
                    create_at = DateTime.Now,
                    create_by = User.Identity.GetUserId().ToString(),
                    status = "1",
                    order_note = Request.Form["OrderNote"].ToString(),
                    delivery_id = 1,
                    oder_date = DateTime.Now,
                    update_at = DateTime.Now,
                    payment_id = 1,
                    update_by = User.Identity.GetUserId().ToString(),
                    total = Convert.ToDouble(TempData["Total"])
                };
                for (int i = 0; i < cart.Item1.Count; i++)
                {
                    var item = cart.Item1[i];
                    var _price = item.price;
                    if (item.Discount != null)
                    {
                        if (item.Discount.discount_start < DateTime.Now && item.Discount.discount_end > DateTime.Now)
                        {
                            _price = item.price - item.Discount.discount_price;
                        }
                    }
                    order.Oder_Detail.Add(new Oder_Detail
                    {
                        create_at = DateTime.Now,
                        create_by = User.Identity.GetUserId().ToString(),
                        disscount_id = item.disscount_id,
                        genre_id = item.genre_id,
                        price = _price,
                        product_id = item.product_id,
                        quantity = cart.Item2[i],
                        status = "1",
                        update_at = DateTime.Now,
                        update_by = User.Identity.GetUserId().ToString(),
                        transection = "transection"
                    });
                    // Xóa cart
                    Response.Cookies["product_" + item.product_id].Expires = DateTime.Now.AddDays(-10);
                    // Thay đổi số lượng và số lượt mua của product 
                    var product = db.Products.SingleOrDefault(p => p.product_id == item.product_id);
                    product.buyturn += cart.Item2[i];
                    product.quantity = (Convert.ToInt32(product.quantity ?? "0") - cart.Item2[i]).ToString();
                    //ProductID+= product.product_name;
                    //UserEmail = User.Identity.GetEmail();
                    listProduct.Add(product);
                }
                
              
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                Session.Remove("Discount");
                TempData["AddOrderSuccess"] = true;
            }
            catch
            {
                TempData["AddOrderSuccess"] = false;
            }
            Notification.set_flash("Đặt hàng thành công" , "success");

            return RedirectToAction("TrackingOrder", "Account");
        }
        //Gửi mail khi đăt hàng thành, chức năng này khóa lại, bạn tự mở ra và viết code xử lý chỗ này nha
        [NonAction]
        public void SendVerificationLinkEmail(string UserEmail,string ProductID)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmailSupport, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(UserEmail);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailOrders" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
                subject = "Đặt hàng thành công ";
                body = body.Replace("@ViewBag.Product",ProductID);
                body = body.Replace("@ViewBag.confirmtext", "Kích hoạt tài khoản");
                body = body.Replace("@ViewBag.bodytext", "Nhấn vào nút bên dưới để xác thực hoàn tất đăng ký cho tài khoản của bạn");
             
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
        //Áp dụng code giảm giá sản phẩm
        public ActionResult UseDiscountCode(string code)
        {
            var discount = db.Discounts.SingleOrDefault(d => d.discounts_code == code);
            if (discount != null)
            {
                if (discount.discount_start < DateTime.Now && discount.discount_end > DateTime.Now)
                {
                    Session["Discount"] = discount.discount_price;
                    return Json(new { success = true, discountPrice = discount.discount_price }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, discountPrice = 0 }, JsonRequestBehavior.AllowGet);
        }

        private Tuple<List<Product>, List<int>> GetCart()
        {
            var cart = Request.Cookies.AllKeys.Where(c => c.IndexOf("product_") == 0);
            var productIds = new List<int>();
            var quantities = new List<int>();

            var errorProduct = new List<string>();
            var cValue = "";
            // Lấy mã sản phẩm & số lượng trong giỏ hàng
            foreach (var item in cart)
            {
                var tempArr = item.Split('_');
                if (tempArr.Length != 2)
                {
                    //Nếu không lấy được thì xem như sản phẩm đó bị lỗi
                    errorProduct.Add(item);
                    continue;
                }
                cValue = Request.Cookies[item].Value;
                productIds.Add(Convert.ToInt32(tempArr[1]));
                quantities.Add(Convert.ToInt32(String.IsNullOrEmpty(cValue) ? "0" : cValue));
            }
            // Select sản phẩm để hiển thị
            var listProduct = new List<Product>();
            foreach (var id in productIds)
            {
                var product = db.Products
                        .SingleOrDefault(p => p.status == "1" && p.product_id == id);
                if (product != null)
                {
                    listProduct.Add(product);
                }
                else
                {
                    // Trường hợp ko chọn được sản phẩm như trong giỏ hàng
                    // Đánh dấu sản phẩm trong giỏ hàng là sản phẩm lỗi
                    errorProduct.Add("product_" + id);
                    quantities.RemoveAt(productIds.IndexOf(id));
                }
            }
            //Xóa sản phẩm bị lỗi khỏi cart
            foreach (var err in errorProduct)
            {
                Response.Cookies[err].Expires = DateTime.Now.AddDays(-1);
            }
            return new Tuple<List<Product>, List<int>>(listProduct, quantities);
        }
    }
}