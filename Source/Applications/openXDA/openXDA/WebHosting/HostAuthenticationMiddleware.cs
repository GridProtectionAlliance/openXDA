//******************************************************************************************************
//  HostAuthenticationMiddleware.cs - Gbtc
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
//  02/17/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data;
using GSF.Security;
using Microsoft.Owin;
using Owin;

namespace openXDA.WebHosting
{
    public class HostAuthenticationMiddleware : OwinMiddleware
    {
        #region [ Members ]

        // Nested Types
        private class AuthorizationHeader
        {
            private const string HeaderKey = "Authorization";

            public AuthorizationHeader(IHeaderDictionary headers)
            {
                IEnumerable<string> values = headers.GetValues(HeaderKey)
                    ?? Enumerable.Empty<string>();

                IList<string> headerValues = values
                    .DefaultIfEmpty("")
                    .First()
                    .Split(' ');

                Type = headerValues.FirstOrDefault();
                Credentials = headerValues.Skip(1).FirstOrDefault();

                if (!AuthorizationType.Equals(Type, StringComparison.Ordinal))
                    return;

                if (Credentials is null)
                    return;

                byte[] credentialData = Convert.FromBase64String(Credentials);
                string decode = Encoding.UTF8.GetString(credentialData);
                string[] splitDecode = decode.Split(':');
                APIKey = splitDecode[0];
                APIToken = splitDecode.Skip(1).FirstOrDefault();
            }

            public string Type { get; }
            public string Credentials { get; }
            public string APIKey { get; }
            public string APIToken { get; }
        }

        private class HostSecurityProvider : SecurityProviderBase
        {
            public HostSecurityProvider(string username)
                : base(username, true, false, false)
            {
                IsUserAuthenticated = true;
                UserData.Roles.Add("Host");
            }

            public override bool RefreshData() => true;
            public override bool Authenticate() => true;

            public override bool ChangePassword(string oldPassword, string newPassword) =>
                throw new NotImplementedException();

            public override bool ResetPassword(string securityAnswer) =>
                throw new NotImplementedException();
        }

        // Constants
        private const string AuthorizationType = "XDA-Host";

        #endregion

        #region [ Constructors ]

        public HostAuthenticationMiddleware(OwinMiddleware next, Func<AdoDataConnection> connectionFactory)
            : base(next)
        {
            ConnectionFactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        public async override Task Invoke(IOwinContext context)
        {
            IOwinRequest request = context.Request;
            AuthorizationHeader authorization = new AuthorizationHeader(request.Headers);

            if (IsAuthorized(authorization))
            {
                string apiKey = authorization.APIKey;
                ISecurityProvider provider = new HostSecurityProvider(apiKey);
                SecurityIdentity identity = new SecurityIdentity(provider);
                SecurityPrincipal principal = new SecurityPrincipal(identity);
                context.Request.User = principal;
            }

            await Next.Invoke(context);
        }

        private bool IsAuthorized(AuthorizationHeader authorization)
        {
            string apiKey = authorization.APIKey;
            string requestToken = authorization.APIToken;
            string hostToken = GetHostToken(apiKey);

            return
                !(apiKey is null) &&
                !(requestToken is null) &&
                requestToken == hostToken;
        }

        private string GetHostToken(string registrationKey)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueryFormat =
                    "SELECT APIToken " +
                    "FROM ActiveHost " +
                    "WHERE RegistrationKey = {0}";

                return connection.ExecuteScalar<string>(QueryFormat, new object[] { registrationKey });
            }
        }

        #endregion
    }

    namespace Extensions
    {
        public static class HostAuthenticationMiddlewareExtensions
        {
            public static void UseHostAuthentication(this IAppBuilder app, Func<AdoDataConnection> connectionFactory) =>
                app.Use<HostAuthenticationMiddleware>(connectionFactory);
        }
    }
}
