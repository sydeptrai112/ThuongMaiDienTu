using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class ProductsAdminController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View list sản phẩm
        public ActionResult ProductIndex(int? size, int? page, string product_name, string show, string sortOrder) // hiển thị tất cả sp online
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.countTrash = db.Products.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                ViewBag.countProductsAdmin = db.Products.Count(a => a.status != "2"); //  đếm tổng sp 
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                ViewBag.QuantitySortParm = sortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";
                ViewBag.GenreSortParm = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
                ViewBag.BrandSortParm = sortOrder == "brand_asc" ? "brand_desc" : "brand_asc";
                ViewBag.ViewSortParm = sortOrder == "view_desc" ? "view_desc" : "view_desc";
                ViewBag.BuySortParm = sortOrder == "buy_desc" ? "buy_desc" : "buy_desc";
                ViewBag.LaptopSortParm = sortOrder == "laptop_sort" ? "laptop_sort" : "laptop_sort";
                ViewBag.ComponentSortParm = sortOrder == "component_sort" ? "component_sort" : "component_sort";
                ViewBag.MonitorSortParm = sortOrder == "monitor_sort" ? "monitor_sort" : "monitor_sort";
                ViewBag.TableSortParm = sortOrder == "table_sort" ? "table_sort" : "table_sort";
                ViewBag.AccessorySortParm = sortOrder == "accessory_sort" ? "accessory_sort" : "accessory_sort";
                var list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join d in db.Brands on a.brand_id equals d.brand_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where a.status == "1"
                           orderby a.product_id descending
                           select new ProductDTOs
                           {
                               product_name = a.product_name,
                               quantity = a.quantity,
                               price = a.price,
                               Image = a.image,
                               genre_name = c.genre_name,
                               status = a.status,
                               brand_name = d.brand_name,
                               discount_name = e.discount_name,
                               discount_start = e.discount_start,
                               discount_end = e.discount_end,
                               discount_price = e.discount_price,
                               discount_status = e.status,
                               product_id = a.product_id,
                               slug=a.slug,
                           };
                //sắp xếp
                switch (sortOrder)
                {
                    case "date_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where a.status == "1"
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "date_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where a.status == "1"
                               orderby a.product_id ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;

                    case "laptop_sort":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" && a.Genre.ParentGenres.id==2 )
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "component_sort":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" && a.Genre.ParentGenres.id == 4)
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "monitor_sort":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" && a.Genre.ParentGenres.id == 10)
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "table_sort":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" && a.Genre.ParentGenres.id == 8)
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "accessory_sort":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" && a.Genre.ParentGenres.id == 3)
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "id_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.product_id ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "price_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby (a.price - a.Discount.discount_price) ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "price_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby (a.price - a.Discount.discount_price) descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "buy_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.buyturn descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "view_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.view descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "quantity_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.quantity ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "quantity_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.quantity descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "genre_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.Genre.genre_name ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "genre_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.Genre.genre_name descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "brand_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.Brand.brand_name ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    case "brand_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.Brand.brand_name descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                        break;
                    default:  // Name ascending 
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1")
                               orderby a.product_id descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   product_id = a.product_id,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   slug = a.slug,
                               };
                        break;
                }
                //search filter
                if (!string.IsNullOrEmpty(product_name))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.product_name.Trim().Contains(product_name) ||
                        s.product_id.ToString().Trim().Contains(product_name) || s.genre_name.Trim().Contains(product_name)
                         || s.quantity.Trim().ToString().Contains(product_name) || s.brand_name.ToString().Trim().Contains(product_name) ||
                        s.price.ToString().Trim().Contains(product_name));
                    else if (show.Equals("2"))//theo tên sản phẩm
                        list = list.Where(s => s.product_name.Contains(product_name));
                    else if (show.Equals("3"))//theo giá sản phẩm
                        list = list.Where(s => s.price.ToString().Contains(product_name));
                    else if (show.Equals("4"))//theo id sản phẩm
                        list = list.Where(s => s.product_id.ToString().Contains(product_name));
                    else if (show.Equals("5"))//theo thương hiệu
                        list = list.Where(s => s.brand_name.Trim().Contains(product_name));
                    else if (show.Equals("6"))//theo thể loại
                        list = list.Where(s => s.genre_name.Trim().Contains(product_name));
                    else if (show.Equals("7"))//theo ct giảm giá
                        return View("ProductIndex", list.ToPagedList(pageNumber, 50));
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
        public JsonResult GetProductSearch(string Prefix)
        {
            var search = (from c in db.Products
                          where c.product_name.Contains(Prefix)
                          orderby c.product_name ascending
                          select new { c.product_name, c.slug, c.image, c.price, c.Discount.discount_price });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //View list trash sản phẩm
        public ActionResult ProductTrash(string product_name, string show, int? size, int? page, string sortOrder) // hiển thị tất cả sp online
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_asc" : "";
                ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                ViewBag.QuantitySortParm = sortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";
                ViewBag.GenreSortParm = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
                ViewBag.BrandSortParm = sortOrder == "brand_asc" ? "brand_desc" : "brand_asc";
                ViewBag.ViewSortParm = sortOrder == "view_desc" ? "view_desc" : "view_desc";
                ViewBag.BuySortParm = sortOrder == "buy_desc" ? "buy_desc" : "buy_desc";
                var list = from a in db.Products
                           join c in db.Genres on a.genre_id equals c.genre_id
                           join d in db.Brands on a.brand_id equals d.brand_id
                           join e in db.Discounts on a.disscount_id equals e.disscount_id
                           where a.status == "0"
                           orderby a.update_at descending // giảm dần
                           select new ProductDTOs
                           {
                               product_name = a.product_name,
                               quantity = a.quantity,
                               price = a.price,
                               Image = a.image,
                               update_at = a.update_at,
                               genre_name = c.genre_name,
                               brand_name = d.brand_name,
                               status = a.status,
                               product_id = a.product_id,
                               discount_name = e.discount_name,
                               count_Banner_detail=a.Banner_Detail.Count,
                               count_Order_detail=a.Oder_Detail.Count,
                               count_feedback=a.Feedbacks.Count

                           };
                switch (sortOrder)
                {
                    case "id_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.product_id ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "price_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby (a.price - a.Discount.discount_price) ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "price_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby (a.price - a.Discount.discount_price) descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "buy_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.buyturn descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "view_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.view descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "quantity_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.quantity ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "quantity_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.quantity descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "genre_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.Genre.genre_name ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "genre_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.Genre.genre_name descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "brand_asc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.Brand.brand_name ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    case "brand_desc":
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.Brand.brand_name descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   product_id = a.product_id,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                    default:  // Name ascending 
                        list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "0")
                               orderby a.update_at descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   genre_name = c.genre_name,
                                   status = a.status,
                                   brand_name = d.brand_name,
                                   product_id = a.product_id,
                                   discount_name = e.discount_name,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_status = e.status,
                                   count_Banner_detail = a.Banner_Detail.Count,
                                   count_Order_detail = a.Oder_Detail.Count,
                                   count_feedback = a.Feedbacks.Count
                               };
                        break;
                }
                if (!string.IsNullOrEmpty(product_name))
                {
                    if (show.Equals("1"))//tìm kiếm tất cả
                        list = list.Where(s => s.product_name.Trim().Contains(product_name) ||
                        s.product_id.ToString().Trim().Contains(product_name) || s.genre_name.Trim().Contains(product_name)
                         || s.quantity.Trim().ToString().Contains(product_name) || s.brand_name.ToString().Trim().Contains(product_name) ||
                        s.price.ToString().Trim().Contains(product_name));
                    else if (show.Equals("2"))//theo tên sản phẩm
                        list = list.Where(s => s.product_name.ToString().Trim().Contains(product_name));
                    else if (show.Equals("3"))//theo giá sản phẩm
                        list = list.Where(s => s.price.ToString().Contains(product_name));
                    else if (show.Equals("4"))//theo id sản phẩm
                        list = list.Where(s => s.product_id.ToString().Trim().Contains(product_name));
                    else if (show.Equals("5"))//theo thương hiệu
                        list = list.Where(s => s.brand_name.Trim().Contains(product_name));
                    else if (show.Equals("6"))//theo thể loại
                        list = list.Where(s => s.genre_name.Trim().Contains(product_name));
                    else if (show.Equals("7"))//theo ct giảm giá
                        return View("ProductTrash", list.ToPagedList(pageNumber, 50));
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");

            }
        }
        //Thông tin sản phẩm
        public ActionResult ProductDetails(int? id)
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                var product = (from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join d in db.Brands on a.brand_id equals d.brand_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               join i in db.Product_Images on a.product_id equals i.product_id
                               where a.product_id == id
                               orderby a.create_at descending // giảm dần
                               select new ProductDTOs
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   image_1 = i.image_1,
                                   image_2 = i.image_2,
                                   image_3 = i.image_3,
                                   image_4 = i.image_4,
                                   image_5 = i.image_5,
                                   genre_name = c.genre_name,
                                   brand_name = d.brand_name,
                                   product_id = a.product_id,
                                   description = a.description,
                                   specification = a.specification,
                                   parent_genre_name=c.ParentGenres.name,
                                   create_at = a.create_at,
                                   create_by = a.create_by,
                                   status = a.status,
                                   seo_title = a.title_seo,
                                   buyturn = a.buyturn,
                                   view = a.view,
                                   update_at = a.update_at,
                                   update_by = a.updateby,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_price = e.discount_price,
                                   discount_name = e.discount_name,
                                   discount_status = e.status,
                                   discount_id = e.disscount_id,
                                   product_img_id = i.product_image_id,
                                   genre_id = c.genre_id,
                                   brand_id = d.brand_id
                               }).FirstOrDefault();
                ViewBag.countfeedback = db.Feedbacks.Count(a => a.status != "2" && a.product_id == id); //  đếm tổng đánh giá
                if (product == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + product.product_name + "", "warning");
                    return RedirectToAction("ProductIndexProductIndex");
                }
                return View(product);
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //View thêm sản phẩm
        public ActionResult ProductCreate()
        {
            if (User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2")
            {
                ViewBag.ListParentGenre = new SelectList(db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name), "id", "name", 0);
                ViewBag.ListDiscount = new SelectList(db.Discounts.Where(m => ((m.status == "1" || m.status == "0") && m.discounts_type == 1)).OrderByDescending(m => m.disscount_id), "disscount_id", "discount_name", 0);
                ViewBag.ListBrand = new SelectList(db.Brands.Where(m => (m.status == "1" || m.status == "0")).OrderBy(m => m.brand_name), "brand_id", "brand_name", 0);
                ViewBag.ListGenre = new SelectList(db.Genres.Where(m => (m.status == "1" || m.status == "0")).OrderBy(m => m.genre_name), "genre_id", "genre_name", 0);
                return View();
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Code xử lý thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ProductCreate(ProductDTOs Prodtos, Product product, Product_Image product_image)
        {
            ViewBag.ListParentGenre = new SelectList(db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name), "id", "name", 0);
            ViewBag.ListDiscount = new SelectList(db.Discounts.Where(m => ((m.status == "1" || m.status == "0") && m.discounts_type == 1)).OrderByDescending(m => m.create_at), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => (m.status == "1" || m.status == "0")).OrderBy(m => m.brand_name), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => (m.status == "1" || m.status == "0")).OrderBy(m => m.genre_name), "genre_id", "genre_name", 0);
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(Prodtos.slug);
            try
            {
                var checkslug = db.Products.Any(m => m.slug == slug);
                if (checkslug)
                {
                    product.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                }
                else
                {
                    product.slug = SlugGenerator.SlugGenerator.GenerateSlug(Prodtos.slug);
                }
                product.image = product.image;
                //add data vào table product
                product.brand_id = Prodtos.brand_id;
                product.disscount_id = Prodtos.discount_id;
                product.genre_id = Prodtos.genre_id;
                product.status = Prodtos.status;
                product.title_seo = Prodtos.seo_title;
                product.view = 0;
                product.specification = Prodtos.specification;
                product.description = Prodtos.description;
                product.buyturn = 0;
                product.create_at = DateTime.Now;
                product.update_at = DateTime.Now;
                product.create_by = User.Identity.GetEmail();
                product.updateby = User.Identity.GetEmail();
                db.Products.Add(product);
                db.SaveChanges();
                //add data vào table product_image
                int product_id = product.product_id;
                product_image.product_id = product_id;
                product_image.genre_id = product.genre_id;
                product_image.discount_id = product.disscount_id;
                product_image.image_1 = Prodtos.image_1;
                product_image.image_2 = Prodtos.image_2;
                product_image.image_3 = Prodtos.image_3;
                product_image.image_4 = Prodtos.image_4;
                product_image.image_5 = Prodtos.image_5;
                product_image.create_at = DateTime.Now;
                product_image.update_at = DateTime.Now;
                product_image.create_by = User.Identity.GetEmail();
                product_image.update_by = User.Identity.GetEmail();
                product_image.status = product.status;
                db.Product_Images.Add(product_image);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + product.product_name + "", "success");
                    return RedirectToAction("ProductIndex");
            }
            catch
            {
                Notification.set_flash("Thêm sản phẩm không thành công", "danger");
            }

            return View(product);
        }
        //Thêm nhanh thể loại
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGenres(Genre genre, ProductDTOs product_DTOs)
        {
            try
            {
                genre.parent_genre_id = product_DTOs.parent_genre_id;
                genre.genre_name = product_DTOs.genre_name;
                genre.status = "1";
                genre.create_at = DateTime.Now;
                genre.create_by = User.Identity.GetEmail();
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                genre.genre_image = "/Images/ImagesCollection/no-image-available.png";
                db.Genres.Add(genre);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công thể loại: " + genre.genre_name + "", "success");
                return RedirectToAction("ProductCreate");
            }
            catch
            {
                Notification.set_flash("Thêm thể loại không thành công", "danger");
            }

            return View(genre);
        }
        //Thêm nhanh giảm giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDiscounts(Discount discount, ProductDTOs product_DTOs)
        {
            try
            {
                discount.discount_name = product_DTOs.discount_name;
                discount.create_at = DateTime.Now;
                discount.create_by = User.Identity.GetEmail();
                discount.update_at = DateTime.Now;
                discount.discounts_type = 1;
                discount.status = product_DTOs.discount_status;
                discount.discount_price = product_DTOs.discount_price;
                discount.discount_start = product_DTOs.discount_start;
                discount.discount_end = product_DTOs.discount_end;
                discount.update_by = User.Identity.GetEmail();
                db.Discounts.Add(discount);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công CTGG: " + discount.discount_name + "", "success");
                return RedirectToAction("ProductCreate");
            }
            catch
            {
                Notification.set_flash("Thêm mã giảm giá không thành công!", "danger");
            }
            return View(discount);
        }
        //Thêm nhanh thương hiệu
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBrands(Brand brand, ProductDTOs product_DTOs)
        {
            try
            {
                brand.brand_name = product_DTOs.brand_name;
                brand.create_at = DateTime.Now;
                brand.create_by = User.Identity.GetEmail();
                brand.brand_image = "/Images/ImagesCollection/no-image-available.png";
                brand.update_at = DateTime.Now;
                brand.status = "1";
                brand.update_by = User.Identity.GetEmail();
                db.Brands.Add(brand);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công thương hiệu: " + brand.brand_name + "", "success");
                return RedirectToAction("ProductCreate");
            }
            catch
            {
                Notification.set_flash("Thêm thương hiệu không thành công", "danger");
            }
            return View(brand);
        }
        //View chỉnh sửa thông tin sản phẩm
        public ActionResult ProductEdit(int? id, string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("ProductEdit", new { returnUrl = Request.UrlReferrer.ToString()});
            }
            var product = db.Products.FirstOrDefault(x => x.product_id == id);
            if ((User.Identity.GetRole() == "0" || User.Identity.GetRole() == "2") && (product.status == "1" || product.status == "0"))
            {
                ViewBag.countfeedback = db.Feedbacks.Count(a => a.status != "2" && a.product_id == id); //  đếm tổng đánh giá
                ViewBag.ListDiscount = new SelectList(db.Discounts.Where(m => m.status == "1" || m.status == "0"), "disscount_id", "discount_name", 0);
                ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status == "1" || m.status == "0").OrderBy(m => m.brand_name), "brand_id", "brand_name", 0);
                ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status == "1" || m.status == "0").OrderBy(m => m.genre_name), "genre_id", "genre_name", 0);
                if (product == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + product.product_name + "", "warning");
                    return RedirectToAction("ProductIndex");
                }
                return View(product);
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Code xử lý thông tin sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ProductEdit(Product productDtOs, string returnUrl)
        {
            ViewBag.ListDiscount = new SelectList(db.Discounts.Where(m => m.status != "2"), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status == "1" || m.status == "0").OrderBy(m => m.brand_name), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status == "1" || m.status == "0").OrderBy(m => m.genre_name), "genre_id", "genre_name", 0);
            var product = db.Products.SingleOrDefault(x => x.product_id == productDtOs.product_id);
            try
            {
                product.product_name = productDtOs.product_name;
                product.quantity = productDtOs.quantity;
                product.description = productDtOs.description;
                product.status = productDtOs.status;
                product.title_seo = productDtOs.title_seo;
                product.price = productDtOs.price;
                product.specification = productDtOs.specification;
                product.brand_id = productDtOs.brand_id;
                product.image = productDtOs.image;
                product.update_at = DateTime.Now;
                product.updateby = User.Identity.GetEmail();
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công: " + product.product_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ProductIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(productDtOs);
        }

        //Xóa sản phẩm
        [HttpPost]
        public JsonResult ProductDelete(int id)
        {
            string result;
            var product = db.Products.FirstOrDefault(m => m.product_id == id);
            var newsProducts = db.NewsProducts.FirstOrDefault(a => a.product_id == id);
            var product_image = db.Product_Images.FirstOrDefault(m => m.product_id == id);

            if (product.Oder_Detail.Count > 0)
            {
                result = "ExitOrder";
            }
            else if (product.Banner_Detail.Count > 0)
            {
                result = "ExitBanner";
            }
            else if (product.Feedbacks.Count > 0)
            {
                result = "ExitFeedback";
            }
            else
            {
                if (product.NewsProducts.Count>0)
                {
                    db.NewsProducts.Remove(newsProducts);
                    db.SaveChanges();
                }
                //xóa product image trước vì dính có id của product_id trong product_image
                if (product.Product_Images.Count > 0) {
                    db.Product_Images.Remove(product_image);
                    db.SaveChanges();
                }
                db.Products.Remove(product);
                db.SaveChanges();
                result = "Success";
                return Json(new { result, reload = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //thay đổi trạng thái sản phẩm
        [HttpPost]
        public JsonResult ChangeStatus(int id,int state = 0)
        {
            Product product = db.Products.Where(m=>m.product_id==id).FirstOrDefault();
            int title = product.product_id;
            product.status = state.ToString();
            string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
            product.update_at = DateTime.Now;
            product.updateby = User.Identity.GetEmail();
            db.SaveChanges();
            return Json(new { Message = "Đã chuyển trạng thái 'ID" + title + "' sang "+ prefix }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}