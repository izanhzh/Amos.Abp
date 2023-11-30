namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    public class TempTableInsertOptions
    {
        private TempTableBulkExecutorOptions _tempTableBulkExecutorOptions;

        /// <summary>
        /// Options for bulk executor.
        /// </summary>
        public TempTableBulkExecutorOptions TempTableBulkExecutorOptions
        {
            get => _tempTableBulkExecutorOptions ?? (_tempTableBulkExecutorOptions = new TempTableBulkExecutorOptions());
            set => _tempTableBulkExecutorOptions = value;
        }

        private TempTableCreationOptions _tempTableCreationOptions;

        /// <summary>
        /// Options for creation of the temp table.
        /// Default is set to <c>true</c>.
        /// </summary>
        public TempTableCreationOptions TempTableCreationOptions
        {
            get => _tempTableCreationOptions ?? (_tempTableCreationOptions = new TempTableCreationOptions());
            set => _tempTableCreationOptions = value;
        }

        /// <summary>
        /// Creates a clustered primary key spanning all columns of the temp table after the bulk insert.
        /// Default is set to <c>true</c>.
        /// </summary>
        public TempTablePrimaryKeyCreation PrimaryKeyCreation { get; set; } = TempTablePrimaryKeyCreation.AfterBulkInsert;
    }
}
