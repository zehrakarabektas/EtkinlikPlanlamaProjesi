using BusinessLayer;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazlab1etkinlikplanlamauygulamasi.Attribute;

namespace yazlab1etkinlikplanlamauygulamasi.Controllers
{
    public class IlgiAlanlariController : Controller
    {
        IlgiAlanlariManager ilgiAlaniManager = new IlgiAlanlariManager(new EfIlgiAlanlariDal());
        EtkinlikKategoriManager kategoriManager = new EtkinlikKategoriManager(new EfEtkinlikKategoriDal());
        IlgiAlanlariValidator ilgiAlaniValidator = new IlgiAlanlariValidator();

        // GET: IlgiAlanlari
        [AdminAttribute]
        public ActionResult AdminIlgiAlanlariListesi()
        {
            var ilgiAlanlari = ilgiAlaniManager.GetAll().OrderBy(ilgi => ilgi.OnaylanmaDurumu == OnayDurumu.OnayBekliyor ? 0 :
                     ilgi.OnaylanmaDurumu == OnayDurumu.Onayli ? 2 : 1).ToList();
            ViewBag.Kategoriler = kategoriManager.GetAll().Select(x => new SelectListItem
            {
                Text = x.KategoriAdi,
                Value = x.KategoriID.ToString()
            }).ToList();

            return View(ilgiAlanlari);
        }
        [AdminAttribute]
        public ActionResult Onayla(int illgiALaniID)
        {
            var ilgiAlani = ilgiAlaniManager.GetIlgiAlaniID(illgiALaniID);
            if (ilgiAlani != null)
            {
                ilgiAlani.OnaylanmaDurumu = OnayDurumu.Onayli;
                ilgiAlaniManager.IlgiAlaniGuncelle(ilgiAlani);
            }
            return RedirectToAction("AdminIlgiAlanlariListesi");
        }
        [AdminAttribute]
        public ActionResult Reddet(int illgiALaniID)
        {
            var ilgiAlani = ilgiAlaniManager.GetIlgiAlaniID(illgiALaniID);
            if (ilgiAlani != null)
            {
                ilgiAlani.OnaylanmaDurumu = OnayDurumu.OnayliDegil;
                ilgiAlaniManager.IlgiAlaniGuncelle(ilgiAlani);
            }
            return RedirectToAction("AdminIlgiAlanlariListesi");
        }
        [HttpGet]
        [AdminAttribute]
        public ActionResult AdminIlgiAlanGuncelle(int ilgiAlaniId)
        {
            var ilgiAlani = ilgiAlaniManager.GetAll().FirstOrDefault(x => x.IlgiAlaniID == ilgiAlaniId);
            ViewBag.Kategoriler = kategoriManager.GetAll().Select(x => new SelectListItem
            {
                Text = x.KategoriAdi,
                Value = x.KategoriID.ToString()
            }).ToList();

            return View(ilgiAlani);
        }
        [HttpPost]
        [AdminAttribute]
        public ActionResult AdminIlgiAlanGuncelle(IlgiAlanlari ilgiAlani)
        {
            ValidationResult result = ilgiAlaniValidator.Validate(ilgiAlani);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                ViewBag.Kategoriler = kategoriManager.GetAll().Select(x => new SelectListItem
                {
                    Text = x.KategoriAdi,
                    Value = x.KategoriID.ToString()
                }).ToList();

                return View(ilgiAlani);
            }
            ilgiAlaniManager.IlgiAlaniGuncelle(ilgiAlani); 
            return RedirectToAction("AdminIlgiAlanlariListesi");

        }
        [HttpPost]
        [AdminAttribute]
        public ActionResult IlgiAlaniEkleme(string IlgiAlaniAdi, int kategoriID, int OnayDurum)
        {
            OnayDurumu onayDurumuEnum = (OnayDurumu)OnayDurum;


            var ilgiAlani = new IlgiAlanlari
            {
                IlgiAlaniAdi = IlgiAlaniAdi,
                KategoriID = kategoriID,
                OnaylanmaDurumu = onayDurumuEnum
            };

            ValidationResult result = ilgiAlaniValidator.Validate(ilgiAlani);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                ViewBag.Kategoriler = kategoriManager.GetAll().Select(x => new SelectListItem
                {
                    Text = x.KategoriAdi,
                    Value = x.KategoriID.ToString()
                }).ToList();

                return View(ilgiAlani);
            }
        
            ilgiAlaniManager.IlgiAlaniEkle(ilgiAlani);
            return RedirectToAction("AdminIlgiAlanlariListesi");
        }
       
    }
}