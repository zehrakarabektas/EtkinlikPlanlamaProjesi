using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class KullaniciGirisManager : IKullaniciGirisService
    {
        IKullanicilarDal _kullaniciDal;
        Sifreleme sifreSifreleme = new Sifreleme();

        public KullaniciGirisManager(IKullanicilarDal kullaniciDal)
        {
            _kullaniciDal=kullaniciDal;
        }

        public Kullanicilar GetKullanici(string kullaniciAdi, string sifre)
        {
            string hashliSifre = sifreSifreleme.Sifrele(sifre);

            return _kullaniciDal.Get(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == hashliSifre);
        }
        public Kullanicilar GetSifreYenileKullaniciDogrulama(string kullaniciAdi, string Eposta)
        {
            return _kullaniciDal.Get(x => x.KullaniciAdi == kullaniciAdi && x.Eposta == Eposta);
        }
       

    }
}
