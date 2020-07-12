//******************************************************************************************************
//  StringExtensions.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/10/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FaultData.DataWriters.GTC.StringExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Applies string interpolation to the given format string at runtime.
        /// </summary>
        /// <typeparam name="T">The type of the object that defines the parameters for the format string.</typeparam>
        /// <param name="format">The format string to be interpolated.</param>
        /// <param name="parameters">The parameters that can be referenced by the format string.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding <paramref name="parameters"/>.</returns>
        /// <remarks>
        /// <para>
        /// This overload uses reflection to obtain the name and value of <paramref name="parameters"/>' properties
        /// to make those available to the interpolated format string by name. This can be used with user-defined
        /// types, but is mainly intended to be used with anonymous types.
        /// </para>
        ///
        /// <code>
        /// string format = "{hello} {world}!";
        /// var parameters = new { hello = "Hello", world = "World" };
        /// Console.WriteLine(format.Interpolate(parameters));
        /// </code>
        ///
        /// <para>
        /// If <paramref name="parameters"/> can be safely cast to <code>IEnumerable{KeyValuePair{string, object}}</code>,
        /// this function will skip the reflection and call <see cref="Interpolate(string, IEnumerable{KeyValuePair{string, object}})"/>
        /// directly.
        /// </para>
        /// </remarks>
        public static string Interpolate<T>(this string format, T parameters)
        {
            IEnumerable<KeyValuePair<string, object>> EnumerableParameters()
            {
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    string key = property.Name;
                    object value = property.GetValue(parameters) ?? "";
                    yield return new KeyValuePair<string, object>(key, value);
                }
            }

            IEnumerable<KeyValuePair<string, object>> enumerableParameters = parameters as IEnumerable<KeyValuePair<string, object>>;
            return format.Interpolate(enumerableParameters ?? EnumerableParameters());
        }

        /// <summary>
        /// Applies string interpolation to the given format string at runtime.
        /// </summary>
        /// <param name="format">The format string to be interpolated.</param>
        /// <param name="parameters">The parameters that can be referenced by the format string.</param>
        /// <returns>A copy of <paramref name="format"/> in which the format items have been replaced by the string representation of the corresponding <paramref name="parameters"/>.</returns>
        /// <remarks>
        /// <para>
        /// This overload is intended to be used for scenarios in which the parameters available to the format string
        /// are stored in a <see cref="Dictionary{TKey, TValue}"/> or <see cref="System.Dynamic.ExpandoObject"/>.
        /// Note that dynamic variables cannot be used when calling extension functions.
        /// </para>
        ///
        /// <code>
        /// string format = "{hello} {world}!";
        /// dynamic parameters = new ExpandoObject();
        /// parameters.hello = "Hello";
        /// parameters.world = "World";
        ///
        /// Console.WriteLine(format.Interpolate(parameters));                  // This raises a compiler error
        /// Console.WriteLine(format.Interpolate((ExpandoObject)parameters);    // This is okay
        /// Console.WriteLine(StringExtensions.Interpolate(format, parameters); // This is also okay
        /// </code>
        /// </remarks>
        public static string Interpolate(this string format, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            Lazy<List<KeyValuePair<string, object>>> lazyList = new Lazy<List<KeyValuePair<string, object>>>(parameters.ToList);

            Lazy<Dictionary<string, int>> lazyLookup = new Lazy<Dictionary<string, int>>(() => lazyList.Value
                .Select((parameter, Index) => new { parameter.Key, Index })
                .ToDictionary(obj => obj.Key, obj => obj.Index));

            string indexed = Regex.Replace(format, @"{{|{(?<arg>[^}:]+)(?::(?<fmt>[^}]+))?}", match =>
            {
                if (!match.Groups["arg"].Success)
                    return match.Value;

                string arg = match.Groups["arg"].Value;

                if (!lazyLookup.Value.TryGetValue(arg, out int index))
                    return match.Value;

                if (!match.Groups["fmt"].Success)
                    return $"{{{index}}}";

                string fmt = match.Groups["fmt"].Value;
                return $"{{{index}:{fmt}}}";
            });

            if (!lazyLookup.IsValueCreated)
                return format;

            object[] parameterValues = lazyList.Value
                .Select(parameter => parameter.Value)
                .ToArray();

            return string.Format(indexed, parameterValues);
        }
    }
}
