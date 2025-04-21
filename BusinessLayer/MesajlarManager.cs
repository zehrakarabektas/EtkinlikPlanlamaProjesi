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
    public class MesajlarManager : IMesajlarService
    {
        IMesajlarDal _mesajlarDal;

        public MesajlarManager(IMesajlarDal mesajlarDal)
        {
            _mesajlarDal=mesajlarDal;
        }

        public List<Mesajlar> GetAll()
        {
            return _mesajlarDal.GetAll();
        }

        public void MesajEkle(Mesajlar mesaj)
        {
            _mesajlarDal.Ekle(mesaj);
        }

        public void MesajGuncelle(Mesajlar mesaj)
        {
            _mesajlarDal.Guncelle(mesaj);
        }

        public void MesajSil(Mesajlar mesaj)
        {
            _mesajlarDal.Sil(mesaj);
        } 
        public Mesajlar GetMesajID(int mesajId)
        {
            throw new NotImplementedException();
        }
    }
}
