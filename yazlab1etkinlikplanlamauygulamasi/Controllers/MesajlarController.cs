using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer;
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
    public class MesajlarController : Controller
    {
        KullanicilarManager kullanicilarManager = new KullanicilarManager(new EfKullanicilarDal());
        MesajlarManager mesajManager = new MesajlarManager(new EfMesajlarDal());
        BildirimManager bildirimManager=new BildirimManager(new EfBildirimDal());

        // GET: Mesajlar
        [AdminAttribute]
        public ActionResult AdminGeriBildirimMesajlari(int kullaniciID, string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            Session["KullaniciID"]=kullaniciId;

            var tumkullanicilar= kullanicilarManager.GetAll().Where(x=>x.KullaniciID!=kullaniciId).ToList();
            ViewBag.TumKullanicilar=tumkullanicilar;
            var kullanici= kullanicilarManager.GetKullaniciID(kullaniciID);
            ViewBag.Kullanicilar=kullanici;
            var mesajlar = mesajManager.GetAll().Where(m => m.AliciID == kullaniciID).OrderBy(m => m.GonderimZamani).ToList();

            return View(mesajlar);

        }
        [AdminAttribute]
        public ActionResult AdminBildirimMesajGonder(int kullaniciID, string girisYapanKullaniciAdi, string mesajMetni)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            if (!string.IsNullOrEmpty(mesajMetni))
            {
                var mesaj = new Mesajlar
                {
                    GondericiID = kullaniciId,
                    AliciID = kullaniciID,
                    MesajMetni = mesajMetni,
                    GonderimZamani = DateTime.Now
                };
                mesajManager.MesajEkle(mesaj);
                var mesajId = mesaj.MesajID;

                var bildirim = new Bildirimler
                {
                    KullaniciID = kullaniciID,
                    MesajID = mesajId,
                    Mesaj = $"{girisYapanKullaniciAdi} bir mesaj gönderdi.",
                    Durum = BildirimDurumu.Okunmadı,
                    Tarih = DateTime.Now
                };
                bildirimManager.BildiriEkle(bildirim);


                return RedirectToAction("AdminGeriBildirimMesajlari", new { kullaniciID = kullaniciID });
            }

            return View();
        }
        [AdminAttribute]
        public ActionResult AdminBildirimMesajSil(int mesajID, string girisYapanKullaniciAdi)
        {
            var mesaj = mesajManager.GetAll().Where(x => x.MesajID==mesajID).FirstOrDefault();
            if (mesaj != null)
            {
                mesajManager.MesajSil(mesaj);
                return RedirectToAction("AdminGeriBildirimMesajlari", new { kullaniciID = mesaj.AliciID });
            }
            return RedirectToAction("AdminGeriBildirimMesajlari", new { kullaniciID = mesaj.AliciID });
        }
        [KullaniciAttribute]
        public ActionResult GeriBidirim()
        {
            Context context = new Context();
            string girisYapanKullaniciAdi = (string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            Session["KullaniciID"]=kullaniciId;

            var tumkullanicilar = kullanicilarManager.GetAll().Where(x => x.KullaniciID != kullaniciId).ToList();
            ViewBag.TumKullanicilar = tumkullanicilar;

            var adminler = tumkullanicilar.Where(x => x.KullaniciRole== "Admin").Select(x => x.KullaniciID).ToList();
            ViewBag.Adminler = adminler;    
            var mesajlar = mesajManager.GetAll().Where(m => adminler.Contains(m.AliciID))
                                              .OrderBy(m => m.GonderimZamani)
                                              .ToList();

            return View(mesajlar);
        }
        [KullaniciAttribute]
        public ActionResult GeriBildirimMesajGonder(List<Kullanicilar> adminler,string girisYapanKullaniciAdi, string mesajMetni)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            if (!string.IsNullOrEmpty(mesajMetni))
            {
               
                foreach (var adminId in adminler) 
                {
                    var mesaj = new Mesajlar
                    {
                        GondericiID = kullaniciId,
                        AliciID = adminId.KullaniciID,
                        MesajMetni = mesajMetni,
                        GonderimZamani = DateTime.Now
                    };
                    mesajManager.MesajEkle(mesaj); 
                    var mesajId = mesaj.MesajID;

                    var bildirim = new Bildirimler
                    {
                        KullaniciID = adminId.KullaniciID,
                        MesajID = mesajId,
                        Mesaj = $"{girisYapanKullaniciAdi} bir mesaj gönderdi.",
                        Durum = BildirimDurumu.Okunmadı,
                        Tarih = DateTime.Now
                    };
                    bildirimManager.BildiriEkle(bildirim); 
                }

                return RedirectToAction("GeriBidirim");
            }

            return View();

        }
    }
}