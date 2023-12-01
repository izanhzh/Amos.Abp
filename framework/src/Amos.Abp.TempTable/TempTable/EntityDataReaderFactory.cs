using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Amos.Abp.TempTable
{
    /// <summary>
    /// Factory for <see cref="IEntityDataReader"/>.
    /// </summary>
    public class EntityDataReaderFactory : IEntityDataReaderFactory, ISingletonDependency
    {
        /// <inheritdoc />
        public IEntityDataReader Create<T>(IEnumerable<T> entities, IList<IProperty> properties)
           where T : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            return new EntityDataReader<T>(entities, properties);
        }
    }
}
