//******************************************************************************************************
//  PluginFactory.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/11/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Reflection;

namespace FaultData
{
    public class PluginFactory<T>
    {
        public T Create(string assemblyName, string typeName, params object[] parameters)
        {
            if (assemblyName is null)
                throw new ArgumentNullException(nameof(assemblyName));

            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            try
            {
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type type = assembly.GetType(typeName);
                return Create(type, parameters);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create object of type {typeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }

        public T Create(Type type, params object[] parameters)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            Type targetType = typeof(T);

            if (!typeof(T).IsAssignableFrom(type))
                throw new ArgumentException($"{type.FullName} cannot be assigned to a reference of type {targetType.FullName}.");

            if (parameters.Length == 0 && type.GetConstructor(Type.EmptyTypes) is null)
                throw new ArgumentException($"{type.FullName} does not define a default constructor.", nameof(type));

            return (T)Activator.CreateInstance(type, parameters);
        }
    }
}
