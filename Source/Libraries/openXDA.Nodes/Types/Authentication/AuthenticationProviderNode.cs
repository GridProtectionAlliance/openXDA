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
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Security.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using Node = openXDA.Model.Node;

namespace openXDA.Nodes.Types.Authentication
{
    public class AuthenticationProviderNode : NodeBase
    {
        #region [ Members ]

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
            public string Nonce { get; set; }
        }

        public class TokenResponse
        {
            public string access_token { get; set; }
            public string id_token { get; set; }
        }

        private class AuthenticationProviderWebController : ApiController
        {
            private AuthenticationProviderNode Node { get; }

            public AuthenticationProviderWebController(AuthenticationProviderNode node) =>
                Node = node;

            [HttpPost]
            public IHttpActionResult AuthorizeCode(string userId, string userName, string userGivenName, string userLastName, string userPhone, string userEmail, string userRoles, string appId, string nonce) =>
                Ok(Node.GenerateCode(userId, userName, userGivenName, userLastName, userPhone, userEmail, userRoles, appId, nonce));

            [HttpPost]
            public IHttpActionResult GetToken(string code, string appId) => Ok(Node.GenerateToken(code, appId));
        }

        #endregion

        #region [ Constructors ]

        public AuthenticationProviderNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                DateTime expiration = DateTime.UtcNow.AddYears(10);

                List<ApplicationSustainedUser> sessions = new TableOperations<ApplicationSustainedUser>(connection).QueryRecords().ToList();
                sessions.ForEach((session) =>
                {
                    UserAccount user = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", session.UserAccountID);
                    UserInfoAuth userInfoAuth = new UserInfoAuth()
                    {
                        UserId = user.ID.ToString(),
                        UserName = user.Name,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.Phone,
                        Roles = "Viewer",
                        Expires = expiration,
                        AppId = session.ApplicationNodeID,
                        Nonce = "Not A Valid Nonce"
                    };

                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                    cacheItemPolicy.AbsoluteExpiration = expiration;
                    s_codeStore.Add(session.Code, userInfoAuth, cacheItemPolicy);
                });
            }            
        }

        #endregion

        #region [ Methods ]

        public string GenerateCode(string userId, string userName, string userGivenName, string userLastName, string userPhone, string userEmail, string userRoles, string appId, string nonce)
        {
            byte[] codeBytes = new byte[16];
            GSF.Security.Cryptography.Random.GetBytes(codeBytes);

            string code = BitConverter.ToString(codeBytes).Replace("-", "");
            DateTime expiration = DateTime.UtcNow.AddMinutes(10);

            UserInfoAuth userInfoAuth = new UserInfoAuth()
            {
                UserId = userId,
                UserName = userName,
                FirstName = userGivenName,
                LastName = userLastName,
                Email = userEmail,
                Phone = userPhone,
                Roles = userRoles,
                Expires = expiration,
                AppId = appId,
                Nonce = nonce
            };

            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = expiration;
            s_codeStore.Add(code, userInfoAuth, cacheItemPolicy);

            return code;
        }

        public TokenResponse GenerateToken(string code, string appId)
        {
            UserInfoAuth user = s_codeStore.Get(code) as UserInfoAuth;

            if (user is null)
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
            idToken.Add("nonce", user.Nonce);
            idToken.Add("name", user.UserName);
            idToken.Add("given_name", user.FirstName);
            idToken.Add("family_name", user.LastName);
            idToken.Add("phone_number", user.Phone);
            idToken.Add("email", user.Email);
            idToken.Add("roles", user.Roles);

            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(header.ToString())) + "." + Convert.ToBase64String(Encoding.UTF8.GetBytes(idToken.ToString()));
            return new TokenResponse() { id_token = token, access_token = token };
        }

        public override IHttpController CreateWebController() =>
            new AuthenticationProviderWebController(this);

        #endregion

        #region [ Static ]

        // Static Fields
        private static MemoryCache s_codeStore = new MemoryCache(typeof(AuthenticationProviderNode).FullName);
      
        #endregion
    }
}
