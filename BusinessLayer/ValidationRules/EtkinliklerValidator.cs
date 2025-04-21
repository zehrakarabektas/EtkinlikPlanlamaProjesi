using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class EtkinliklerValidator: AbstractValidator<Etkinlikler>
    {
        public EtkinliklerValidator()
        {
            RuleFor(x => x.EtkinlikAdi).NotEmpty().WithMessage("Etkinlik adı boş bırakılamaz.");
            RuleFor(x => x.EtkinlikAdi).Length(3, 100).WithMessage("Etkinlik adı 3 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.Aciklama).NotEmpty().WithMessage("Etkinlik açıklaması boş bırakılamaz.");
            RuleFor(x => x.Aciklama).MaximumLength(500).WithMessage("Etkinlik açıklaması en fazla 500 karakter olabilir.");

            RuleFor(x => x.EtkinlikTarih).NotEmpty().WithMessage("Etkinlik tarihi boş bırakılamaz.");
            RuleFor(x => x.EtkinlikTarih).GreaterThan(DateTime.Now).WithMessage("Etkinlik Tarihi bugünden sonra olmalıdır.");

            RuleFor(x => x.EtkinlikSaati).NotEmpty().WithMessage("Etkinlik Saati boş bırakılamaz.");

            //RuleFor(x => x.IlgiAlaniID).NotNull().WithMessage("Bir ilgi alanı seçmelisiniz.");
          
            RuleFor(x => x.EtkinlikSuresi).GreaterThan(0).WithMessage("Etkinlik Süresi sıfırdan büyük olmalıdır.");

            RuleFor(x => x.EtkinlikKonumu).NotEmpty().WithMessage("Etkinlik Konumu boş bırakılamaz.");
            RuleFor(x => x.EtkinlikKonumu).MaximumLength(255).WithMessage("Etkinlik Konumu en fazla 255 karakter olabilir.");

        }
    }
}
