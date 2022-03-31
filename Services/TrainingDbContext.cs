using Training.Helpers;
using Training.Models;
//using ClosedXML.Excel;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Training.Services
{
    public class TrainingDbContext : DbContext
    {
        public DbSet<Profil> Profil { get; set; }

        // Liste des valeurs (Autre, Vélo, Course, Natation, Musculation)
        public DbSet<TypeActivite> TypeActivite { get; set; }

        // Liste des valeurs (Echauffement, Exercices, Récupération)
        // à l'interieur d'une activité
        public DbSet<TypeExercice> TypeExercice { get; set; }

        // Liste des valeurs (Echauffement, Exercices, RetourAuCalme, MesureHrr)
        public DbSet<PhaseActivite> PhaseActivite { get; set; }

        public DbSet<Utilisateur> Utilisateurs { get; set; }

        //public DbSet<OfflineMetadata> OfflineMetadata { get; set; }

        private readonly IServiceProvider _serviceProvider;
        //private readonly CurrentUserProvider _currentUtilisateurService;
        //private readonly UtilisateurNameService _utilisateurNameService;
        private readonly bool _localDB;

        [ActivatorUtilitiesConstructor]
        public TrainingDbContext(DbContextOptions options, IServiceProvider serviceProvider,
            //CurrentUserProvider currentUtilisateurService, 
            //////////////UtilisateurNameService utilisateurNameService, 
            IConfiguration configuration) : base(options)
        {
            _serviceProvider = serviceProvider;
            ///////_currentUtilisateurService = currentUtilisateurService;
            //_utilisateurNameService = utilisateurNameService;
            //_localDB = configuration?.OfflineMode() ?? false;
            _localDB = false;
            SavingChanges += TrainingDbContext_SavingChanges;
        }

        public TrainingDbContext(DbContextOptions options, bool localDB = false) : base(options)
        {
            _localDB = localDB;
            SavingChanges += TrainingDbContext_SavingChanges;
        }


        private void TrainingDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            //////if (_currentUtilisateurService == null)
            ///////    return;
            //var currentUserId = _currentUtilisateurService.GetCurrentUser().GetUtilisateurId();
            //foreach (var operationEntity in ChangeTracker.Entries<Operation>().Where(e => e.State == EntityState.Added))
            //    if (operationEntity.Entity.UtilisateurId == default)
            //            operationEntity.Entity.UtilisateurId = currentUserId;

        }
        private bool IsMigration { get; set; } = false;

        public class DesignTimeDbContextFactor : IDesignTimeDbContextFactory<TrainingDbContext>
        {
            public TrainingDbContext CreateDbContext(string[] args)
            {
                var optionBuilder = new DbContextOptionsBuilder<TrainingDbContext>();
                //var connectionString = "Data Source = (localdb)\\MSSQLLocalDB;MultipleActiveResultSets = True;Initial Catalog = Training;"
                var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build().GetConnectionString("TrainingDB");
                optionBuilder.UseSqlServer(connectionString);
                return new TrainingDbContext(optionBuilder.Options)
                {
                    IsMigration = true
                };
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //UNCOMMENT TO LOG SQL
            //optionsBuilder.LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if (!IsMigration && _localDB)
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var idProperty = entityType.FindProperty("Id");
                    if (idProperty == null || idProperty.ClrType == typeof(Guid))
                        continue;

                    idProperty.ValueGenerated = ValueGenerated.Never;
                }
            }
            else
            {
               /////////SeedData.Seed(modelBuilder);
            }
        }

        public void LoadUtilisateursWithName()
        {
            Utilisateurs.Load();
            //_utilisateurNameService?.LoadNames(Utilisateurs.Local);
        }
        public void LoadReferenceTables()
        {
            Profil.Load();
            TypeActivite.Load();
            TypeExercice.Load();
            PhaseActivite.Load();

            LoadUtilisateursWithName();
        }

        //public IQueryable<RevisionDechet> GetChildCandidate(Guid? revisionDechetId = null)
        //{
        //    var query = RevisionsDechet.GetLastRevisions()
        //        .Include(e => e.Dechet)
        //        .Include(e => e.Operation)
        //        .Where(e => e.Expedition == null && e.Etat.TypeEtat != TypeEtatDechet.Expedie)
        //        .Where(e => e.Etat.TypeEtat != TypeEtatDechet.Supprime)
        //        .Where(e => (e.AssemblagesDechetParent.Count == 0 ||
        //                e.AssemblagesDechetParent.Any(e => e.DechetParentId == revisionDechetId))
        //            && e.AssemblagesDechetEnfant.Count == 0);

        //    if (revisionDechetId != null)
        //        query = query.Where(e => e.Id != revisionDechetId);

        //    return query;
        //}


        //public IQueryable<RevisionDechet> GetLastRevisionsDechet(DateTime? dateTime = null)
        //{
        //    IQueryable<RevisionDechet> query;
        //    if (dateTime == null)
        //        query = RevisionsDechet.GetLastRevisions();
        //    else
        //        query = RevisionsDechet.GetLastRevisions(dateTime.Value);
        //    return query
        //            .Include(e => e.Dechet)
        //            .Include(e => e.Operation)
        //            .Include(e => e.AssemblagesDechetEnfant)
        //            .ThenInclude(e => e.DechetEnfant)
        //            .Where(e => e.AssemblagesDechetParent.Count == 0);
        //}

        //public IQueryable<RevisionDechet> GetLastRevisionsStockDechet(DateTime? dateTime = null)
        //{
        //    IQueryable<RevisionDechet> query;
        //    if (dateTime == null)
        //        query = RevisionsDechet.GetLastRevisions();
        //    else
        //        query = RevisionsDechet.GetLastRevisions(dateTime.Value);
        //    return query
        //            .Include(e => e.Dechet)
        //            .Include(e => e.Operation)
        //            .Include(e => e.AssemblagesDechetEnfant)
        //            .ThenInclude(e => e.DechetEnfant)
        //            .Where(e => e.AssemblagesDechetParent.Count == 0)
        //            .Where(e => e.BatimentProduction.Comportement444)
        //            .Where(e => e.Etat.TypeEtat != TypeEtatDechet.Expedie)
        //            .Where(e => e.Etat.TypeEtat != TypeEtatDechet.Supprime)
        //            .Where(e => e.Etat.TypeEtat != TypeEtatDechet.Assemble);
        //}


        //public void RemoveLock(TerminalHorsLigne terminal)
        //{
        //    foreach (var dechet in Dechets.Where(e => e.Verrou == terminal))
        //        dechet.Verrou = null;

        //    foreach (var effluent in Effluents.Where(e => e.Verrou == terminal))
        //        effluent.Verrou = null;

        //    foreach (var batimentEntreposage in BatimentsEntreposage.Where(e => e.Verrou == terminal))
        //        batimentEntreposage.Verrou = null;
        //}

        public bool ValidateContextWithNotification(ITrainingNotificationService notificationService)
        {
            using var scope = _serviceProvider.CreateScope();

            var changedEntities = ChangeTracker
            .Entries()
            .Where(_ => _.State == EntityState.Added ||
            _.State == EntityState.Modified);

            var results = new List<string>();
            foreach (var e in changedEntities)
            {
                var entity = e.Entity;
                var abstractValidatorType = typeof(IValidator<>).MakeGenericType(entity.GetType());

                if (scope.ServiceProvider.GetService(abstractValidatorType) is IValidator validator)
                {
                    var fluentValidationResult = validator.Validate(ValidationContext<object>.CreateWithOptions(entity, opt => opt.IncludeAllRuleSets()));
                    results.AddRange(fluentValidationResult.Errors.Select(e => e.ErrorMessage));
                }
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);
                results.AddRange(validationResults.Select(e => e.ErrorMessage));
            }

            if (results.Count == 0)
                return true;

            notificationService.Notify(results, NotificationSeverity.Error, "Erreur de validation");
            return false;
        }

        public bool SaveChangesWithNotification(ITrainingNotificationService notificationService)
        {
            string errorMessage;
            try
            {
                SaveChanges();
                return true;
            }
            catch (DbUpdateException dbe)
            {
                errorMessage = DbUpdateExceptionParser(dbe);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            notificationService.Notify(NotificationSeverity.Error, "Erreur lors de l'enregistrement en base", errorMessage);
            return false;
        }

        private static readonly Regex SqlErrorDuplicateRx = new(
            @"Cannot insert duplicate key row in object 'dbo\.(?:[^']+)' with unique index '(?:[^']+)'\. The duplicate key value is \(([^)]+)\)",
            RegexOptions.Compiled);


        private static string DbUpdateExceptionParser(DbUpdateException dbe)
        {
            var e = (SqlException)dbe.InnerException;
            switch (e.Number)
            {
                case 2601:
                    {
                        var match = SqlErrorDuplicateRx.Match(e.Message);
                        if (match.Success && match.Groups.Count == 2)
                        {
                            var value = match.Groups[1].Value;
                            return $"Impossible d'insérer une valeur dupliquée ({value})";
                        }
                    }
                    break;
            }
            return e.Message;
        }

    }

    public class HostnameInjectorInterceptor : DbConnectionInterceptor
    {
        private readonly string _hostname;
        public HostnameInjectorInterceptor(string hostname)
        {
            _hostname = hostname;
        }
        private DbCommand GetSetHostnameCommand(DbConnection connection)
        {
            using var command = connection.CreateCommand();
            var commandText = "EXEC sys.sp_set_session_context @key, @value";
            var name = new SqlParameter("@value", Environment.MachineName);
            command.Parameters.Add(new SqlParameter("@key", "Hostname"));
            command.Parameters.Add(new SqlParameter("@value", _hostname));
            command.CommandText = commandText;
            return command;
        }
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            GetSetHostnameCommand(connection).ExecuteNonQuery();
        }

        public override async Task ConnectionOpenedAsync(
    DbConnection connection,
    ConnectionEndEventData eventData,
    CancellationToken cancellationToken = default)
        {
            await GetSetHostnameCommand(connection).ExecuteNonQueryAsync(cancellationToken);
        }
    }
    public static class PlaningDbContextExtensionMethods
    {

        //public static IQueryable<TEntity> GetLastRevisions<TEntity>(this DbSet<TEntity> source, DateTime dateMax) where TEntity : class, IRevisionable
        //{
        //    var tableName = $"[{source.EntityType.GetTableName()}]";
        //    var soi = StoreObjectIdentifier.Table(source.EntityType.GetTableName(), null);
        //    var revisionGroupColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.RevisionGroupId)).GetColumnName(soi)}]";
        //    var revisionColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.Revision)).GetColumnName(soi)}]";
        //    var operationColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.OperationId)).GetColumnName(soi)}]";
        //    return source.FromSqlRaw(
        //        $"SELECT {tableName}.* FROM {tableName} INNER JOIN " +
        //            $"(SELECT {tableName}.{revisionGroupColumn} AS {revisionGroupColumn}, MAX({revisionColumn}) AS [maxRev] FROM {tableName} " +
        //                $"INNER JOIN [Operations] ON {tableName}.{operationColumn}=[Operations].[{nameof(Operation.Id)}] AND [Operations].[{nameof(Operation.DateTime)}] <= {{0}} " +
        //                $"GROUP BY {tableName}.{revisionGroupColumn}) AS [maxRevQuery] " +
        //           $"ON {tableName}.{revisionGroupColumn}=[maxRevQuery].{revisionGroupColumn} AND {tableName}.{revisionColumn}=[maxRevQuery].[maxRev]", dateMax);
        //}
        //public static IQueryable<TEntity> GetRevisionsBetween<TEntity>(this DbSet<TEntity> source, DateTime dateStart, DateTime dateEnd) where TEntity : class, IRevisionable
        //{
        //    var tableName = $"[{source.EntityType.GetTableName()}]";
        //    var soi = StoreObjectIdentifier.Table(source.EntityType.GetTableName(), null);
        //    var revisionGroupColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.RevisionGroupId)).GetColumnName(soi)}]";
        //    var revisionColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.Revision)).GetColumnName(soi)}]";
        //    var operationColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.OperationId)).GetColumnName(soi)}]";
        //    return source.FromSqlRaw(
        //        $"SELECT {tableName}.* FROM {tableName} INNER JOIN " +
        //            $"(SELECT {tableName}.{revisionGroupColumn} AS {revisionGroupColumn}, MAX({revisionColumn}) AS [maxRev], MIN({revisionColumn}) AS [minRev] FROM {tableName} " +
        //                $"INNER JOIN [Operations] ON {tableName}.{operationColumn}=[Operations].[{nameof(Operation.Id)}] AND [Operations].[{nameof(Operation.DateTime)}] > {{0}} AND [Operations].[{nameof(Operation.DateTime)}] <= {{1}} " +
        //                $"GROUP BY {tableName}.{revisionGroupColumn}) AS [revRangeQuery] " +
        //           $"ON {tableName}.{revisionGroupColumn}=[revRangeQuery].{revisionGroupColumn} AND {tableName}.{revisionColumn}<=[revRangeQuery].[maxRev] AND {tableName}.{revisionColumn}>=[revRangeQuery].[minRev]", dateStart, dateEnd);
        //}
        //public static IQueryable<TEntity> GetLastRevisions<TEntity>(this DbSet<TEntity> source) where TEntity : class, IRevisionable
        //{
        //    var tableName = $"[{source.EntityType.GetTableName()}]";
        //    var soi = StoreObjectIdentifier.Table(source.EntityType.GetTableName(), null);
        //    var revisionGroupColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.RevisionGroupId)).GetColumnName(soi)}]";
        //    var revisionColumn = $"[{source.EntityType.FindProperty(nameof(IRevisionable.Revision)).GetColumnName(soi)}]";
        //    return source.FromSqlRaw(
        //        $"SELECT {tableName}.* FROM {tableName} INNER JOIN " +
        //            $"(SELECT {tableName}.{revisionGroupColumn} AS {revisionGroupColumn}, MAX({revisionColumn}) AS [maxRev] FROM {tableName} " +
        //                $"GROUP BY {tableName}.{revisionGroupColumn}) AS [maxRevQuery] " +
        //           $"ON {tableName}.{revisionGroupColumn}=[maxRevQuery].{revisionGroupColumn} AND {tableName}.{revisionColumn}=[maxRevQuery].[maxRev]");
        //}

        /// <summary>
        /// <para>
        /// Filtre sur les éléments visibles (dons la propriété Visible est à true)
        /// </para><para>
        /// Exemple d'utilisation : peuplement d'une liste déroulante avec les valeurs visibles et la valeur actuelle si celle-ci a été marquée en invisible
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="value">Paramètre optionel permetant de forcer l'inclusion d'un valeur</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetFiltered<TEntity>(this IQueryable<TEntity> source, TEntity value = null) where TEntity : SoftDeletable
        {
            if (value != null)
                return source.Where(e => !e.Deleted || e == value);
            else
                return source.Where(e => !e.Deleted);
        }

        /// <inheritdoc cref="GetFiltered{TEntity}(IQueryable{TEntity}, TEntity)"/>
        public static IEnumerable<TEntity> GetFiltered<TEntity>(this IEnumerable<TEntity> source, TEntity value = null) where TEntity : SoftDeletable
            => GetFiltered(source.AsQueryable(), value);

    }
}


