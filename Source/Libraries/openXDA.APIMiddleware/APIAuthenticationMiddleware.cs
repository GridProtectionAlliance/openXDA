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

namespace openXDA.APIMiddleware
{
    /// <summary>
    /// Middleware to authenticate users of the API via <see cref="APIAccessKey"/>.
    /// </summary>
    public partial class APIAuthenticationMiddleware : OwinMiddleware
    {
        #region [ Members ]

        // Nested Types
        protected class AuthorizationHeader
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

                if (!AuthorizationType.Equals(Type, StringComparison.Ordinal) && !ImpersonationType.Equals(Type, StringComparison.Ordinal))
                    return;

                if (Credentials is null)
                    return;

                byte[] credentialData = Convert.FromBase64String(Credentials);
                string decode = Encoding.UTF8.GetString(credentialData);
                int index = decode.IndexOf(':');

                APIKey = decode.Substring(0, index);

                if (AuthorizationType.Equals(Type, StringComparison.Ordinal))
                {
                    APIToken = decode.Substring(index + 1);
                    ImpersonationToken = null;
                }
                else
                { 
                    int impersonationIndex = decode.LastIndexOf(':');
                    if (index == impersonationIndex)
                        return;
                    APIToken = decode.Substring(index + 1, impersonationIndex - index - 1);
                    ImpersonationToken = decode.Substring(impersonationIndex + 1);
                }

                Valid = true;
            }

            public string Type { get; }
            public string Credentials { get; }
            public string APIKey { get; }
            public string APIToken { get; }
            public string ImpersonationToken { get; }
            public bool Valid { get; }
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
        private const string ImpersonationType = "XDA-API-IMP";

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
            Authorize(context.Request);
            await Next.Invoke(context); 
        }
        
        private void Authorize(IOwinRequest request)
        {
            AuthorizationHeader authorization = new AuthorizationHeader(request.Headers);

            if (!authorization.Valid || !IsAuthorized(authorization))
                return;
            
            if (!UseImpersonation(authorization))
            {
                string apiKey = authorization.APIKey;
                ISecurityProvider provider = new HostSecurityProvider(apiKey);
                SecurityIdentity identity = new SecurityIdentity(provider);
                SecurityPrincipal principal = new SecurityPrincipal(identity);
                request.User = principal;
            }
            else
            {
                string impersonatedUser = authorization.ImpersonationToken;
                ISecurityProvider provider = new ImpersonationSecurityProvider(impersonatedUser);

                if (!provider.RefreshData())
                    return;
                
                if (!provider.Authenticate())
                    return;

                SecurityIdentity identity = new SecurityIdentity(provider);
                SecurityPrincipal principal = new SecurityPrincipal(identity);
                request.User = principal;
            }
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

        /// <summary>
        /// Function that returns the API Token associated with a given API Key
        /// </summary>
        /// <param name="registrationKey"> The API Key</param>
        /// <returns>The API Token associated with this Key</returns>
        protected virtual string GetAPIToken(string registrationKey)
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

        /// <summary>
        /// Returns a flag determining whether the Given AuthorizationHeader is allowed to impersonate a different user
        /// </summary>
        /// <param name="header">The authorization header containing the API key and optional impersonation token.</param>
        /// <returns>true if the request is allowed to impersonate a different user; otherwise, false.</returns>
        protected virtual bool UseImpersonation(AuthorizationHeader header)
        {
            string registrationKey = header.APIKey;

            if (registrationKey is null || header.ImpersonationToken is null)
                return false;

            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueryFormat =
                    "SELECT AllowImpersonation " +
                    "FROM APIAccessKey " +
                    "WHERE RegistrationKey = {0} AND (Expires IS NULL OR Expires > GETUTCDATE())";

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
