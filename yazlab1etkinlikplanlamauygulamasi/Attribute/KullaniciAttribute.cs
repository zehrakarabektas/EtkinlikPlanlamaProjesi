using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yazlab1etkinlikplanlamauygulamasi.Attribute
{
    public class KullaniciAttribute:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var userRole = httpContext.Session["KullaniciRole"]?.ToString();
                if (userRole == "Kullanici")
                {
                    return true;
                }
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/UygulamaGirisSayfasi");
        }
    }
}