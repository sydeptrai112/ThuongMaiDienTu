using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BannersController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View List Banner
        public ActionResult BannerIndex(int? size,int? page,string search,string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = (size ?? 9);
                var pageNumber = (page ?? 1);
                ViewBag.countTrash = db.Banners.Count(a => a.status == "2");
                var list = from a in db.Banners
                           where (a.status == "1" || a.status == "0")
                           orderby a.banner_id descending
                           select a;
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search) || s.banner_name.Contains(search)
                        || s.create_by.Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên thể loại
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_name.Contains(search));
                    return View("BannerIndex", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View list Banner Trash
        public ActionResult BannerTrash(int? size, int? page, string search, string show)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = (size ?? 9);
                var pageNumber = (page ?? 1);
                var list = from a in db.Banners
                           where (a.status == "2")
                           orderby a.update_at descending
                           select a;
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search) || s.banner_name.Contains(search)
                        || s.create_by.Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên thể loại
                        list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_name.Contains(search));
                    return View("BannerTrash", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Thông tin chi tiết Banner
        public ActionResult BannerDetails(int? id)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại: " +banner.banner_name, "warning");
                return RedirectToAction("Index");
            }

            return View(banner);
        }
        //View Thêm khuyến mãi
        public ActionResult BannerCreate()
        {
            return View();
        }
        //Code xử lý thêm khuyến mãi
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BannerCreate(Banner banner)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(banner.banner_name);
            try
            {
                var checkslug = db.Banners.Any(m => m.slug == slug);
                if (checkslug)
                {
                    banner.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                }
                else
                {
                    banner.slug = SlugGenerator.SlugGenerator.GenerateSlug(banner.banner_name);
                }
                banner.banner_name = banner.banner_name;
                banner.image_thumbnail = banner.image_thumbnail;
                banner.banner_type = banner.banner_type;
                banner.description = banner.description;
                banner.banner_start = banner.banner_start;
                banner.banner_end = banner.banner_end;
                banner.create_by = User.Identity.GetEmail();
                banner.update_by = User.Identity.GetEmail();
                banner.update_at = DateTime.Now;
                banner.create_at = DateTime.Now;
                db.Banners.Add(banner);
                db.SaveChanges();

                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("BannerIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(banner);
        }
        //View Chỉnh sửa khuyến mãi
        public ActionResult BannerEdit(int? id,string returnUrl)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if ((User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2") && (banner.status == "1" || banner.status == "0"))
            {
                if (banner == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + banner.banner_name + "", "warning");
                    return RedirectToAction("BannerIndex");
                }
                return View(banner);
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Code xử lý Chỉnh sửa khuyến mãi
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BannerEdit(Banner banner)
        {
            try
            {
                banner.description = banner.description;
                banner.update_at = DateTime.Now;
                banner.update_by = User.Identity.GetEmail();
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công: " + banner.banner_name + "", "success");
                return RedirectToAction("BannerIndex");
            }
            catch
            {
                Notification.set_flash("cập nhật thất bại!", "danger");
            }
            return View(banner);
        }
        //Vô hiệu xóa khuyến mãi
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại: "+banner.banner_name, "warning");
                return RedirectToAction("BannerIndex");
            }
            banner.status = "2";
            banner.update_at = DateTime.Now;
            banner.update_by = User.Identity.GetEmail();
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Đã chuyển: "+ banner.banner_name+ " vào thùng rác", "success");
            return RedirectToAction("BannerIndex");
        }
        //Khôi phục khuyến mãi từ thùng rác
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("BannerIndex");
            }
            banner.status = "1";
            banner.update_at = DateTime.Now;
            banner.update_by = User.Identity.GetEmail();
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Khôi phục thành công: " + banner.banner_name , "success");
            return RedirectToAction("BannerTrash");
        }
        //Xóa khuyến mãi
        public ActionResult BannerDelete(int? id,string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("BannerDelete", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                if (banner == null)
                {
                    Notification.set_flash("Không tồn tại: " + banner.banner_name + "", "warning");
                    return RedirectToAction("BannerTrash");
                }
                return View(banner);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận xóa vĩnh viễn
        [HttpPost]
        [ActionName("BannerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string returnUrl)
        {
          
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                db.Banners.Remove(banner);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công: " + banner.banner_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("BannerTrash");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id, int state = 0)
        {
            Banner banner = db.Banners.Where(m => m.banner_id == id).FirstOrDefault();
            int title = banner.banner_id;
            banner.status = state.ToString();
            string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
            banner.update_at = DateTime.Now;
            banner.update_by = User.Identity.GetEmail();
            db.SaveChanges();
            return Json(new { Message = "Đã chuyển " + "ID" + title + " sang " + prefix }, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}