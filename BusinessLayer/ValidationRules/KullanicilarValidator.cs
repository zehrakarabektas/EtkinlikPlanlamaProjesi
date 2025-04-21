using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class KullanicilarValidator: AbstractValidator<Kullanicilar>
    {
        public KullanicilarValidator()
        {

            //RuleFor(x => x.KullaniciAdi).NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.");
            //RuleFor(x => x.KullaniciAdi).Length(1, 100).WithMessage("Kullanıcı adı en fazla 100 karakter olabilir.");
            //RuleFor(x => x.KullaniciAdi).Matches("^[a-zA-Z0-9]*$").WithMessage("Kullanıcı adı sadece harf ve rakamlardan oluşmalıdır.");

            //RuleFor(x => x.Eposta).NotEmpty().WithMessage("E-posta alanını boş geçemezsiniz.");
            //RuleFor(x => x.Eposta).EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
            //RuleFor(x => x.Eposta).MaximumLength(100).WithMessage("E-posta 100 karakterden uzun olamaz.");

            ////RuleFor(x => x.Sifre).NotEmpty().WithMessage("Şifre alanını boş geçemezsiniz.");
            //RuleFor(x => x.Sifre).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
            //RuleFor(x => x.Sifre).MaximumLength(25).WithMessage("Şifre 25 karakterden uzun olamaz.");

            //RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş bırakılamaz.");
            //RuleFor(x => x.Ad).Length(1, 50).WithMessage("Ad en fazla 50 karakter olabilir.");

            //RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad boş bırakılamaz.");
            //RuleFor(x => x.Soyad).Length(1, 50).WithMessage("Soyad en fazla 50 karakter olabilir.");

            //RuleFor(x => x.TelefonNumarasi).Matches(@"^\d{10}$").WithMessage("Telefon numarası 10 haneli olmalıdır.");

            //RuleFor(x => x.ProfilFotografi).Length(0, 255).WithMessage("Profil fotoğrafı yolu en fazla 255 karakter olabilir.");
            ////RuleFor(x => x.ProfilFotografi).Must(IsAllowedExtension).WithMessage("Profil fotoğrafı sadece .jpg, .jpeg, .png, .gif uzantılı olabilir.");

            //RuleFor(x => x.Konum).MaximumLength(255).WithMessage("Konum 255 karakterden uzun olamaz.");

            //RuleFor(x => x.DogumTarihi).NotEmpty().WithMessage("Doğum tarihi boş bırakılamaz.");

            //RuleFor(x => x.Cinsiyet).NotEmpty().WithMessage("Cinsiyet boş bırakılamaz.");
            //RuleFor(x => x.Cinsiyet).Length(1, 10).WithMessage("Cinsiyet 1 ile 10 karakter arasında olmalıdır.");

        }
    } 
}
