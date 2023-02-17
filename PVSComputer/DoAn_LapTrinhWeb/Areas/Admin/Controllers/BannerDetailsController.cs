using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BannerDetailsController : Controller
    {
        private DbContext db = new DbContext();
        //View list sản phẩm của banner
        public ActionResult BannerDIndex(string search,string show,int ? size, int? page)
        {

            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.countTrash = db.Banner_Detail.Count(a => a.status == "2"); //  đếm tổng sp có trong thùng rác
                var list = from bd in db.Banner_Detail
                           join b in db.Banners on bd.banner_id equals b.banner_id
                           join p in db.Products on bd.product_id equals p.product_id
                           where (bd.status == "1" || bd.status == "0")
                           orderby bd.banner_id descending // giảm dần
                           select new BannerDTOs
                           {
                               product_slug=p.slug,
                               banner_id=bd.banner_id,
                               banner_name = b.banner_name,
                               banner_detail_id = bd.id,
                               image_thumbnail=b.image_thumbnail,
                               product_img=p.image,
                               product_id=p.product_id,
                               product_name=p.product_name,
                               bannerdetail_create_at=bd.create_at,
                               bannerdetail_status=bd.status,
                               bannerdetail_create_by=bd.create_by,
                               banner_start=b.banner_start,
                               banner_end=b.banner_end
                           };
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.banner_name.Contains(search) || s.banner_id.ToString().Contains(search) || s.product_id.ToString().Contains(search) || s.product_name.Contains(search));
                    else if (show.Equals("2"))//theo tên sản phẩm
                        list = list.Where(s => s.banner_name.Contains(search));
                    else if (show.Equals("3"))//theo giá sản phẩm
                        list = list.Where(s => s.product_name.Contains(search));
                        return View("BannerDIndex", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View thêm sản phẩm vào khuyến mãi
        public ActionResult BannerDCreate()
        {
            ViewBag.banner_id = new SelectList(db.Banners.Where(m => (m.status == "1" || m.status == "0")).OrderByDescending(m => m.banner_id), "banner_id", "banner_name", 0);
            ViewBag.product_id = new SelectList(db.Products.Where(m => (m.status == "1")).OrderBy(m => m.product_name), "product_id", "product_name", 0);
            return View();
        }
        //Code xử lý thêm sản phẩm vào khuyến mãi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BannerDCreate(Banner_Detail banner_detail)
        {
            try
            {
                var product = db.Products.Where(m=>banner_detail.product_id==m.product_id).SingleOrDefault();
                banner_detail.banner_id = banner_detail.banner_id;
                banner_detail.product_id = banner_detail.product_id;
                banner_detail.disscount_id = product.disscount_id;
                banner_detail.genre_id = product.genre_id;
                banner_detail.status = "1";
                banner_detail.create_by = User.Identity.GetEmail();
                banner_detail.create_at = DateTime.Now;
                db.Banner_Detail.Add(banner_detail);
                db.SaveChanges();
                var banner = db.Banners.Where(m => m.banner_id==banner_detail.banner_id).SingleOrDefault();
                banner.status = "1";
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: "+product.product_name +"-ID: "+banner_detail.banner_id, "success");
                return RedirectToAction("BannerDIndex");
            }
            catch
            {
                Notification.set_flash("CT khuyến mãi đã có sản phẩm này vui lòng chọn sản phẩm khác!", "danger");
            }
             ViewBag.banner_id = new SelectList(db.Banners.Where(m => (m.status == "1"|| m.status == "0")).OrderByDescending(m => m.banner_id), "banner_id", "banner_name", 0);
            ViewBag.product_id = new SelectList(db.Products.Where(m => (m.status == "1")).OrderBy(m => m.product_name), "product_id", "product_name", 0);
            return View();
        }
        //Xóa sản phẩm khỏi chương trình khuyến mãi
        public ActionResult BannerDDelete(int? id,string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("BannerDDelete", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var banner_detail = db.Banner_Detail.SingleOrDefault(a => a.id == id);
                if (banner_detail == null)
                {
                    Notification.set_flash("Không tồn tại: " + banner_detail.banner_id + "", "warning");
                    return RedirectToAction("BannerDIndex");
                }
                return View(banner_detail);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận xóa sản phẩm
        [HttpPost, ActionName("BannerDDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string returnUrl)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var banner_detail = db.Banner_Detail.SingleOrDefault(a => a.id == id);
                db.Banner_Detail.Remove(banner_detail);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công sản phẩm ID:" + banner_detail.product_id + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("BannerDIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
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
