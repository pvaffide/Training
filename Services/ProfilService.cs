using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Training.Helpers;
using Training.Models;
using Training.ViewModels;

namespace Training.Services
{
    public class ProfilService
    {
        private readonly IMapper _mapper;
        private readonly ITrainingNotificationService _trainingNotificationService;
        IDbContextFactory<TrainingDbContext> _dbContextFactory;

        public ProfilService(IMapper mapper, ITrainingNotificationService trainingNotificationService, IDbContextFactory<TrainingDbContext> dbContextFactory)
        {
            _mapper = mapper;
            _trainingNotificationService = trainingNotificationService;
            _dbContextFactory = dbContextFactory;
        }

        public ProfilViewModel GetProfilViewModel(int profilId, bool suppression = false)
        {
            ProfilViewModel profilViewModel;
            Profil profil = null;

            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.LoadReferenceTables();

            profil = dbContext.Profil.SingleOrDefault(e => e.Id == profilId);
            if (profil == null)
                throw new NotFoundException(nameof(profilId));

            //if (suppression && !((Profil)profil).EstSupprimable())
            //    throw new Exception("Profil non modifiable");

            profilViewModel = new ProfilViewModel { Id = profilId, Profil = profil, Nom = profil.Nom
            , Prenom = profil.Prenom, EMail = profil.EMail }; 
            //_mapper.Map<ProfilViewModel>(((Profil)profil));
            
            //profilViewModel.Supprimable = ((Profil)profil).EstSupprimable();
            return profilViewModel;
        }


        public bool TrySaveProfilViewModel(ProfilViewModel profilViewModel)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var profil = _mapper.Map<Profil>(profilViewModel);
            
            profil.Id = default;
            dbContext.Attach(profil);

            return dbContext.SaveChangesWithNotification(_trainingNotificationService);
        }

        
    }


    public static class ProfilExtensionMethods
    {
        public static bool EstSupprimable(this Profil profil)
        {
            return true;
        }
        public static bool EstModifiable(this Profil profil)
        {
            return true;
        }
    }
}
