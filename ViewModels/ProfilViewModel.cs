using Training.Helpers;
using Training.Models;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Training.ViewModels
{
    public class ProfilViewModel
    {
        public int Id { get; set; }
        public Profil Profil { get; set; }
        public bool Deleted { get; set; }

        [RequiredA]
        [Display(Name = "Nom du profil")]
        public string Nom { get; set; }

        [RequiredA]
        [Display(Name = "Prénom du profil")]
        public string Prenom { get; set; }

        [RequiredA]
        [Display(Name = "EMail")]
        public string EMail { get; set; }

       
        [StringLength(32)]
        [Display(Name = "Taille")]
        public Decimal Numero { get; set; }

        [Display(Name = "Taille")]
        [DisplayFormat(DataFormatString = "{0:0.###E0}")]
        public decimal? Taille { get; set; }

        [Display(Name = "Poids")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public decimal? Poids { get; set; }

        [Display(Name = "VMA")]
        [DisplayFormat(DataFormatString = "{0:0.###}")]
        public decimal? VMA { get; set; }

        [Display(Name = "FTP")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal? FTP { get; set; }

        [Display(Name = "FCMAX")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal? FCMax { get; set; }

        //public IList<Statistique> Statistiques { get; set; } = new List<Statistique>();
    }

    public class ProfilValidator : AbstractValidator<ProfilViewModel>
    {
        public ProfilValidator()
        {
            RuleFor(e => e.Nom).NotEmpty().WithMessage("Veuillez saisir au moins le nom");
            RuleFor(e => e.EMail).NotEmpty().WithMessage("Veuillez saisir au moins le EMail");
        }
    }

}
