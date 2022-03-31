using Training.Helpers;
using System;

namespace Training.Models
{
    public class PhaseActivite : INameable
    {
        public int Id { get; set; }

        // Liste des valeurs (Echauffement, Exercices, RetourAuCalme, MesureHrr)
        [RequiredA]
        [StringLengthA(20)]
        public string Nom { get; set; }

        public override string ToString() => Nom;
    }
}
