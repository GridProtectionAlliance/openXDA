//******************************************************************************************************
//  AppModel.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  05/02/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Web.Model;
using RazorEngine.Templating;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDAAlarmCreationApp.Model
{
    /// <summary>
    /// Defines a base application model with convenient global settings and functions.
    /// </summary>
    /// <remarks>
    /// Custom view models should inherit from AppModel because the "Global" property is used by Layout.cshtml.
    /// </remarks>
    public class AppModel
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="AppModel"/>.
        /// </summary>
        public AppModel()
        {
            Global = (object)MainWindow.Model != null ? MainWindow.Model.Global : new GlobalSettings();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets global settings for application.
        /// </summary>
        public GlobalSettings Global
        {
            get;
        }

        #endregion

        /// <summary>
        /// Generates template based select field based on reflected modeled table field attributes with values derived from ValueList table.
        /// </summary>
        /// <typeparam name="T">Modeled table for select field.</typeparam>
        /// <param name="groupName">Value list group name as defined in ValueListGroup table.</param>
        /// <param name="fieldName">Field name for value of select field.</param>
        /// <param name="fieldLabel">Label name for select field, pulls from <see cref="LabelAttribute"/> if defined, otherwise defaults to <paramref name="fieldName"/>.</param>
        /// <param name="fieldID">ID to use for select field; defaults to select + <paramref name="fieldName"/>.</param>
        /// <param name="groupDataBinding">Data-bind operations to apply to outer form-group div, if any.</param>
        /// <param name="labelDataBinding">Data-bind operations to apply to label, if any.</param>
        /// <param name="customDataBinding">Extra custom data-binding operations to apply to field, if any.</param>
        /// <param name="dependencyFieldName">Defines default "enabled" subordinate data-bindings based a single boolean field, e.g., a check-box.</param>
        /// <param name="optionDataBinding">Data-bind operations to apply to each option value, if any.</param>
        /// <param name="toolTip">Tool tip text to apply to field, if any.</param>
        /// <returns>Generated HTML for new text field based on modeled table field attributes.</returns>
        public static string AddDirectoryBrowser(string fieldName, bool required, int maxLength = 0, string inputType = null, string fieldLabel = null, string fieldID = null, string groupDataBinding = null, string labelDataBinding = null, string requiredDataBinding = null, string customDataBinding = null, string dependencyFieldName = null, string toolTip = null)
        {

            IRazorEngine m_razorEngine = RazorEngine<CSharpEmbeddedResource>.Default;
            RazorView addInputFieldTemplate = new RazorView(m_razorEngine, $"DirectoryBrowser.cshtml", null);
            DynamicViewBag viewBag = addInputFieldTemplate.ViewBag;

            if (string.IsNullOrEmpty(fieldID))
                fieldID = $"input{fieldName}";

            viewBag.AddValue("FieldName", fieldName);
            viewBag.AddValue("Required", required);
            viewBag.AddValue("MaxLength", maxLength);
            viewBag.AddValue("InputType", inputType ?? "text");
            viewBag.AddValue("FieldLabel", fieldLabel ?? fieldName);
            viewBag.AddValue("FieldID", fieldID);
            viewBag.AddValue("GroupDataBinding", groupDataBinding);
            viewBag.AddValue("LabelDataBinding", labelDataBinding);
            viewBag.AddValue("RequiredDataBinding", requiredDataBinding);
            viewBag.AddValue("CustomDataBinding", customDataBinding);
            viewBag.AddValue("DependencyFieldName", dependencyFieldName);
            viewBag.AddValue("ToolTip", toolTip);

            return addInputFieldTemplate.Execute();
        }


    }
}
