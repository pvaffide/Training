using Training.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Training.Models
{
    public class Profil : SoftDeletable, INameable
    {
        public int Id { get; set; }

        // Liste des valeurs (Echauffement, Exercices, Récupération, MesureHrr)
        [RequiredA]
        [Display(Name = "Nom")]
        [StringLengthA(255)]
        public string Nom { get; set; }

        [RequiredA]
        [Display(Name = "Prénom")]
        [StringLengthA(255)]
        public string Prenom { get; set; }

        [RequiredA] [StringLengthA(255)]
        [Display(Name = "EMail")]
        public string EMail { get; set; } //Format 

        [RequiredA]
        [Display(Name = "Poids")]
        public int Poids { get; set; } // en Kg

        [Display(Name = "Taille")]
        public int Taille { get; set; } // en cm

        [RequiredA]
        [Display(Name = "VMA")]
        public decimal VMA { get; set; } //Vitesse Maximale Anaerobie

        [RequiredA]
        [Display(Name = "FTP (Watt)")]
        public int FTP { get; set; } // Watt

        [RequiredA]
        [Display(Name = "Fréquence Cardiaque Maximale")]
        public int FCMax { get; set; } // Frequence cardiaque maximale

        public override string ToString() => Nom;
    }
}
