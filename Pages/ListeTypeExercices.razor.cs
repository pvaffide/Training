using System;
using System.Collections.Generic;
using System.Linq;

using Training.Models;
using Training.Services;

namespace Training.Pages
{
    public partial class ListeTypeExercices : IDisposable
    {
        IList<TypeExercice> TypeExercices;

        TrainingDbContext dbContext;

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            dbContext = dbContextFactory.CreateDbContext();
            TypeExercices = dbContext.TypeExercice.ToList();
        }
    }
}