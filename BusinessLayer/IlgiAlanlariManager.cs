using BusinessLayer.Abstract;
using DataAccessLayer;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class IlgiAlanlariManager : IIlgiAlanlariService
    {
        IIlgiAlanlariDal _ilgiAlaniDal;

        public IlgiAlanlariManager(IIlgiAlanlariDal ilgiAlaniDal)
        {
            _ilgiAlaniDal=ilgiAlaniDal;
        }

        public List<IlgiAlanlari> GetAll()
        {
            return _ilgiAlaniDal.GetAll();
        }

        public IlgiAlanlari GetIlgiAlaniID(int ilgiAlaniID)
        {
            return _ilgiAlaniDal.Get(X => X.IlgiAlaniID == ilgiAlaniID);
        }

        public void IlgiAlaniEkle(IlgiAlanlari ilgiAlani)
        {
            _ilgiAlaniDal.Ekle(ilgiAlani);
        }

        public void IlgiAlaniGuncelle(IlgiAlanlari ilgiAlani)
        {
            _ilgiAlaniDal.Guncelle(ilgiAlani);
        }

        public void IlgiAlaniSil(IlgiAlanlari ilgiAlani)
        {
            _ilgiAlaniDal.Sil(ilgiAlani);
        }
    }
}
