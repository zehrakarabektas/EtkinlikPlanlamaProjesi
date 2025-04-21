using BusinessLayer;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yazlab1etkinlikplanlamauygulamasi
{
    public class BildirimFiltre : ActionFilterAttribute
    {
        private readonly BildirimManager _bildirimManager;
        private readonly KullanicilarManager _kullaniciManager;

        public BildirimFiltre()
        {
            _bildirimManager = new BildirimManager(new EfBildirimDal());
            _kullaniciManager = new KullanicilarManager(new EfKullanicilarDal());
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var girisYapanKullaniciAdi = filterContext.HttpContext.Session["KullaniciAdi"]?.ToString();
           

            if (string.IsNullOrEmpty(girisYapanKullaniciAdi))
            {
                return;
            }

            var kullanici = _kullaniciManager.GetAll()
               .Where(x => x.KullaniciAdi == girisYapanKullaniciAdi)
               .FirstOrDefault();

            if (kullanici != null)
            {
                var bildirimsayisi = _bildirimManager.GetAll()
                    .Where(b => b.KullaniciID == kullanici.KullaniciID && b.Durum == BildirimDurumu.Okunmadı)
                    .Count();

                filterContext.Controller.ViewBag.BildirimSay = bildirimsayisi;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
