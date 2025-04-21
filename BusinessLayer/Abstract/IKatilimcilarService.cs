using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IKatilimcilarService
    {
        List<Katilimcilar> GetAll();
        void KatilimciEkle(Katilimcilar katilimci);
        void KatilimciGuncelle(Katilimcilar katilimci);  
        void KatilimciSil(Katilimcilar katilimci);
        List<int> GetKullaniciKatildigiEtkinliklerID(int kullaniciID);
        List<int> GetKatilimciList(int etkinlikId);

    }
}
