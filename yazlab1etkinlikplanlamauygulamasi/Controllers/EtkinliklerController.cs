using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer;
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
    public class EtkinliklerController : Controller
    {
        EtkinliklerManager etkinliklerManager = new EtkinliklerManager(new EfEtkinliklerDal());
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        KullanicilarManager kullanicilarManager = new KullanicilarManager(new EfKullanicilarDal());
        EtkinliklerValidator etkinlikValidator = new EtkinliklerValidator();
        KatilimcilarManager katilimcilarManager = new KatilimcilarManager(new EfKatilimcilarDal());
        EtkinlikMesajManager etkinlikMesajManager = new EtkinlikMesajManager(new EfEtkinlikMesajDal());
        // GET: Etkinlikler
        [AdminAttribute]
        public ActionResult EtkinliklerSayfasi(int kategoriID)
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x=>x.KategoriID==kategoriID);
            var etkinlikler = etkinliklerManager.GetAll().Where(x => ilgiAlanlari.Any(y => y.IlgiAlaniID == x.IlgiAlaniID)).ToList();

            var kullanicilar = kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlikler);
        }
        [KullaniciAttribute]
        public ActionResult KullaniciEtkinliklerSayfasi(int kategoriID)
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.KategoriID==kategoriID);
            var etkinlikler = etkinliklerManager.GetAll().Where(x => ilgiAlanlari.Any(y => y.IlgiAlaniID == x.IlgiAlaniID) && x.EtkinlikOnayDurumu==OnayDurumu.Onayli).ToList();
            var kullanicilar = kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlikler);
        }

        [AdminAttribute]
        public ActionResult AdminEtkinliklerListesi()
        {
            var etkinlikler = etkinliklerManager.GetAll().Where(x=>x.EtkinlikOnayDurumu==OnayDurumu.Onayli).ToList();
            var kullanicilar=kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar; 
            return View(etkinlikler);  
        }
        [AdminAttribute]
        public ActionResult OnayBekleyenEtkinlikListesi()
        {
            var etkinlik = etkinliklerManager.GetOnayBekleyenler();
            var kullanicilar = kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlik);
        }
        [AdminAttribute]
        public ActionResult ReddedilenEtkinlikListesi()
        {
            var etkinlik = etkinliklerManager.GetReddedilenler();
            var kullanicilar = kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlik);
        }
        [HttpGet]
        [AdminAttribute]
        public ActionResult AdminEtkinlikGoruntule(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            List<SelectListItem> IlgiAlanlari = (from x in ilgiAlaniManager.GetAll()
                                                 where x.OnaylanmaDurumu != OnayDurumu.OnayliDegil 
                                                 select new SelectListItem
                                                 {
                                                     Text = x.IlgiAlaniAdi,
                                                     Value = x.IlgiAlaniID.ToString(),
                                                     Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID)

                                                    
                                                 }).ToList();

            ViewBag.IlgiAlanlari = IlgiAlanlari;
            var seciliIlgiAlaniID = etkinlik?.IlgiAlaniID;  
            var seciliIlgiAlani = ilgiAlaniManager.GetAll().FirstOrDefault(x => x.IlgiAlaniID == seciliIlgiAlaniID);

            if (seciliIlgiAlani != null && seciliIlgiAlani.OnaylanmaDurumu != OnayDurumu.Onayli)
            {
                ViewBag.IlgiAlaniOnaysiz = true; 
            }
            else
            {
                ViewBag.IlgiAlaniOnaysiz = false;  
            }
            var kullanicilar = kullanicilarManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlik);
        }
        [HttpPost]
        [AdminAttribute]
        public ActionResult AdminEtkinlikGoruntule(Etkinlikler etkinlik, string SelectedIlgiAlaniID)
        {
            var mevcutEtkinlik = etkinliklerManager.GetEtkinlikID(etkinlik.EtkinlikID);

            ValidationResult result = etkinlikValidator.Validate(etkinlik);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                List<SelectListItem> IlgiAlanlari = (from x in ilgiAlaniManager.GetAll()
                                                     where x.OnaylanmaDurumu != OnayDurumu.OnayliDegil
                                                     select new SelectListItem
                                                     {
                                                         Text = x.IlgiAlaniAdi,
                                                         Value = x.IlgiAlaniID.ToString(),
                                                         Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID)


                                                     }).ToList();

                ViewBag.IlgiAlanlari = IlgiAlanlari;
                var seciliIlgiAlaniID = etkinlik?.IlgiAlaniID;
                var seciliIlgiAlani = ilgiAlaniManager.GetAll().FirstOrDefault(x => x.IlgiAlaniID == seciliIlgiAlaniID);

                if (seciliIlgiAlani != null && seciliIlgiAlani.OnaylanmaDurumu != OnayDurumu.Onayli)
                {
                    ViewBag.IlgiAlaniOnaysiz = true;
                }
                else
                {
                    ViewBag.IlgiAlaniOnaysiz = false;
                }
                var kullanicilar = kullanicilarManager.GetAll().ToList();
                ViewBag.Kullanicilar=kullanicilar;
                TempData["ErrorMessage"] = "Etkinlik güncellenirken bir hata oluştu. Lütfen tekrar deneyin.";

                return View(etkinlik);
            }
            int? ilgiAlaniID = null;

            if (!string.IsNullOrEmpty(SelectedIlgiAlaniID))
            {
                ilgiAlaniID = int.Parse(SelectedIlgiAlaniID);
            }

            if (ilgiAlaniID.HasValue)
            {
                etkinlik.IlgiAlaniID = ilgiAlaniID.Value;
            }
           
            else
            {
                etkinlik.IlgiAlaniID=mevcutEtkinlik.IlgiAlaniID;
                ViewBag.IlgiAlaniHataMesaji = "Lütfen bir ilgi alanı seçin veya yeni bir ilgi alanı girin.";
                List<SelectListItem> IlgiAlanlari = (from x in ilgiAlaniManager.GetAll()
                                                     where x.OnaylanmaDurumu != OnayDurumu.OnayliDegil
                                                     select new SelectListItem
                                                     {
                                                         Text = x.IlgiAlaniAdi,
                                                         Value = x.IlgiAlaniID.ToString(),
                                                         Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID)


                                                     }).ToList();

                ViewBag.IlgiAlanlari = IlgiAlanlari;
                var seciliIlgiAlaniID = etkinlik?.IlgiAlaniID;
                var seciliIlgiAlani = ilgiAlaniManager.GetAll().FirstOrDefault(x => x.IlgiAlaniID == seciliIlgiAlaniID);

                if (seciliIlgiAlani != null && seciliIlgiAlani.OnaylanmaDurumu != OnayDurumu.Onayli)
                {
                    ViewBag.IlgiAlaniOnaysiz = true;
                }
                else
                {
                    ViewBag.IlgiAlaniOnaysiz = false;
                }
                var kullanicilar = kullanicilarManager.GetAll().ToList();
                ViewBag.Kullanicilar=kullanicilar;
                TempData["ErrorMessage"] = "Etkinlik güncellenirken bir hata oluştu. Lütfen tekrar deneyin.";
                return View(etkinlik);
            }
            int? gelenId = etkinlik.IlgiAlaniID;
            Debug.WriteLine("Gelen ID: " + gelenId);
            string[] validExtensions = { ".png", ".jpg", ".jpeg", ".gif" };

            if (Request.Files.Count > 0)
            {
                string dosyaUzantisi = Path.GetExtension(Request.Files[0].FileName).ToLower();

                if (validExtensions.Contains(dosyaUzantisi))
                {
                    string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                    string yol = "~/Images/etkinliklerImages/" + benzersizDosyaAdi;

                    Request.Files[0].SaveAs(Server.MapPath(yol));

                    etkinlik.EtkinlikResmi = "/Images/etkinliklerImages/" + benzersizDosyaAdi;
                }
                else
                {
                    etkinlik.EtkinlikResmi = mevcutEtkinlik.EtkinlikResmi;
                }
            }
            else
            {
                etkinlik.EtkinlikResmi = mevcutEtkinlik.EtkinlikResmi;
            }

            etkinlik.KullaniciID=mevcutEtkinlik.KullaniciID;
            etkinliklerManager.EtkinlikGuncelle(etkinlik);
            TempData["SuccessMessage"] = "Etkinlik başarıyla güncellendi.";
            return RedirectToAction("AdminEtkinlikGoruntule",etkinlik);

        }
        [AdminAttribute]
        public ActionResult AdminEtkinlikOnayla(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            etkinlik.EtkinlikOnayDurumu=OnayDurumu.Onayli;
            etkinliklerManager.EtkinlikGuncelle(etkinlik);
            return RedirectToAction("AdminEtkinliklerListesi");
        }
        [AdminAttribute]
        public ActionResult AdminEtkinlikReddet(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            etkinlik.EtkinlikOnayDurumu=OnayDurumu.OnayliDegil;
            etkinliklerManager.EtkinlikGuncelle(etkinlik);
            return RedirectToAction("ReddedilenEtkinlikListesi");
        }
        [AdminAttribute]
        public ActionResult AdminEtkinlikOnayBekleme(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            etkinlik.EtkinlikOnayDurumu=OnayDurumu.OnayBekliyor;
            etkinliklerManager.EtkinlikGuncelle(etkinlik);
            return RedirectToAction("OnayBekleyenEtkinlikListesi");
        }
        [AdminAttribute]
        public ActionResult EtkinlikDetaySil(int etkinlikId)
        {
            try
            {
                var kullaniciIdleri = katilimcilarManager.GetKatilimciList(etkinlikId);
                foreach (var katilimci in kullaniciIdleri)
                {
                    var katilim = new Katilimcilar
                    {
                        EtkinlikID = etkinlikId,
                        KullaniciID = katilimci
                    };
                    katilimcilarManager.KatilimciSil(katilim);
                }

                var etkinlikMesajlari = etkinlikMesajManager.GetAll().Where(X => X.EtkinlikID == etkinlikId).ToList();
                foreach (var mesaj in etkinlikMesajlari)
                {
                    etkinlikMesajManager.EtkinlikMesajSil(mesaj);
                }

                var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikId);
                etkinliklerManager.EtkinlikSil(etkinlik);

                TempData["Message"] = "Etkinlik başarıyla silindi.";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Etkinlik silinemedi.";
                TempData["MessageType"] = "error";
            }
            return RedirectToAction("AdminAnaSayfasi", "WebAdmin");
        }
        [AdminAttribute]
        public ActionResult OnayliEtkinlikSil(int etkinlikId)
        {
            try
            {
                var kullaniciIdleri = katilimcilarManager.GetKatilimciList(etkinlikId);
                foreach (var katilimci in kullaniciIdleri)
                {
                    var katilim = new Katilimcilar
                    {
                        EtkinlikID = etkinlikId,
                        KullaniciID = katilimci
                    };
                    katilimcilarManager.KatilimciSil(katilim);
                }

                var etkinlikMesajlari = etkinlikMesajManager.GetAll().Where(X => X.EtkinlikID == etkinlikId).ToList();
                foreach (var mesaj in etkinlikMesajlari)
                {
                    etkinlikMesajManager.EtkinlikMesajSil(mesaj);
                }

                var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikId);
                etkinliklerManager.EtkinlikSil(etkinlik);

                TempData["Message"] = "Etkinlik başarıyla silindi.";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Etkinlik silinemedi.";
                TempData["MessageType"] = "error";
            }
            return RedirectToAction("AdminEtkinliklerListesi");
        }
        [AdminAttribute]
        public ActionResult ReddedilenEtkinlikSil(int etkinlikId)
        {
            try
            {
                var kullaniciIdleri = katilimcilarManager.GetKatilimciList(etkinlikId);
                foreach (var katilimci in kullaniciIdleri)
                {
                    var katilim = new Katilimcilar
                    {
                        EtkinlikID = etkinlikId,
                        KullaniciID = katilimci
                    };
                    katilimcilarManager.KatilimciSil(katilim);
                }

                var etkinlikMesajlari = etkinlikMesajManager.GetAll().Where(X => X.EtkinlikID == etkinlikId).ToList();
                foreach (var mesaj in etkinlikMesajlari)
                {
                    etkinlikMesajManager.EtkinlikMesajSil(mesaj);
                }

                var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikId);
                etkinliklerManager.EtkinlikSil(etkinlik);

                TempData["Message"] = "Reddedilen etkinlik başarıyla silindi.";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Reddedilen etkinlik silinemedi.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("ReddedilenEtkinlikListesi");
        }
        [AdminAttribute]
        public ActionResult OnayBekleyenEtkinlikSil(int etkinlikId)
        {
            try
            {
                var kullaniciIdleri = katilimcilarManager.GetKatilimciList(etkinlikId);
                foreach (var katilimci in kullaniciIdleri)
                {
                    var katilim = new Katilimcilar
                    {
                        EtkinlikID = etkinlikId,
                        KullaniciID = katilimci
                    };
                    katilimcilarManager.KatilimciSil(katilim);
                }
                var etkinlikMesajlari = etkinlikMesajManager.GetAll().Where(X => X.EtkinlikID == etkinlikId).ToList();
                foreach (var mesaj in etkinlikMesajlari)
                {
                    etkinlikMesajManager.EtkinlikMesajSil(mesaj);
                }

                var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikId);
                etkinliklerManager.EtkinlikSil(etkinlik);

                TempData["Message"] = "Onay bekleyen etkinlik başarıyla silindi.";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Onay bekleyen etkinlik silinemedi.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("OnayBekleyenEtkinlikListesi");
        }

    }

}