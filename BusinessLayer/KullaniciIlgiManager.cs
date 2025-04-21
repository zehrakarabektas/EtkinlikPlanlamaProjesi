using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class KullaniciIlgiManager: IKullaniciIlgiAlaniService
    {
        IKullaniciIlgiAlanlariDal _kullaniciIlgiAlaniDal;

        public KullaniciIlgiManager(IKullaniciIlgiAlanlariDal kullaniciIlgiAlaniDal)
        {
            _kullaniciIlgiAlaniDal = kullaniciIlgiAlaniDal;
        }

        public List<KullaniciIlgiAlanlari> GetAll()
        {
            return _kullaniciIlgiAlaniDal.GetAll();
        }

        public List<KullaniciIlgiAlanlari> GetKullaniciIlgi(int kullaniciID)
        {
            return _kullaniciIlgiAlaniDal.GetAll(x => x.KullaniciID == kullaniciID); 
        }

        public void KullaniciIlgiAlaniEkle(KullaniciIlgiAlanlari kullaniciIlgiAlani)
        {
            _kullaniciIlgiAlaniDal.Ekle(kullaniciIlgiAlani);
        }

        public void KullaniciIlgiAlaniGuncelle(KullaniciIlgiAlanlari kullaniciIlgiAlani)
        {
            _kullaniciIlgiAlaniDal.Guncelle(kullaniciIlgiAlani);
        }

        public void KullaniciIlgiAlaniSil(KullaniciIlgiAlanlari kullaniciIlgiAlani)
        {
            _kullaniciIlgiAlaniDal.Sil(kullaniciIlgiAlani);
        }
       


        //public List<KullaniciIlgiAlanlari> GetAll()
        //{
        //    return repository.GetAll().ToList();
        //}
        //public void KullaniciIlgiEkle(KullaniciIlgiAlanlari kullaniciIlgi)
        //{
        //    repository.Ekle(kullaniciIlgi);
        //}
    }
}
