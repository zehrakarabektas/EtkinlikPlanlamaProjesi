using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    [AdminAttribute]
    public class WebAdminController : Controller
    {
        KullanicilarManager kullaniciManager = new KullanicilarManager(new EfKullanicilarDal());
        KullaniciIlgiManager kullaniciIlgiManager = new KullaniciIlgiManager(new EfKullaniciIlgiAlanlariDal());
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        EtkinliklerManager etkinliklerManager = new EtkinliklerManager(new EfEtkinliklerDal());
        KatilimcilarManager katilimcilarManager = new KatilimcilarManager(new EfKatilimcilarDal());
        Sifreleme sifrele = new Sifreleme();
        WebAdminValidator adminValidator = new WebAdminValidator();
        KullanicilarValidator kullaniciValidator = new KullanicilarValidator();
        EtkinliklerValidator etkinlikValidator = new EtkinliklerValidator();
        KullaniciIlgiAlanlariValidator kullaniciIlgiValidator = new KullaniciIlgiAlanlariValidator();
        Sifreleme sifreSifreleme = new Sifreleme();
        PuanlarManager puanManager = new PuanlarManager(new EfPuanlarDal());
        EtkinlikMesajManager etkinlikmesajManager = new EtkinlikMesajManager(new EfEtkinlikMesajDal());

        // GET: WebAdmin
        public ActionResult AdminAnaSayfasi(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            var kullanici = kullaniciManager.GetKullaniciID(kullaniciId);
            if (kullanici == null)
            {
                return RedirectToAction("AdminGirisi", "Home");
            }
            Session["KullaniciAdi"] = kullanici.KullaniciAdi;
            Session["AdSoyad"] = kullanici.Ad + " " + kullanici.Soyad;
            Session["ProfilResmiUrl"] = "~"+kullanici.ProfilFotografi;

            ViewBag.KullaniciSayisi = kullaniciManager.GetAll().Count();
            ViewBag.EtkinlikSayisi = etkinliklerManager.GetAll().Count();
            ViewBag.IlgiAlanSayisi = ilgiAlaniManager.GetAll().Count();
            ViewBag.TumEtkinlikler= etkinliklerManager.GetAll().ToList();
            return View();
        }
        [HttpGet]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("UygulamaGirisSayfasi", "Home");
        }
        public ActionResult AdminKullanicilarListesi()
        {
            var kullanicilar = kullaniciManager.GetAll().Where(kullanici => kullanici.KullaniciRole == "Kullanici").ToList();
            List<IlgiAlanlari> ilgiAlanlari = ilgiAlaniManager.GetAll();
            ViewBag.IlgiAlanlari = ilgiAlanlari;
            return View(kullanicilar);
        }
        [HttpGet]
        public ActionResult AdminProfilimDuzenle(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            var kullanici = kullaniciManager.GetKullaniciID(kullaniciId);
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult AdminProfilimDuzenle(Kullanicilar kullanici)
        {
            ValidationResult validationResult = adminValidator.Validate(kullanici);
            if (!validationResult.IsValid)
            {
                foreach (var hata in validationResult.Errors)
                {
                    ModelState.AddModelError(hata.PropertyName, hata.ErrorMessage);
                }
                return View(kullanici);
            }
            if (Request.Files.Count>0)
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                string dosyaUzantisi = Path.GetExtension(Request.Files[0].FileName);
                string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                string yol = "~/Images/kullaniciProfilleri/" + benzersizDosyaAdi;

                Request.Files[0].SaveAs(Server.MapPath(yol));
                kullanici.ProfilFotografi="/Images/kullaniciProfilleri/"+benzersizDosyaAdi;
            }
            if (!string.IsNullOrEmpty(kullanici.Sifre))
            {
                kullanici.Sifre = sifrele.Sifrele(kullanici.Sifre);
            }
            else
            {
                var mevcutKullanici = kullaniciManager.GetKullaniciID(kullanici.KullaniciID);
                kullanici.Sifre = mevcutKullanici.Sifre;
            }

            kullaniciManager.KullaniciGuncelle(kullanici);

            return RedirectToAction("AdminAnaSayfasi");
        }

        [HttpGet]
        public ActionResult KullaniciGoruntule(int kullaniciId)
        {

            var kullanici = kullaniciManager.GetKullaniciID(kullaniciId);
            var ilgiAlanlari = ilgiAlaniManager.GetAll();
            ViewBag.IlgiAlanlari = ilgiAlanlari;
            var mevcutKullaniciIlgiAlanlari = kullaniciIlgiManager.GetKullaniciIlgi(kullaniciId);
            var secilenIlgiAlanlari = mevcutKullaniciIlgiAlanlari.Select(x => x.IlgiAlaniID).ToList();
            ViewBag.kullaniciIlgiAlanlari = secilenIlgiAlanlari;
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult KullaniciGoruntule(Kullanicilar kullanici, List<int> secilenIlgiAlanlari)
        {
            ValidationResult validationResult = kullaniciValidator.Validate(kullanici);
            ValidationResult validationResult1 = new ValidationResult();
            bool ilgiAlanlariValid = true;
            if (secilenIlgiAlanlari != null)
            {
                foreach (var ilgiAlaniId in secilenIlgiAlanlari)
                {
                    validationResult1 = kullaniciIlgiValidator.Validate(new KullaniciIlgiAlanlari { KullaniciID = kullanici.KullaniciID, IlgiAlaniID = ilgiAlaniId });

                    if (!validationResult1.IsValid)
                    {
                        ilgiAlanlariValid = false;
                        foreach (var error in validationResult1.Errors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
            }
            if (validationResult.IsValid && ilgiAlanlariValid)
            {
                if (!string.IsNullOrEmpty(kullanici.Sifre))
                {
                    kullanici.Sifre = sifrele.Sifrele(kullanici.Sifre);
                }
                else
                {
                    var mevcutKullanici = kullaniciManager.GetKullaniciID(kullanici.KullaniciID);
                    kullanici.Sifre = mevcutKullanici.Sifre;
                }
                if (Request.Files.Count>0)
                {
                    string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                    string dosyaUzantisi = Path.GetExtension(Request.Files[0].FileName);
                    string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                    string yol = "~/Images/kullaniciProfilleri/" + benzersizDosyaAdi;

                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    kullanici.ProfilFotografi="/Images/kullaniciProfilleri/"+benzersizDosyaAdi;
                }
                else
                {
                    var mevcutKullanici = kullaniciManager.GetKullaniciID(kullanici.KullaniciID);
                    kullanici.ProfilFotografi = mevcutKullanici.ProfilFotografi;
                }
                kullaniciManager.KullaniciGuncelle(kullanici);

                if (secilenIlgiAlanlari != null)
                {
                    var mevcutIlgiAlanlari = kullaniciIlgiManager.GetKullaniciIlgi(kullanici.KullaniciID);

                    if (mevcutIlgiAlanlari != null)
                    {
                        var silinecekIlgiAlanlari = mevcutIlgiAlanlari
                            .Where(ma => !secilenIlgiAlanlari.Contains(ma.IlgiAlaniID))
                            .ToList();

                        foreach (var kullaniciIlgiAlani in silinecekIlgiAlanlari)
                        {
                            kullaniciIlgiManager.KullaniciIlgiAlaniSil(kullaniciIlgiAlani);
                        }

                        var yeniIlgiAlanlari = secilenIlgiAlanlari
                            .Where(sa => !mevcutIlgiAlanlari.Any(ma => ma.IlgiAlaniID == sa))
                            .ToList();

                        foreach (var ilgiAlaniId in yeniIlgiAlanlari)
                        {
                            var yeniIlgiAlani = new KullaniciIlgiAlanlari
                            {
                                KullaniciID = kullanici.KullaniciID,
                                IlgiAlaniID = ilgiAlaniId
                            };
                            kullaniciIlgiManager.KullaniciIlgiAlaniEkle(yeniIlgiAlani);
                        }
                    }
                }

                return RedirectToAction("AdminKullanicilarListesi");
            }
            else
            {

                foreach (var item in validationResult.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                foreach (var item in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(item.ErrorMessage);
                }

                return View(kullanici);
            }
        }
        [HttpGet]
        public ActionResult KullaniciEkle()
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari=ilgiAlanlari;
            return View(new Kullanicilar());
        }
        [HttpPost]
        public ActionResult KullaniciEkle(Kullanicilar kullanici, int[] secilenIlgiAlanlari)
        {
            ValidationResult validationResult = kullaniciValidator.Validate(kullanici);
            if (validationResult.IsValid)
            {
                kullanici.Sifre = sifreSifreleme.Sifrele(kullanici.Sifre);
                kullanici.KullaniciRole="Kullanici";
                kullaniciManager.KullaniciEkle(kullanici);
                if (Request.Files.Count>0)
                {
                    string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                    string dosyaUzantisi = Path.GetExtension(Request.Files[0].FileName);
                    string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                    string yol = "~/Images/" + benzersizDosyaAdi;

                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    kullanici.ProfilFotografi="/Images/"+benzersizDosyaAdi;
                }
                if (secilenIlgiAlanlari != null)
                {
                    foreach (var ilgiAlaniId in secilenIlgiAlanlari)
                    {
                        var ilgiAlani = new KullaniciIlgiAlanlari
                        {
                            KullaniciID = kullanici.KullaniciID,
                            IlgiAlaniID = ilgiAlaniId
                        };

                        kullaniciIlgiManager.KullaniciIlgiAlaniEkle(ilgiAlani);
                    }
                }
                var bonusPuan = new Puanlar
                {
                    KullaniciID = kullanici.KullaniciID,
                    KazanilanTarih = DateTime.Now,
                    PuanTuru = PuanTuru.Bonus,

                };

                puanManager.PuanEkle(bonusPuan);

                return RedirectToAction("AdminKullanicilarListesi");
            }
            else
            {
                var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
                ViewBag.IlgiAlanlari=ilgiAlanlari;
                return View(new Kullanicilar());
            }

        }
        public ActionResult KullaniciSil(int kullaniciID)
        {
            try
            {
                var etkinlikKatilimlari = katilimcilarManager.GetKullaniciKatildigiEtkinliklerID(kullaniciID);
                foreach (var katilim in etkinlikKatilimlari)
                {
                    var katilimlari = new Katilimcilar
                    {
                        KullaniciID = kullaniciID,
                        EtkinlikID = katilim
                    };
                    katilimcilarManager.KatilimciSil(katilimlari); 
                }

                var ilgiAlanlari = kullaniciIlgiManager.GetKullaniciIlgi(kullaniciID);
                foreach (var ilgi in ilgiAlanlari)
                {
                    kullaniciIlgiManager.KullaniciIlgiAlaniSil(ilgi); 
                }

                var olusturduguEtkinlikler = etkinliklerManager.GetAll().Where(x => x.KullaniciID == kullaniciID).ToList();
                foreach (var etkinlik in olusturduguEtkinlikler)
                {
                    etkinliklerManager.EtkinlikSil(etkinlik); 
                }

                var etkinlikMesajlari = etkinlikmesajManager.GetAll().Where(x => x.GondericiID == kullaniciID).ToList();
                foreach (var mesaj in etkinlikMesajlari)
                {
                    etkinlikmesajManager.EtkinlikMesajSil(mesaj); 
                }

                var kullanici = kullaniciManager.GetKullaniciID(kullaniciID);
                kullaniciManager.KullaniciSil(kullanici); 

                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";

                return RedirectToAction("AdminKullanicilarListesi");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu: " + ex.Message;

                return RedirectToAction("AdminKullanicilarListesi");
            }
        }

    }
}