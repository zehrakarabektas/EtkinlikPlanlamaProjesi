using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    public class EtkinlikKategoriController : Controller
    {
        EtkinlikKategoriManager etkinlikKategoriManager=new EtkinlikKategoriManager(new EfEtkinlikKategoriDal());
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        // GET: EtkinlikKategori

        [AdminAttribute]
        public ActionResult AdminEtkinlikKategoriSayfasi()
        {
            var etkinlikKategori=etkinlikKategoriManager.GetAll();
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari=ilgiAlanlari;
            return View(etkinlikKategori);
        }

        [KullaniciAttribute]
        public ActionResult KullaniciEtkinlikKategoriSayfasi()
        {
            var etkinlikKategori = etkinlikKategoriManager.GetAll();
            var ilgiAlanlari = ilgiAlaniManager.GetAll().ToList();
            ViewBag.IlgiAlanlari=ilgiAlanlari;
            return View(etkinlikKategori);
        }
        [HttpGet]
        [AdminAttribute]
        public ActionResult AdminEtkinlikKategoriEkle()
        {
            return View();
        }
        [HttpPost]
        [AdminAttribute]
        public ActionResult AdminEtkinlikKategoriEkle(EtkinlikKategori kategori)
        {
            EtkinlikKategoriValidator kategoriValidator = new EtkinlikKategoriValidator();
            ValidationResult validateResult=kategoriValidator.Validate(kategori);
            if (validateResult.IsValid) 
            {
                etkinlikKategoriManager.EtkinlikKategoriEkle(kategori);
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                }
                return RedirectToAction("AdminEtkinlikKategoriSayfasi");
            }
            else
            {
                foreach(var item in validateResult.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        [AdminAttribute]
        public ActionResult KategoriSil(int kategoriId)
        {
            var kategori=etkinlikKategoriManager.GetKategoriID(kategoriId);
            etkinlikKategoriManager.EtkinlikKategoriSil(kategori);
            return RedirectToAction("AdminEtkinlikKategoriSayfasi");
        }
        [HttpGet]
        [AdminAttribute]
        public ActionResult KategoriGuncelle(int kategoriID)
        {
            var kategori = etkinlikKategoriManager.GetKategoriID(kategoriID);

            return View(kategori);
        }
        [HttpPost]
        [AdminAttribute]
        public ActionResult KategoriGuncelle(EtkinlikKategori kategori)
        {
            etkinlikKategoriManager.EtkinlikKategoriGuncelle(kategori);
            return RedirectToAction("AdminEtkinlikKategoriSayfasi");
        }
        
    }
}