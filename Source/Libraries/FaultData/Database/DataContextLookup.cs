//******************************************************************************************************
//  DataContextLookup.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/01/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace FaultData.Database
{
    /// <summary>
    /// Represents a lookup table, backed by a database table, that can be
    /// accessed and modified in a thread-safe manner across an application
    /// to prevent duplication of keys in the database table.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the lookup table.</typeparam>
    /// <typeparam name="TValue">The type of the values in the lookup table.</typeparam>
    public class DataContextLookup<TKey, TValue> where TValue : class
    {
        #region [ Members ]

        // Fields
        private DataContext m_dataContext;
        private Func<TValue, TKey> m_keyFunction;
        private Dictionary<TKey, TValue> m_lookupTable;

        private List<Expression<Func<TValue, bool>>> m_filterExpressions;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="DataContextLookup{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context to be used to access the database table.</param>
        /// <param name="keyFunction">The function to generate keys from values that were loaded from the database.</param>
        public DataContextLookup(DataContext dataContext, Func<TValue, TKey> keyFunction)
        {
            if ((object)dataContext == null)
                throw new ArgumentNullException("dataContext");

            if ((object)keyFunction == null)
                throw new ArgumentNullException("keyFunction");

            m_dataContext = dataContext;
            m_keyFunction = keyFunction;
            m_filterExpressions = new List<Expression<Func<TValue, bool>>>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the value associated with the given key.
        /// </summary>
        /// <param name="key">The key of the item to be retrieved from the lookup table.</param>
        /// <returns>The value associated with the given key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return LookupTable[key];
            }
        }

        private Dictionary<TKey, TValue> LookupTable
        {
            get
            {
                if ((object)m_lookupTable == null)
                    m_lookupTable = CreateLookupTable();

                return m_lookupTable;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds a filter expression to the lookup table to filter the results of the database query.
        /// </summary>
        /// <param name="filterExpression">The expression used to filter the results of the database query.</param>
        /// <returns>The lookup table.</returns>
        public DataContextLookup<TKey, TValue> WithFilterExpression(Expression<Func<TValue, bool>> filterExpression)
        {
            m_filterExpressions.Add(filterExpression);
            return this;
        }

        /// <summary>
        /// Attempts to get the value associated with the given key.
        /// </summary>
        /// <param name="key">The key of the item to be retrieved from the database.</param>
        /// <param name="value">The value associated with the given key or null if the key is not found in the lookup table.</param>
        /// <returns>True if the given key is found in the lookup table; false otherwise.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return LookupTable.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the value associated with the given key or adds a new value to
        /// the lookup table and database if no such key exists in the database.
        /// </summary>
        /// <param name="key">The key of the item to be retrieved from the database.</param>
        /// <param name="valueFactory">The function used to create a new value for the given key if the key does not exist in the database.</param>
        /// <returns>The value associated with the given key.</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;

            // Attempt to get the value from the existing
            // lookup before taking the table lock
            if (TryGetValue(key, out value))
                return value;

            lock (TableLock)
            {
                // Refresh the lookup table with the
                // latest data from the database
                m_lookupTable = CreateLookupTable();

                if (!TryGetValue(key, out value))
                {
                    // Create a new value via the value factory
                    value = valueFactory(key);

                    // Update the internal lookup table
                    m_lookupTable.Add(key, value);

                    // Update the database with the new value
                    m_dataContext.GetTable<TValue>().InsertOnSubmit(value);
                    m_dataContext.SubmitChanges();
                }
            }

            return value;
        }

        private Dictionary<TKey, TValue> CreateLookupTable()
        {
            IQueryable<TValue> table = m_dataContext.GetTable<TValue>();

            return m_filterExpressions
                .Aggregate(table, (current, expression) => current.Where(expression))
                .ToDictionary(m_keyFunction);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object TableLock = new object();

        #endregion
    }
}
