using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IIlgiAlanlariService
    {
        List<IlgiAlanlari> GetAll();
        void IlgiAlaniEkle(IlgiAlanlari ilgiAlani);
        void IlgiAlaniGuncelle(IlgiAlanlari ilgiAlani);
        void IlgiAlaniSil(IlgiAlanlari ilgiAlani);
        IlgiAlanlari GetIlgiAlaniID(int ilgiAlaniID);
      
    }
}
