﻿using Amos.Abp.TempTable.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Amos.Abp.EntityFrameworkCore
{
    public static class ModelBuilderExtension
    {
        public static void AutoAddTempTableToModel<TDbContext>(this ModelBuilder modelBuilder) where TDbContext : IEfCoreDbContext
        {
            var tempTables = TempTableFinder.GetAutoAddTempTables(typeof(TDbContext));
            foreach (var tempTable in tempTables)
            {
                var isExists = modelBuilder.Model.FindEntityType(tempTable) != null;
                if (!isExists)
                {
                    modelBuilder.Model.AddEntityType(tempTable);
                    modelBuilder.Entity(tempTable, (b) =>
                    {
                        //b.ToView(null);
                        b.ToTable("#TempTable_" + tempTable.Name, (t) => t.ExcludeFromMigrations());
                        b.HasNoKey();
                        b.ConfigureByConvention();
                    });
                }
            }
        }
    }
}