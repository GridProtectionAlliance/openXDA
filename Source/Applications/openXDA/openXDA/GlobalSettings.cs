using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA
{
    public class GlobalSettings
    {
        public string CompanyName
        {
            get;
            set;
        }

        public string CompanyAcronym
        {
            get;
            set;
        }

        public Guid NodeID
        {
            get;
            set;
        }

        public string ApplicationName
        {
            get;
            set;
        }

        public string ApplicationDescription
        {
            get;
            set;
        }

        public string ApplicationKeywords
        {
            get;
            set;
        }

        public string DateFormat
        {
            get;
            set;
        }

        public string TimeFormat
        {
            get;
            set;
        }

        public string DateTimeFormat
        {
            get;
            set;
        }

        public string PasswordRequirementsRegex
        {
            get;
            set;
        }

        public string PasswordRequirementsError
        {
            get;
            set;
        }

        public string BootstrapTheme
        {
            get;
            set;
        }

        public int DefaultDialUpRetries
        {
            get;
            set;
        }

        public int DefaultDialUpTimeout
        {
            get;
            set;
        }

        public string DefaultFTPUserName
        {
            get;
            set;
        }

        public string DefaultFTPPassword
        {
            get;
            set;
        }

        public string DefaultRemotePath
        {
            get;
            set;
        }

        public string DefaultLocalPath
        {
            get;
            set;
        }

    }
}
