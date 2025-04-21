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
using System.Web.Security;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    [KullaniciAttribute]
    public class KullanicilarController : Controller
    {
        KullanicilarManager kullaniciManager=new KullanicilarManager(new EfKullanicilarDal());
        KullaniciIlgiManager kullaniciIlgiManager=new KullaniciIlgiManager(new EfKullaniciIlgiAlanlariDal());
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        EtkinliklerManager etkinliklerManager = new EtkinliklerManager(new EfEtkinliklerDal());
        KatilimcilarManager katilimcilarManager = new KatilimcilarManager(new EfKatilimcilarDal());
        PuanlarManager puanManager = new PuanlarManager(new EfPuanlarDal());
        Sifreleme sifrele = new Sifreleme();
        KullanicilarValidator kullaniciValidator = new KullanicilarValidator();
        EtkinliklerValidator etkinlikValidator = new EtkinliklerValidator();
        KullaniciIlgiAlanlariValidator kullaniciIlgiValidator = new KullaniciIlgiAlanlariValidator();
        BildirimManager bildirimManager = new BildirimManager(new EfBildirimDal());
        // GET: Kullanicilar

        public ActionResult KullanicilarAnaSayfa(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            var kullanici = kullaniciManager.GetKullaniciID(kullaniciId);
            if (kullanici == null)
            {
                return RedirectToAction("KullaniciGirisi", "Home");
            }
            var toplamPuan=puanManager.ToplamPuanHesapla(kullaniciId);
            Session["KullaniciAdi"] = kullanici.KullaniciAdi;
            Session["AdSoyad"] = kullanici.Ad + " " + kullanici.Soyad;
            Session["ProfilResmiUrl"] ="~"+ kullanici.ProfilFotografi;
            Session["ToplamPuan"] = toplamPuan;
            Session["KullaniciAdresi"]=kullanici.Konum;
            var mevcutKullaniciIlgiAlanlari = kullaniciIlgiManager.GetKullaniciIlgi(kullaniciId);
            var ilgiAlaniIdleri = mevcutKullaniciIlgiAlanlari.Select(x => x.IlgiAlaniID).ToList();
            var kullaniciIlgiAlanlari= ilgiAlaniManager.GetAll().Where(x => ilgiAlaniIdleri.Contains(x.IlgiAlaniID)).ToList();

            var tumIlgiAlanlari = ilgiAlaniManager.GetAll();
            var kullaniciKatildigiEtkinlikIdleri= katilimcilarManager.GetKullaniciKatildigiEtkinliklerID(kullaniciId);
            var kullaniciKatildigiEtkinlikler=etkinliklerManager.GetKullaniciKatildigiEtkinlikler(kullaniciKatildigiEtkinlikIdleri);
            var (ilgiAlaninaGoreOneriler, benzerIlgiAlaninaGoreOneriler, katildiginaGoreOneriler) = etkinliklerManager.GetOnerilenEtkinlikler(kullanici.KullaniciID, kullaniciIlgiAlanlari, tumIlgiAlanlari, kullaniciKatildigiEtkinlikler);
            //var onerilenEtkinlikler=etkinliklerManager.GetOnerilenEtkinlikler(kullanici.KullaniciID, kullaniciIlgiAlanlari, tumIlgiAlanlari, kullaniciKatildigiEtkinlikler).Where(etkinlik => etkinlik.EtkinlikTarih >= DateTime.Now).ToList();
            // ViewBag.OnerilenEtkinlikler = onerilenEtkinlikler;
            var ilgiAlaninaGoreOnerilerFiltreli = ilgiAlaninaGoreOneriler.Where(etkinlik => etkinlik.EtkinlikTarih >= DateTime.Now).ToList();

            // Benzer ilgi alanına göre önerilen etkinliklerin tarih filtresi ile alınması
            var benzerIlgiAlaninaGoreOnerilerFiltreli = benzerIlgiAlaninaGoreOneriler
                .Where(etkinlik => etkinlik.EtkinlikTarih >= DateTime.Now)
                .ToList();

            // Katıldığa göre önerilen etkinliklerin tarih filtresi ile alınması
            var katildiginaGoreOnerilerFiltreli = katildiginaGoreOneriler
                .Where(etkinlik => etkinlik.EtkinlikTarih >= DateTime.Now)
                .ToList();

            ViewBag.IlgiAlanlarinaGoreOneriler = ilgiAlaninaGoreOnerilerFiltreli;
            ViewBag.BenzerIlgiAlaninaGoreOneriler = benzerIlgiAlaninaGoreOnerilerFiltreli;
            ViewBag.KatildiginaGoreOneriler = katildiginaGoreOnerilerFiltreli;
            var tumEtkinlikler = etkinliklerManager.GetAll()
                                         .Where(etkinlik => etkinlik.EtkinlikTarih >= DateTime.Now)
                                         .ToList();

            var tumEtkinliklerFiltre = tumEtkinlikler
                .Where(etkinlik => !ilgiAlaninaGoreOnerilerFiltreli.Contains(etkinlik) &&
                                   !benzerIlgiAlaninaGoreOnerilerFiltreli.Contains(etkinlik) &&
                                   !katildiginaGoreOnerilerFiltreli.Contains(etkinlik))
                .ToList();

            ViewBag.TumEtkinlikler = tumEtkinliklerFiltre;
            
            return View();
        }
        
        

        [HttpGet]
        public ActionResult KullaniciProfilimDuzenle(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();

            var kullanici =kullaniciManager.GetKullaniciID(kullaniciId);
            var ilgiAlanlari=ilgiAlaniManager.GetAll();
            ViewBag.IlgiAlanlari = ilgiAlanlari;
            var mevcutKullaniciIlgiAlanlari = kullaniciIlgiManager.GetKullaniciIlgi(kullaniciId);  
            var secilenIlgiAlanlari = mevcutKullaniciIlgiAlanlari.Select(x => x.IlgiAlaniID).ToList();  
            ViewBag.kullaniciIlgiAlanlari = secilenIlgiAlanlari;
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult KullaniciProfilimDuzenle(Kullanicilar kullanici, List<int> secilenIlgiAlanlari)
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

                return RedirectToAction("KullanicilarAnaSayfa");
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
        public ActionResult KullaniciEtkinlikOlusturmaSayfasi()
        {
            ViewBag.IlgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.OnaylanmaDurumu == OnayDurumu.Onayli).Select(x => new SelectListItem{
                                   Text = x.IlgiAlaniAdi,
                                   Value = x.IlgiAlaniID.ToString()
                                   }).ToList();
           
            return View();
        }
        [HttpPost]
        public ActionResult KullaniciEtkinlikOlusturmaSayfasi(Etkinlikler etkinlik, string SelectedIlgiAlaniID, string yeniIlgiAlani, string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
           
            ValidationResult result = etkinlikValidator.Validate(etkinlik);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.OnaylanmaDurumu == OnayDurumu.Onayli).Select(x => new SelectListItem
                {
                    Text = x.IlgiAlaniAdi,
                    Value = x.IlgiAlaniID.ToString()
                }).ToList();
                ViewBag.IlgiAlanlari = ilgiAlanlari;

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
            else if (!string.IsNullOrEmpty(yeniIlgiAlani))
            {
                var yeniIlgiAlaniObjesi = new IlgiAlanlari { IlgiAlaniAdi = yeniIlgiAlani, KategoriID = 10 };
                ilgiAlaniManager.IlgiAlaniEkle(yeniIlgiAlaniObjesi);

                etkinlik.IlgiAlaniID = yeniIlgiAlaniObjesi.IlgiAlaniID;
            }
            else
            {
            
                ViewBag.IlgiAlaniHataMesaji = "Lütfen bir ilgi alanı seçin veya yeni bir ilgi alanı girin.";
                var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.OnaylanmaDurumu == OnayDurumu.Onayli).Select(x => new SelectListItem
                {
                    Text = x.IlgiAlaniAdi,
                    Value = x.IlgiAlaniID.ToString()
                }).ToList();
                ViewBag.IlgiAlanlari = ilgiAlanlari;

                return View(etkinlik);
            }
            int? gelenId = etkinlik.IlgiAlaniID;
            Debug.WriteLine("Gelen ID: " + gelenId);
            if(Request.Files.Count>0)
            {
                string dosyaAdi=Path.GetFileName(Request.Files[0].FileName);    
                string dosyaUzantisi=Path.GetExtension(Request.Files[0].FileName);
                string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                string yol = "~/Images/etkinliklerImages/" + benzersizDosyaAdi;

                Request.Files[0].SaveAs(Server.MapPath(yol));
                etkinlik.EtkinlikResmi="/Images/etkinliklerImages/"+benzersizDosyaAdi;
            }
           
            etkinlik.KullaniciID = kullaniciId;
            etkinlik.EtkinlikOnayDurumu=OnayDurumu.OnayBekliyor;
            etkinliklerManager.EtkinlikEkle(etkinlik);

            var olusturmaPuani = new Puanlar
            {
                KullaniciID = kullaniciId,
                KazanilanTarih = DateTime.Now,
                PuanTuru = PuanTuru.Olusturma
            };
            puanManager.PuanEkle(olusturmaPuani);
            var toplamPuan = puanManager.ToplamPuanHesapla(kullaniciId);
            Session["ToplamPuan"] = toplamPuan;
            TempData["Mesaj"] = "Etkinlik başarıyla oluşturuldu.";
            return RedirectToAction("DüzenlenenEtkinliklerListesi");
           
        }
        public ActionResult KullaniciGelecekEtkinliklerim(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId=context.Kullanicilars.Where(x=>x.KullaniciAdi==girisYapanKullaniciAdi).Select(y=>y.KullaniciID).FirstOrDefault();
            var katilinilanetkinlikler = katilimcilarManager.GetKullaniciKatildigiEtkinliklerID(kullaniciId);
            var gelecekEtkinlikler = etkinliklerManager.GetGelecekEtkinlikler(katilinilanetkinlikler);
            return View(gelecekEtkinlikler);
        }
        public ActionResult KullaniciGecmisEtkinliklerim(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            var katilinilanetkinlikler = katilimcilarManager.GetKullaniciKatildigiEtkinliklerID(kullaniciId);
            var gelecekEtkinlikler = etkinliklerManager.GetGecmisEtkinlikler(katilinilanetkinlikler);
            return View(gelecekEtkinlikler);
        }
        public ActionResult DüzenlenenEtkinliklerListesi(string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            var etkinlik = etkinliklerManager.GetDuzenleyenIDAll(kullaniciId);
            return View(etkinlik);
        }
        [HttpGet]
        public ActionResult EtkinlikGuncelle(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            List<SelectListItem> IlgiAlanlari = (from x in ilgiAlaniManager.GetAll()
                                                 where x.OnaylanmaDurumu == OnayDurumu.Onayli
                                                 select new SelectListItem
                                                 {
                                                     Text = x.IlgiAlaniAdi,
                                                     Value = x.IlgiAlaniID.ToString(),
                                                     Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID) 
                                                 }).ToList();

            ViewBag.IlgiAlanlari = IlgiAlanlari;
            return View(etkinlik);
        }
        [HttpPost]
        public ActionResult EtkinlikGuncelle(Etkinlikler etkinlik, string SelectedIlgiAlaniID, string yeniIlgiAlani)
        {
            var mevcutEtkinlik = etkinliklerManager.GetEtkinlikID(etkinlik.EtkinlikID);
          
            ValidationResult result = etkinlikValidator.Validate(etkinlik);
           
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.OnaylanmaDurumu == OnayDurumu.Onayli).Select(x => new SelectListItem
                {
                    Text = x.IlgiAlaniAdi,
                    Value = x.IlgiAlaniID.ToString(),
                    Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID) 
                }).ToList();
           
                ViewBag.IlgiAlanlari = ilgiAlanlari;

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
            else if (!string.IsNullOrEmpty(yeniIlgiAlani))
            {
                var yeniIlgiAlaniObjesi = new IlgiAlanlari { IlgiAlaniAdi = yeniIlgiAlani, KategoriID = 10 };
                ilgiAlaniManager.IlgiAlaniEkle(yeniIlgiAlaniObjesi);

                etkinlik.IlgiAlaniID = yeniIlgiAlaniObjesi.IlgiAlaniID;
            }
            else
            {
                etkinlik.IlgiAlaniID=mevcutEtkinlik.IlgiAlaniID;
                ViewBag.IlgiAlaniHataMesaji = "Lütfen bir ilgi alanı seçin veya yeni bir ilgi alanı girin.";
                var ilgiAlanlari = ilgiAlaniManager.GetAll().Where(x => x.OnaylanmaDurumu == OnayDurumu.Onayli).Select(x => new SelectListItem
                {
                    Text = x.IlgiAlaniAdi,
                    Value = x.IlgiAlaniID.ToString(),
                    Selected = (etkinlik != null && x.IlgiAlaniID == etkinlik.IlgiAlaniID) 
                }).ToList();
                ViewBag.IlgiAlanlari = ilgiAlanlari;

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
            return RedirectToAction("DüzenlenenEtkinliklerListesi");
        }
        public ActionResult EtkinliktenAyrıl(int etkinlikId,string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
           
            var katilimci = new Katilimcilar
            {
                KullaniciID = kullaniciId, 
                EtkinlikID = etkinlikId
            };
            katilimcilarManager.KatilimciSil(katilimci);

            return RedirectToAction("KullaniciGelecekEtkinliklerim");
        }
        public ActionResult GecmisEtkinlikSil(int etkinlikId, string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            
            var katilimci = new Katilimcilar
            {
                KullaniciID = kullaniciId,
                EtkinlikID = etkinlikId
            };
            katilimcilarManager.KatilimciSil(katilimci);

            return RedirectToAction("KullaniciGecmisEtkinliklerim");
        }
        public ActionResult EtkinlikSil(int etkinlikId)
        {
            var kullaniciIdleri=katilimcilarManager.GetKatilimciList(etkinlikId);
            foreach (var katilimci in kullaniciIdleri)
            {
                var katilim = new Katilimcilar
                {
                    EtkinlikID = etkinlikId,
                    KullaniciID = katilimci 
                };
                katilimcilarManager.KatilimciSil(katilim);
            }
            var etkinlik=etkinliklerManager.GetEtkinlikID(etkinlikId);
            etkinliklerManager.EtkinlikSil(etkinlik);

            return RedirectToAction("DüzenlenenEtkinliklerListesi");
        }
        public ActionResult EtkinlikKatil(int etkinlikID,string girisYapanKullaniciAdi)
        {
            Context context = new Context();
            girisYapanKullaniciAdi=(string)Session["KullaniciAdi"];
            var kullaniciId = context.Kullanicilars.Where(x => x.KullaniciAdi==girisYapanKullaniciAdi).Select(y => y.KullaniciID).FirstOrDefault();
            var kullanici = kullaniciManager.GetKullaniciID(kullaniciId);

            var kullaniciKatildigiEtkinlikIdleri = katilimcilarManager.GetKullaniciKatildigiEtkinliklerID(kullaniciId);
            var kullaniciKatildigiEtkinlikler = etkinliklerManager.GetKullaniciKatildigiEtkinlikler(kullaniciKatildigiEtkinlikIdleri);

            var etkinlik =etkinliklerManager.GetEtkinlikID(etkinlikID);

            DateTime etkinlikTarih = etkinlik.EtkinlikTarih;
            TimeSpan etkinlikBaslangicSaat=etkinlik.EtkinlikSaati;
            DateTime etkinlikBaslangici = etkinlikTarih.Add(etkinlikBaslangicSaat); 
            DateTime etkinlikBitisi = etkinlikBaslangici.AddMinutes(etkinlik.EtkinlikSuresi);

            foreach (var katildigiEtkinlik in kullaniciKatildigiEtkinlikler)
            {
                DateTime mevcutEtkinlikTarih = katildigiEtkinlik.EtkinlikTarih;
                TimeSpan mevcutEtkinlikSaati = katildigiEtkinlik.EtkinlikSaati;
                DateTime mevcutEtkinlikBaslangici = mevcutEtkinlikTarih.Add(mevcutEtkinlikSaati);
                DateTime mevcutEtkinlikBitisi = mevcutEtkinlikBaslangici.AddMinutes(katildigiEtkinlik.EtkinlikSuresi);

                if ((etkinlikBaslangici < mevcutEtkinlikBitisi && etkinlikBitisi > mevcutEtkinlikBaslangici))
                {
                    TempData["uyariMesaj"] = "O tarih ve saatte başka bir etkinliğiniz var. Lütfen başka bir etkinliğe katılmayı seçin.";
                    return RedirectToAction("KullanicilarAnaSayfa"); 
                }
            }
            var katilimPuan = new Puanlar
            {
                KullaniciID = kullaniciId,
                KazanilanTarih=DateTime.Now,
                PuanTuru=PuanTuru.Katilim
            };
            puanManager.PuanEkle(katilimPuan);
            var toplamPuan = puanManager.ToplamPuanHesapla(kullaniciId);
            var katilimci = new Katilimcilar
            {
                KullaniciID=kullaniciId,
                EtkinlikID=etkinlikID
            };
           
            katilimcilarManager.KatilimciEkle(katilimci);
            Session["ToplamPuan"] = toplamPuan;
            TempData["onayMesaj"] = "Başarıyla etkinliğe katıldınız. Katıldığınız etkinlikleri etkinliklerim bölümünden ulaşabilirsiniz.";
            return RedirectToAction("KullanicilarAnaSayfa");
        }
        public ActionResult EtkinlikDetay(int etkinlikID)
        {
            var etkinlik = etkinliklerManager.GetEtkinlikID(etkinlikID);
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari = ilgiAlanlari;

            var kullanicilar = kullaniciManager.GetAll().ToList();
            ViewBag.Kullanicilar=kullanicilar;
            return View(etkinlik);
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
            return RedirectToAction("KullaniciGirisi", "Home");
        }
       
        
    }
}
