//******************************************************************************************************
//  DataPoint.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/01/2014 - Stephen C. Wills
//       Generated original version of source code.
//  07/09/2019 - Christoph Lackner
//       Added value comparison.
//
//******************************************************************************************************

using System;

namespace FaultData.DataAnalysis
{
    public class DataPoint
    {
        public DateTime Time;
        public double Value;

        public DataPoint Shift(TimeSpan timeShift)
        {
            return new DataPoint()
            {
                Time = Time.Add(timeShift),
                Value = Value
            };
        }

        public DataPoint Negate()
        {
            return new DataPoint()
            {
                Time = Time,
                Value = -Value
            };
        }

        public DataPoint Add(DataPoint point)
        {
            if (Time != point.Time)
                throw new InvalidOperationException("Cannot add datapoints with mismatched times");

            return new DataPoint()
            {
                Time = Time,
                Value = Value + point.Value
            };
        }

        public DataPoint Subtract(DataPoint point)
        {
            return Add(point.Negate());
        }

        public DataPoint Add(double value)
        {
            return new DataPoint()
            {
                Time = Time,
                Value = Value + value
            };
        }

        public DataPoint Subtract(double value)
        {
            return Add(-value);
        }

        public DataPoint Multiply(double value)
        {
            return new DataPoint()
            {
                Time = Time,
                Value = Value * value
            };
        }

        public Boolean LargerThan(double comparison)
        {
            return (Value > comparison);
        }

        public Boolean LargerThan(DataPoint point)
        {
            return (LargerThan(point.Value));
        }
    }
}
