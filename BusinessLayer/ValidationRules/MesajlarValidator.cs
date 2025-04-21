using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class MesajlarValidator : AbstractValidator<Mesajlar>
    {
        public MesajlarValidator()
        {
            RuleFor(x => x.GondericiID).NotEmpty().WithMessage("Gönderici ID boş bırakılamaz.");

            RuleFor(x => x.AliciID).NotEmpty().WithMessage("Alıcı ID boş bırakılamaz.");

            RuleFor(x => x.MesajMetni)
                .NotEmpty().WithMessage("Mesaj metni boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Mesaj metni 500 karakterden fazla olamaz.");

            RuleFor(x => x.GonderimZamani).NotEmpty().WithMessage("Gönderim zamanı boş bırakılamaz.");
        }
    }
}
