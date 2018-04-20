//******************************************************************************************************
//  DataObjectWrapper.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  01/14/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using FaultData.Configuration;
using FaultData.DataOperations;
using FaultData.DataReaders;
using log4net;

namespace openXDA
{
    /// <summary>
    /// Defines a wrapper around the given type to provide
    /// an identification number and the dispose pattern.
    /// </summary>
    /// <typeparam name="T">The type being wrapped.</typeparam>
    public class DataObjectWrapper<T> : IDisposable
    {
        #region [ Members ]

        // Fields
        private int m_id;
        private T m_dataObject;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="DataObjectWrapper{T}"/> class.
        /// </summary>
        /// <param name="id">The ID of the data object.</param>
        /// <param name="dataObjectType">The actual type of the data object to be instantiated.</param>
        public DataObjectWrapper(int id, Type dataObjectType)
        {
            Type targetType = typeof(T);

            if ((object)dataObjectType == null)
                throw new ArgumentNullException(nameof(dataObjectType));

            if (!typeof(T).IsAssignableFrom(dataObjectType))
                throw new ArgumentException($"{dataObjectType.FullName} cannot be assigned to a reference of type {targetType.FullName}.", nameof(dataObjectType));

            if ((object)dataObjectType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException($"{dataObjectType.FullName} does not define a default constructor.", nameof(dataObjectType));

            m_id = id;
            m_dataObject = (T)Activator.CreateInstance(dataObjectType);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets an integer ID for the data object.
        /// </summary>
        public int ID
        {
            get
            {
                return m_id;
            }
        }

        /// <summary>
        /// Gets the instance of the data object.
        /// </summary>
        public T DataObject
        {
            get
            {
                return m_dataObject;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DataObjectWrapper"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public void Dispose()
        {
            IDisposable dataObject;

            if (m_disposed)
                return;

            try
            {
                dataObject = m_dataObject as IDisposable;
                dataObject?.Dispose();
            }
            catch (Exception ex)
            {
                Type t = m_dataObject.GetType();
                string message = $"Exception occurred while disposing {t.FullName}: {ex.Message}";
                Log.Error(new Exception(message, ex));
            }
            finally
            {
                m_disposed = true;
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static ILog Log = LogManager.GetLogger(typeof(DataObjectWrapper<T>));

        // Static Methods

        /// <summary>
        /// Converts the given wrapper to its underlying data object.
        /// </summary>
        /// <param name="wrapper">The wrapper to be converted.</param>
        public static implicit operator T(DataObjectWrapper<T> wrapper)
        {
            return wrapper.DataObject;
        }

        #endregion
    }

    /// <summary>
    /// Defines a wrapper around the <see cref="IConfigurationLoader"/> interface.
    /// </summary>
    public class ConfigurationLoaderWrapper : DataObjectWrapper<IConfigurationLoader>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationLoaderWrapper"/> class.
        /// </summary>
        /// <param name="id">The ID of the configuration loader.</param>
        /// <param name="dataReaderType">The actual type of the configuration loader.</param>
        public ConfigurationLoaderWrapper(int id, Type configurationLoaderType)
            : base(id, configurationLoaderType)
        {
        }
    }

    /// <summary>
    /// Defines a wrapper around the <see cref="IDataReader"/> interface.
    /// </summary>
    public class DataReaderWrapper : DataObjectWrapper<IDataReader>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DataReaderWrapper"/> class.
        /// </summary>
        /// <param name="id">The ID of the data reader.</param>
        /// <param name="dataReaderType">The actual type of the data reader.</param>
        public DataReaderWrapper(int id, Type dataReaderType)
            : base(id, dataReaderType)
        {
        }
    }

    /// <summary>
    /// Defines a wrapper around the <see cref="IDataOperation"/> interface.
    /// </summary>
    public class DataOperationWrapper : DataObjectWrapper<IDataOperation>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DataOperationWrapper"/> class.
        /// </summary>
        /// <param name="id">The ID of the data operation.</param>
        /// <param name="dataOperationType">The actual type of the data operation.</param>
        public DataOperationWrapper(int id, Type dataOperationType)
            : base(id, dataOperationType)
        {
        }
    }
}
