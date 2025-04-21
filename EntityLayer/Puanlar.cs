using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public enum PuanTuru
    {
        Bonus = 0,
        Katilim = 1,  
        Olusturma = 2
            
    }

    public class Puanlar
    {
        [Key, Column(Order = 0)]
        public int KullaniciID { get; set; }

        [Key, Column(Order = 1)]
        public DateTime KazanilanTarih { get; set; }

        public PuanTuru PuanTuru { get; set; } 

        public virtual Kullanicilar Kullanicilar { get; set; }
    }
}
