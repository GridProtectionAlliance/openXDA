//******************************************************************************************************
//  DataPoint.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  05/15/2025 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace openXDA.Model
{
    /// <summary>
    /// Represents a single data point in a time series.
    /// </summary>
    public readonly struct DataPoint
    {
        #region [ Properties ]

        public DateTime Time { get; init; }
        public double Value { get; init; }

        #endregion

        #region [ Methods ]

        public DataPoint NewTime(DateTime time) => new()
        {
            Time = time,
            Value = Value
        };

        public DataPoint NewValue(double value) => new()
        {
            Time = Time,
            Value = value
        };

        public DataPoint Shift(TimeSpan timeShift) =>
            NewTime(Time + timeShift);

        public DataPoint Negate() =>
            NewValue(-Value);

        public DataPoint Add(DataPoint point)
        {
            if (Time != point.Time)
                throw new InvalidOperationException("Cannot add datapoints with mismatched times");

            return NewValue(Value + point.Value);
        }

        public DataPoint Subtract(DataPoint point) =>
            Add(point.Negate());

        public DataPoint Add(double value) =>
            NewValue(Value + value);

        public DataPoint Subtract(double value) =>
            Add(-value);

        public DataPoint Multiply(double value) =>
            NewValue(Value * value);

        public bool IsLargerThan(double comparison) =>
            Value > comparison;

        public bool IsLargerThan(DataPoint point) =>
            IsLargerThan(point.Value);

        [Obsolete("Use IsLargerThan() instead")]
        public bool LargerThan(double comparison) =>
            IsLargerThan(comparison);

        [Obsolete("Use IsLargerThan() instead")]
        public bool LargerThan(DataPoint point) =>
            IsLargerThan(point);

        #endregion
    }
}
