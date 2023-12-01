﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data;

namespace Amos.Abp.TempTable
{
    /// <summary>
    /// Data reader for Entity Framework Core entities.
    /// </summary>
    /// <typeparam name="T">Type of the entity.</typeparam>
    public sealed class EntityDataReader<T> : IEntityDataReader
       where T : class
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly Dictionary<int, Func<T, object>> _propertyGetterLookup;

        /// <inheritdoc />
        public IList<IProperty> Properties { get; }

        /// <inheritdoc />
        public int FieldCount => Properties.Count;

        /// <summary>
        /// Initializes <see cref="EntityDataReader{T}"/>
        /// </summary>
        /// <param name="entities">Entities to read.</param>
        /// <param name="properties">Properties to read.</param>
        public EntityDataReader(IEnumerable<T> entities, IList<IProperty> properties)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (properties.Count == 0)
                throw new ArgumentException("The properties collection cannot be empty.", nameof(properties));

            Properties = properties;
            _propertyGetterLookup = BuildPropertyGetterLookup(properties);
            _enumerator = entities.GetEnumerator();
        }

        private static Dictionary<int, Func<T, object>> BuildPropertyGetterLookup(IList<IProperty> properties)
        {
            var lookup = new Dictionary<int, Func<T, object>>();

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var getter = property.GetGetter();

                if (getter == null)
                    throw new ArgumentException($"The property '{property.Name}' of entity '{property.DeclaringEntityType.Name}' has no property getter.");

                lookup.Add(i, getter.GetClrValue);
            }

            return lookup;
        }

        /// <inheritdoc />
        public int GetPropertyIndex(IProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            var index = Properties.IndexOf(property);

            if (index >= 0)
                return index;

            throw new ArgumentException($"The property '{property.Name}' of type '{property.ClrType.FullName}' cannot be read by current reader.");
        }

        /// <inheritdoc />
        public object GetValue(int i)
        {
            return _propertyGetterLookup[i](_enumerator.Current);
        }

        /// <inheritdoc />
        public bool Read()
        {
            return _enumerator.MoveNext();
        }

        /// <inheritdoc />
        public bool IsDBNull(int i)
        {
            // we are reading entities (.NET objects), there must be no properties of type "DBNull".
            return false;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _enumerator.Dispose();
        }

        // The following methods are not needed for bulk insert.
        object IDataRecord.this[int i] => throw new NotSupportedException();
        object IDataRecord.this[string name] => throw new NotSupportedException();
        int IDataReader.Depth => throw new NotSupportedException();
        int IDataReader.RecordsAffected => throw new NotSupportedException();
        bool IDataReader.IsClosed => throw new NotSupportedException();
        void IDataReader.Close() => throw new NotSupportedException();
        string IDataRecord.GetName(int i) => throw new NotSupportedException();
        string IDataRecord.GetDataTypeName(int i) => throw new NotSupportedException();
        Type IDataRecord.GetFieldType(int i) => throw new NotSupportedException();
        int IDataRecord.GetValues(object[] values) => throw new NotSupportedException();
        int IDataRecord.GetOrdinal(string name) => throw new NotSupportedException();
        bool IDataRecord.GetBoolean(int i) => throw new NotSupportedException();
        byte IDataRecord.GetByte(int i) => throw new NotSupportedException();
        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length) => throw new NotSupportedException();
        char IDataRecord.GetChar(int i) => throw new NotSupportedException();
        long IDataRecord.GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length) => throw new NotSupportedException();
        Guid IDataRecord.GetGuid(int i) => throw new NotSupportedException();
        short IDataRecord.GetInt16(int i) => throw new NotSupportedException();
        int IDataRecord.GetInt32(int i) => throw new NotSupportedException();
        long IDataRecord.GetInt64(int i) => throw new NotSupportedException();
        float IDataRecord.GetFloat(int i) => throw new NotSupportedException();
        double IDataRecord.GetDouble(int i) => throw new NotSupportedException();
        string IDataRecord.GetString(int i) => throw new NotSupportedException();
        decimal IDataRecord.GetDecimal(int i) => throw new NotSupportedException();
        DateTime IDataRecord.GetDateTime(int i) => throw new NotSupportedException();
        IDataReader IDataRecord.GetData(int i) => throw new NotSupportedException();
        DataTable IDataReader.GetSchemaTable() => throw new NotSupportedException();
        bool IDataReader.NextResult() => throw new NotSupportedException();
    }
}
