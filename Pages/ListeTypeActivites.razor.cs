using System;
using System.Collections.Generic;
using System.Linq;

using Training.Models;
using Training.Services;

namespace Training.Pages
{
    public partial class ListeTypeActivites : IDisposable
    {
        IList<TypeActivite> TypeActivites;

        TrainingDbContext dbContext;

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            dbContext = dbContextFactory.CreateDbContext();
            TypeActivites = dbContext.TypeActivite.ToList();
        }
    }
}