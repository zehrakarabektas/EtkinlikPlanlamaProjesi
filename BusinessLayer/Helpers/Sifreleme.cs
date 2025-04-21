using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Sifreleme
    {
        public string Sifrele(string sifre)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] baytlar = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(sifre));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in baytlar)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

    }
}
