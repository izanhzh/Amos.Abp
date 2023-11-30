using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Data;

namespace Amos.Abp.TempTable.EntityFrameworkCore
{
    public interface IEntityDataReader : IDataReader
    {
        /// <summary>
        /// Gets the properties the reader is created for.
        /// </summary>
        /// <returns>A collection of <see cref="PropertyInfo"/>.</returns>
        IList<IProperty> Properties { get; }

        /// <summary>
        /// Gets the index of the provided <paramref name="property"/> that matches with the one of <see cref="IDataRecord.GetValue"/>.
        /// </summary>
        /// <param name="property">Property info to get the index for.</param>
        /// <returns>Index of the property.</returns>
        int GetPropertyIndex(IProperty property);
    }
}
