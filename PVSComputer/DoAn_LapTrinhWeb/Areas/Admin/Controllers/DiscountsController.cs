using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DiscountsController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        //View list giảm giá
        public ActionResult DiscountIndex(string search, string show, int? size, int? page,string sortOrder)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = (page ?? 1);
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.PriceSortParm = sortOrder == "price_desc" ? "price_asc" : "price_desc";
                ViewBag.ProductSortParm = sortOrder == "discount_product" ? "discount_product" : "discount_product";
                ViewBag.CodeSortParm = sortOrder == "discount_code" ? "discount_code" : "discount_code";
                ViewBag.countTrash = _db.Discounts.Count(a => a.status == "2");
                var list = from a in _db.Discounts
                           where (a.status == "1"||a.status == "0")
                           orderby a.disscount_id descending
                           select a;
                switch (sortOrder)
                {
                    case "date_desc":
                        list = from a in _db.Discounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.disscount_id descending
                               select a;
                        break;
                    case "date_asc":
                        list = from a in _db.Discounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.disscount_id ascending
                               select a;
                        break;
                    case "price_desc":
                        list = from a in _db.Discounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.discount_price descending
                               select a;
                        break;
                    case "price_asc":
                        list = from a in _db.Discounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.discount_price ascending
                               select a;
                        break;
                    case "discount_product":
                        list = from a in _db.Discounts
                               where ((a.status == "1" || a.status == "0") && a.discounts_type==1)
                               orderby a.disscount_id descending
                               select a;
                        break;
                    case "discount_code":
                        list = from a in _db.Discounts
                               where ((a.status == "1" || a.status == "0") && a.discounts_type == 2)
                               orderby a.disscount_id descending
                               select a;
                        break;
                    default:
                        list = from a in _db.Discounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.disscount_id descending
                               select a;
                        break;
                }
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.disscount_id.ToString().Contains(search) || s.discount_price.ToString().Contains(search)
                        || s.discount_name.Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.disscount_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên chương trình giảm giá
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.discount_name.Contains(search));
                    else if (show.Equals("4"))//theo mức giảm
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.discount_price.ToString().Contains(search));
                    return View("DiscountIndex", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View list trash giảm giá
        public ActionResult DiscountTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = (size ?? 10);
                var pageNumber = (page ?? 1);
                ViewBag.countTrash = _db.Discounts.Count(a => a.status == "0" && a.discounts_type==2);
                var list = from a in _db.Discounts
                           where a.status == "2"
                           orderby a.disscount_id descending
                           select a;
                if (!string.IsNullOrEmpty(search))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.disscount_id.ToString().Contains(search) || s.discount_price.ToString().Contains(search)
                        || s.discount_name.Contains(search));
                    else if (show.Equals("2"))//theo id
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.disscount_id.ToString().Contains(search));
                    else if (show.Equals("3"))//theo tên chương trình giảm giá
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.discount_name.Contains(search));
                    else if (show.Equals("4"))//theo mức giảm
                        list = (IOrderedQueryable<Discount>)list.Where(s => s.discount_price.ToString().Contains(search));
                    return View("DiscountTrash", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Thông tin giảm giá
        public ActionResult DiscountDetails(int? id)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
                if (discount == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + discount.discount_name + "", "warning");
                    return RedirectToAction("DiscountIndex");
                }
                return View(discount);
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View thêm giảm giá
        public ActionResult DiscountCreate()
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                return View();
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Code xử lý thêm giảm giá
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DiscountCreate(Discount discount)
        {
            try
            {
                discount.create_at = DateTime.Now;
                discount.create_by = User.Identity.GetEmail();
                discount.update_at = DateTime.Now;
                discount.discounts_type = discount.discounts_type;
                discount.status = discount.status;
                discount.discount_start = discount.discount_start;
                discount.discount_end = discount.discount_end;
                discount.update_by = User.Identity.GetEmail();
                _db.Discounts.Add(discount);
                _db.SaveChanges();
                Notification.set_flash("Thêm mới thành công chương trình giảm giá: " + discount.discount_name + "", "success");
                return RedirectToAction("DiscountIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi!", "danger");
            }
            return View(discount);
        }
        //View chỉnh sửa thông tin giảm giá
        public ActionResult DiscountEdit(int? id,string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("DiscountEdit", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if ((User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2") && (discount.status == "1" || discount.status == "0"))
            {
                if (discount != null && id != null) { return View(discount); }
                else { 
                Notification.set_flash("Không tồn tại: " + discount.discount_name + "", "warning");
                return RedirectToAction("DiscountIndex");
                }
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Code xử lý chỉnh sửa thông tin giảm giá
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DiscountEdit(Discount discount,string returnUrl)
        {
            try
            {
                discount.update_at = DateTime.Now;
                discount.update_by = User.Identity.GetEmail();
                discount.status = discount.status;
                discount.quantity = discount.quantity;
                discount.discounts_type = discount.discounts_type;
                discount.discount_price = discount.discount_price;
                discount.discounts_code = discount.discounts_code;
                discount.discount_end = discount.discount_end;
                _db.Entry(discount).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin: " + discount.discount_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("DiscountIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi!", "danger");
            }
            return View(discount);
        }
        //Vô hiệu hóa giảm giá
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
                if (discount == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + discount.discount_name + "", "warning");
                    return RedirectToAction("DiscountIndex");
                }
                discount.status = "2";
                discount.update_at = DateTime.Now;
                discount.update_by = User.Identity.GetEmail();
                _db.Entry(discount).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển: " + discount.discount_name + " vào thùng rác", "success");
                return RedirectToAction("DiscountIndex");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Khôi phục giảm giá
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
                if (discount == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + discount.discount_name + "", "warning");
                    return RedirectToAction("DiscountIndex");
                }
                discount.status = "1";
                discount.update_at = DateTime.Now;
                discount.update_by = User.Identity.GetEmail();
                _db.Entry(discount).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Khôi phục thành công: " + discount.discount_name + "", "success");
                return RedirectToAction("DiscountTrash");
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xóa giảm giá
        public ActionResult DiscountDelete(int? id, string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("DiscountDelete", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
                if (discount == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + discount.discount_name + "", "warning");
                    return RedirectToAction("DiscountTrash");
                }
                return View(discount);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Xác nhận xóa giảm giá
        [HttpPost]
        [ActionName("DiscountDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("DeleteConfirmed", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {

                var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
                _db.Discounts.Remove(discount);
                _db.SaveChanges();
                Notification.set_flash("Đã xoá vĩnh viễn: " + discount.discount_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("DiscountTrash");
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
            Discount discount = _db.Discounts.Where(m => m.disscount_id == id).FirstOrDefault();
            int title = discount.disscount_id;
            discount.status = state.ToString();
            string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
            discount.update_at = DateTime.Now;
            discount.update_by = User.Identity.GetEmail();
            _db.SaveChanges();
            return Json(new { Message = "Đã chuyển " + "ID" + title + " sang " + prefix }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}