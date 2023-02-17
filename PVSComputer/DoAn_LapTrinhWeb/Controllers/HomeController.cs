using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
// using DoAn_LapTrinhWeb.Models;


namespace DoAn_LapTrinhWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();

        public ActionResult Index( int ? page)
        {
            //hiển thị những sản phẩm mới được mua nhiều nhất
            List<Product> newproduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").Take(4).OrderByDescending(m => m.product_id).ToList();
            ViewBag.NewProduct = newproduct;
            //hiển thị những sản phẩm hot được mua nhiều nhất
            List<Product> hotproduct = db.Products.Where(item => item.status == "1" && item.quantity != "0").Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.HotProduct = hotproduct;
            List<Product> hotproductmobile = db.Products.Where(item => item.status == "1" && item.quantity != "0").Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.HotProductmobile = hotproductmobile;
            //hiển thị những linh kiện điện tử được mua nhiều nhất
            List<Product> componentsales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Genre.ParentGenres.id == 4).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.ComponentSales = componentsales;
            //hiển thị những màn hình được mua nhiều nhất
            List<Product> monitorsales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Genre.ParentGenres.id == 10).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.MonitorSales = monitorsales;
            //hiển thị những laptop được mua nhiều nhất
            List<Product> laptopsales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Genre.ParentGenres.id == 2).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.LaptopSales = laptopsales;
            //hiển thị những linh kiện điện tử được mua nhiều nhất
            List<Product> tablesales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Genre.ParentGenres.id == 8).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.TableAndChairSales = tablesales;
            //hiển thị những phụ kiện được mua nhiều nhất
            List<Product> accessorysales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Genre.ParentGenres.id == 3).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.AccessorySales = accessorysales;
            //hiển thị những phụ kiện được mua nhiều nhất
            List<News> recentnews = db.News.Where(item => item.status == "1").Take(3).OrderByDescending(item => item.ViewCount).ToList();
            ViewBag.Recentnews = recentnews;
            List<Brand> brand = db.Brands.Where(item => item.status == "1").Take(5).OrderByDescending(m=>m.Products.Count()).ToList();
            ViewBag.Brand = brand;
            //banner khuyến mãi
            ViewBag.BannerHeader = db.Banners.OrderByDescending(m => m.banner_id).Where(m=>m.status=="1" && m.banner_type==1).Take(8).ToList();
            ViewBag.BannerBottom = db.Banners.OrderByDescending(m => m.banner_id).Where(m => m.status == "1" && m.banner_type == 2).Take(4).ToList();
            ViewBag.BannerVertical = db.Banners.OrderByDescending(m => m.banner_id).Where(m => m.status == "1" && m.banner_type == 3).Take(1).ToList();
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.OrderFeedback = db.Oder_Detail.ToList();
            return View();       
        }
        //Error 404 hiện khi sai URL
        public ActionResult PageNotFound()
        {
            return View();
        }
        //View Gửi yêu cầu hỗ trợ
        public ActionResult SentRequest()
        {
            return View();
        }
        //Code cử lý Gửi yêu càu hỗ trợ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SentRequest(Contact contact)
        {
            try
            {
                contact.phone = contact.phone;
                contact.content = contact.content;
                contact.name = contact.name;
                contact.image = contact.image;
                contact.email = contact.email;
                contact.flag = 0;
                contact.status = "1";
                contact.create_by = contact.email;
                contact.update_by = contact.email;
                contact.update_at = DateTime.Now;
                contact.create_at = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                Notification.set_flash("Gửi yêu cầu thành công", "success");
                return View("SentRequest");
            }
            catch
            {
                Notification.set_flash("Gửi yêu cầu thất bại", "danger");
                return View();
            }
        }
    }
}