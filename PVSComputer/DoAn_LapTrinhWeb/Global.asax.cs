using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;


namespace DoAn_LapTrinhWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //dòng này được thêm vào để sử dụng messages toastr xem  file notification.cs
            Application["Notification"] = "";
            Application["VisitorCounter"] = 0;
            Application["VisitorOnline"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["VisitorCounter"] = (int)Application["VisitorCounter"] + 1;
            Application["VisitorOnline"] = (int)Application["VisitorOnline"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();
            Application["VisitorOnline"] = (int)Application["VisitorOnline"] - 1;
            Application.UnLock();
        }

    }
}