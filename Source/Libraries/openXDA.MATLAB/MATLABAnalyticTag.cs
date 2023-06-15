//******************************************************************************************************
//  MATLABAnalyticTag.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  04/11/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using MathWorks.MATLAB.NET.Arrays;
using Newtonsoft.Json;

namespace openXDA.MATLAB
{
    public class MATLABAnalyticTag
    {
        public string Name { get; }
        public string JSONData { get; }

        public MATLABAnalyticTag(string name, string jsonData)
        {
            Name = name;
            JSONData = jsonData;
        }

        public static MATLABAnalyticTag Create(MWStructArray structArray, int tagIndex)
        {
            MWArray nameField = structArray.GetField("Name", tagIndex);
            string name = nameField?.ToString();

            if (name is null || !(nameField.IsCharArray || nameField.IsStringArray))
                return null;

            MWStructArray dataField = structArray.GetField("Data", tagIndex) as MWStructArray;

            if (dataField is null)
                return new MATLABAnalyticTag(name, null);

            using (StringWriter stringWriter = new StringWriter())
            {
                JsonTextWriter writer = new JsonTextWriter(stringWriter);
                WriteJSON(writer, dataField);
                return new MATLABAnalyticTag(name, stringWriter.ToString());
            }
        }

        private static void WriteJSON(JsonWriter writer, MWStructArray structArray)
        {
            void WriteEmptyObject()
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }

            int structCount = structArray.Dimensions[1];

            if (structCount > 1)
                writer.WriteStartArray();

            writer.WriteStartObject();

            // Fields in struct arrays use a zero-based index
            for (int i = 0; i < structCount; i++)
            {
                foreach (string fieldName in structArray.FieldNames)
                {
                    writer.WritePropertyName(fieldName);

                    MWArray field = structArray.GetField(fieldName, i);

                    if (field is MWStructArray childStructArray)
                        WriteJSON(writer, childStructArray);
                    else if (field is MWStringArray childStringArray)
                        WriteJSON(writer, childStringArray);
                    else if (field is MWCharArray childCharArray)
                        WriteJSON(writer, childCharArray);
                    else if (field is MWNumericArray childNumericArray)
                        WriteJSON(writer, childNumericArray);
                    else
                        WriteEmptyObject();
                }
            }

            writer.WriteEndObject();

            if (structCount > 1)
                writer.WriteEndArray();
        }

        private static void WriteJSON(JsonWriter writer, MWStringArray stringArray)
        {
            string[] strings = (string[])stringArray;

            if (strings.Length != 1)
                writer.WriteStartArray();

            foreach (string str in strings)
                writer.WriteValue(str);

            if (strings.Length != 1)
                writer.WriteEndArray();
        }

        private static void WriteJSON(JsonWriter writer, MWCharArray charArray)
        {
            string value = charArray.ToString();
            writer.WriteValue(value);
        }

        private static void WriteJSON(JsonWriter writer, MWNumericArray numericArray)
        {
            int[] dimensions = numericArray.Dimensions;

            if (dimensions.Any(dimension => dimension <= 0))
            {
                writer.WriteStartArray();
                writer.WriteEndArray();
                return;
            }

            int[] indices = Enumerable
                .Repeat(1, dimensions.Length)
                .ToArray();

            Func<double> readFunc = GetReadFunc(numericArray, indices);

            // Scalar values are represented by MATLAB as
            // a 2D matrix with one row and one column
            if (dimensions.Length == 2 && dimensions[0] == 1 && dimensions[1] == 1)
            {
                writer.WriteValue(readFunc());
                return;
            }

            void WriteJSON(int dimensionIndex)
            {
                Action writeAction = (dimensionIndex < dimensions.Length)
                    ? new Action(() => WriteJSON(dimensionIndex + 1))
                    : new Action(() => writer.WriteValue(readFunc()));

                writer.WriteStartArray();

                for (int i = 1; i <= dimensions[dimensionIndex]; i++)
                {
                    indices[dimensionIndex] = i;
                    writeAction();
                }

                writer.WriteEndArray();
            }

            int startDimension = 0;

            // 1-dimensional arrays are represented
            // by MATLAB as a 2D matrix with one row.
            // In that case we can skip the first dimension
            if (dimensions.Length == 2 && dimensions[0] == 1)
                startDimension++;

            WriteJSON(startDimension);
        }

        private static Func<double> GetReadFunc(MWNumericArray array, int[] indices)
        {
            switch (array.NumericType)
            {
                default:
                case MWNumericType.Double:
                    return () => array[indices].ToScalarDouble();

                case MWNumericType.Single:
                    return () => array[indices].ToScalarFloat();

                case MWNumericType.Int8:
                case MWNumericType.UInt8:
                    return () => array[indices].ToScalarByte();

                case MWNumericType.Int16:
                case MWNumericType.UInt16:
                    return () => array[indices].ToScalarShort();

                case MWNumericType.Int32:
                case MWNumericType.UInt32:
                    return () => array[indices].ToScalarInteger();

                case MWNumericType.Int64:
                case MWNumericType.UInt64:
                    return () => array[indices].ToScalarLong();
            }
        }
    }
}
