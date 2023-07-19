//******************************************************************************************************
//  MATLABAnalytic.cs - Gbtc
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
//  03/27/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using MathWorks.MATLAB.NET.Arrays;

namespace openXDA.MATLAB
{
    public class MATLABAnalyticSettingField
    {
        public MATLABAnalyticSettingField(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }
    }

    public class MATLABAnalytic
    {
        #region [ Constructors ]

        public MATLABAnalytic(MATLABAnalysisFunctionInvokerFactory analysisFunctionInvokerFactory) =>
            AnalysisFunctionInvokerFactory = analysisFunctionInvokerFactory;

        #endregion

        #region [ Properties ]

        private MATLABAnalysisFunctionInvokerFactory AnalysisFunctionInvokerFactory { get; }

        #endregion

        #region [ Methods ]

        public List<MATLABAnalyticTag> Execute(VIDataGroup viDataGroup, IEnumerable<MATLABAnalyticSettingField> settings)
        {
            double samplesPerSecond = viDataGroup.Data
                .Select(dataSeries => dataSeries.SampleRate)
                .DefaultIfEmpty(1.0D)
                .First();

            // The MATLAB runtime must be initialized before MWArray can be used.
            // The invoker must be instantiated here as it will also instantiate the class that
            // contains the analysis function and initialize the MATLAB runtime as a side-effect
            using (IMATLABAnalysisFunctionInvoker invoker = AnalysisFunctionInvokerFactory())
            using (MWArray voltage = ToVoltageArray(viDataGroup))
            using (MWArray current = ToCurrentArray(viDataGroup))
            using (MWArray analog = new MWNumericArray(/* TODO not sure what belongs here */))
            using (MWArray fs = new MWNumericArray(samplesPerSecond))
            using (MWArray setting = ToSettingsArray(settings))
            {
                MWArray[] arrays = invoker.Invoke(1, voltage, current, analog, fs, setting);

                try
                {
                    int tagCount = arrays[0].Dimensions[1];
                    List<MATLABAnalyticTag> tags = new List<MATLABAnalyticTag>(tagCount);

                    if (tagCount == 0)
                        return tags;

                    MWStructArray tagsArray = arrays[0] as MWStructArray;

                    if (tagsArray is null)
                        throw new InvalidOperationException($"Invalid type for tags array: {arrays[0].ArrayType}");

                    // Struct arrays use a zero-based index for some reason
                    for (int i = 0; i < tagCount; i++)
                    {
                        MATLABAnalyticTag tag = MATLABAnalyticTag.Create(tagsArray, i);
                        tags.Add(tag);
                    }

                    return tags;
                }
                finally
                {
                    foreach (MWArray array in arrays)
                        array.Dispose();
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        private static MWArray ToVoltageArray(VIDataGroup viDataGroup)
        {
            DataSeries[] voltageSeries =
            {
                viDataGroup.VA,
                viDataGroup.VB,
                viDataGroup.VC,
                viDataGroup.VAB,
                viDataGroup.VBC,
                viDataGroup.VCA
            };

            return ToMWArray(voltageSeries);
        }

        private static MWArray ToCurrentArray(VIDataGroup viDataGroup)
        {
            DataSeries[] currentSeries =
            {
                viDataGroup.IA,
                viDataGroup.IB,
                viDataGroup.IC,
                viDataGroup.IR
            };

            return ToMWArray(currentSeries);
        }

        private static MWArray ToSettingsArray(IEnumerable<MATLABAnalyticSettingField> fields)
        {
            List<MWArray> structFields = new List<MWArray>();

            foreach (MATLABAnalyticSettingField field in fields)
            {
                structFields.Add(new MWCharArray(field.Name));
                structFields.Add(ToMWScalar(field.Value));
            }

            return new MWStructArray(structFields.ToArray());
        }

        private static MWArray ToMWScalar(object scalar)
        {
            if (scalar is string str)
                return new MWStringArray(str);

            if (scalar is bool b)
                return new MWLogicalArray(b);

            if (scalar is IConvertible convertible)
                return new MWNumericArray(convertible.ToDouble(null));

            return new MWObjectArray(scalar);
        }

        private static MWArray ToMWArray(DataSeries[] dataSeriesList)
        {
            double GetValue(DataSeries dataSeries, int index) =>
                (index <= dataSeries.DataPoints.Count)
                    ? dataSeries[index].Value
                    : double.NaN;

            int maxCount = dataSeriesList
                .Select(dataSeries => dataSeries.DataPoints.Count)
                .DefaultIfEmpty(0)
                .Max();

            double[] ToValues(int index) => dataSeriesList
                .Select(dataSeries => GetValue(dataSeries, index))
                .ToArray();

            double[][] allValues = Enumerable
                .Range(0, maxCount)
                .Select(ToValues)
                .ToArray();

            return new MWNumericArray(allValues);
        }

        #endregion
    }
}
