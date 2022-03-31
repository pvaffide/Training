using Microsoft.EntityFrameworkCore;
using System;

namespace Training.Models
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            {
                var builder = modelBuilder.Entity<TypeActivite>();
                // Autre, Vélo, Course, Natation, Musculation
                int id = 1;
                //TypeActivite typeActivite = new TypeActivite { Id = id++, Nom = "Autre" };
                builder.HasData(new TypeActivite { Id = id++, Nom = "Autre" });
                builder.HasData(new TypeActivite { Id = id++, Nom = "Vélo" });
                builder.HasData(new TypeActivite { Id = id++, Nom = "Course" });
                builder.HasData(new TypeActivite { Id = id++, Nom = "Natation" });
                builder.HasData(new TypeActivite { Id = id++, Nom = "Musculation" });
            }
            {
                // Liste des valeurs (Echauffement, Exercices, Récupération)
                // à l'interieur d'une activité
                var builder = modelBuilder.Entity<TypeExercice>();

                int id = 1;
                builder.HasData(new TypeExercice { Id = id++, Nom = "Echauffement" });
                builder.HasData(new TypeExercice { Id = id++, Nom = "Exercices" });
                builder.HasData(new TypeExercice { Id = id++, Nom = "Récupération" });
            }
            {
                // Liste des valeurs (Echauffement, Exercices, RetourAuCalme, MesureHrr)
                var builder = modelBuilder.Entity<PhaseActivite>();

                int id = 1;
                builder.HasData(new PhaseActivite { Id = id++, Nom = "Echauffement" });
                builder.HasData(new PhaseActivite { Id = id++, Nom = "Exercices" });
                builder.HasData(new PhaseActivite { Id = id++, Nom = "RetourAuCalme" });
                builder.HasData(new PhaseActivite { Id = id++, Nom = "MesureHrr" });
            }
            {
                var builder = modelBuilder.Entity<Profil>();

                int id = 1;
                builder.HasData(new Profil { Id = id++, Nom = "VAFFIDES", Prenom = "Patrick", EMail="pvaffides@gmail.com", Poids = 76, Taille=174, VMA=320, FTP=17, FCMax=165 });
                builder.HasData(new Profil { Id = id++, Nom = "FORTINI", Prenom = "Sophie", EMail = "sophiefortini@sfr.fr", Poids = 53, Taille = 168, VMA = 220, FTP = 14, FCMax = 160 });
                //builder.HasData(new Profil { Id = id++, Nom = "NT0300", LocalCuveId = 1 });
            }
            
            {
                var builder = modelBuilder.Entity<Utilisateur>();

                int id = 1;
                foreach (var username in new string[] { "fmartin", "florent", "chequet", "nabaoudene", "pvaffides", "rartus", "asavalli", "crenous" })
                {
                    byte[] sid;
                    try
                    {
                        sid = GetSidFromUsername(username);
                    }
                    catch
                    {
                        continue;
                    }

                    builder.HasData(new Utilisateur
                    {
                        Id = id++,
                        UserSid = sid,
                        NameHint = username,
                        Role = RoleUtilisateur.Supervisor
                    });
                }
            }

            
        }
        private static byte[] GetSidFromUsername(string username)
        {
            var securityIdentifier = (System.Security.Principal.SecurityIdentifier)
               new System.Security.Principal.NTAccount(username).Translate(typeof(System.Security.Principal.SecurityIdentifier));
            var sid = new byte[securityIdentifier.BinaryLength];
            securityIdentifier.GetBinaryForm(sid, 0);
            return sid;
        }
    }
}
