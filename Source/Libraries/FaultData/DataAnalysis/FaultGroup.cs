//******************************************************************************************************
//  FaultGroup.cs - Gbtc
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
//  02/18/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Parsing;
using log4net;

namespace FaultData.DataAnalysis
{
    public class FaultGroup
    {
        #region [ Members ]

        // Fields
        private bool? m_faultDetectionLogicResult;
        private bool m_faultValidationLogicResult;
        private List<Fault> m_faults;

        #endregion

        #region [ Constructors ]

        public FaultGroup(DataGroup dataGroup, List<Fault> faults, string expressionText)
        {
            m_faultDetectionLogicResult = CheckFaultDetectionLogic(dataGroup, expressionText);
            m_faultValidationLogicResult = CheckFaultValidationLogic(faults);
            m_faults = faults;
        }

        #endregion

        #region [ Properties ]

        public bool FaultValidationLogicResult
        {
            get
            {
                return m_faultValidationLogicResult;
            }
        }

        public bool? FaultDetectionLogicResult
        {
            get
            {
                return m_faultDetectionLogicResult;
            }
        }

        public List<Fault> Faults
        {
            get
            {
                return m_faults;
            }
        }

        #endregion

        #region [ Methods ]

        private bool? CheckFaultDetectionLogic(DataGroup dataGroup, string expressionText)
        {
            try
            {
                if ((object)expressionText == null)
                    throw new Exception($"Expression text is not defined for line '{dataGroup.Line.AssetKey}'.");

                // Parse fault detection logic into a boolean expression
                BooleanExpression expression = new BooleanExpression(expressionText);

                // Put digital values into a lookup table
                Dictionary<string, bool> digitalLookup = dataGroup.DataSeries
                    .Where(series => series.SeriesInfo.Channel.MeasurementType.Name.Equals("Digital", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(series => series.SeriesInfo.Channel.Name)
                    .Where(grouping => grouping.Count() == 1)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Single().DataPoints.Any(dataPoint => Convert.ToBoolean(dataPoint.Value)));

                // Apply the digital values to the variables in the boolean expression
                foreach (BooleanExpression.Variable variable in expression.Variables)
                {
                    if (!digitalLookup.TryGetValue(variable.Identifier, out variable.Value))
                        throw new Exception($"Channel '{variable.Identifier}' that was required for fault detection logic was missing from the meter data set.");
                }

                // Evaluate the boolean expression
                return expression.Evaluate();
            }
            catch (Exception ex)
            {
                // Store the exception so it
                // can be handled elsewhere
                Log.Error(ex.Message, ex);
                return null;
            }
        }

        private bool CheckFaultValidationLogic(List<Fault> faults)
        {
            return faults.Any(fault => !fault.IsSuppressed && fault.Summaries.Any(summary => summary.IsValid));
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultGroup));

        #endregion
    }
}
