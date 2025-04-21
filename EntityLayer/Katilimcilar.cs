using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Katilimcilar
    {
        [Key, Column(Order = 0)]
        public int KullaniciID { get; set; }

        [Key, Column(Order = 1)]
        public int EtkinlikID { get; set; }
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual Etkinlikler Etkinlikler { get; set; }
    }
}
