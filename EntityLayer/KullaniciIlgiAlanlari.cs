using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class KullaniciIlgiAlanlari
    {
        [Key, Column(Order = 0)]
        public int KullaniciID { get; set; }

        [Key, Column(Order = 1)]
        public int IlgiAlaniID { get; set; }

        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual IlgiAlanlari IlgiAlanlari { get; set; }

    }
}
