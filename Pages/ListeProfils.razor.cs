using System;
using System.Linq;
using System.Collections.Generic;

using Training.Models;
using Training.Services;

namespace Training.Pages
{
    public partial class ListeProfils : IDisposable
	{
		IList<Profil> profils;
		DateTime? DateTime = null;
		TrainingDbContext dbContext;

        public void Dispose()
        {
            dbContext?.Dispose();
        }
        protected override void OnInitialized()
		{
			base.OnInitialized();
            dbContext = dbContextFactory.CreateDbContext();
            dbContext.LoadReferenceTables();
			RefreshGrid();
		}

		void RefreshGrid()
		{
			profils = dbContext.Profil.ToList();
		}
	}
}
