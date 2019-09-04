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
using System.ComponentModel;
using System.Configuration;
using ExpressionEvaluator;
using GSF.ComponentModel;
using GSF.Configuration;

namespace XDACloudDataPusher
{
    /// <summary>
    /// Defines settings for the XDACloudDataPusher application.
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
    public sealed class Settings : CategorizedSettingsBase<Settings>
    {
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
        [TypeConvertedValueExpression("Form.XDAUrlTextBox.Text")]
        [Description("Root URL for openXDA instance to query.")]
        [UserScopedSetting]
        public string XDARootURL { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if source type to query is lines.
        /// </summary>
        [TypeConvertedValueExpression("Form.QueryLinesRadioButton.Checked")]
        [Description("Flag that determines if the source type to query is lines. Value is mutually exclusive of SourceQueryTypeIsMeters.")]
        [UserScopedSetting]
        public bool SourceQueryTypeIsLines { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if source type to query is meters.
        /// </summary>
        [TypeConvertedValueExpression("Form.QueryMetersRadioButton.Checked")]
        [Description("Flag that determines if the source type to query is meters. Value is mutually exclusive of SourceQueryTypeIsLines.")]
        [UserScopedSetting]
        public bool SourceQueryTypeIsMeters { get; set; }

        /// <summary>
        /// Gets or sets start date/time for query.
        /// </summary>
        [TypeConvertedValueExpression("Form.StartDateTimePicker.Value")]
        [Description("Start date/time for query.")]
        [UserScopedSetting]
        public DateTime StartDateTimeForQuery { get; set; }

        /// <summary>
        /// Gets or sets end date/time for query.
        /// </summary>
        [TypeConvertedValueExpression("Form.EndDateTimePicker.Value")]
        [Description("End date/time for query.")]
        [UserScopedSetting]
        public DateTime EndDateTimeForQuery { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if cloud repository is Azure.
        /// </summary>
        [TypeConvertedValueExpression("Form.AzureRadioButton.Checked")]
        [Description("Flag that determines if cloud repository is Azure. Value is mutually exclusive of CloudRepositoryIsAWS.")]
        [UserScopedSetting]
        public bool CloudRepositoryIsAzure { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if cloud repository is Amazon Web Services.
        /// </summary>
        [TypeConvertedValueExpression("Form.AWSRadioButton.Checked")]
        [Description("Flag that determines if cloud repository is Amazon Web Services. Value is mutually exclusive of CloudRepositoryIsAzure.")]
        [UserScopedSetting]
        public bool CloudRepositoryIsAWS { get; set; }

        /// <summary>
        /// Gets or sets cloud repository connection string.
        /// </summary>
        [TypeConvertedValueExpression("Form.ConnectionStringTextBox.Text")]
        [Description("Defines the cloud repository connection string.")]
        [UserScopedSetting]
        public string CloudRepostioryConnectionString { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if event data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportEventDataCheckBox.Checked")]
        [Description("Flag that determines event data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportEventData { get; set; }
        
        /// <summary>
        /// Gets or sets flag that determines if fault data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportFaultDataCheckBox.Checked")]
        [Description("Flag that determines fault data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportFaultData { get; set; }
        
        /// <summary>
        /// Gets or sets flag that determines if disturbance data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportDisturbanceDataCheckBox.Checked")]
        [Description("Flag that determines disturbance data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportDisturbanceData { get; set; }
        
        /// <summary>
        /// Gets or sets flag that determines if breaker operation data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportBreakerOperationCheckBox.Checked")]
        [Description("Flag that determines breaker operation data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportBreakerOperationData { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if waveform data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportWaveformDataCheckBox.Checked")]
        [Description("Flag that determines waveform data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportWaveformData { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if frequency domain data should be exported to cloud.
        /// </summary>
        [TypeConvertedValueExpression("Form.ExportFrequencyDomainDataCheckBox.Checked")]
        [Description("Flag that determines frequency domain data should be exported to cloud.")]
        [UserScopedSetting]
        public bool ExportFrequencyDomainData { get; set; }
    }
}
