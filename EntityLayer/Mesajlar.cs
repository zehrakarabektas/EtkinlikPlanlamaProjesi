using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Mesajlar
    {
        [Key]
        public int MesajID { get; set; }
        public int GondericiID { get; set; }
        public int AliciID { get; set; }

        [StringLength(500)]
        public string MesajMetni { get; set; }
        public DateTime GonderimZamani { get; set; }
        public virtual Kullanicilar Gonderici { get; set; }
        public virtual Kullanicilar Alici { get; set; }
    }
}
