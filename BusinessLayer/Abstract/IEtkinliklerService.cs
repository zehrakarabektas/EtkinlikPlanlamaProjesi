using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IEtkinliklerService
    {
        List<Etkinlikler> GetAll();
        void EtkinlikEkle(Etkinlikler etkinlik);
        void EtkinlikGuncelle(Etkinlikler etkinlik);  
        void EtkinlikSil(Etkinlikler etkinlik);
        Etkinlikler GetEtkinlikID(int etkinlikID);
        List<Etkinlikler> GetDuzenleyenIDAll(int kullaniciID);
        List<Etkinlikler> GetGelecekEtkinlikler(List<int> etkinlikIdleri);
        List<Etkinlikler> GetGecmisEtkinlikler(List<int> etkinlikIdleri);
        List<Etkinlikler> GetOnayBekleyenler();
        List<Etkinlikler> GetReddedilenler();
        List<Etkinlikler> GetKullaniciKatildigiEtkinlikler(List<int> etkinlikIdleri);

    }
}
