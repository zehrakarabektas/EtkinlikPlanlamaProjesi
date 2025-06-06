﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace yazlab1etkinlikplanlamauygulamasi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest()
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["KullaniciAdi"] == null)
            {
                Response.Redirect("/Home/KullaniciGirisi");
            }
            if (HttpContext.Current.Session != null &&HttpContext.Current.Session["AdminAdi"] == null)
            {
                Response.Redirect("/Home/AdminGirisi");
            }
        }
    }
}
