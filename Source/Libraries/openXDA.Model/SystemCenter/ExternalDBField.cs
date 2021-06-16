//******************************************************************************************************
//  ExternalDBField.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  06/15/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

namespace SystemCenter.Model
{
    /// <summary>
    /// Model used for temporary storage of External Fields obtained from external Database.
    /// Once the User confirms these turn into <see cref="AdditionalFieldValue"/>.
    /// </summary>
    public class ExternalDBField
    {
        /// <summary>
        /// If the Value already exists this is it' ID, otherwise it's null
        /// </summary>
        public int? FieldValueID { get; set; }
        /// <summary>
        /// This is the ID of the Corresponding XDA object
        /// </summary>
        public int OpenXDAParentTableID { get; set; }
        /// <summary>
        /// This is the ID of the Additional Field id it is an AdditionalField in System Center
        /// </summary>
        public int AdditionalFieldID { get; set; }
        /// <summary>
        /// This is the new Value from the external DB
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// This is the name of the Field - Either the Additional Field or the XDA Field
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// This is the previous Value of the Field
        /// </summary>
        public string PreviousValue { get; set; }
        /// <summary>
        /// If it encountered an Error trying to update the field this will be true
        /// </summary>
        public bool Error { get; set; }
        /// <summary>
        /// This is a potential Error or Warning Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// This is True if it is an XDA Field and needs additional Processing
        /// </summary>
        public bool isXDAField { get; set; }
        /// <summary>
        /// This Flag indicates whether the user made a change before saving the information
        /// </summary>
        public bool Changed { get; set; }
        /// <summary>
        /// This the Name of the object to be displayed if multiple objects are present
        /// </summary>
        public string DisplayName { get; set; }
    }
}
