using System;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View thống kê
        public ActionResult Index(string returnUrl)
        {
            ViewBag.VisitorOnline = HttpContext.Application["VisitorOnline"].ToString(); //lay so luong nguoi truy cap
            DateTime today = DateTime.Today;
            //Đếm đơn hàng mới
            ViewBag.CountOrderWaitting = (from a in db.Oder_Detail
                                          join b in db.Orders on a.order_id equals b.order_id
                                          group a by new { a.order_id, b } into g
                                          where g.Key.b.status == "1" && g.Key.b.oder_date.Month == today.Month
                                          orderby g.Key.b.create_at descending
                                          select new OrderDTOs
                                          {
                                              order_id = g.Key.order_id,
                                              price = g.Sum(m => m.price),
                                              status = g.Key.b.status,
                                              create_at = g.Key.b.create_at
                                          }).Count();
            //Đếm đơn hàng chờ xử lý
            ViewBag.CountOrderProcessing = (from a in db.Oder_Detail
                                          join b in db.Orders on a.order_id equals b.order_id
                                          group a by new { a.order_id, b } into g
                                          where g.Key.b.status == "2" && g.Key.b.oder_date.Month == today.Month
                                          orderby g.Key.b.create_at descending
                                          select new OrderDTOs
                                          {
                                              order_id = g.Key.order_id,
                                              price = g.Sum(m => m.price),
                                              status = g.Key.b.status,
                                              create_at = g.Key.b.create_at
                                          }).Count();
            //Đếm đơn hàng đã hoàn thành
            ViewBag.CountOrderComplete = (from a in db.Oder_Detail
                                          join b in db.Orders on a.order_id equals b.order_id
                                          group a by new { a.order_id, b } into g
                                          where g.Key.b.status == "3" && g.Key.b.oder_date.Month == today.Month
                                          orderby g.Key.b.create_at descending
                                          select new OrderDTOs
                                          {
                                              order_id = g.Key.order_id,
                                              price = g.Sum(m => m.price),
                                              status = g.Key.b.status,
                                              create_at = g.Key.b.create_at
                                          }).Count();
            //Đếm đơn hàng đã hủy
            ViewBag.CountOrderCancled = (from a in db.Oder_Detail
                                          join b in db.Orders on a.order_id equals b.order_id
                                          group a by new { a.order_id, b } into g
                                          where g.Key.b.status == "0" && g.Key.b.oder_date.Month == today.Month
                                          orderby g.Key.b.create_at descending
                                          select new OrderDTOs
                                          {
                                              order_id = g.Key.order_id,
                                              price = g.Sum(m => m.price),
                                              status = g.Key.b.status,
                                              create_at = g.Key.b.create_at
                                          }).Count();
            //Đếm số lượng sản phẩm đẵ bán trong tháng
            ViewBag.CountProductSale = (from a in db.Oder_Detail
                                          join b in db.Orders on a.order_id equals b.order_id
                                          join c in db.Products on a.product_id equals c.product_id
                                          group a by new { a.order_id, b } into g
                                          where g.Key.b.status == "3" && g.Key.b.oder_date.Month == today.Month
                                          orderby g.Key.b.create_at descending
                                          select new OrderDTOs
                                          {
                                              total_quantity=g.Sum(m=>m.quantity) ,
                                              order_id = g.Key.order_id,
                                              status = g.Key.b.status,
                                              create_at = g.Key.b.create_at 
                                          }).Sum(m=> (int?)m.total_quantity) ?? 0;
            //Đếm sản phẩm mới
            ViewBag.CountProductsNew = db.Products.Where(m => m.create_at.Month == today.Month).Count();
            //Đếm liên lạc
            ViewBag.CountContact = db.Contacts.Where(m => m.create_at.Month == today.Month && m.status!="2").Count();
            //Chõ này phải chuyển status 1 đơn hàng qua status=3 thì sẽ không bị báo lồi khi vào trang admin
            ViewBag.CountTurnover = db.Orders.Where(m => m.status == "3" && m.oder_date.Month == today.Month).Sum(x => (int?)x.total) ?? 0;
            //Tổng doanh thu năm
            ViewBag.CountTurnover1 = db.Orders.Where(m => m.status == "3" && m.oder_date.Year == today.Year).Sum(x => (int?)x.total) ?? 0;
            //Sản phẩm mới
            ViewBag.CountProducts = db.Products.Count(m => m.status == "1" && m.create_at.Month == today.Month);
            //Bài viết đã tạo trong tháng
            ViewBag.CountPost = db.News.Count(m => m.status != "2" && m.create_at.Month == today.Month);
            //Thành viên mới
            ViewBag.CountUsers = db.Accounts.Count(m => m.status == "1" && m.Role == "1" && m.create_at.Month == today.Month);
            ViewBag.ProcessingOrder = "processing";
            ViewBag.CompleteOrder = "complete";
            ViewBag.WaitingOrder = "waiting";
            return View();
        }
        //Biểu đồ top 10 sản phẩm bán chạy
        public ActionResult Getbarcharts()
        {
            DateTime today = DateTime.Today;
            var query = db.Oder_Detail.Include("Product")
                    .Where(m=>m.Order.oder_date.Month==today.Month && m.Order.status=="3")
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity)}).OrderByDescending(m=>m.count).Take(10).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}