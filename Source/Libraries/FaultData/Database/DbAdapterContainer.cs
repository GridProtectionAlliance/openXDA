//******************************************************************************************************
//  DbAdapterContainer.cs - Gbtc
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
//  02/24/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GSF.Collections;

namespace FaultData.Database
{
    public sealed class DbAdapterContainer : IDisposable
    {
        #region [ Members ]

        // Fields
        private Dictionary<Type, object> m_adapters;
        private SqlConnection m_connection;
        private int m_commandTimeout;

        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public DbAdapterContainer(string connectionString)
        {
            m_adapters = new Dictionary<Type, object>();
            m_connection = new SqlConnection(connectionString);
            m_connection.Open();
        }

        public DbAdapterContainer(SqlConnection connection)
        {
            m_connection = connection;
        }

        public DbAdapterContainer(string connectionString, int commandTimeout)
            : this(connectionString)
        {
            m_commandTimeout = commandTimeout;
        }

        public DbAdapterContainer(SqlConnection connection, int commandTimeout)
            : this(connection)
        {
            m_commandTimeout = commandTimeout;
        }

        #endregion

        #region [ Properties ]

        public SqlConnection Connection
        {
            get
            {
                return m_connection;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return m_commandTimeout;
            }
        }

        #endregion

        #region [ Methods ]

        public T GetAdapter<T>()
        {
            return (T)m_adapters.GetOrAdd(typeof(T), type =>
            {
                Func<SqlConnection, int, object> factory = AdapterFactories.GetOrAdd(type, type1 =>
                {
                    if (typeof(DataContext).IsAssignableFrom(type))
                        return GetDataContextFactory(type);

                    return GetAdapterFactory<T>(type);
                });

                return factory(m_connection, m_commandTimeout);
            });
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                Dispose(m_connection);

                foreach (object adapter in m_adapters.Values)
                    Dispose(adapter as IDisposable);

                m_disposed = true;
            }
        }

        private Func<SqlConnection, int, object> GetDataContextFactory(Type type)
        {
            ConstructorInfo constructor;

            List<ParameterExpression> parameterExpressions;
            NewExpression newExpression;
            LambdaExpression lambdaExpression;

            Func<SqlConnection, object> factory;

            constructor = type.GetConstructor(ConstructorTypes);

            if ((object)constructor == null)
                throw new InvalidOperationException(string.Format("\"{0}\" is not a valid adapter type", type.FullName));

            parameterExpressions = ConstructorTypes.Select(Expression.Parameter).ToList();
            newExpression = Expression.New(constructor, parameterExpressions);
            lambdaExpression = Expression.Lambda(typeof(Func<SqlConnection, object>), newExpression, parameterExpressions);

            factory = (Func<SqlConnection, object>)lambdaExpression.Compile();

            return (connection, commandTimeout) =>
            {
                DataContext dataContext = (DataContext)factory(connection);
                dataContext.CommandTimeout = commandTimeout;
                return dataContext;
            };
        }

        private Func<SqlConnection, int, object> GetAdapterFactory<T>(Type type)
        {
            ConstructorInfo constructor;

            PropertyInfo connectionProperty;
            PropertyInfo adapterProperty;
            PropertyInfo commandCollectionProperty;

            Func<T, SqlConnection, SqlConnection> connectionSetter;
            Func<T, SqlDataAdapter> adapterGetter;
            Func<T, SqlCommand[]> commandCollectionGetter;

            constructor = type.GetConstructor(Type.EmptyTypes);

            if ((object)constructor == null)
                throw new InvalidOperationException(string.Format("\"{0}\" is not a valid adapter type", type.FullName));

            connectionProperty = type.GetProperty("Connection", PropertyFlags);

            if ((object)connectionProperty == null || connectionProperty.PropertyType != typeof(SqlConnection))
                throw new InvalidOperationException(string.Format("\"{0}\" is not a valid adapter type", type.FullName));

            adapterProperty = type.GetProperty("Adapter", PropertyFlags);

            if ((object)adapterProperty == null || adapterProperty.PropertyType != typeof(SqlDataAdapter))
                throw new InvalidOperationException(string.Format("\"{0}\" is not a valid adapter type", type.FullName));

            commandCollectionProperty = type.GetProperty("CommandCollection", PropertyFlags);

            if ((object)commandCollectionProperty == null || commandCollectionProperty.PropertyType != typeof(SqlCommand[]))
                throw new InvalidOperationException(string.Format("\"{0}\" is not a valid adapter type", type.FullName));

            connectionSetter = (Func<T, SqlConnection, SqlConnection>)CompileSetter(type, connectionProperty);
            adapterGetter = (Func<T, SqlDataAdapter>)CompileGetter(type, adapterProperty);
            commandCollectionGetter = (Func<T, SqlCommand[]>)CompileGetter(type, commandCollectionProperty);

            return (connection, commandTimeout) =>
            {
                T adapter;
                SqlDataAdapter dataAdapter;
                SqlCommand[] commandCollection;

                adapter = (T)Activator.CreateInstance(type);

                connectionSetter(adapter, connection);
                dataAdapter = adapterGetter(adapter);
                commandCollection = commandCollectionGetter(adapter);

                foreach (SqlCommand command in commandCollection)
                    command.CommandTimeout = commandTimeout;

                dataAdapter.InsertCommand.CommandTimeout = commandTimeout;
                dataAdapter.UpdateCommand.CommandTimeout = commandTimeout;
                dataAdapter.DeleteCommand.CommandTimeout = commandTimeout;

                return adapter;
            };
        }

        private Delegate CompileGetter(Type type, PropertyInfo property)
        {
            ParameterExpression instanceParameter = Expression.Parameter(type);
            MemberExpression propertyExpression = Expression.Property(instanceParameter, property);
            return Expression.Lambda(propertyExpression, instanceParameter).Compile();
        }

        private Delegate CompileSetter(Type type, PropertyInfo property)
        {
            ParameterExpression instanceParameter = Expression.Parameter(type);
            ParameterExpression valueParameter = Expression.Parameter(property.PropertyType);
            MemberExpression propertyExpression = Expression.Property(instanceParameter, property);
            BinaryExpression assignmentExpression = Expression.Assign(propertyExpression, valueParameter);
            return Expression.Lambda(assignmentExpression, instanceParameter, valueParameter).Compile();
        }

        private void Dispose(IDisposable obj)
        {
            if ((object)obj != null)
                obj.Dispose();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private const BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly Type[] ConstructorTypes = { typeof(SqlConnection) };
        private static readonly ConcurrentDictionary<Type, Func<SqlConnection, int, object>> AdapterFactories = new ConcurrentDictionary<Type, Func<SqlConnection, int, object>>();

        #endregion
    }
}
