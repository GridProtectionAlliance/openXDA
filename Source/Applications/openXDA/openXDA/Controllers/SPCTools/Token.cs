//******************************************************************************************************
//  SPCTools/Token.cs - Gbtc
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
//  11/04/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using HIDS;


namespace openXDA.Controllers
{

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

    public enum TokenType
    {
        Scalar,
        Series,
        Matrix,
        Slice,

    }
    public enum ComputationAction
    {
        Addition,
        Subtraction,
        Multiplication,
        Mean,
        Min,
        Max,
        StdDev
    }


    public class Token
    {
        private string m_formula;
        private TokenType OutputType;
        private bool m_isLeaf;

        private string m_error;
        private bool m_isValid;
        private List<Token> children;

        private ComputationAction m_action;

        private Dictionary<int, List<Point>> m_data;
        private List<int> m_channels;
        private DataFilter m_filter;

        public Token(string formula, Dictionary<int,List<Point>> data, List<int> channels, DataFilter filter)
        {
           m_isValid = true;

            // this needs to be replaced by HIDDS request but for now it's Ok
            m_data = data;

            m_formula = formula.Trim().ToLower();
            children = new List<Token>();
           
            m_channels = channels;
            m_filter = filter;
          
            Match chars = s_Char.Match(m_formula);
            if (chars.Success)
            {
                m_isValid = false;
                m_error = "'"+ chars.Groups[1] + "' is not a valid Character.";
                return;
            }
            // If ( does not match ) it is invalid....
            if (m_formula.CharCount('(') != m_formula.CharCount(')'))
            {
                m_isValid = false;

                if (m_formula.CharCount('(') > m_formula.CharCount(')'))
                    m_error = "')' is missing ";
                else
                    m_error = "'(' is missing";

                return;
            }

           
            // Check if it is a single token i.e. no + * or (
            if (!m_formula.Contains("+") && !m_formula.Contains("*") && !m_formula.Contains("(") && !m_formula.Contains("-") && !m_formula.Contains("/"))
                m_isLeaf = true;

            if (m_isLeaf)
            {
                // Start by assuming it does not exist
                m_isValid = false;
                m_error = "'" + m_formula + "' is not a valid number or variable. The following variables are available: Vbase, Xmin, Xmax, Xavg";
                // Look for Number
                Match match = s_IsNumber.Match(m_formula);

                if (match.Success)
                {
                    OutputType = TokenType.Scalar;
                    m_isValid = true;
                    return;
                }

                // look for VoltageBase
                match = s_Vbase.Match(m_formula);

                if (match.Success)
                {
                    OutputType = TokenType.Slice;
                    m_isValid = true;
                    return;
                }

                // look for Xmax
                match = s_Xmax.Match(m_formula);

                if (match.Success)
                {
                    OutputType = TokenType.Matrix;
                    m_isValid = true;
                    return;
                }

                // look for Xmin
                match = s_Xmin.Match(m_formula);

                if (match.Success)
                {
                    OutputType = TokenType.Matrix;
                    m_isValid = true;
                    return;
                }
                // look for Xavg
                match = s_Xavg.Match(m_formula);

                if (match.Success)
                {
                    OutputType = TokenType.Matrix;
                    m_isValid = true;
                }

            }
            else if (isLeafFunction(m_formula))
            {
                
                // Take appart into Paramters and set Action Properly
                if (m_formula.StartsWith("min"))
                    m_action = ComputationAction.Min;
                else if (m_formula.StartsWith("max"))
                    m_action = ComputationAction.Max;
                else if (m_formula.StartsWith("mean"))
                    m_action = ComputationAction.Mean;
                else if (m_formula.StartsWith("stdev"))
                    m_action = ComputationAction.StdDev;
                else
                {
                    m_isValid = false;
                    m_error = "'" + m_formula.Substring(0,m_formula.IndexOf('('))  +"' is not a valid Function. Available functions include Min(), Max(), Mean(), StDev().";
                }
                List<string> parameters = m_formula.Substring(m_formula.IndexOf('(') + 1, m_formula.Length - m_formula.IndexOf('(') - 2).Split(',').Select(item => item.Trim()).ToList();

                children = parameters.Select( item => new Token(item,m_data,m_channels,m_filter)).ToList();

                if (children.Count == 0)
                {
                    m_error = m_action.GetFormattedName() + " requires a parameter. Suggested parameters are Xmin, Xavg or Xmax";
                    m_isValid = false;
                }
                else
                {
                    // process Individual function for parameter number and Type
                    if (m_action == ComputationAction.Min)
                    {
                        if (children.Count > 1)
                        {
                            m_error = "'MIN' requires one parameter such as a Variable";
                            m_isValid = false;
                        }
                        OutputType = TokenType.Scalar;
                    }
                    else if (m_action == ComputationAction.Max)
                    {
                        if (children.Count > 1)
                        {
                            m_isValid = false;
                            m_error = "'MAX' requires one parameter such as a Variable";
                        }
                      
                        OutputType = TokenType.Scalar;

                    }
                    else if (m_action == ComputationAction.Mean)
                    {
                        if (children.Count > 1)
                        {
                            m_isValid = false;
                            m_error = "'MEAN' requires one parameter such as a Variable";
                        }
                       
                        OutputType = TokenType.Scalar;
                    }
                    else if (m_action == ComputationAction.StdDev)
                    {
                        if (children.Count > 1)
                        {
                            m_isValid = false;
                            m_error = "'STDEV' requires one parameter such as a Variable";
                        }

                        OutputType = TokenType.Scalar;
                    }
                }
            }
            else
            {
                
                // if it is not a single leave start by tokenizing ()
                string original = m_formula;
                string tokenized = FindToken(original);
                while (original != tokenized)
                {
                    original = tokenized;
                    tokenized = FindToken(original);
                }

                //Then Deal with + and -
                if (tokenized.Contains("+"))
                {
                    m_action = ComputationAction.Addition;
                    List<Token> oldOrder = children.ToList();

                    List<string> vars = tokenized.Split('+').Select(item => item.Trim()).ToList();
                    int originalIndex = 0;
                    children = vars.Select((item, index) =>
                    {
                        if (item == "T")
                        {
                            originalIndex++;
                            return oldOrder[originalIndex - 1];
                        }
                        else if (item.Contains("T"))
                        {
                            int count = item.CharCount('T');
                            string untokenized = item;
                            for (int i=0; i<count; i++)
                            {
                                originalIndex++;
                                int Tin = untokenized.IndexOf("T");
                                untokenized = untokenized.Substring(0, Tin) + oldOrder[originalIndex - 1].Formula +  untokenized.Substring(Tin+1);
                            }
                            return new Token(untokenized, m_data, m_channels, m_filter);
                        }
                        return new Token(item, m_data, m_channels, m_filter);
                    }).ToList();

                    // Set Output Type and Validtiy
                    if (children.Any(item => item.OutputType == TokenType.Matrix))
                        OutputType = TokenType.Matrix;
                    else if (children.Any(item => item.OutputType == TokenType.Slice))
                        OutputType = TokenType.Slice;
                    else if (children.Any(item => item.OutputType == TokenType.Series))
                        OutputType = TokenType.Series;
                    else
                        OutputType = TokenType.Scalar;
                }

                else if (tokenized.Contains("-"))
                {
                    m_action = ComputationAction.Subtraction;
                    List<Token> oldOrder = children.ToList();

                    List<string> vars = tokenized.Split('-').Select(item => item.Trim()).ToList();
                    int originalIndex = 0;
                    children = vars.Select((item, index) =>
                    {
                        if (item == "T")
                        {
                            originalIndex++;
                            return oldOrder[originalIndex - 1];
                        }
                        else if (item.Contains("T"))
                        {
                            int count = item.CharCount('T');
                            string untokenized = item;
                            for (int i = 0; i < count; i++)
                            {
                                originalIndex++;
                                int Tin = untokenized.IndexOf("T");
                                untokenized = untokenized.Substring(0, Tin) + oldOrder[originalIndex - 1].Formula + untokenized.Substring(Tin+1);
                            }
                            return new Token(untokenized, m_data, m_channels, m_filter);
                        }
                        return new Token(item, m_data, m_channels, m_filter);
                    }).ToList();

                    // Set Output Type and Validtiy
                    if (children.Any(item => item.OutputType == TokenType.Matrix))
                        OutputType = TokenType.Matrix;
                    else if (children.Any(item => item.OutputType == TokenType.Slice))
                        OutputType = TokenType.Slice;
                    else if (children.Any(item => item.OutputType == TokenType.Series))
                        OutputType = TokenType.Series;
                    else
                        OutputType = TokenType.Scalar;
                }

                //Then deal with * and /
                else if (tokenized.Contains("*"))
                {
                    m_action = ComputationAction.Multiplication;
                    List<Token> oldOrder = children.ToList();

                    List<string> vars = tokenized.Split('*').Select(item => item.Trim()).ToList();
                    int originalIndex = 0;
                    children = vars.Select((item, index) =>
                    {
                        if (item == "T")
                        {
                            originalIndex++;
                            return oldOrder[originalIndex - 1];
                        }
                        else if (item.Contains("T"))
                        {
                            int count = item.CharCount('T');
                            string untokenized = item;
                            for (int i = 0; i < count; i++)
                            {
                                originalIndex++;
                                int Tin = untokenized.IndexOf("T");
                                untokenized = untokenized.Substring(0, Tin) + oldOrder[originalIndex - 1].Formula + untokenized.Substring(Tin+1);
                            }
                            return new Token(untokenized, m_data, m_channels, m_filter);
                        }
                        return new Token(item, m_data, m_channels, m_filter);
                    }).ToList();

                    // Set Output Type and Validtiy
                    if (children.Any(item => item.OutputType == TokenType.Matrix))
                        OutputType = TokenType.Matrix;
                    else if (children.Any(item => item.OutputType == TokenType.Slice))
                        OutputType = TokenType.Slice;
                    else if (children.Any(item => item.OutputType == TokenType.Series))
                        OutputType = TokenType.Series;
                    else
                        OutputType = TokenType.Scalar;
                }

            }

        }

        private string FindToken(string input)
        {
            int i = 0;

            bool funcToken = false;

            while (i < input.Length)
            {
                if (input[i] == '(')
                    break;
                i++;
            }
            
            // No () Token Found
            if (i == input.Length)
                return input;

            funcToken = isFunction(input.Substring(0, i));

            int nPrent = 1;

            int start = i;
            i++;
            while (nPrent != 0)
            {
                if (input[i] == '(')
                    nPrent++;
                if (input[i] == ')')
                    nPrent--;
                i++;
            }

            //This is where we should add a Child Token
            if (funcToken)
            {
             int j = start-1;

                while (j > 0 && (input[j] != '*' && input[j] != '/' && input[j] != '+' && input[j] != '-' && input[j] != '(' && input[j] != ' '))
                    j--;
                children.Add(new Token(input.Substring(j , i - j ), m_data, m_channels, m_filter));

                return input.Substring(0, j) + "T" + input.Substring(i);
            }
            children.Add(new Token(input.Substring(start+1,i-start-2), m_data, m_channels, m_filter));

            return input.Substring(0,start) + "T" + input.Substring(i); 
        }

        private bool isFunction(string test)
        {
            if (test.Length == 0)
                return false;
            if (test.EndsWith("max"))
                return true;
            if (test.EndsWith("min"))
                return true;
            if (test.EndsWith("mean"))
                return true;
            if (test.EndsWith("stdev"))
                return true;
            return false;
        }

        private bool isLeafFunction(string test)
        {
            // Start by checking it it matches a valid Function
            Match match = s_isFunction.Match(m_formula);

            if (!match.Success)
                return false;

            int i = test.IndexOf('(');

            int nPrent = 1;

            i++;
            while (nPrent != 0)
            {
                if (test[i] == '(')
                    nPrent++;
                if (test[i] == ')')
                    nPrent--;
                i++;
            }

            if (i == test.Length)
                return true;
            else
                return false;
        }

        public double ComputeScalar()
        {
            if (!Valid)
                return double.NaN;

            if (OutputType != TokenType.Scalar)
                throw new Exception("Token is not a Scalar");

            if (m_isLeaf)
            {
                //Functions are never Leafes and Variables are not a Scalar so this is guaranteed a Number
                return (double.Parse(m_formula));
            }

            if (m_action == ComputationAction.Addition)
                return children.Sum(item => item.ComputeScalar());
            if (m_action == ComputationAction.Subtraction)
                return children[0].ComputeScalar() - children.Skip(1).Sum(item => item.ComputeScalar());
            if (m_action == ComputationAction.Multiplication)
                return children.Select(item => item.ComputeScalar()).Aggregate((a,b) => a*b);
            if (m_action == ComputationAction.Max)
                return children.Max(item => item.ComputeMatrix().Max(i => i.Where(pt => !double.IsNaN(pt[1])).Max(pt => pt[1])));
            if (m_action == ComputationAction.Min)
                return children.Min(item => item.ComputeMatrix().Min(i => i.Where(pt =>!double.IsNaN(pt[1])).Min(pt => pt[1])));
            if (m_action == ComputationAction.Mean)
                return (children.Sum(item => item.ComputeMatrix().Sum(i => i.Sum(pt => (double.IsNaN(pt[1]) ? 0.0d : pt[1]))))/ children.Sum(item => item.ComputeMatrix().Sum(i => i.Sum(pt => (double.IsNaN(pt[1]) ? 0 : 1)))));
            if (m_action == ComputationAction.StdDev)
            {
                double x2 = children.Sum(item => item.ComputeMatrix().Sum(i => i.Sum(pt => (double.IsNaN(pt[1]) ? 0.0d : pt[1]*pt[1]))));
                double x = children.Sum(item => item.ComputeMatrix().Sum(i => i.Sum(pt => (double.IsNaN(pt[1]) ? 0.0d : pt[1]))));
                int N = children.Sum(item => item.ComputeMatrix().Sum(i => i.Sum(pt => (double.IsNaN(pt[1]) ? 0 : 1))));

                return x2 + 2.0D * x * x / ((double)N) + x / ((double)(N * N));
            }
                

            return double.NaN;
        }

        public List<double> ComputeSlice()
        {
            if (!m_isValid)
                return new List<double>();

            if (OutputType != TokenType.Scalar && OutputType != TokenType.Slice )
                throw new Exception("Token is not a Slice");

            if (OutputType == TokenType.Scalar)
                return Enumerable.Repeat(ComputeScalar(),m_channels.Count).ToList();

            if (m_isLeaf)
            {
                //Right now the only Actual Leaf slice is Vbase
                using (AdoDataConnection connection = new AdoDataConnection("SystemSettings"))
                {
                    string VbaseRequest = @"SELECT Asset.VoltageKV
                                            FROM Channel LEFT JOIN
                                                Asset ON Channel.AssetID = Asset.ID
                                            WHERE Channel.ID = {0}";

                    return m_channels.Select(item => connection.ExecuteScalar<double>(String.Format(VbaseRequest,item))).ToList();
                }
            }

            if (m_action == ComputationAction.Addition)
                return children.Select(item => item.ComputeSlice()).Aggregate((a, b) => a.Select((p, i) => p + b[i]).ToList());
            if (m_action == ComputationAction.Subtraction)
                return children.Select(item => item.ComputeSlice()).Aggregate((a, b) => a.Select((p, i) => p - b[i]).ToList());
            if (m_action == ComputationAction.Multiplication)
                return children.Select(item => item.ComputeSlice()).Aggregate((a, b) => a.Select((p, i) => p * b[i]).ToList());

            return Enumerable.Repeat(0.0D, m_channels.Count).ToList();


        }

        private double ApplyDataFilter(double input)
        {
            if (m_filter == null)
                return input;
            double output = input;

            if (m_filter.FilterZero && output == 0.0D)
                output = double.NaN ;
            if (m_filter.FilterLower && output < m_filter.LowerLimit)
                output =  double.NaN;
            if (m_filter.FilterUpper && output > m_filter.UpperLimit)
                output = double.NaN;

            return output;
        }
        public List<List<double[]>> ComputeMatrix() 
        {
            if (!m_isValid)
                return new List<List<double[]>>();

            if (OutputType == TokenType.Slice || OutputType == TokenType.Scalar)
                return ComputeSlice().Select((item,index) => Enumerable.Repeat(new double[] { double.NaN, item }, m_data[m_channels[index]].Count).ToList()).ToList();

            if (m_isLeaf)
            {
                
                Match match = s_Xmax.Match(m_formula);

                //start.Subtract(m_epoch).TotalMilliseconds
                if (match.Success)
                {
                    return m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Maximum) }).ToList()).ToList();
                }

                // look for Xmin
                match = s_Xmin.Match(m_formula);

                if (match.Success)
                {
                    return m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Minimum) }).ToList()).ToList();
                }
                // look for Xavg
                match = s_Xavg.Match(m_formula);

                if (match.Success)
                {
                    return m_channels.Select(ch => m_data[ch].Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, ApplyDataFilter(pt.Average) }).ToList()).ToList();
                }

                return ComputeSlice().Select((item, index) => Enumerable.Repeat(new double[] { double.NaN, item }, m_data[m_channels[index]].Count).ToList()).ToList();
            }

            if (m_action == ComputationAction.Addition)
                return children.Select(item => item.ComputeMatrix()).Aggregate((a,b) => a.Select( (row, ir) => row.Select( (pt, ip) => (new double[] {(double.IsNaN(pt[0])? b[ir][ip][0] : pt[0]), pt[1] + b[ir][ip][1] }) ).ToList()).ToList()) ;
            if (m_action == ComputationAction.Subtraction)
            {
                List<List<double[]>> sm = children.Skip(1).Select(item => item.ComputeMatrix()).Aggregate((a, b) => a.Select((row, ir) => row.Select((pt, ip) => (new double[] { (double.IsNaN(pt[0]) ? b[ir][ip][0] : pt[0]), pt[1] + b[ir][ip][1] })).ToList()).ToList());
                return children[0].ComputeMatrix().Select((row, ir) => row.Select((pt, ip) => (new double[] { (double.IsNaN(pt[0]) ? sm[ir][ip][0] : pt[0]), pt[1] - sm[ir][ip][1] })).ToList()).ToList();
            }
            if (m_action == ComputationAction.Multiplication)
                return children.Select(item => item.ComputeMatrix()).Aggregate((a, b) => a.Select((row, ir) => row.Select((pt, ip) => (new double[] { (double.IsNaN(pt[0]) ? b[ir][ip][0] : pt[0]), pt[1] * b[ir][ip][1] })).ToList()).ToList());

            return ComputeSlice().Select((item, index) => Enumerable.Repeat(new double[] { double.NaN, item }, m_data[m_channels[index]].Count).ToList()).ToList();
        }

        public bool Valid 
        { 
            get
            {
                return m_isValid && children.TrueForAll(item => item.Valid);
            } 
        }

        public string Error
        {
            get
            {
                if (!m_isValid)
                    return m_error;
                else if (m_isLeaf)
                    return "";
                else
                    return children.Select(item => item.Error).Where(item => !string.IsNullOrEmpty(item)).FirstOrDefault();

            }
        }

        public bool isScalar => OutputType == TokenType.Scalar;

        public bool isSlice => OutputType == TokenType.Slice;

        public string Formula => m_formula;

        private static readonly Regex s_IsNumber = new Regex("^[0-9.]*$", RegexOptions.Compiled| RegexOptions.IgnoreCase);
        private static readonly Regex s_isFunction = new Regex("^([^*\\-+\\/]+)\\(.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_Vbase = new Regex("^Vbase$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_Xmin = new Regex("^Xmin", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_Xmax = new Regex("^Xmax", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_Xavg = new Regex("^Xavg", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_Char = new Regex("[^a-z0-9*\\-+\\s\\(\\).]", RegexOptions.Compiled);

        private static readonly DateTime m_epoch = new DateTime(1970, 1, 1);

    }

    
}
