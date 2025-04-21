using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EtkinliklerManager : IEtkinliklerService
    {
        IEtkinliklerDal _etkinliklerDal;
        public EtkinliklerManager(IEtkinliklerDal etkinliklerDal)
        {
            _etkinliklerDal=etkinliklerDal;
        }
        public List<Etkinlikler> GetAll()
        {
            return _etkinliklerDal.GetAll();
        }
        public List<Etkinlikler> GetKullaniciKatildigiEtkinlikler(List<int> etkinlikIdleri)
        {
            return _etkinliklerDal.GetAll().Where(e => etkinlikIdleri.Contains(e.EtkinlikID)).ToList();
        }
        public void EtkinlikEkle(Etkinlikler etkinlik)
        {
            _etkinliklerDal.Ekle(etkinlik);
        }

        public void EtkinlikGuncelle(Etkinlikler etkinlik)
        {
            _etkinliklerDal.Guncelle(etkinlik);
        }

        public void EtkinlikSil(Etkinlikler etkinlik)
        {
            _etkinliklerDal.Sil(etkinlik);
        }

        public Etkinlikler GetEtkinlikID(int etkinlikID)
        {
            return _etkinliklerDal.Get(X => X.EtkinlikID==etkinlikID);
        }

        public List<Etkinlikler> GetDuzenleyenIDAll(int kullaniciID)
        {
            return _etkinliklerDal.GetAll(x => x.KullaniciID==kullaniciID);
        }
        public List<Etkinlikler> GetGelecekEtkinlikler(List<int> etkinlikIdleri)
        {
            DateTime currentDateTime = DateTime.Now;

            return _etkinliklerDal.GetAll()
                .Where(x => etkinlikIdleri.Contains(x.EtkinlikID) &&
                            DateTime.Parse(x.EtkinlikTarih.ToString("yyyy-MM-dd") + " " + x.EtkinlikSaati.ToString(@"hh\:mm\:ss")) > currentDateTime)
                .OrderBy(x => DateTime.Parse(x.EtkinlikTarih.ToString("yyyy-MM-dd") + " " + x.EtkinlikSaati.ToString(@"hh\:mm\:ss")))
                .ToList();
        }
        public List<Etkinlikler> GetGecmisEtkinlikler(List<int> etkinlikIdleri)
        {
            DateTime currentDateTime = DateTime.Now;

            return _etkinliklerDal.GetAll()
                .Where(x => etkinlikIdleri.Contains(x.EtkinlikID) &&
                            DateTime.Parse(x.EtkinlikTarih.ToString("yyyy-MM-dd") + " " + x.EtkinlikSaati.ToString(@"hh\:mm\:ss")) < currentDateTime)
                .OrderByDescending(x => DateTime.Parse(x.EtkinlikTarih.ToString("yyyy-MM-dd") + " " + x.EtkinlikSaati.ToString(@"hh\:mm\:ss")))
                .ToList();
        }
        public List<Etkinlikler> GetOnayBekleyenler()
        {
            return _etkinliklerDal.GetAll().Where(x => x.EtkinlikOnayDurumu==OnayDurumu.OnayBekliyor).ToList();
        }
        public List<Etkinlikler> GetReddedilenler()
        {
            return _etkinliklerDal.GetAll().Where(x => x.EtkinlikOnayDurumu==OnayDurumu.OnayliDegil).ToList();
        }
        public (List<Etkinlikler> ilgiAlaninaGoreOneriler, List<Etkinlikler> benzerIlgiAlaninaGoreOneriler, List<Etkinlikler> katildiginaGoreOneriler) GetOnerilenEtkinlikler(int kullaniciID, List<IlgiAlanlari> kullaniciIlgiAlanlari, List<IlgiAlanlari> tumIlgiAlanlari, List<Etkinlikler> kullaniciKatildigiEtkinlikler)
        {
            var tumEtkinlikler = _etkinliklerDal.GetAll();
            List<Etkinlikler> onerilecekEtkinlikler = new List<Etkinlikler>();
            List<Etkinlikler> ilgiAlaninaGoreOneriler = new List<Etkinlikler>();
            List<Etkinlikler> benzerIlgiAlaninaGoreOneriler = new List<Etkinlikler>();
            List<Etkinlikler> katildiginaGoreOneriler = new List<Etkinlikler>();
            List<IlgiAlanlari> onerilebilecekBenzerIlgiAlanlari = new List<IlgiAlanlari>();

            foreach (var ilgiAlani in kullaniciIlgiAlanlari)
            {
                foreach (var etkinlik in tumEtkinlikler)
                {
                    if (etkinlik.IlgiAlaniID == ilgiAlani.IlgiAlaniID &&
                        !kullaniciKatildigiEtkinlikler.Any(katilan => katilan.EtkinlikID == etkinlik.EtkinlikID) && 
                        etkinlik.KullaniciID != kullaniciID)
                    {
                        if (!onerilecekEtkinlikler.Contains(etkinlik))
                        {
                            onerilecekEtkinlikler.Add(etkinlik);
                            ilgiAlaninaGoreOneriler.Add(etkinlik);
                        }
                    }
                }

                foreach (var benzerIlgiAlani in tumIlgiAlanlari)
                {
                    if (benzerIlgiAlani.KategoriID == ilgiAlani.KategoriID && !kullaniciIlgiAlanlari.Contains(benzerIlgiAlani))
                    {
                        onerilebilecekBenzerIlgiAlanlari.Add(benzerIlgiAlani);
                    }
                }
            }

            foreach (var katilinanEtkinlik in kullaniciKatildigiEtkinlikler)
            {
                var etkinlikIlgiAlani = katilinanEtkinlik.IlgiAlaniID;

                if (etkinlikIlgiAlani.HasValue)
                {
                    foreach (var etkinlik in tumEtkinlikler)
                    {
                        if (etkinlik.IlgiAlaniID == etkinlikIlgiAlani.Value &&
                            etkinlik.EtkinlikID != katilinanEtkinlik.EtkinlikID && 
                            etkinlik.KullaniciID != kullaniciID)
                        {
                            if (!kullaniciKatildigiEtkinlikler.Any(k => k.EtkinlikID == etkinlik.EtkinlikID)) 
                            {
                                if (!onerilecekEtkinlikler.Contains(etkinlik))
                                {
                                    onerilecekEtkinlikler.Add(etkinlik);
                                    katildiginaGoreOneriler.Add(etkinlik);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var benzerIlgiAlani in onerilebilecekBenzerIlgiAlanlari)
            {
                foreach (var etkinlik in tumEtkinlikler)
                {
                    if (etkinlik.IlgiAlaniID == benzerIlgiAlani.IlgiAlaniID &&
                        !kullaniciKatildigiEtkinlikler.Any(katilan => katilan.EtkinlikID == etkinlik.EtkinlikID) && 
                        etkinlik.KullaniciID != kullaniciID)
                    {
                        if (!onerilecekEtkinlikler.Contains(etkinlik))
                        {
                            onerilecekEtkinlikler.Add(etkinlik);
                            benzerIlgiAlaninaGoreOneriler.Add(etkinlik);
                        }
                    }
                }
            }

            return (ilgiAlaninaGoreOneriler, benzerIlgiAlaninaGoreOneriler, katildiginaGoreOneriler);

        }

        }
    }
