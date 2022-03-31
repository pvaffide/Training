using Training.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Models
{

/// <summary>
/// Droit incrémental : un role hérite des droits inférieurs
/// </summary>
    public enum RoleUtilisateur
    {
        [Display(Name = "Lecteur")]
        Reader = 0,
        [Display(Name = "Utilisateur")]
        Writer = 10,
        [Display(Name = "Superviseur")]
        Supervisor = 100
    }
    public class Utilisateur : INameable
    {
        private string _nom;

        public int Id { get; set; }
        [RequiredA]
        [MaxLength(85)]
        public byte[] UserSid { get; set; }

        [NotMapped]
        [Display(Name = "Utilisateur")]
        public string Nom { get => _nom ?? NameHint; set => _nom = value; }

        [MaxLength(255)]
        public string NameHint { get; set; }

        public RoleUtilisateur Role { get; set; }
        public override string ToString() => Nom;
    }

    class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
    {
        public void Configure(EntityTypeBuilder<Utilisateur> builder)
        {
            builder.HasIndex(e => e.UserSid).IsUnique();

            builder.Property(e => e.Role);
        }
    }
}
