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
using GSF.Data.Model;
using GSF.Identity;
using GSF.TimeSeries.Adapters;
using GSF.Web;
using GSF.Web.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Controllers
{
    public enum TokenType
    {
        Scalar,
        Series,
        Matrix,
        Slice,

    }

    public enum ComputationType
    {
        Number,
        Function,
        Variable

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
        private ComputationType m_computation;

        private DateTime m_start;
        private DateTime m_end;
        private List<int> m_channels;
        private int m_NumPoints;

        public Token(string formula, DateTime start, DateTime end, List<int> channels)
        {
           m_isValid = true;

            // this needs to be replaced by HIDDS request but for now it's Ok
            m_NumPoints = 1000;

            m_formula = formula.Trim().ToLower();
            children = new List<Token>();
            m_start = start;
            m_end = end;
            m_channels = channels;

            m_computation = ComputationType.Function;

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
                    m_computation = ComputationType.Number;
                    OutputType = TokenType.Scalar;
                    m_isValid = true;
                    return;
                }

                // look for VoltageBase
                match = s_Vbase.Match(m_formula);

                if (match.Success)
                {
                    m_computation = ComputationType.Variable;
                    OutputType = TokenType.Slice;
                    m_isValid = true;
                    return;
                }

                // look for Xmax
                match = s_Xmax.Match(m_formula);

                if (match.Success)
                {
                    m_computation = ComputationType.Variable;
                    OutputType = TokenType.Matrix;
                    m_isValid = true;
                    return;
                }

                // look for Xmin
                match = s_Xmin.Match(m_formula);

                if (match.Success)
                {
                    m_computation = ComputationType.Variable;
                    OutputType = TokenType.Matrix;
                    m_isValid = true;
                    return;
                }
                // look for Xavg
                match = s_Xavg.Match(m_formula);

                if (match.Success)
                {
                    m_computation = ComputationType.Variable;
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

                children = parameters.Select( item => new Token(item,m_start,m_end,m_channels)).ToList();

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
                            return new Token(untokenized, m_start, m_end, m_channels);
                        }
                        return new Token(item,m_start,m_end,m_channels);
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
                            return new Token(untokenized, m_start, m_end, m_channels);
                        }
                        return new Token(item, m_start, m_end, m_channels);
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
                            return new Token(untokenized, m_start, m_end, m_channels);
                        }
                        return new Token(item, m_start, m_end, m_channels);
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
                children.Add(new Token(input.Substring(j , i - j ), m_start, m_end, m_channels));

                return input.Substring(0, j) + "T" + input.Substring(i);
            }
            children.Add(new Token(input.Substring(start+1,i-start-2),m_start,m_end,m_channels));

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
                return children.Max(item => item.ComputeMatrix().Max(i => i.Max()));
            if (m_action == ComputationAction.Min)
                return children.Min(item => item.ComputeMatrix().Min(i => i.Min()));
            if (m_action == ComputationAction.Min)
                return children.Min(item => item.ComputeMatrix().Min(i => i.Min()));
            if (m_action == ComputationAction.StdDev)
            {
                return 42.0D;
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

        public List<List<double>> ComputeMatrix() 
        {
            if (!m_isValid)
                return new List<List<double>>();

            if (OutputType == TokenType.Scalar)
                return Enumerable.Repeat(Enumerable.Repeat(ComputeScalar(), m_NumPoints).ToList(), m_channels.Count).ToList();
            if (OutputType == TokenType.Slice)
                return ComputeSlice().Select(item => Enumerable.Repeat(item, m_NumPoints).ToList()).ToList();

            if (m_isLeaf)
            {
                //For now we are using random Data from StaticAlarmCreationController

                Match match = s_Xmax.Match(m_formula);

                if (match.Success)
                {
                    return m_channels.Select(ch => StaticAlarmCreationController.createData(ch, m_start, m_end).Select(pt => pt[1] + 1.0D).ToList()).ToList();
                }

                // look for Xmin
                match = s_Xmin.Match(m_formula);

                if (match.Success)
                {
                    return m_channels.Select(ch => StaticAlarmCreationController.createData(ch, m_start, m_end).Select(pt => pt[1] - 1.0D).ToList()).ToList();
                }
                // look for Xavg
                match = s_Xavg.Match(m_formula);

                if (match.Success)
                {
                    return m_channels.Select(ch => StaticAlarmCreationController.createData(ch, m_start, m_end).Select(pt => pt[1]).ToList()).ToList();
                }
                return Enumerable.Repeat(Enumerable.Repeat(double.Parse(m_formula), 1).ToList(), 1).ToList();
            }

            if (m_action == ComputationAction.Addition)
                return children.Select(item => item.ComputeMatrix()).Aggregate((a,b) => a.Select((s,i) => s.Select((p,j) => p + b[i][j]).ToList() ).ToList() ) ;
            if (m_action == ComputationAction.Subtraction)
                return children.Select(item => item.ComputeMatrix()).Aggregate((a, b) => a.Select((s, i) => s.Select((p, j) => (p - b[i][j])).ToList()).ToList());
            if (m_action == ComputationAction.Multiplication)
                return children.Select(item => item.ComputeMatrix()).Aggregate((a, b) => a.Select((s, i) => s.Select((p, j) => p * b[i][j]).ToList()).ToList());

            return Enumerable.Repeat(Enumerable.Repeat(0.0D, m_NumPoints).ToList(), m_channels.Count).ToList();
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
        private static readonly Regex s_Char = new Regex("[^a-z0-9*\\-+\\s\\(\\)]", RegexOptions.Compiled);

    }

    
}
