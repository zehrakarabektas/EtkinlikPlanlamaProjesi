using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    public class EtkinlikMesajController : Controller
    {
        EtkinlikMesajManager mesajManager = new EtkinlikMesajManager(new EfEtkinlikMesajDal());
        KullanicilarManager kullaniciManager = new KullanicilarManager(new EfKullanicilarDal());
        EtkinliklerManager etkinlikManager=new EtkinliklerManager(new EfEtkinliklerDal());
        BildirimManager bildirimManager=new BildirimManager(new EfBildirimDal());
        KatilimcilarManager katilimcilarManager = new KatilimcilarManager(new EfKatilimcilarDal());
        // GET: EtkinlikMesaj
        public ActionResult Index()
        {
            return View();
        }
        [KullaniciAttribute]
        public ActionResult EtkinlikMesajlari(int etkinlikId,string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            Session["KullaniciID"]=kullaniciId;

            var katilinilanetkinlikler = katilimcilarManager.GetAll().Where(x => x.KullaniciID == kullaniciId).Select(x => x.EtkinlikID).ToList(); 

            var  katildigietkinlikler= etkinlikManager.GetAll().Where(x => katilinilanetkinlikler.Contains(x.EtkinlikID)).ToList();
            ViewBag.KatilinanEtkinlikler=katildigietkinlikler;

            var kullanicilar = kullaniciManager.GetAll();
            ViewBag.Kullanicilar=kullanicilar;


            var mesajlar = mesajManager.GetAll().Where(m => m.EtkinlikID == etkinlikId).OrderBy(m => m.GonderimZamani).ToList();
            var etkinlikler = etkinlikManager.GetEtkinlikID(etkinlikId);
            ViewBag.Etkinlik=etkinlikler;
            return View(mesajlar);

        }
        [KullaniciAttribute]
        public ActionResult DuzenlenenEtkinliklerSohbeti(int etkinlikId, string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            Session["KullaniciID"]=kullaniciId;

            var duzenlenenetkinlikler =  etkinlikManager.GetDuzenleyenIDAll(kullaniciId);
            ViewBag.DuzenlenenEtkinlikler=duzenlenenetkinlikler;

            var kullanicilar = kullaniciManager.GetAll();
            ViewBag.Kullanicilar=kullanicilar;


            var mesajlar = mesajManager.GetAll().Where(m => m.EtkinlikID == etkinlikId).OrderBy(m => m.GonderimZamani).ToList();
            var etkinlikler = etkinlikManager.GetEtkinlikID(etkinlikId);
            ViewBag.Etkinlik=etkinlikler;
            return View(mesajlar);

        }
        [KullaniciAttribute]
        public ActionResult MesajGonder(int etkinlikId, string girisYapanKullaniciAdi, string mesajMetni)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            if (!string.IsNullOrEmpty(mesajMetni))
            {
                var etkinlikMesaj = new EtkinlikMesaj
                {
                    GondericiID = kullaniciId,
                    EtkinlikID = etkinlikId,
                    MesajMetni = mesajMetni,
                    GonderimZamani = DateTime.Now
                };
                mesajManager.EtkinlikMesajEkle(etkinlikMesaj);
                var mesajId = etkinlikMesaj.MesajID;
                var katilanlar = katilimcilarManager.GetAll().Where(x => x.EtkinlikID == etkinlikId && x.KullaniciID != kullaniciId).Select(x => x.KullaniciID).ToList();

                foreach (var katilan in katilanlar)
                {
                    var bildirim = new Bildirimler
                    {
                        KullaniciID = katilan,
                        MesajID = mesajId,
                        Mesaj = $"{girisYapanKullaniciAdi} bir mesaj gönderdi.",
                        Durum = BildirimDurumu.Okunmadı,
                        Tarih = DateTime.Now
                    };
                     bildirimManager.BildiriEkle(bildirim);
                }

               return RedirectToAction("EtkinlikMesajlari", new { etkinlikId = etkinlikId });
            }

            return View();
        }
        [KullaniciAttribute]
        public ActionResult MesajSil(int mesajID, string girisYapanKullaniciAdi)
        {
            var mesaj= mesajManager.GetAll().Where(x=>x.MesajID==mesajID).FirstOrDefault();
            if (mesaj != null)
            {
                mesajManager.EtkinlikMesajSil(mesaj);
                return RedirectToAction("EtkinlikMesajlari", new { etkinlikId = mesaj.EtkinlikID });
            }
            return RedirectToAction("EtkinlikMesajlari", new { etkinlikId = mesaj.EtkinlikID });
           

        }
        [KullaniciAttribute]
        public ActionResult DMesajGonder(int etkinlikId, string girisYapanKullaniciAdi, string mesajMetni)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            if (!string.IsNullOrEmpty(mesajMetni))
            {
                var etkinlikMesaj = new EtkinlikMesaj
                {
                    GondericiID = kullaniciId,
                    EtkinlikID = etkinlikId,
                    MesajMetni = mesajMetni,
                    GonderimZamani = DateTime.Now
                };
                mesajManager.EtkinlikMesajEkle(etkinlikMesaj);
                var mesajId = etkinlikMesaj.MesajID;
                var katilanlar = katilimcilarManager.GetAll().Where(x => x.EtkinlikID == etkinlikId && x.KullaniciID != kullaniciId).Select(x => x.KullaniciID).ToList();

                foreach (var katilan in katilanlar)
                {
                    var bildirim = new Bildirimler
                    {
                        KullaniciID = katilan,
                        MesajID = mesajId,
                        Mesaj = $"{girisYapanKullaniciAdi} bir mesaj gönderdi.",
                        Durum = BildirimDurumu.Okunmadı,
                        Tarih = DateTime.Now
                    };
                    bildirimManager.BildiriEkle(bildirim);
                }

                return RedirectToAction("DuzenlenenEtkinliklerSohbeti", new { etkinlikId = etkinlikId });
            }

            return View();
        }
        [KullaniciAttribute]
        public ActionResult DMesajSil(int mesajID, string girisYapanKullaniciAdi)
        {
            var mesaj = mesajManager.GetAll().Where(x => x.MesajID==mesajID).FirstOrDefault();
            if (mesaj != null)
            {
                mesajManager.EtkinlikMesajSil(mesaj);
                return RedirectToAction("DuzenlenenEtkinliklerSohbeti", new { etkinlikId = mesaj.EtkinlikID });
            }
            return RedirectToAction("DuzenlenenEtkinliklerSohbeti", new { etkinlikId = mesaj.EtkinlikID });


        }
        [AdminAttribute]
        public ActionResult AdminEtkinlikMesajlari(int etkinlikId, string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            Session["KullaniciID"]=kullaniciId;



            var tumetkinlikler = etkinlikManager.GetAll();
            ViewBag.TumEtkinlikler=tumetkinlikler;

            var kullanicilar = kullaniciManager.GetAll();
            ViewBag.Kullanicilar=kullanicilar;


            var mesajlar = mesajManager.GetAll().Where(m => m.EtkinlikID == etkinlikId).OrderBy(m => m.GonderimZamani).ToList();
            var etkinlikler = etkinlikManager.GetEtkinlikID(etkinlikId);
            ViewBag.Etkinlik=etkinlikler;
            return View(mesajlar);

        }
        [AdminAttribute]
        public ActionResult AMesajGonder(int etkinlikId, string girisYapanKullaniciAdi, string mesajMetni)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            if (!string.IsNullOrEmpty(mesajMetni))
            {
                var etkinlikMesaj = new EtkinlikMesaj
                {
                    GondericiID = kullaniciId,
                    EtkinlikID = etkinlikId,
                    MesajMetni = mesajMetni,
                    GonderimZamani = DateTime.Now
                };
                mesajManager.EtkinlikMesajEkle(etkinlikMesaj);
                var mesajId = etkinlikMesaj.MesajID;
                var katilanlar = katilimcilarManager.GetAll().Where(x => x.EtkinlikID == etkinlikId && x.KullaniciID != kullaniciId).Select(x => x.KullaniciID).ToList();

                foreach (var katilan in katilanlar)
                {
                    var bildirim = new Bildirimler
                    {
                        KullaniciID = katilan,
                        MesajID = mesajId,
                        Mesaj = $"{girisYapanKullaniciAdi} bir mesaj gönderdi.",
                        Durum = BildirimDurumu.Okunmadı,
                        Tarih = DateTime.Now
                    };
                    bildirimManager.BildiriEkle(bildirim);
                }

                return RedirectToAction("AdminEtkinlikMesajlari", new { etkinlikId = etkinlikId });
            }

            return View();
        }
        [AdminAttribute]
        public ActionResult AMesajSil(int mesajID, string girisYapanKullaniciAdi)
        {
            var mesaj = mesajManager.GetAll().Where(x => x.MesajID==mesajID).FirstOrDefault();
            if (mesaj != null)
            {
                mesajManager.EtkinlikMesajSil(mesaj);
                return RedirectToAction("AdminEtkinlikMesajlari", new { etkinlikId = mesaj.EtkinlikID });
            }
            return RedirectToAction("AdminEtkinlikMesajlari", new { etkinlikId = mesaj.EtkinlikID });


        }

    }
}