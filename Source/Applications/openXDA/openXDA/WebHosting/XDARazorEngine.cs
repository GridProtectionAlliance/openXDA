//******************************************************************************************************
//  XDARazorEngine.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  01/04/2024 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Threading.Tasks;
using GSF.Web.Model;
using openXDA.Nodes;
using RazorEngine.Templating;

namespace openXDA.WebHosting
{
    public class XDARazorEngine<TLanguage> : IRazorEngine where TLanguage : LanguageConstraint, new()
    {
        #region [ Constructors ]

        public XDARazorEngine(Host nodeHost, string templatePath)
        {
            NodeHost = nodeHost;
            GSFRazorEngine = new RazorEngine<TLanguage>(templatePath);
        }

        #endregion

        #region [ Properties ]

        public string TemplatePath =>
            GSFRazorEngine.TemplatePath;

        private Host NodeHost { get; }
        private IRazorEngine GSFRazorEngine { get; }
        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public void AddTemplate(ITemplateKey key, ITemplateSource templateSource) =>
            GSFRazorEngine.AddTemplate(key, templateSource);

        public void Compile(ITemplateKey key, Type modelType = null) =>
            GSFRazorEngine.Compile(key, modelType);

        public ITemplateKey GetKey(string name, ResolveType resolveType = ResolveType.Global, ITemplateKey context = null) =>
            GSFRazorEngine.GetKey(name, resolveType, context);

        public bool IsTemplateCached(ITemplateKey key, Type modelType) =>
            GSFRazorEngine.IsTemplateCached(key, modelType);

        public Task PreCompile(Action<Exception> exceptionHandler = null) =>
            GSFRazorEngine.PreCompile(exceptionHandler);

        public void Run(ITemplateKey key, TextWriter writer, Type modelType = null, object model = null, DynamicViewBag viewBag = null)
        {
            viewBag?.AddValue(nameof(NodeHost), NodeHost);
            GSFRazorEngine.Run(key, writer, modelType, model, viewBag);
        }

        public void RunCompile(ITemplateKey key, TextWriter writer, Type modelType = null, object model = null, DynamicViewBag viewBag = null)
        {
            viewBag?.AddValue(nameof(NodeHost), NodeHost);
            GSFRazorEngine.RunCompile(key, writer, modelType, model, viewBag);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            try { GSFRazorEngine.Dispose(); }
            finally { IsDisposed = true; }
        }

        #endregion
    }
}