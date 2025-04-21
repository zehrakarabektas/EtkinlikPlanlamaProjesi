using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        KullanicilarManager kullanicilarManager = new KullanicilarManager(new EfKullanicilarDal());
        KullaniciGirisManager kullaniciGirisManager = new KullaniciGirisManager(new EfKullanicilarDal());
        KullaniciIlgiManager kullaniciIlgiManager = new KullaniciIlgiManager(new EfKullaniciIlgiAlanlariDal());
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        PuanlarManager puanManager = new PuanlarManager(new EfPuanlarDal());
        Sifreleme sifreSifreleme = new Sifreleme();
        KullanicilarValidator kullaniciValidator = new KullanicilarValidator();
        public ActionResult UygulamaGirisSayfasi()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AdminGirisi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminGirisi(Kullanicilar admin)
        {
            var adminuser = kullaniciGirisManager.GetKullanici(admin.KullaniciAdi, admin.Sifre);
            if (adminuser!=null)
            {
                if (adminuser.KullaniciRole == "Admin")  
                {
                    var ticket = new FormsAuthenticationTicket(
                              1,  
                              adminuser.KullaniciAdi,  
                              DateTime.Now,  
                              DateTime.Now.AddHours(3),  
                              false,  
                              adminuser.KullaniciRole,  
                              FormsAuthentication.FormsCookiePath  
                     );
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Expires = ticket.Expiration; 
                    Response.Cookies.Add(cookie);
                    FormsAuthentication.SetAuthCookie(adminuser.KullaniciAdi, false);
                    Session["KullaniciAdi"]=adminuser.KullaniciAdi;
                    Session["KullaniciRole"] = adminuser.KullaniciRole;
                    return RedirectToAction("AdminAnaSayfasi", "WebAdmin");
                }
                else
                {
                    TempData["Mesaj"] = "Yalnızca admin olanlar giriş yapabilir.";
                    return RedirectToAction("AdminGirisi");
                } 
            }
            else
            {
                TempData["Mesaj"] = "Kullanıcı adı veya şifre hatalı.";
                return RedirectToAction("AdminGirisi");
            }
        }
        [HttpGet]
        public ActionResult SifreYenileme()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifreYenileme(Kullanicilar kullanici)
        {
            if (ModelState.IsValid)
            {
                var kullaniciuser = kullaniciGirisManager.GetSifreYenileKullaniciDogrulama(kullanici.KullaniciAdi, kullanici.Eposta);

                if (kullaniciuser != null)
                {
                    string yeniSifre = Guid.NewGuid().ToString().Substring(0, 8);
                    EmailGonder(kullanici.Eposta, "Şifre Yenileme", yeniSifre);
                    ViewBag.Message = "Yeni şifreniz e-posta adresinize gönderildi.";

                    kullaniciuser.Sifre = sifreSifreleme.Sifrele(yeniSifre);
                    kullanicilarManager.KullaniciGuncelle(kullaniciuser);
                    if (kullaniciuser.KullaniciRole == "Admin") 
                    {
                        return RedirectToAction("AdminGirisi"); 
                    }
                    else
                    {
                        return RedirectToAction("KullaniciGirisi"); 
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya e-posta adresi yanlış.");
                    if (kullaniciuser.KullaniciRole == "Admin")
                    {
                        return RedirectToAction("AdminGirisi");
                    }
                    else
                    {
                        return RedirectToAction("KullaniciGirisi");
                    }
                }
            }
            else
            {
                return View();
            }

        }

        private void EmailGonder(string alici, string konu, string yeniSifre)
        {
            try
            {
                var gonderen = new SmtpClient("smtp.gmail.com", 587) 
                {
                    Credentials = new NetworkCredential("planora2441@gmail.com", "bxabjjknkitiamym"),
                    EnableSsl = true
                };

                var mail = new MailMessage("planora2441@gmail.com", alici)
                {
                    Subject = konu,
                    IsBodyHtml = true,
                    Body = $@"
                <html>
                    <body>
                        <div style='text-align:center;'> <!-- Logo Ortalamak için -->
                            <img src='cid:logoImage' alt='Planora Logo' style='max-width: 200px;' />
                        </div>
                        <h3 style='text-align:center;'>Şifre Yenileme İşlemi</h3>
                        <p style='text-align:center;'>Planora etkinlik planlama sitesinde şifre yenileme yaptınız.</p>
                        <p style='text-align:center; font-size: 24px; color:  #430202; font-weight: bold;'>Yeni şifreniz: {yeniSifre}</p>
                    </body>
                </html>"
                };

                var logo = new LinkedResource("C:\\Users\\zehra\\source\\repos\\yazlab1etkinlikplanlamauygulamasi\\yazlab1etkinlikplanlamauygulamasi\\Images\\logo1.png", "image/png")
                {
                    ContentId = "logoImage",
                    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                };

                
                var alternateView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                alternateView.LinkedResources.Add(logo);
                mail.AlternateViews.Add(alternateView);

                gonderen.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("E-posta gönderilirken bir hata oluştu: " + ex.Message);
            }
        }
        [HttpGet]
        public ActionResult KullaniciGirisi()
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari=ilgiAlanlari;
            return View();
        }
        [HttpPost]
        public ActionResult KullaniciGirisi(Kullanicilar kullanici)
        {
            var kullaniciuser=kullaniciGirisManager.GetKullanici(kullanici.KullaniciAdi, kullanici.Sifre);
            if (kullaniciuser!=null)
            {
                if (kullaniciuser.KullaniciRole == "Kullanici")
                {
                    var ticket = new FormsAuthenticationTicket(
                             1,
                            kullaniciuser.KullaniciAdi,
                             DateTime.Now,
                             DateTime.Now.AddHours(3),
                             false,
                             kullaniciuser.KullaniciRole,
                             FormsAuthentication.FormsCookiePath
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Expires = ticket.Expiration;
                    Response.Cookies.Add(cookie);
                    FormsAuthentication.SetAuthCookie(kullaniciuser.KullaniciAdi, false);
                    Session["KullaniciAdi"]=kullaniciuser.KullaniciAdi;
                    Session["KullaniciRole"] = kullaniciuser.KullaniciRole;
                    return RedirectToAction("KullanicilarAnaSayfa", "Kullanicilar");
                }
                else
                {
                    TempData["Mesaj"] = "Yalnızca kullanıcı olanlar giriş yapabilir.";
                    return RedirectToAction("KullaniciGirisi");
                }
                
            }
            else
            {
                TempData["Mesaj"] = "Kullanıcı adı veya şifre hatalı.";
                return RedirectToAction("KullaniciGirisi");
            }
        }
        [HttpGet]
        public ActionResult KullaniciKayit()
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari=ilgiAlanlari;
            return RedirectToAction("KullaniciGirisi");
        }

        [HttpPost]
        public ActionResult KullaniciKayit(Kullanicilar kullanici, int[] secilenIlgiAlanlari)
        {
            var mevcutKullanici = kullanicilarManager.GetAll().FirstOrDefault(x => x.KullaniciAdi == kullanici.KullaniciAdi);


            if (mevcutKullanici != null)
            {
                TempData["Message"] = "Bu kullanıcı adı zaten alınmış. Lütfen başka bir kullanıcı adı seçin.";
                TempData["MessageType"] = "error";

                var ilgiAlanlari = ilgiAlaniManager.GetAll();
                ViewBag.IlgiAlanlari=ilgiAlanlari;
                return RedirectToAction("KullaniciGirisi");
            }
            ValidationResult validationResult = kullaniciValidator.Validate(kullanici);
            if (validationResult.IsValid)
            {
                kullanici.Sifre = sifreSifreleme.Sifrele(kullanici.Sifre);
                kullanici.KullaniciRole="Kullanici";
              
                if (Request.Files.Count>0)
                {
                    string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                    string dosyaUzantisi = Path.GetExtension(Request.Files[0].FileName);
                    string benzersizDosyaAdi = Guid.NewGuid().ToString() + dosyaUzantisi;

                    string yol = "~/Images/" + benzersizDosyaAdi;

                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    kullanici.ProfilFotografi="/Images/"+benzersizDosyaAdi;
                }
                kullanicilarManager.KullaniciEkle(kullanici);
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
                TempData["Message"] = "Kullanıcı başarıyla kaydedildi.";
                TempData["MessageType"] = "success";
                return RedirectToAction("KullaniciGirisi");
            }
            else
            {
                TempData["Message"] = "Lütfen tüm alanları düzgün geçerli bir şekilde doldurun.";
                TempData["MessageType"] = "error";
                var ilgiAlanlari = ilgiAlaniManager.GetAll();
                ViewBag.IlgiAlanlari=ilgiAlanlari;
                return RedirectToAction("KullaniciGirisi");
            }

        }

    }
}