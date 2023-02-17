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
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class Product_ImageController : Controller
    {
        private readonly DbContext db = new DbContext();
        //View list ảnh sản phẩm
        public ActionResult ProductImgIndex(int? page,int? size,string search,string show,string sortOrder)
        {
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            var list = from a in db.Products
                       join c in db.Genres on a.genre_id equals c.genre_id
                       join i in db.Product_Images on a.product_id equals i.product_id
                       join e in db.Discounts on a.disscount_id equals e.disscount_id
                       where (a.status == "1" || a.status == "0")
                       orderby i.update_at ascending // giảm dần
                       select new ProductDTOs
                       {
                           product_img_status=i.status,
                           Image=a.image,
                           discount_name=e.discount_name,
                           genre_name=c.genre_name,
                           product_id=a.product_id,
                           product_name = a.product_name,
                           genre_id = c.genre_id,
                           discount_id = e.disscount_id,
                           discount_start=e.discount_start,
                           discount_end=e.discount_end,
                           discount_status=e.status,
                           price=a.price,
                           discount_price=e.discount_price,
                           product_img_id = i.product_image_id
                       };
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
            ViewBag.ImgSortParm = sortOrder == "image" ? "image" : "image";
            ViewBag.OneImgSortParm = sortOrder == "one_image" ? "one_image" : "one_image";
            ViewBag.TwoImgSortParm = sortOrder == "two_images" ? "two_images" : "two_images";
            ViewBag.ThreeImgSortParm = sortOrder == "three_images" ? "three_images" : "three_images";
            ViewBag.FourImgSortParm = sortOrder == "four_images" ? "four_images" : "four_images";
            switch (sortOrder)
            {
                case "date_desc":
                    list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join i in db.Product_Images on a.product_id equals i.product_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" || a.status == "0")
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_img_status = i.status,
                                   Image = a.image,
                                   discount_name = e.discount_name,
                                   genre_name = c.genre_name,
                                   product_id = a.product_id,
                                   product_name = a.product_name,
                                   genre_id = c.genre_id,
                                   discount_id = e.disscount_id,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_status = e.status,
                                   price = a.price,
                                   discount_price = e.discount_price,
                                   product_img_id = i.product_image_id
                               };
                    break;
                case "date_asc":
                     list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join i in db.Product_Images on a.product_id equals i.product_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" || a.status == "0")
                               orderby a.product_id ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_img_status = i.status,
                                   Image = a.image,
                                   discount_name = e.discount_name,
                                   genre_name = c.genre_name,
                                   product_id = a.product_id,
                                   product_name = a.product_name,
                                   genre_id = c.genre_id,
                                   discount_id = e.disscount_id,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_status = e.status,
                                   price = a.price,
                                   discount_price = e.discount_price,
                                   product_img_id = i.product_image_id
                               };
                    break;
                case "image":
                    list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join i in db.Product_Images on a.product_id equals i.product_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 != null && i.image_5 != null)
                           orderby i.product_id descending // giảm dần
                           select new ProductDTOs
                           {
                               product_img_status = i.status,
                               Image = a.image,
                               discount_name = e.discount_name,
                               genre_name = c.genre_name,
                               product_id = a.product_id,
                               product_name = a.product_name,
                               genre_id = c.genre_id,
                               discount_id = e.disscount_id,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_status = e.status,
                               price = a.price,
                               discount_price = e.discount_price,
                               product_img_id = i.product_image_id
                           };
                    break;
                case "one_image":
                     list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join i in db.Product_Images on a.product_id equals i.product_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where ((a.status == "1" || a.status == "0") && i.image_1!= null && i.image_2 == null && i.image_3 == null && i.image_4 == null && i.image_5 == null)
                               orderby i.update_at ascending // giảm dần
                                select new ProductDTOs
                               {
                                   product_img_status = i.status,
                                   Image = a.image,
                                   discount_name = e.discount_name,
                                   genre_name = c.genre_name,
                                   product_id = a.product_id,
                                   product_name = a.product_name,
                                   genre_id = c.genre_id,
                                   discount_id = e.disscount_id,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_status = e.status,
                                   price = a.price,
                                   discount_price = e.discount_price,
                                   product_img_id = i.product_image_id
                               };
                    break;
                case "two_images":
                    list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join i in db.Product_Images on a.product_id equals i.product_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 == null && i.image_4 == null && i.image_5 == null)
                           orderby i.update_at ascending // giảm dần
                           select new ProductDTOs
                           {
                               product_img_status = i.status,
                               Image = a.image,
                               discount_name = e.discount_name,
                               genre_name = c.genre_name,
                               product_id = a.product_id,
                               product_name = a.product_name,
                               genre_id = c.genre_id,
                               discount_id = e.disscount_id,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_status = e.status,
                               price = a.price,
                               discount_price = e.discount_price,
                               product_img_id = i.product_image_id
                           };
                    break;
                case "three_images":
                    list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join i in db.Product_Images on a.product_id equals i.product_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 == null && i.image_5 == null)
                           orderby i.update_at ascending // giảm dần
                           select new ProductDTOs
                           {
                               product_img_status = i.status,
                               Image = a.image,
                               discount_name = e.discount_name,
                               genre_name = c.genre_name,
                               product_id = a.product_id,
                               product_name = a.product_name,
                               genre_id = c.genre_id,
                               discount_id = e.disscount_id,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_status = e.status,
                               price = a.price,
                               discount_price = e.discount_price,
                               product_img_id = i.product_image_id
                           };
                    break;
                case "four_images":
                    list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join i in db.Product_Images on a.product_id equals i.product_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 != null && i.image_5 == null)
                           orderby i.update_at ascending // giảm dần
                           select new ProductDTOs
                           {
                               product_img_status = i.status,
                               Image = a.image,
                               discount_name = e.discount_name,
                               genre_name = c.genre_name,
                               product_id = a.product_id,
                               product_name = a.product_name,
                               genre_id = c.genre_id,
                               discount_id = e.disscount_id,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_status = e.status,
                               price = a.price,
                               discount_price = e.discount_price,
                               product_img_id = i.product_image_id
                           };
                    break;
                default:  
                    list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join i in db.Product_Images on a.product_id equals i.product_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where (a.status == "1" || a.status == "0")
                           orderby i.update_at ascending  // giảm dần
                           select new ProductDTOs
                           {
                               product_img_status = i.status,
                               Image = a.image,
                               discount_name = e.discount_name,
                               genre_name = c.genre_name,
                               product_id = a.product_id,
                               product_name = a.product_name,
                               genre_id = c.genre_id,
                               discount_id = e.disscount_id,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_status = e.status,
                               price = a.price,
                               discount_price = e.discount_price,
                               product_img_id = i.product_image_id
                           };
                    break;
            }
            if (!string.IsNullOrEmpty(search))
            {
                if (show.Equals("1"))//tìm kiếm tất cả
                    list = list.Where(s => s.product_name.Trim().Contains(search) ||
                    s.product_id.ToString().Trim().Contains(search) || s.genre_id.ToString().Contains(search)
                     || s.discount_id.ToString().Contains(search) || s.product_img_id.ToString().Contains(search));
                else if (show.Equals("2"))//theo id img
                    list = list.Where(s => s.product_name.Contains(search));
                else if (show.Equals("3"))//theo id product
                    list = list.Where(s => s.price.ToString().Contains(search));
                else if (show.Equals("4"))//theo tên sản phẩm
                    list = list.Where(s => s.product_id.ToString().Trim().Contains(search));
                else if (show.Equals("5"))//theo id thể loại
                    list = list.Where(s => s.discount_id.ToString().Contains(search));
                else if (show.Equals("6"))//theo id discount
                    return View("ProductImgIndex", list.ToPagedList(pageNumber, 50));
            }
            return View(list.ToPagedList(pageNumber, pageSize));

            //var product_Images = db.Product_Images.Include(p => p.Product);
            //return View(product_Images.ToList());
        }
        //View Chỉnh sửa ảnh sản phẩm
        public ActionResult ProductImgEdit(int? id,string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("ProductImgEdit", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            var productimg = db.Product_Images.Where(m => m.product_id == id).FirstOrDefault();
            if (productimg == null)
            {
                return HttpNotFound();
            }
            return View(productimg);
        }
        //Code xử lý Chỉnh sửa ảnh sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductImgEdit(Product_Image product_Image, string returnUrl,int id)
        {
            var productimg = db.Product_Images.Where(m => m.product_id == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                productimg.image_1 = product_Image.image_1;
                productimg.image_2 = product_Image.image_2;
                productimg.image_3 = product_Image.image_3;
                productimg.image_4 = product_Image.image_4;
                productimg.image_5 = product_Image.image_5;
                productimg.update_at = DateTime.Now;
                productimg.update_by = User.Identity.GetEmail();
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công:" +product_Image.product_id, "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ProductImgIndex");
            }
            return View(product_Image);
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
