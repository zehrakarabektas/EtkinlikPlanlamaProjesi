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
    public class BildirimManager:IBildirimService
    {
        IBildirimDal _bildirimDal;

        public BildirimManager(IBildirimDal bildirimDal)
        {
            _bildirimDal=bildirimDal;
        }

        public void BildiriEkle(Bildirimler bildiri)
        {
            _bildirimDal.Ekle(bildiri);
        }

        public void BildiriGuncelle(Bildirimler bildiri)
        {
            _bildirimDal.Guncelle(bildiri);
        }

        public void BildiriSil(Bildirimler bildiri)
        {
            _bildirimDal.Sil(bildiri);
        }

        public List<Bildirimler> GetAll()
        {
            return _bildirimDal.GetAll();
        }

        public Bildirimler GetBildiriID(int bildiriID)
        {
            return _bildirimDal.Get(x => x.BildirimID==bildiriID);
        }
    }
 
}
