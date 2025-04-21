using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IKullaniciIlgiAlaniService
    {
        List<KullaniciIlgiAlanlari> GetAll();
        void KullaniciIlgiAlaniEkle(KullaniciIlgiAlanlari kullaniciIlgiAlani);
        void KullaniciIlgiAlaniGuncelle(KullaniciIlgiAlanlari kullaniciIlgiAlani); 
        void KullaniciIlgiAlaniSil(KullaniciIlgiAlanlari kullaniciIlgiAlani);
        List<KullaniciIlgiAlanlari> GetKullaniciIlgi(int kullaniciID);
    }
}
