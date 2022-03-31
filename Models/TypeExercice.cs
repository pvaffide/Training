using Training.Helpers;
using System;

namespace Training.Models
{
    public class TypeExercice : INameable
    {
        public int Id { get; set; }

        // Liste des valeurs (Echauffement, Exercices, Récupération)
        // à l'interieur d'une activité
        [RequiredA]
        [StringLengthA(20)]
        public string Nom { get; set; }

        public override string ToString() => Nom;
    }
}
