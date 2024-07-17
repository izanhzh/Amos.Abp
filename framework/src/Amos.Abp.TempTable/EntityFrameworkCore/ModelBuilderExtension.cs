using Amos.Abp.TempTable;
using Amos.Abp.TempTable.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Note: Call it before base.OnModelCreating
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="_"></param>
        public static void AutoAddTempTableToModel<TDbContext>(this ModelBuilder modelBuilder, TDbContext _) where TDbContext : IEfCoreDbContext
        {
            var tempTables = TempTableFinder.GetAutoAddTempTables(typeof(TDbContext));
            foreach (var tempTable in tempTables)
            {
                modelBuilder.AddTempTableToModel(tempTable);
            }
        }

        /// <summary>
        /// Note: Call it before base.OnModelCreating
        /// </summary>
        /// <typeparam name="TTempTable"></typeparam>
        /// <param name="modelBuilder"></param>
        public static void AddTempTableToModel<TTempTable>(this ModelBuilder modelBuilder) where TTempTable : ITempTable
        {
            modelBuilder.AddTempTableToModel(typeof(TTempTable));
        }

        internal static void AddTempTableToModel(this ModelBuilder modelBuilder, Type tempTableType)
        {
            var isExists = modelBuilder.Model.FindEntityType(tempTableType) != null;
            if (!isExists)
            {
                modelBuilder.Model.AddEntityType(tempTableType);
                modelBuilder.Entity(tempTableType, (b) =>
                {
                    //b.ToView(null);
                    b.ToTable("TempTable_" + tempTableType.Name, (t) => t.ExcludeFromMigrations());
                    b.HasNoKey();
                    b.ConfigureByConvention();
                });
            }
        }
    }
}
