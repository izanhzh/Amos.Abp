using Microsoft.Data.SqlClient;
using System;

namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    public class TempTableBulkExecutorOptions
    {
        public TimeSpan? BulkCopyTimeout { get; set; }

        public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; }

        public int? BatchSize { get; set; }

        public bool EnableStreaming { get; set; } = true;

        public IEntityMembersProvider EntityMembersProvider { get; set; }
    }
}
