using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Context : DbContext
    {
        public DbSet<EtkinlikKategori> EtkinlikKategoris { get; set; }
        public DbSet<Etkinlikler> Etkinliklers { get; set; }
        public DbSet<IlgiAlanlari> IlgiAlanlaris { get; set; }
        public DbSet<Katilimcilar> Katilimcilars { get; set; }
        public DbSet<KullaniciIlgiAlanlari> KullaniciIlgiAlanlaris { get; set; }
        public DbSet<Kullanicilar> Kullanicilars { get; set; }
        public DbSet<Mesajlar> Mesajlars { get; set; }
        public DbSet<Puanlar> Puanlars { get; set; }
        public DbSet<EtkinlikMesaj> EtkinlikMesajs { get; set; }
        public DbSet<Bildirimler> Bildirimler { get; set; }


    }
}
