﻿namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    /// <summary>
    ///  Defines whether and when the primary key should be created.
    /// </summary>
    public enum TempTablePrimaryKeyCreation
    {
        /// <summary>
        /// No primary key should be created.
        /// </summary>
        None,

        /// <summary>
        /// Primary key should be created before bulk insert.
        /// </summary>
        BeforeBulkInsert,

        /// <summary>
        /// Primary key should be created after bulk insert.
        /// </summary>
        AfterBulkInsert
    }
}
