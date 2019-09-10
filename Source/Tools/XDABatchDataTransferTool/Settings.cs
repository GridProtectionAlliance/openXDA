//******************************************************************************************************
//  Settings.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  09/04/2019 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Configuration;
using ExpressionEvaluator;
using GSF.ComponentModel;
using GSF.Configuration;

namespace XDABatchDataTransferTool
{
    /// <summary>
    /// Defines settings for the XDABatchDataTransferTool application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default value expressions in this class reference the primary form instance, as a result,
    /// instances of this class should only be created from the primary UI thread or otherwise
    /// use <see cref="System.Windows.Forms.Form.Invoke(System.Delegate)"/>.
    /// </para>
    /// <para>
    /// In order for properties of this class decorated with <see cref="TypeConvertedValueExpressionAttribute"/>
    /// to have access to form element values, the form elements should be declared with "public" access. To
    /// give a form element public access, click on the element in the Windows Forms design mode and access its
    /// properties. Find the "Modifiers" property and set it to "Public".
    /// </para>
    /// <para>
    /// Additionally, when a form element changes, this settings class should be notified so it can update its
    /// state. To do this find the appropriate "changed" event in the element properties and set the event
    /// function to the "MainForm.FormElementChanged" handler.
    /// </para>
    /// </remarks>
    public sealed class Settings : CategorizedSettingsBase<Settings>, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;  

        private readonly ConcurrentDictionary<string, object> m_propertyValues = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);

        private T GetPropertyValue<T>(string propertyName)
        {
            return (T)m_propertyValues.GetOrAdd(propertyName, default(T));
        }

        private void SetPropertyValue(string propertyName, object value)
        {
            m_propertyValues[propertyName] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a new <see cref="Settings"/> instance.
        /// </summary>
        /// <param name="typeRegistry">
        /// Type registry to use when parsing <see cref="TypeConvertedValueExpressionAttribute"/> instances,
        /// or <c>null</c> to use <see cref="ValueExpressionParser.DefaultTypeRegistry"/>.
        /// </param>
        public Settings(TypeRegistry typeRegistry) : base("systemSettings", typeRegistry)
        {
        }

        /// <summary>
        /// Gets or sets root URL for openXDA instance to query.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.XDAUrlTextBox) + ".Text")]
        [Description("Root URL for openXDA instance to query.")]
        [UserScopedSetting]
        public string XDARootURL
        { 
            get => GetPropertyValue<string>(nameof(XDARootURL));
            set => SetPropertyValue(nameof(XDARootURL), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if source type to query is lines.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.QueryLinesRadioButton) + ".Checked")]
        [Description("Flag that determines if the source type to query is lines. Value is mutually exclusive of SourceQueryTypeIsMeters.")]
        [UserScopedSetting]
        public bool SourceQueryTypeIsLines
        { 
            get => GetPropertyValue<bool>(nameof(SourceQueryTypeIsLines));
            set => SetPropertyValue(nameof(SourceQueryTypeIsLines), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if source type to query is meters.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.QueryMetersRadioButton) + ".Checked")]
        [Description("Flag that determines if the source type to query is meters. Value is mutually exclusive of SourceQueryTypeIsLines.")]
        [UserScopedSetting]
        public bool SourceQueryTypeIsMeters
        { 
            get => GetPropertyValue<bool>(nameof(SourceQueryTypeIsMeters));
            set => SetPropertyValue(nameof(SourceQueryTypeIsMeters), value);
        }

        /// <summary>
        /// Gets or sets start date/time for query.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.StartDateTimePicker) + ".Value")]
        [Description("Start date/time for query.")]
        [UserScopedSetting]
        public DateTime StartDateTimeForQuery
        { 
            get => GetPropertyValue<DateTime>(nameof(StartDateTimeForQuery));
            set => SetPropertyValue(nameof(StartDateTimeForQuery), value);
        }

        /// <summary>
        /// Gets or sets end date/time for query.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.EndDateTimePicker) + ".Value")]
        [Description("End date/time for query.")]
        [UserScopedSetting]
        public DateTime EndDateTimeForQuery
        { 
            get => GetPropertyValue<DateTime>(nameof(EndDateTimeForQuery));
            set => SetPropertyValue(nameof(EndDateTimeForQuery), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if receiving repository is Azure.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.AzureRadioButton) + ".Checked")]
        [Description("Flag that determines if receiving repository is Azure. Value is mutually exclusive of RepositoryIsAWS, RepositoryIsPQDS, and RepositoryIsGoogle.")]
        [UserScopedSetting]
        public bool RepositoryIsAzure
        { 
            get => GetPropertyValue<bool>(nameof(RepositoryIsAzure));
            set => SetPropertyValue(nameof(RepositoryIsAzure), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if receiving repository is Amazon Web Services.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.AWSRadioButton) + ".Checked")]
        [Description("Flag that determines if receiving repository is Amazon Web Services. Value is mutually exclusive of RepositoryIsAzure, RepositoryIsPQDS, and RepositoryIsGoogle.")]
        [UserScopedSetting]
        public bool RepositoryIsAWS
        { 
            get => GetPropertyValue<bool>(nameof(RepositoryIsAWS));
            set => SetPropertyValue(nameof(RepositoryIsAWS), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if receiving repository is PQDS.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.PQDSRadioButton) + ".Checked")]
        [Description("Flag that determines if receiving repository is Power Quality Data Sharing format. Value is mutually exclusive of RepositoryIsAzure, RepositoryIsAWS, and RepositoryIsGoogle.")]
        [UserScopedSetting]
        public bool RepositoryIsPQDS
        { 
            get => GetPropertyValue<bool>(nameof(RepositoryIsPQDS));
            set => SetPropertyValue(nameof(RepositoryIsPQDS), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if receiving repository is Google.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.GoogleRadioButton) + ".Checked")]
        [Description("Flag that determines if receiving repository is Google. Value is mutually exclusive of RepositoryIsAzure, RepositoryIsAWS, and RepositoryIsPQDS.")]
        [UserScopedSetting]
        public bool RepositoryIsGoogle
        { 
            get => GetPropertyValue<bool>(nameof(RepositoryIsGoogle));
            set => SetPropertyValue(nameof(RepositoryIsGoogle), value);
        }

        /// <summary>
        /// Gets or sets receiving repository connection string.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ConnectionStringTextBox) + ".Text")]
        [Description("Defines the receiving repository connection string.")]
        [UserScopedSetting]
        public string RepositoryConnectionString
        { 
            get => GetPropertyValue<string>(nameof(RepositoryConnectionString));
            set => SetPropertyValue(nameof(RepositoryConnectionString), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if event data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportEventDataCheckBox) + ".Checked")]
        [Description("Flag that determines event data should be exported.")]
        [UserScopedSetting]
        public bool ExportEventData
        { 
            get => GetPropertyValue<bool>(nameof(ExportEventData));
            set => SetPropertyValue(nameof(ExportEventData), value);
        }
        
        /// <summary>
        /// Gets or sets flag that determines if fault data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportFaultDataCheckBox) + ".Checked")]
        [Description("Flag that determines fault data should be exported.")]
        [UserScopedSetting]
        public bool ExportFaultData
        { 
            get => GetPropertyValue<bool>(nameof(ExportFaultData));
            set => SetPropertyValue(nameof(ExportFaultData), value);
        }
        
        /// <summary>
        /// Gets or sets flag that determines if disturbance data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportDisturbanceDataCheckBox) + ".Checked")]
        [Description("Flag that determines disturbance data should be exported.")]
        [UserScopedSetting]
        public bool ExportDisturbanceData
        { 
            get => GetPropertyValue<bool>(nameof(ExportDisturbanceData));
            set => SetPropertyValue(nameof(ExportDisturbanceData), value);
        }
        
        /// <summary>
        /// Gets or sets flag that determines if breaker operation data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportBreakerOperationCheckBox) + ".Checked")]
        [Description("Flag that determines breaker operation data should be exported.")]
        [UserScopedSetting]
        public bool ExportBreakerOperationData
        { 
            get => GetPropertyValue<bool>(nameof(ExportBreakerOperationData));
            set => SetPropertyValue(nameof(ExportBreakerOperationData), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if waveform data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportWaveformDataCheckBox) + ".Checked")]
        [Description("Flag that determines waveform data should be exported.")]
        [UserScopedSetting]
        public bool ExportWaveformData
        { 
            get => GetPropertyValue<bool>(nameof(ExportWaveformData));
            set => SetPropertyValue(nameof(ExportWaveformData), value);
        }

        /// <summary>
        /// Gets or sets flag that determines if frequency domain data should be exported.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.ExportFrequencyDomainDataCheckBox) + ".Checked")]
        [Description("Flag that determines frequency domain data should be exported.")]
        [UserScopedSetting]
        public bool ExportFrequencyDomainData
        { 
            get => GetPropertyValue<bool>(nameof(ExportFrequencyDomainData));
            set => SetPropertyValue(nameof(ExportFrequencyDomainData), value);
        }

        /// <summary>
        /// Gets or sets the post size limit for repository data.
        /// </summary>
        [TypeConvertedValueExpression("Form." + nameof(MainForm.PostSizeLimitMaskedTextBox) + ".Text")]
        [Description("Post size limit for repository data.")]
        [UserScopedSetting]
        public int PostSizeLimit
        { 
            get => GetPropertyValue<int>(nameof(PostSizeLimit));
            set => SetPropertyValue(nameof(PostSizeLimit), value);
        }
    }
}
