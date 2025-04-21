using BusinessLayer;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    public class KatilimcilarController : Controller
    {
        KatilimcilarManager katilimciManager = new KatilimcilarManager(new EfKatilimcilarDal());
        KullanicilarManager kullaniciManager=new KullanicilarManager(new EfKullanicilarDal());
       
        [AdminAttribute]
        public ActionResult GetKatilimcilar(int etkinlikID)
        {
            var katilimcilar = new List<Kullanicilar>();
            ViewBag.EtkinlikID = etkinlikID;

            var katilimcilarListesi = katilimciManager.GetKatilimciList(etkinlikID);
            foreach (var katilimci in katilimcilarListesi)
            {
                var kullanici = kullaniciManager.GetKullaniciID(katilimci);
                if (kullanici != null)
                {
                    katilimcilar.Add(kullanici);
                }
            }
            if (katilimcilarListesi == null || !katilimcilarListesi.Any())
            {
                Console.WriteLine("Katılımcı listesi boş.");
            }

            return View(katilimcilar);
        }
        [KullaniciAttribute]
        public ActionResult KullaniciDuzenlenenKatilimcilar(int etkinlikID)
        {
            var katilimcilar = new List<Kullanicilar>();
            ViewBag.EtkinlikID = etkinlikID;

            var katilimcilarListesi = katilimciManager.GetKatilimciList(etkinlikID);
            foreach (var katilimci in katilimcilarListesi)
            {
                var kullanici = kullaniciManager.GetKullaniciID(katilimci);
                if (kullanici != null)
                {
                    katilimcilar.Add(kullanici);
                }
            }
            if (katilimcilarListesi == null || !katilimcilarListesi.Any())
            {
                Console.WriteLine("Katılımcı listesi boş.");
            }

            return View(katilimcilar);
        }
        [KullaniciAttribute]
        public ActionResult KatilimciSil(int etkinlikID, int katilimciID)
        {
           
            var katilimci = new Katilimcilar
            {
                KullaniciID = katilimciID,
                EtkinlikID = etkinlikID
            };
            katilimciManager.KatilimciSil(katilimci);

            return RedirectToAction("KullaniciDuzenlenenKatilimcilar", new { etkinlikID = etkinlikID });
        }
        [KullaniciAttribute]
        public ActionResult KullaniciKatildigiEtkinlikKatilimcilar(int etkinlikID)
        {
            var katilimcilar = new List<Kullanicilar>();
            ViewBag.EtkinlikID = etkinlikID;

            var katilimcilarListesi = katilimciManager.GetKatilimciList(etkinlikID);
            foreach (var katilimci in katilimcilarListesi)
            {
                var kullanici = kullaniciManager.GetKullaniciID(katilimci);
                if (kullanici != null)
                {
                    katilimcilar.Add(kullanici);
                }
            }
            if (katilimcilarListesi == null || !katilimcilarListesi.Any())
            {
                Console.WriteLine("Katılımcı listesi boş.");
            }

            return View(katilimcilar);
        }
        [AdminAttribute]
        public ActionResult AdminKatilimciSil(int etkinlikID, int katilimciID)
        {

            var katilimci = new Katilimcilar
            {
                KullaniciID = katilimciID,
                EtkinlikID = etkinlikID
            };
            katilimciManager.KatilimciSil(katilimci);

            return RedirectToAction("GetKatilimcilar", new { etkinlikID = etkinlikID });
        }

    }
}