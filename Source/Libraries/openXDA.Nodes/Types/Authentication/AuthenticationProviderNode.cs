//******************************************************************************************************
//  AuthenticationProviderNode.cs - Gbtc
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
//  10/07/2021 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes.Types.Authentication
{
    public class AuthenticationProviderNode : NodeBase
    {
        #region [ Members ]

        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);
        }

        private class UserInfoAuth
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Roles { get; set; }
            public DateTime Expires { get; set; }
            public string AppId { get; set; }
        }

        public class TokenResponse
        {
            public string access_token { get; set; }
            public string id_token { get; set; }
        }

        private class AutheticationProviderWebController : ApiController
        {
            private AuthenticationProviderNode Node { get; }

            public AutheticationProviderWebController(AuthenticationProviderNode node) =>
                Node = node;

            [HttpPost]
            public IHttpActionResult AuthorizeCode(string userId, string userName, string userGivenName, string userLastName, string userPhone, string userEmail, string userRoles, string appId) =>
                Ok(Node.GenerateCode(userId, userName, userGivenName, userLastName, userPhone, userEmail, userRoles, appId));

            [HttpPost]
            public IHttpActionResult GetToken(string code, string appId) => Ok(Node.GenerateToken(code, appId));
        }

        #endregion

        #region [ Constructors ]

        public AuthenticationProviderNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Configurator = GetConfigurator();
        }

        #endregion

        #region [ Properties ]

        private Action<object> Configurator { get; set; }

        #endregion

        #region [ Methods ]


        public string GenerateCode(string userId, string userName, string userGivenName, string userLastName, string userPhone, string userEmail, string userRoles, string appId)
        {
          
            byte[] codeBytes = new byte[16];
            new Random().NextBytes(codeBytes);

            string code = BitConverter.ToString(codeBytes).Replace("-", "");
            s_codeStore.Add(code, new UserInfoAuth()
            {
                UserId = userId,
                UserName = userName,
                FirstName = userGivenName,
                LastName = userLastName,
                Email = userEmail,
                Phone = userPhone,
                Roles = userRoles,
                Expires = DateTime.UtcNow.AddMinutes(10),
                AppId = appId
            });

            s_codeStore = s_codeStore.Where(item => item.Value.Expires > DateTime.UtcNow).ToDictionary(i => i.Key, i => i.Value);

            return code;
        }

        public TokenResponse GenerateToken(string code, string appId)
        {
            UserInfoAuth user;
            if (!s_codeStore.TryGetValue(code, out user))
                return new TokenResponse() { id_token = "", access_token = "" };

            if (user.AppId != appId)
                return new TokenResponse() { id_token = "", access_token = "" };

            if (user.Expires < DateTime.UtcNow)
                return new TokenResponse() { id_token = "", access_token = "" };

            JObject header = new JObject();
            header.Add("alg", "None");
            header.Add("typ", "JWT");
            header.Add("kid", "None");

            JObject idToken = new JObject();
            idToken.Add("sub", user.UserId);
            idToken.Add("name", user.UserName);
            idToken.Add("given_name", user.FirstName);
            idToken.Add("family_name", user.LastName);
            idToken.Add("phone_number", user.Phone);
            idToken.Add("email", user.Email);
            idToken.Add("roles", user.Roles);
            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(header.ToString())) + "." + Convert.ToBase64String(Encoding.UTF8.GetBytes(idToken.ToString()));

            
            return new TokenResponse() { id_token= token, access_token = token };
        }

        protected override void OnReconfigure(Action<object> configurator) =>
            Configurator = configurator;

        public override IHttpController CreateWebController() =>
            new AutheticationProviderWebController(this);

      

        
        private void Configure(object obj) => Configurator(obj);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthenticationProviderNode));
        private static Dictionary<string, UserInfoAuth> s_codeStore = new Dictionary<string, UserInfoAuth>();

      
        #endregion
    }
}
