//******************************************************************************************************
//  SPCTools/ExpressionOperations.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/12/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using Ciloci.Flee;
using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDS;
using System.Text.RegularExpressions;

namespace SPCTools
{
    public interface IExpressionOperands { }

    /// <summary>
    /// Filter for Data when applying Setpoints
    /// </summary>
    public class DataFilter
    {
        public bool FilterZero { get; set; }
        public bool FilterUpper { get; set; }
        public double UpperLimit { get; set; }
        public bool FilterLower { get; set; }
        public double LowerLimit { get; set; }
    }

    public class ExpressionOperations
    {
        private List<int> m_channels;
        private Dictionary<int, List<Point>> m_data;
        private static readonly DateTime m_epoch = new DateTime(1970, 1, 1);
        private DataFilter m_filter;
        private ExpressionContext m_evaluator;
        private string m_formula;
        private Func<DateTime, bool> m_timeFilter;

        public ExpressionOperations(string formula, Dictionary<int, List<Point>> data, List<int> channels, DataFilter filter, Func<DateTime, bool> timeFilter)
        {
            m_channels = channels;
            m_data = data;
            m_filter = filter;
            m_formula = formula;
            m_timeFilter = timeFilter;
        }

        public static IExpressionOperands abs(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                Scalar scalar = operand as Scalar;
                return new Scalar(Math.Abs(scalar.Value));
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                return new Slice(slice.Values.Select(value => Math.Abs(value)).ToList());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                return new Matrix(matrix.Values.Select(channel => channel.Select(array => new double[] { array[0], Math.Abs(array[1]) }).ToList()).ToList());
            }
            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands min(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                Scalar scalar = operand as Scalar;
                return new Scalar(scalar);
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                if (slice.Values.Count() == 0)
                    return new Scalar(0.0D)
                return new Scalar(slice.Values.Min());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                double minValue = matrix.Values.SelectMany(channel => channel).Where(array => !double.IsNaN(array[1])).Min(array => array[1]);
                return new Scalar(minValue);
            }
            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands min(double num)
        {
            return new Scalar(num);
        }

        public static IExpressionOperands max(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                Scalar scalar = operand as Scalar;
                return new Scalar(scalar);
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                if (slice.Values.Count() == 0)
                    return new Scalar(0.0D)
                return new Scalar(slice.Values.Max());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                double minValue = matrix.Values.SelectMany(channel => channel).Where(array => !double.IsNaN(array[1])).Max(array => array[1]);
                return new Scalar(minValue);
            }
            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands max(double num)
        {
            return new Scalar(num);
        }

        public static IExpressionOperands mean(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                Scalar scalar = operand as Scalar;
                return new Scalar(scalar.Value);
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                if (slice.Values.Count() == 0)
                    return new Scalar(0.0D)
                return new Scalar(slice.Values.Sum() / slice.Values.Count());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                double totalSum = matrix.Values.SelectMany(channel => channel).Sum(array => array[1]);
                int totalCount = matrix.Values.SelectMany(channel => channel).Count(array => !double.IsNaN(array[1]));
                return new Scalar(totalSum / totalCount);
            }
            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands mean(double num)
        {
            return new Scalar(num);
        }

        public static IExpressionOperands stdev(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                return new Scalar(0);
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                double mean = slice.Values.Average();
                double variance = slice.Values.Select(value => Math.Pow(value - mean, 2)).Average();
                return new Scalar(Math.Sqrt(variance));
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                double x2 = matrix.Values.Sum(channel => channel.Sum(array => (double.IsNaN(array[1]) ? 0.0d : array[1] * array[1])));
                double x = matrix.Values.Sum(channel => channel.Sum(array => (double.IsNaN(array[1]) ? 0.0d : array[1])));
                int N = matrix.Values.Sum(channel => channel.Sum(array => (double.IsNaN(array[1]) ? 0 : 1)));
                double variance = (x2 - x * x / ((double)N)) / (double)N;
                return new Scalar(Math.Sqrt(variance));
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands stdev(double num)
        {
            return new Scalar(0);
        }

        public static IExpressionOperands channelmin(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                throw new InvalidOperationException("Can not perform operation ChannelMin on a Scalar operand");
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                double maxValue = 0.0D;
                if (slice.Values.Count() > 0)
                    maxValue = slice.Values.Min();
                return new Slice(slice.Values.Select(_ => maxValue).ToList());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                List<double> values = matrix.Values.Select(channel => channel.Where(array => !double.IsNaN(array[1])).Min(array => array[1])).ToList();
                return new Slice(values);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands channelmin(double operand)
        {
            return new Scalar(operand);
        }

        public static IExpressionOperands channelmax(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                throw new InvalidOperationException("Can not perform operation ChannelMax on a Scalar operand");
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                double maxValue = 0.0D;
                 if (slice.Values.Count() > 0)
                    maxValue = slice.Values.Max();
                return new Slice(slice.Values.Select(_ => maxValue).ToList());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                List<double> values = matrix.Values.Select(channel => channel.Where(array => !double.IsNaN(array[1])).Max(array => array[1])).ToList();
                return new Slice(values);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static IExpressionOperands channelmax(double operand)
        {
            return new Scalar(operand);
        }

        public static IExpressionOperands channelmean(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                throw new InvalidOperationException("Can not perform operation ChannelMean on a Scalar operand");
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                double min = slice.Values.Sum()/slice.Values.Count();
                return new Slice(slice.Values.Select(_ => min).ToList());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                List<double> sum = matrix.Values.Select(channel => channel.Where(pt => !double.IsNaN(pt[1])).Sum(pt => pt[1])).ToList();
                List<int> N = matrix.Values.Select(channel => channel.Sum(pt => (double.IsNaN(pt[1]) ? 0 : 1))).ToList();
                return new Slice(sum.Select((pt, index) => pt / (double)N[index]).ToList());
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }
        public static IExpressionOperands channelmean(double operand)
        {
            return new Scalar(operand);
        }

        public static IExpressionOperands channelstdev(IExpressionOperands operand)
        {
            if (operand is Scalar)
            {
                throw new InvalidOperationException("Can not perform operation ChannelStDev on a Scalar operand");
            }
            else if (operand is Slice)
            {
                Slice slice = operand as Slice;
                double mean = slice.Values.Average();
                double variance = slice.Values.Select(val => Math.Pow(val - mean, 2)).Average();
                double stdDev = Math.Sqrt(variance);
                return new Slice(slice.Values.Select(_ => stdDev).ToList());
            }
            else if (operand is Matrix)
            {
                Matrix matrix = operand as Matrix;
                List<double> x2 = matrix.Values.Select(channel => channel.Sum(pt => (double.IsNaN(pt[1]) ? 0.0d : pt[1] * pt[1]))).ToList();
                List<double> x = matrix.Values.Select(channel => channel.Sum(pt => (double.IsNaN(pt[1]) ? 0.0d : pt[1]))).ToList();
                List<int> N = matrix.Values.Select(channel => channel.Sum(pt => (double.IsNaN(pt[1]) ? 0 : 1))).ToList();
                return new Slice(x2.Select((pt, index) => Math.Sqrt((pt - x[index] * x[index] / ((double)N[index])) / (double)N[index])).ToList());
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }
        public static IExpressionOperands channelstdev(double operand)
        {
            return new Scalar(0);
        }


        public DataResponse Evaluate()
        {

            m_evaluator = new ExpressionContext();
            m_evaluator.Imports.AddType(typeof(Math));
            m_evaluator.Imports.AddType(typeof(ExpressionOperations));
            m_evaluator.Imports.AddType(typeof(Scalar));
            m_evaluator.Imports.AddType(typeof(Slice));
            m_evaluator.Imports.AddType(typeof(Matrix));

            m_evaluator.Variables["vbase"] = Vbase();
            m_evaluator.Variables["xmax"] = Xmax();
            m_evaluator.Variables["xmin"] = Xmin();
            m_evaluator.Variables["xavg"] = Xavg();

            DataResponse result = new DataResponse();
            object dynamicEvaluatedObject = 0;

            try
            {
                IDynamicExpression e = m_evaluator.CompileDynamic(m_formula);
                dynamicEvaluatedObject = e.Evaluate();
            }
            catch (Exception ex)
            {
                result.Valid = false;
                result.IsScalar = false;
                result.Value = new List<double> { 0 };
                result.TimeFilter = m_timeFilter;
                result.Message = ex.Message;

                // Cleaning up exception message
                Match match = s_ContainSemi.Match(ex.Message);
                if (match.Success)
                {
                    result.Message = match.Value;
                }
                return result;
            }

            if(dynamicEvaluatedObject is Matrix)
            {
                result.Valid = false;
                result.IsScalar = false;
                result.Message = "Return type of expression can not be a Matrix.";
                result.Value = new List<double> { 0 };
                result.TimeFilter = m_timeFilter;
                return result;
            }
            else if (dynamicEvaluatedObject is Slice sliceResult)
            {
                result.Valid = true;
                result.IsScalar = false;
                result.Value = sliceResult.Values;
                result.Message = "";
                result.TimeFilter = m_timeFilter;
                return result;
            }
            else
            {
                result.Valid = true;
                result.IsScalar = true;
                result.Value = ControllerResult(dynamicEvaluatedObject);
                result.Message = "";
                result.TimeFilter = m_timeFilter;
                return result;
            }
            throw new InvalidOperationException("Unexpected type of returned from Expression Evaluation");
        }

        private Slice Vbase()
        {
            using (AdoDataConnection connection = new AdoDataConnection("SystemSettings"))
            {
                string VbaseQuery = @"SELECT Asset.VoltageKV
                            FROM Channel LEFT JOIN
                                Asset ON Channel.AssetID = Asset.ID
                            WHERE Channel.ID = {0}"
                ;
                var values = m_channels.Select(channel => connection.ExecuteScalar<double>(String.Format(VbaseQuery, channel))).ToList();
                return new Slice(values);
            }
        }

        private IExpressionOperands Xmin()
        {
            return new Matrix(m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Minimum) }).ToList()).ToList());
        }

        private IExpressionOperands Xavg()
        {
            return new Matrix(m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Average) }).ToList()).ToList());
        }

        private IExpressionOperands Xmax()
        {
            return new Matrix(m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Maximum) }).ToList()).ToList());
        }

        private double ApplyDataFilter(double input)
        {
            if (m_filter == null)
                return input;
            double output = input;

            if (m_filter.FilterZero && output == 0.0D)
                output = double.NaN;
            if (m_filter.FilterLower && output < m_filter.LowerLimit)
                output = double.NaN;
            if (m_filter.FilterUpper && output > m_filter.UpperLimit)
                output = double.NaN;

            return output;
        }


        private List<double> ControllerResult(object val)
        {
            List<double> result = new List<double> { 0 };
            if (val is Scalar)
            {
                Scalar scalar = val as Scalar;
                result = Enumerable.Repeat(scalar.Value, m_channels.Count).ToList();
            }
            else
            {
                try
                {
                    double num = Convert.ToDouble(val);
                    result = Enumerable.Repeat(num, m_channels.Count).ToList();
                }
                catch{}
            }

            return result;
        }
        public bool Applies(DateTime time) => m_timeFilter(time);
        public List<int> Channels => m_channels;
        public Dictionary<int, List<Point>> Data => m_data;
        public static DateTime Epoch => m_epoch;
        public DataFilter Filter => m_filter;
        public ExpressionContext Evaluator => m_evaluator;
        public string Formula => m_formula;

        public Func<DateTime, bool> TimeFilter => m_timeFilter;

        private static readonly Regex s_ContainSemi = new Regex(@"^(.*?;)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
