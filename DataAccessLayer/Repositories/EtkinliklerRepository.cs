using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EtkinliklerRepository : GenericRepository<Etkinlikler>, IEtkinliklerDal
    {
        public Task<int> GetEtkinlikSayisi()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Etkinlikler>> GetKategoriEtkinligi(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
