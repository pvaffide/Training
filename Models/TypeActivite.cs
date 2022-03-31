using Training.Helpers;
using System;

namespace Training.Models
{
    public class TypeActivite : INameable
    {
        public int Id { get; set; }

        // Liste des valeurs (Autre, Vélo, Course, Natation, Musculation)
        [RequiredA]
        [StringLengthA(20)]
        public string Nom { get; set; }

        public override string ToString() => Nom;
    }
}
