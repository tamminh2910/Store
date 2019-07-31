using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Store
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["SoNguoiGheTham"] = 0;
            Application["SoNguoiOnline"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["SoNguoiGheTham"] = (int)Application["SoNguoiGheTham"] + 1;
            Application["SoNguoiOnline"] = (int)Application["SoNguoiOnline"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();
            Application["SoNguoiOnline"] = (int)Application["SoNguoiOnline"] - 1;
            Application.UnLock();

        }
        protected void Application_End()
        {
            Application.Lock();
            Application["SoNguoiOnline"] = (int)Application["SoNguoiOnline"] - 1;
            Application.UnLock();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs args)
        {
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var roles = authTicket.UserData.Split(new char[] { ',' });
                var userPricipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                Context.User = userPricipal;
            }
        }
    }
}
