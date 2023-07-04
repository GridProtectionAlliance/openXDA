//******************************************************************************************************
//  APIAuthenticationMiddleware.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  07/01/2022 - C. Lackner
//       Generated original version of source code.
//  01/04/2023 - C. Lackner
//       Added Impersonation Logic.
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

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Middleware to authenticate users of the API via <see cref="APIAccessKey"/>.
    /// </summary>
    public partial class APIAuthenticationMiddleware : OwinMiddleware
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
                int index = decode.IndexOf(':');
                int impersonationIndex = decode.LastIndexOf(':');

                if (index == impersonationIndex)
                {
                    ImpersonationToken = null;
                    impersonationIndex = decode.Length - 1;
                }
                else
                {
                    ImpersonationToken = decode.Substring(impersonationIndex + 1);
                }

                APIKey = decode.Substring(0, index);
                APIToken = decode.Substring(index + 1,impersonationIndex - index - 1);

            }

            public string Type { get; }
            public string Credentials { get; }
            public string APIKey { get; }
            public string APIToken { get; }
            public string ImpersonationToken { get; }
        }

        private class HostSecurityProvider : SecurityProviderBase
        {
            public HostSecurityProvider(string username)
                : base(username, true, false, false)
            {
                IsUserAuthenticated = true;
                UserData.Roles.Add("API");
            }

            public override bool RefreshData() => true;
            public override bool Authenticate() => true;

            public override bool ChangePassword(string oldPassword, string newPassword) =>
                throw new NotImplementedException();

            public override bool ResetPassword(string securityAnswer) =>
                throw new NotImplementedException();
        }

        // Constants
        private const string AuthorizationType = "XDA-API";

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="APIAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="connectionFactory">Factory function for creating database connections.</param>
        public APIAuthenticationMiddleware(OwinMiddleware next, Func<AdoDataConnection> connectionFactory)
            : base(next)
        {
            ConnectionFactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The context in which the request is made.</param>
        /// <returns>The task that indicates when the request has been processed.</returns>
        public async override Task Invoke(IOwinContext context)
        {
            IOwinRequest request = context.Request;
            AuthorizationHeader authorization = new AuthorizationHeader(request.Headers);

            if (IsAuthorized(authorization) && !UseImpersonation(authorization))
            {
                string apiKey = authorization.APIKey;
                ISecurityProvider provider = new HostSecurityProvider(apiKey);
                SecurityIdentity identity = new SecurityIdentity(provider);
                SecurityPrincipal principal = new SecurityPrincipal(identity);
                context.Request.User = principal;
            }

            else if (IsAuthorized(authorization) && UseImpersonation(authorization))
            {
                string impersonatedUser = authorization.ImpersonationToken;
                string apiKey = authorization.APIKey;

                ISecurityProvider provider = new ImpersonationSecurityProvider(impersonatedUser);

                // if Impersonation fails we fall back to API Authentication
                if (!provider.IsUserAuthenticated)
                    provider = new HostSecurityProvider(apiKey);

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
            string hostToken = GetAPIToken(apiKey);

            return
                !(apiKey is null) &&
                !(requestToken is null) &&
                requestToken == hostToken;
        }

        private string GetAPIToken(string registrationKey)
        {
            if (registrationKey is null)
                return null;

            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueryFormat =
                    "SELECT APIToken " +
                    "FROM APIAccessKey " +
                    "WHERE RegistrationKey = {0} AND (Expires IS NULL OR Expires > GETUTCDATE())";

                return connection.ExecuteScalar<string>(QueryFormat, new object[] { registrationKey });
            }
        }

        private bool UseImpersonation(AuthorizationHeader header)
        {
            string registrationKey = header.APIKey;

            if (registrationKey is null || header.ImpersonationToken is null)
                return false;

            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueryFormat =
                    "SELECT AllowImpersonation " +
                    "FROM APIAccessKey " +
                    "WHERE RegistrationKey = {0} AND (Expires IS NULL OR Expires > GETUTCDATE())" +
                    "UNION SELECT 0 ORDER BY AllowImpersonation DESC";

                return connection.ExecuteScalar<bool>(QueryFormat, new object[] { registrationKey });
            }
        }

        #endregion
    }

    namespace Extensions
    {
        /// <summary>
        /// Extension methods for hosting API authentication.
        /// </summary>
        public static class APIAuthenticationMiddlewareExtensions
        {
            /// <summary>
            /// Enables use of the API authentication middleware in the app.
            /// </summary>
            /// <param name="app">The app in which the middlware will be used.</param>
            /// <param name="connectionFactory">Factory for creating database connections.</param>
            public static void UseAPIAuthentication(this IAppBuilder app, Func<AdoDataConnection> connectionFactory) =>
                app.Use<APIAuthenticationMiddleware>(connectionFactory);
        }
    }
}
