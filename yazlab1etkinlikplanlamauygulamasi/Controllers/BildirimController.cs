using BusinessLayer;
using DataAccessLayer;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    [BildirimFiltre]
    public class BildirimController : Controller
    {
        BildirimManager bildirimManager = new BildirimManager(new EfBildirimDal());
        KullanicilarManager kullaniciManager=new KullanicilarManager(new EfKullanicilarDal());
        // GET: Bildirim
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Bildirimler(string girisYapanKullaniciAdi)
        {
            var kullanici = kullaniciManager.GetAll().Where(x => x.KullaniciAdi == girisYapanKullaniciAdi).FirstOrDefault();
            var bildirimler = bildirimManager.GetAll()
                .Where(b => b.KullaniciID == kullanici.KullaniciID && b.Durum == BildirimDurumu.Okunmadı)
                .OrderByDescending(b => b.Tarih)
                .ToList();

            ViewBag.Bildirimler = bildirimler;
            ViewBag.BildirimSay = bildirimler.Count();

            return View();
        }

    }
}