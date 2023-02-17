using System.Web.Mvc;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using static DoAn_LapTrinhWeb.DTOs.NewsDTO;
using DoAn_LapTrinhWeb.DTOs;
using PagedList;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class NewsController : Controller
    {
        private DbContext db = new DbContext();
        // danh mục cha
        public PartialViewResult Listcategory()
        {
            ViewBag.newscategory = db.ChildCategory.ToList();
            return PartialView("Listcategory", db.ParentCategory.ToList());
        }
        public PartialViewResult Listcategory_mobile()
        {
            ViewBag.newscategory = db.ChildCategory.ToList();
            return PartialView("Listcategory_mobile", db.ParentCategory.ToList());
        }
        //View để get bài viết
        public ActionResult News()
        {
            return View();
        }
        public ActionResult NewsIndex()
        {
            DateTime today = DateTime.Today;
            ViewBag.newscategory = db.ChildCategory;
            //=>
            ViewBag.category = db.ParentCategory;
            //=>
            ViewBag.newsinnewscategory = db.News.OrderByDescending(m => Guid.NewGuid()).Where(m => m.status == "1" && m.create_at.Month == today.Month).Take(5).ToList();
            //=>
            ViewBag.recentnews = db.News.OrderByDescending(m => m.news_id).Where(m => m.status == "1" && m.create_at.Month == today.Month).Take(10).ToList();
            //=> top commnet
            List<News> topcommentofmonth = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => m.NewsComments.Count()).Take(6).ToList();
            ViewBag.topcommentofmonth = topcommentofmonth;
            List<News> topcommentofyear = db.News.Where(m => m.status == "1" && m.create_at.Year == today.Year).OrderByDescending(m => m.NewsComments.Count()).Take(6).ToList();
            ViewBag.topcommentofyear = topcommentofyear;
            List<News> topcomment = db.News.Where(m => m.status == "1").OrderByDescending(m => m.NewsComments.Count()).Take(6).ToList();
            ViewBag.topcomment = topcomment;
            //=>
            List<News> topnewsofmonth = db.News.Where(m =>m.status=="1" && m.create_at.Month == today.Month).OrderByDescending(m => (m.ViewCount+ m.NewsComments.Count())).Take(20).ToList();
            ViewBag.topnewsofmonth = topnewsofmonth;
            List<News> topnewsofyear = db.News.Where(m => m.status == "1" && m.create_at.Year == today.Year).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(20).ToList();
            ViewBag.topnewsofyear = topnewsofyear;
            List<News> topnews = db.News.Where(m => m.status == "1").OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(20).ToList();
            ViewBag.topnews = topnews;
            //=>
            List<Product> Product = db.Products.ToList();
            ViewBag.product = Product;
            //=> Products Review
            List<NewsProducts> proreviews = db.NewsProducts.OrderByDescending(m=>m.News.ViewCount).Distinct().Take(4).ToList();
            ViewBag.productsreviews = proreviews;
            List<Product> product = db.Products.Where(m => m.status == "1" && m.create_at.Month == today.Month).ToList();
            ViewBag.product = product;
            //=> Game
            List<News> game = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month && m.ChildCategory.ParentCategory.parentcategory_id==2).OrderByDescending(m=> (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.game = game;
            //=>
            //List<News> news = db.News.Where(m=>m.;
            ViewBag.product = Product;
            //=> Trendingnow
            List<News> trendingnowofday = db.News.Where(m => m.status == "1" && m.create_at.Day == today.Day).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.trendingnowofday = trendingnowofday;
            List<News> trendingnowofmonth = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.trendingnowofmonth = trendingnowofmonth;
            List<News> trendingnow = db.News.Where(m => m.status == "1").OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.trendingnow = trendingnow;
            //=> Hostpost
            List<StickyPost> stickyPosts = db.StickyPosts.Where(m => m.News.status == "1").OrderByDescending(m =>m.priority).Take(4).ToList();
            ViewBag.stickyPosts = stickyPosts;
            List<News> Hostpostofday = db.News.Where(m => m.status == "1" && m.create_at.Day == today.Day).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.Hostpostofday = Hostpostofday;
            List<News> Hostpostofmonth = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.Hostpostofmonth = Hostpostofmonth;
            List<News> Hostpost = db.News.Where(m => m.status == "1").OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.Hostpost = Hostpost;
            //=>
            List<Tags> listtag = db.Tags.OrderByDescending(m => Guid.NewGuid()).Take(9).ToList();
            ViewBag.listtag = listtag;
            return View();
        }
        //dách sách loại tin của danh mục
        public ActionResult ListNewsCategory(int? page,string slug)
        {
            ViewBag.categoryimage = db.ParentCategory.Where(m => m.slug == slug).FirstOrDefault().image2;
            ViewBag.category = db.ParentCategory.Where(m => m.slug == slug).FirstOrDefault().name;
            return View("ChildCategory", GetNewsCategory(m => m.status == "1" && m.ParentCategory.slug==slug, page));
        }
        //lấy danh sách loại tin
        private IPagedList GetNewsCategory(Expression<Func<ChildCategory, bool>> expr, int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            var list = db.ChildCategory.Where(expr).OrderByDescending(m => m.childcategory_id).ToPagedList(pageNumber, pageSize);
            return list;
        }
        //List bài viết của tag
        public ActionResult ListNewsTags(int? page, int? size, string slug)
        {
            ViewBag.newscategory = db.ChildCategory;
            //=>
            ViewBag.category = db.ParentCategory;
            //=>
            ViewBag.ListName = db.Tags.Where(m=>m.slug==slug).FirstOrDefault().tag_name;
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            var list = from n in db.News
                       join
                        nt in db.NewsTags on n.news_id equals nt.news_id
                       join tg in db.Tags on nt.tag_id equals tg.tag_id
                       where tg.slug == slug &&n.status=="1"
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           tag_slug = tg.slug,
                           news_title = n.news_title,
                           create_at = n.create_at,
                           tag_name = tg.tag_name,
                           author = n.Accounts.Name,
                           image2 = n.image2,
                           news_slug = n.slug
                       };
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //tìm kiếm bài viết
        public ActionResult SearchResult(string s, int? page )
        {
            List<NewsDTO> listtag = (from n in db.News
                                  join
                                   nt in db.NewsTags on n.news_id equals nt.news_id
                                  join tg in db.Tags on nt.tag_id equals tg.tag_id
                                  where n.news_title.Contains(s) && n.status == "1"
                                  orderby n.news_id descending
                                  select new NewsDTO
                                  {
                                      tag_slug = tg.slug,
                                      news_title = n.news_title,
                                      create_at = n.create_at,
                                      tag_name = tg.tag_name,
                                      author = n.Accounts.Name,
                                      image2 = n.image2,
                                      news_slug = n.slug
                                  }).ToList();
            var list = db.News.OrderByDescending(m => m.news_id);
            ViewBag.listtag = listtag;
            ViewBag.ListName = "Tìm kiếm bài viết";
            return View("News", GetNews(m => m.status == "1" && m.news_title.Contains(s), page));
        }
        //gợi ý search
        [HttpPost]
        public JsonResult GetPostSearch(string Prefix)
        {
            //tìm sản phẩm theo tên
            var search = (from c in db.News
                          where c.status == "1" && c.news_title.Contains(Prefix)
                          orderby c.news_title ascending
                          select new { c.news_title, c.slug, c.image});
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //lấy danh sách tin tức của loại tin
        public ActionResult ListNews(int? page, string slug, string sortOrder)
        {
            List<NewsDTO> list = (from n in db.News
                       join
                        nt in db.NewsTags on n.news_id equals nt.news_id
                       join tg in db.Tags on nt.tag_id equals tg.tag_id
                       where n.ChildCategory.slug == slug && n.status == "1"
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           tag_slug = tg.slug,
                           news_title = n.news_title,
                           create_at = n.create_at,
                           tag_name = tg.tag_name,
                           author = n.Accounts.Name,
                           image2 = n.image2,
                           news_slug = n.slug
                       }).ToList();
            ViewBag.listtag = list;
            ViewBag.ListName = db.ChildCategory.Where(m => m.slug == slug).FirstOrDefault().name;
            return View("News", GetNews(m => m.status == "1" && (m.ChildCategory.slug == slug ), page));
        }
        //lấy tất cả tin tức
        public ActionResult AllListNews(int? page, string sortOrder)
        {
            List<NewsDTO> list = (from n in db.News
                                  join
                                   nt in db.NewsTags on n.news_id equals nt.news_id
                                  join tg in db.Tags on nt.tag_id equals tg.tag_id
                                  where n.status == "1"
                                  orderby n.news_id descending
                                  select new NewsDTO
                                  {
                                      tag_slug = tg.slug,
                                      news_title = n.news_title,
                                      create_at = n.create_at,
                                      tag_name = tg.tag_name,
                                      author = n.Accounts.Name,
                                      image2 = n.image2,
                                      news_slug = n.slug
                                  }).Take(10).ToList();
            ViewBag.listtag = list;
            ViewBag.ListName = "Tin tức mới nhất";
            return View("News", GetNews(m => m.status == "1", page));
        }
        //Xử lý code get bài viết
        private IPagedList GetNews(Expression<Func<News, bool>> expr, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.category = db.ParentCategory.ToList();
            ViewBag.newscategory = db.ChildCategory.ToList();
            var list = db.News.Where(expr).OrderByDescending(m => m.news_id).ToPagedList(pageNumber, pageSize);
            return list;
        }
        //Chi tiết bài viết
        public ActionResult NewsDetail(string slug)
        {
            var news = db.News.Where(m => m.slug == slug && m.status=="1").OrderByDescending(m=>m.news_id).ToList().FirstOrDefault();
            ViewBag.recentnews = db.News.Where(m => m.slug != slug && m.status == "1").OrderByDescending(m => m.news_id).ToList().Take(5);
            ViewBag.hotnews  = db.News.Where(m => m.slug != slug && m.status == "1").OrderByDescending(m => m.ViewCount).ToList().Take(3);
            ViewBag.comment = db.NewsComments.Where(m => m.news_id == news.news_id && m.status == "1").ToList();
            ViewBag.countcomment = db.NewsComments.Where(m => m.news_id == news.news_id && m.status == "1").Count();
            ViewBag.newstags = db.NewsTags.Where(m => m.news_id == news.news_id).ToList();
            ViewBag.newsproducts = db.NewsProducts.Where(m => m.news_id == news.news_id ).ToList();
            List<News> relatedpost = db.News.Where(m => m.ChildCategory.ParentCategory.parentcategory_id == news.ChildCategory.ParentCategory.parentcategory_id && m.news_id != news.news_id &&m.status=="1").OrderByDescending(m=>m.news_id).ToList();
            ViewBag.relatedpost = relatedpost;
            ViewBag.newslistcomment = db.NewsComments;
            news.ViewCount++;
            db.SaveChanges();
            return View(news);
        }
    }
}