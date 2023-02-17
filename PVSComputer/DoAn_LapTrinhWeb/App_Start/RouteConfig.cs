    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn_LapTrinhWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //rút gọn link tìm kiếm sản phẩm
            routes.MapRoute(
              name: "search",
              url: "search",
             defaults: new { Controller = "Products", action = "SearchResult" }
           );
            //------------------------- start rút gọn link chi tiết sản phẩm------------------

            //rút gọn link chi tiết sản phẩm phụ kiện
            routes.MapRoute(
              name: "products detail",
              url: "san-pham/{slug}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );

//------------------------- end rút gọn link chi tiết sản phẩm--------------------
//------------------------- start rút gọn link danh mục sản phẩm------------------
            //rút gọn link laptop
            routes.MapRoute(
              name: "danh muc san pham",
              url: "danh-muc/{slug}",
             defaults: new { Controller = "Products", action = "ListProduct" }
           );

            //------------------------- end rút gọn link danh mục sản phẩm------------------

            //rút gọn link giỏ hàng
            routes.MapRoute(
                name: "cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "ViewCart" }
            );

            //rút gọn link thanh toán giỏ hàng
            routes.MapRoute(
               name: "checkout",
               url: "thanh-toan",
               defaults: new { controller = "Cart", action = "Checkout" }
            );

            
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news all",
              url: "bai-viet-moi-nhat",
              defaults: new { controller = "News", action = "AllListNews" } 
           );

            //rút gọn link tin tức

            //rút gọn link search bài viêt
            routes.MapRoute(
              name: "search post",
              url: "tim-kiem-bai-viet",
              defaults: new { controller = "News", action = "SearchResult" }
           );
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news",
              url: "tin-tuc",
              defaults: new { controller = "News", action = "NewsIndex" }
           );
            //tac giả
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news author",
              url: "tin-tuc/tac-gia/{Name}",
              defaults: new { controller = "News", action = "ListNews" }
           );
            //rút gọn link chi tiết tin tức
            routes.MapRoute(
              name: "News detail",
              url: "tin-tuc/{slug}",
              defaults: new { controller = "News", action = "NewsDetail" }
           );
            //rút gọn link chi tiết tin tức
            routes.MapRoute(
              name: "list news tag",
              url: "tin-tuc/tags/{slug}",
              defaults: new { controller = "News", action = "NewsDetail" }
           );
           routes.MapRoute(
              name: "list tags",
              url: "tags/{slug}",
              defaults: new { controller = "News", action = "ListNewsTags" }
           );

            //rút gọn link danh dánh bài viết của loại tin
            routes.MapRoute(
              name: "list news category",
              url: "tin-tuc/chu-de/{slug}",
              defaults: new { controller = "News", action = "ListNews"} 
           );
            //rút gọn link loại tin của danh mục
            routes.MapRoute(
              name: "news category",
              url: "tin-tuc/danh-muc/{slug}",
              defaults: new { controller = "News", action = "ListNewsCategory" } 
           );


            //rút gọn link khuyến mãi
            routes.MapRoute(
              name: "promotion",
              url: "khuyen-mai",
              defaults: new { controller = "Discount", action = "Listbanner" }
           );

            //rút gọn link chi tiet sản phẩm khuyến mãi
            routes.MapRoute(
              name: "promotion detail",
              url: "khuyen-mai/{slug}",
              defaults: new { controller = "Discount", action = "Bannerdetail" }
           );

//------------------------- start rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------

            //rút gọn link đăng nhập
            routes.MapRoute(
             name: "signin",
              url: "dang-nhap",
              defaults: new { controller = "Account", action = "SignIn" }
           );
            //rút gọn link đăng ký
            routes.MapRoute(
                name: "registration",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );

            //rút gọn link quên mật khẩu
            routes.MapRoute(
              name: "forgotpassword",
              url: "quen-mat-khau",
              defaults: new { controller = "Account", action = "ForgotPassword" }
           );

            //thay đổi mật khẩu
            routes.MapRoute(
              name: "changepassword",
              url: "ca-nhan/doi-mat-khau/",
              defaults: new { controller = "Account", action = "ChangePassword" }
           );

            //rút gọn link thông tin cá nhân
            routes.MapRoute(
              name: "profile",
              url: "ca-nhan/thong-tin",
              defaults: new { controller = "Account", action = "Editprofile" }
           );

            //rút gọn link quản lý đơn hàng
            routes.MapRoute(
              name: "tracking orders",
              url: "ca-nhan/don-hang/",
              defaults: new { controller = "Account", action = "TrackingOrder" }
           );
            //cập nhật mật khẩu mới
            routes.MapRoute(
              name: "Reset password",
              url: "ca-nhan/cap-nhat-mat-khau",
              defaults: new { controller = "Account", action = "ResetPassword" }
           );

//------------------------- end rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------
            //gửi yêu cầu hồ trợ
            routes.MapRoute(
              name: "sent request",
              url: "ho-tro",
              defaults: new { controller = "Home", action = "SentRequest" }
           );
            //set error 404
            routes.MapRoute(
              name: "Page Not Found",
              url: "pagenotfoun",
              defaults: new { controller = "Home", action = "PageNotFound" }
           );
            //link mặc định khi khởi động
            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
