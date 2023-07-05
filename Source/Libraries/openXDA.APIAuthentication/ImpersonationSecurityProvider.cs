//******************************************************************************************************
//  ImpersonationSecurityProvider.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  07/04/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Configuration;
using GSF.Diagnostics;
using GSF.Security;


namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Middleware to authenticate users of the API via <see cref="APIAccessKey"/>.
    /// </summary>
    public partial class APIAuthenticationMiddleware
    {
        private class ImpersonationSecurityProvider : AdoSecurityProvider
        {
            #region [ Members ]
            private const string DefaultMessageUserNotDefined = "User \"{0}\" is not defined.";
            private const string DefaultMessageUserIsDisabled = "User \"{0}\" is disabled.";
            private const string DefaultMessageUserIsLockedOut = "User \"{0}\" is not locked out.";
            private const string DefaultMessageUserPasswordExpired = "User \"{0}\" has an expired password or password has not been set.";
            private const string DefaultMessageUserHasNoRoles = "User \"{0}\" has not been assigned any roles and therefore has no rights. Contact your administrator.";
            #endregion

            #region [ Constructor ]
            public ImpersonationSecurityProvider(string username)
              : base(username, true, false, false)
            {
                UserData = new UserData(username);
                IsUserAuthenticated = Authenticate();
            }

            #endregion

            #region [ Methods ]

            public override bool ChangePassword(string oldPassword, string newPassword) =>
                throw new NotImplementedException();

            public override bool ResetPassword(string securityAnswer) =>
                throw new NotImplementedException();

            public override bool Authenticate() 
            {
                bool refresh = RefreshData();
                if (!refresh)
                    return false;

                bool isAuthenticated = false;
                if (!UserData.IsDefined)
                {
                    AuthenticationFailureReason = getUserAuthFailureReason("MessageUserNotDefined", DefaultMessageUserNotDefined);
                }
                else if (UserData.IsDisabled)
                {
                    AuthenticationFailureReason = getUserAuthFailureReason("MessageUserIsDisabled", DefaultMessageUserIsDisabled);
                }
                else if (UserData.IsLockedOut)
                {
                    AuthenticationFailureReason = getUserAuthFailureReason("MessageUserIsLockedOut", DefaultMessageUserIsLockedOut);
                }
                else if (UserData.PasswordChangeDateTime != DateTime.MinValue && UserData.PasswordChangeDateTime <= DateTime.UtcNow)
                {
                    AuthenticationFailureReason = getUserAuthFailureReason("MessageUserPasswordExpired", DefaultMessageUserPasswordExpired);
                }
                else if (UserData.Roles.Count == 0)
                {
                    AuthenticationFailureReason = getUserAuthFailureReason("MessageUserHasNoRoles", DefaultMessageUserHasNoRoles);
                }
                else
                {
                    isAuthenticated = true;
                }

                try
                {
                    // Log user authentication result
                    LogAuthenticationAttempt(isAuthenticated);
                }
                catch (Exception ex)
                {
                    // Writing data will fail for read-only databases;
                    // all we can do is track last exception in this case
                    LastException = ex;
                    Log.Publish(MessageLevel.Warning, MessageFlags.SecurityMessage, "Authenticate", "Failed to log authentication attempt to database.", "Database or AccessLog table may be read-only or inaccessible.", ex);
                }
                return isAuthenticated;
            }

            private string getUserAuthFailureReason(string settingName, string defaultValue)
            {
                string settingValue;

                try
                {
                    ConfigurationFile config = ConfigurationFile.Current;
                    CategorizedSettingsElementCollection settings = config.Settings[SettingsCategory];
                    settingValue = settings[settingName].ValueAs(defaultValue);
                }
                catch (Exception ex)
                {
                    Logger.SwallowException(ex);
                    settingValue = defaultValue;
                }

                return string.Format(settingValue, UserData.LoginID);
            }
            #endregion

            #region [ Static ]

            private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(ImpersonationSecurityProvider), MessageClass.Component);

            #endregion
        }
    }
}
