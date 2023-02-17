using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class ProductsController : Controller
    {
        private DbContext db = new DbContext();
        //Danh danh sản phẩm
        public ActionResult ListProduct(int? page, string slug, string sortOrder)
        {
            var product = db.Products.Where(m => m.status == "1" && (m.Genre.ParentGenres.slug == slug || m.Genre.slug == slug || m.Brand.slug == slug)).FirstOrDefault();
            if (product.Genre.slug == slug)
            {
                ViewBag.Type =product.Genre.genre_name;
            }
            else if (product.Brand.slug == slug)
            {
                ViewBag.Type = "Thương hiệu "+product.Brand.brand_name;
            }
            else if (product.Genre.ParentGenres.slug == slug)
            {
                ViewBag.Type = product.Genre.ParentGenres.name;
            }
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.OrderFeedback = db.Oder_Detail.ToList();
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && (m.Genre.ParentGenres.slug == slug || m.Genre.slug == slug || m.Brand.slug == slug)).Count();
            ViewBag.UrlType = "ListProduct";
            return View("Product", GetProduct(m => (m.Genre.ParentGenres.slug==slug || m.Genre.slug==slug || m.Brand.slug == slug) && m.status=="1", page, sortOrder));
        }
        //Chi tiết sản phẩm
        public ActionResult ProductDetail(string slug,string returnUrl)
        {
            var product = (from p in db.Products
                           from od in db.Orders
                           from odt in db.Oder_Detail
                                join g in db.Genres on p.genre_id equals g.genre_id
                                  join pg in db.ParentGenres on g.parent_genre_id equals pg.id
                                  join dc in db.Discounts on p.disscount_id equals dc.disscount_id
                                  join pig in db.Product_Images on p.product_id equals pig.product_id
                                  where p.slug==slug && p.status == "1"
                                  orderby p.product_id descending
                                  select new ProductDTOs
                                  {
                                      Image=p.image,
                                      product_id=p.product_id,
                                      status = p.status,
                                      image_1 =pig.image_1,
                                      image_2 = pig.image_2,
                                      image_3 = pig.image_3,
                                      image_4 = pig.image_4,
                                      image_5 = pig.image_5,
                                      quantity=p.quantity,
                                      brand_name=p.Brand.brand_name,
                                      brand_slug=p.Brand.slug,
                                      description=p.description,
                                      specification=p.specification,
                                      seo_title=p.title_seo,
                                      slug=p.slug,
                                      product_name=p.product_name,
                                      price=p.price,
                                      genre_id=g.genre_id,
                                      genre_name=g.genre_name,
                                      genre_slug=g.slug,
                                      parent_genre_id=pg.id,
                                      parent_genre_name=pg.name,
                                      parent_genre_slug=pg.slug,
                                      discount_start=dc.discount_start,
                                      discount_end=dc.discount_end,
                                      discount_id=dc.disscount_id,
                                      discount_price=dc.discount_price,
                                      discount_status=dc.status,
                                      orderdetail_product_id=odt.Product.product_id,
                                      order_account_id=od.account_id,
                                  }).FirstOrDefault();
            ViewBag.Share = db.Products.Where(m => m.slug == slug);
            ViewBag.CountFeedback = db.Feedbacks.Where(m => m.product_id == product.product_id && m.status == "2").Count();
            List<Feedback> feedbackproduct = db.Feedbacks.Where(m=>m.status=="2").OrderByDescending(m => m.feedback_id).ToList();
            ViewBag.feedbackproduct = feedbackproduct;
            List<Account> Accountfeedback = db.Accounts.ToList();
            ViewBag.Accountfeedback = Accountfeedback;
            List<Feedback_Image> feedbackimages = db.Feedback_Image.ToList();
            ViewBag.feedbackimages = feedbackimages;
            ViewBag.Orderfeedback = db.Oder_Detail;
            ViewBag.CheckExistBannerDetail = db.Banner_Detail.Where(m => m.product_id == product.product_id).FirstOrDefault();
            ViewBag.BannerDetail = db.Banner_Detail.Where(m=>m.product_id==product.product_id).ToList();
            ViewBag.Banner = db.Banners;
            ViewBag.OrderFeedback = db.Oder_Detail.ToList();
            ViewBag.AvgFeedback = db.Feedbacks.Where(m => m.product_id == product.product_id).ToList();
            List<NewsProducts> newsproducts = db.NewsProducts.Where(m => m.product_id == product.product_id).ToList();
            ViewBag.newsproducts = newsproducts;
            //Sản phẩm liên quan
            List<Product> relatedproduct = db.Products.Where(item => item.status == "1" && item.product_id != product.product_id && item.genre_id == product.genre_id).Take(4).ToList();
            ViewBag.relatedproduct = relatedproduct;
            List<Feedback> rating = db.Feedbacks.Where(m => m.status == "1").ToList();
            ViewBag.rating = rating;
            product.view++;
            db.SaveChanges();
            return View(product);
        }
        //Tìm kiếm sản phẩm
        public ActionResult SearchResult(int? page, string sortOrder,string s)
        {
            ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.BuySortParm = sortOrder == "buy_desc" ? "buy_desc" : "buy_desc";
            ViewBag.ViewSortParm = sortOrder == "view_desc" ? "view_desc" : "view_desc";
            ViewBag.UnderthreeMillionSortParm = sortOrder == "duoi-3-trieu" ? "duoi-3-trieu" : "duoi-3-trieu";
            ViewBag.FromthreeToeightMillionSortParm = sortOrder == "tu-3-8-trieu" ? "tu-3-8-trieu" : "tu-3-8-trieu";
            ViewBag.FromeightTofifteenMillionSortParm = sortOrder == "tu-8-15-trieu" ? "tu-8-15-trieu" : "tu-8-15-trieu";
            ViewBag.FromfifteenTotwentyfiveMillionSortParm = sortOrder == "tu-15-25-trieu" ? "tu-15-25-trieu" : "tu-15-25-trieu";
            ViewBag.MorethantwentyfiveMillionSortParm = sortOrder == "tren-25-trieu" ? "tren-25-trieu" : "tren-25-trieu";
            ViewBag.DiscountSortParm = sortOrder == "discount_desc" ? "discount_asc" : "discount_desc";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.UrlType = "SearchResult";
            ViewBag.Search = s;
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            ViewBag.OrderFeedback = db.Oder_Detail.ToList();
            var list = db.Products.OrderByDescending(m => m.product_id);
            switch (sortOrder)
            {

                case "duoi-3-trieu":
                    ViewBag.sortname = "Dưới 3 triệu";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => ((m.price - m.Discount.discount_price) < 3000000)&& m.status == "1" && m.product_name.Contains(s));
                    break;
                case "tu-3-8-trieu":
                    ViewBag.sortname = "Từ 3 đến 8 triệu";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => ((m.price - m.Discount.discount_price) >= 3000000) && ((m.price - m.Discount.discount_price) <= 8000000) && m.status == "1" && m.product_name.Contains(s));
                    break;
                case "tu-8-15-trieu":
                    ViewBag.sortname = "Từ 3 đến 8 triệu";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 8000000 && (m.price - m.Discount.discount_price) <= 15000000);
                    break;
                case "tu-15-25-trieu":
                    ViewBag.sortname = "Từ 15 đến 25 triệu";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 15000000 && (m.price - m.Discount.discount_price) <= 25000000);
                    break;
                case "tren-25-trieu":
                    ViewBag.sortname = "Trên 25 triệu";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 25000000);
                    break;
                case "view_desc":
                    ViewBag.sortname = "Xem nhiều";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.view).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "buy_desc":
                    ViewBag.sortname = "Bán chạy";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.buyturn).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "price_asc":
                    ViewBag.sortname = "Giá tăng dần";
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => (m.price - m.Discount.discount_price)).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "price_desc":
                    ViewBag.sortname = "Giá giảm dần";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.price - m.Discount.discount_price).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "discount_asc":
                    ViewBag.sortname = "Khuyến mãi tăng dần";
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => m.Discount.discount_price).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "discount_desc":
                    ViewBag.sortname = "Khuyến mãi giảm dần";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.Discount.discount_price).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "date_asc":
                    ViewBag.sortname = "Cũ nhất";
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => m.Discount.discount_price).Where(m=> m.status == "1" && m.product_name.Contains(s));
                    break;
                case "name_asc":
                    ViewBag.sortname = "Tên A-Z";
                    list = (IOrderedQueryable<Product>)db.Products.OrderBy(m => m.product_name).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "name_desc":
                    ViewBag.sortname = "Tên Z-A";
                    ViewBag.searchpage = list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_name).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                case "date_desc":
                    ViewBag.sortname = "Mới nhất";
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id).Where(m => m.status == "1" && m.product_name.Contains(s));
                    break;
                default:
                    list = (IOrderedQueryable<Product>)db.Products.OrderByDescending(m => m.product_id);
                    break;
            }

            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.product_name.Contains(s)).Count();
            ViewBag.Urltype = "SearchResult";
            ViewBag.Type = "Tìm kiếm";
            return View("Product", GetProduct(m => m.status == "1" && m.product_name.Contains(s), page, sortOrder));
        }
        //gợi ý search
        [HttpPost]
        public JsonResult GetProductSearch(string Prefix)
        {
            //tìm sản phẩm theo tên
            var search = (from c in db.Products
                          where c.status=="1" && c.product_name.Contains(Prefix)
                          orderby c.product_name ascending
                          select new { c.product_name, c.slug,c.image,c.price,c.Discount.discount_price });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Phân trang danh sách sản phẩm
        private IPagedList GetProduct(Expression<Func<Product, bool>> expr, int? page, string sortOrder)
        {
            //ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //Sắp xếp sản phẩm
            ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.BuySortParm = sortOrder == "buy_desc" ? "buy_desc" : "buy_desc";
            ViewBag.ViewSortParm = sortOrder == "view_desc" ? "view_desc" : "view_desc";
            ViewBag.UnderthreeMillionSortParm = sortOrder == "duoi-3-trieu" ? "duoi-3-trieu" : "duoi-3-trieu";
            ViewBag.FromthreeToeightMillionSortParm = sortOrder == "tu-3-8-trieu" ? "tu-3-8-trieu" : "tu-3-8-trieu";
            ViewBag.FromeightTofifteenMillionSortParm = sortOrder == "tu-8-15-trieu" ? "tu-8-15-trieu" : "tu-8-15-trieu";
            ViewBag.FromfifteenTotwentyfiveMillionSortParm = sortOrder == "tu-15-25-trieu" ? "tu-15-25-trieu" : "tu-15-25-trieu";
            ViewBag.MorethantwentyfiveMillionSortParm = sortOrder == "tren-25-trieu" ? "tren-25-trieu" : "tren-25-trieu";
            ViewBag.DiscountSortParm = sortOrder == "discount_desc" ? "discount_asc" : "discount_desc";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            //1 trang hiện thỉ tối đa 12 sản phẩm
            int pageSize = 12; 
            int pageNumber = (page ?? 1); 
            var list = db.Products.Where(expr).OrderByDescending(m=>m.product_id).ToPagedList(pageNumber, pageSize);
            switch (sortOrder)
                {
                    case "duoi-3-trieu":
                        ViewBag.sortname = "Dưới 3 triệu";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) < 3000000).ToPagedList(pageNumber, pageSize);
                    ViewBag.Countproduct = list.Count();
                    break;
                    case "tu-3-8-trieu":
                        ViewBag.sortname = "Từ 3 đến 8 triệu";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) >= 3000000 && (m.price - m.Discount.discount_price) <= 8000000).ToPagedList(pageNumber, pageSize);
                    ViewBag.Countproduct = list.Count();
                    break;
                    case "tu-8-15-trieu":
                        ViewBag.sortname = "Từ 3 đến 8 triệu";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 8000000 && (m.price - m.Discount.discount_price) <= 15000000).ToPagedList(pageNumber, pageSize);
                    ViewBag.Countproduct = list.Count();
                    break;
                    case "tu-15-25-trieu":
                        ViewBag.sortname = "Từ 15 đến 25 triệu";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 15000000 && (m.price - m.Discount.discount_price) <= 25000000).ToPagedList(pageNumber, pageSize);
                    ViewBag.Countproduct = list.Count();
                    break;
                    case "tren-25-trieu":
                        ViewBag.sortname = "Trên 25 triệu";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 25000000).ToPagedList(pageNumber, pageSize);
                    ViewBag.Countproduct = list.Count();
                    break;
                    case "view_desc":
                        ViewBag.sortname = "Xem nhiều";
                        list = db.Products.Where(expr).OrderByDescending(m => m.view).ToPagedList(pageNumber, pageSize);
                    break;
                    case "buy_desc":
                        ViewBag.sortname = "Bán chạy";
                        list = db.Products.Where(expr).OrderByDescending(m => m.buyturn).ToPagedList(pageNumber, pageSize);
                    break;
                    case "price_asc":
                        ViewBag.sortname = "Giá tăng dần";
                        list = db.Products.Where(expr).OrderBy(m => (m.price - m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
                    case "price_desc":
                        ViewBag.sortname = "Giá giảm dần";
                        list = db.Products.Where(expr).OrderByDescending(m => (m.price - m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
                    case "discount_asc":
                        ViewBag.sortname = "Khuyến mãi tăng dần";
                        list = db.Products.Where(expr).OrderBy(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                    case "discount_desc":
                        ViewBag.sortname = "Khuyến mãi giảm dần";
                        list = db.Products.Where(expr).OrderByDescending(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                    case "date_asc":
                        ViewBag.sortname = "Cũ nhất";
                        list = db.Products.Where(expr).OrderBy(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                    case "name_asc":
                        ViewBag.sortname = "Tên A-Z";
                        list = db.Products.Where(expr).OrderBy(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                    case "name_desc":
                        ViewBag.sortname = "Tên Z-A";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                    case "date_desc":
                        ViewBag.sortname = "Mới nhất";
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).ToPagedList(pageNumber, pageSize);
                    break;
                    default:
                        list = db.Products.Where(expr).OrderByDescending(m => m.product_id).ToPagedList(pageNumber, pageSize);
                    break;
                }
            return list;
            
        }
        //Danh sách thể loại sản phẩm
        public ActionResult Listgenres()
        {
            ViewBag.productsgenres = db.Genres.OrderBy(m=>m.genre_name).ToList();
            List<ParentGenres> parents = db.ParentGenres.Where(m => m.status == "1").Take(6).ToList();
            if (parents.FirstOrDefault().status == "1")
            {
                return PartialView("Listgenres", parents);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Đánh giá sản phẩm
        [HttpPost]
        public ActionResult RatingProduct(Feedback feedback,Feedback_Image feedback_Image)
        {
            try
            {
                feedback.account_id = User.Identity.GetUserId();
                feedback.create_at = DateTime.Now;
                feedback.update_at = DateTime.Now;
                feedback.create_by = User.Identity.GetEmail();
                feedback.update_by = User.Identity.GetEmail();
                feedback.status = "1";
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                if (feedback_Image.UploadImage != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(feedback_Image.UploadImage.FileName);
                    string extension = Path.GetExtension(feedback_Image.UploadImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                    feedback_Image.image = "/Images/FeedbackImages/"+ fileName;
                    feedback_Image.feedback_id = feedback.feedback_id;
                    feedback_Image.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/FeedbackImages/"), fileName));
                    feedback_Image.account_id = User.Identity.GetUserId();
                    feedback_Image.create_at = DateTime.Now;
                    feedback_Image.update_at = DateTime.Now;
                    feedback_Image.create_by = User.Identity.GetUserId().ToString();
                    feedback_Image.update_by = User.Identity.GetUserId().ToString();
                    feedback_Image.status = "1";
                    db.Feedback_Image.Add(feedback_Image);
                    db.SaveChanges();
                }
                else
                {
                    feedback_Image.image = "/Images/ImagesCollection/no-image-available.png";
                    feedback_Image.feedback_id = feedback.feedback_id;
                    feedback_Image.account_id = User.Identity.GetUserId();
                    feedback_Image.create_at = DateTime.Now;
                    feedback_Image.update_at = DateTime.Now;
                    feedback_Image.create_by = User.Identity.GetUserId().ToString();
                    feedback_Image.update_by = User.Identity.GetUserId().ToString();
                    feedback_Image.status = "1";
                    db.Feedback_Image.Add(feedback_Image);
                    db.SaveChanges();
                }
                Notification.set_flash("Đánh giá của bản đã được gửi đi và đang chờ duyệt", "success");
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "!lỗi" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Listgenres_mobile()
        {
            ViewBag.productsgenres = db.Genres.OrderBy(m => m.genre_name).ToList();
            List<ParentGenres> parents = db.ParentGenres.Where(m => m.status == "1").Take(6).ToList();
            if (parents.FirstOrDefault().status == "1")
            {
                return PartialView("Listgenres_mobile", parents);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}