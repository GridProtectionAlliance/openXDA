using GSF.Configuration;
using GSF.Data;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openXDA.NotificationDataSources
{
    public class SQLDataSource : ITriggeredDataSource
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [SettingName("SQLStatement")]
            public string SQL { get; } = "SELECT NULL FOR XML PATH('Data')";
        }

        #endregion

        #region [ Constructors ]

        public SQLDataSource(TriggeredEmailDataSource definition, Func<AdoDataConnection> connectionFactory)
        {
            Definition = definition;
            Connectionfactory = connectionFactory;
        }
        #endregion

        #region [ Properties]

      
        public TriggeredEmailDataSource Definition { get; }

        public Func<AdoDataConnection> Connectionfactory { get; }

        #endregion

        #region [ Methods ]

        
        protected Action<object> GetConfigurator()
        {
            int dataSoruceID = Definition.ID;

            ConfigurationLoader configurationLoader = new ConfigurationLoader(Definition.ID, Connectionfactory);
            return configurationLoader.Configure;
        }

        public XElement Process(Event evt)
        {
            try
            {
                Settings settings = new Settings(GetConfigurator());

                using (AdoDataConnection connection = Connectionfactory())
                {
                    return XElement.Parse(connection.ExecuteScalar<string>(settings.SQL, evt.ID));
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
