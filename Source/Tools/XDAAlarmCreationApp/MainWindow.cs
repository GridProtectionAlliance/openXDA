//******************************************************************************************************
//  MainWindow.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/02/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using XDAAlarmCreationApp.Model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using GSF;
using GSF.Configuration;
using GSF.IO;
using GSF.Reflection;
using GSF.Web.Hosting;
using GSF.Web.Model;
using GSF.Web.Security;
using GSF.Security.Model;
using Microsoft.Owin.Hosting;
namespace XDAAlarmCreationApp
{
    public partial class MainWindow : Form
    {
        #region [ Members ]

        // Fields
        private int m_maxLines = 1000;
        private IDisposable m_webAppHost;

        #endregion

        #region [ Constructors ]

        public MainWindow()
        {
            InitializeComponent();

            string errorLogPath = ErrorLogger.ErrorLog.FileName;

            if (!Path.IsPathRooted(errorLogPath))
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string clientDataPath = Path.Combine(appData, "Grid Protection Alliance", "EPRI Drive Configuration");
                ErrorLogger.ErrorLog.FileName = Path.Combine(clientDataPath, errorLogPath);
            }
        }

        #endregion


        #region [ Methods ]

        private void MainWindow_Load(object sender, EventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                Model.Global.DefaultProjectPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(Model.Global.DefaultProjectPath));
                Directory.CreateDirectory(Model.Global.DefaultProjectPath);
            }
            catch (Exception ex)
            {
                LogException(new InvalidOperationException($"Failed to initialize default project path: {ex.Message}", ex));
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }

            try
            {
                // Attach to default web server events
                WebServer webServer = WebServer.Default;
                webServer.StatusMessage += WebServer_StatusMessage;
                webServer.ExecutionException += LoggedExceptionHandler;

                // Define types for Razor pages - self-hosted web service does not use view controllers so
                // we must define configuration types for all paged view model based Razor views here:
                webServer.PagedViewModelTypes.TryAdd("LineSegment.cshtml", new Tuple<Type, Type>(typeof(LineSegmentView), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("IndividualResult.cshtml", new Tuple<Type, Type>(typeof(IndividualResult), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("NodePoint.cshtml", new Tuple<Type, Type>(typeof(NodePoint), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("ColorGradients.cshtml", new Tuple<Type, Type>(typeof(ColorGradients), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("LineCharacteristicsToDisplay.cshtml", new Tuple<Type, Type>(typeof(LineCharacteristicsToDisplay), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("ResultsToDisplay.cshtml", new Tuple<Type, Type>(typeof(ResultsToDisplay), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Users.cshtml", new Tuple<Type, Type>(typeof(UserAccount), typeof(SecurityHub)));
                webServer.PagedViewModelTypes.TryAdd("Groups.cshtml", new Tuple<Type, Type>(typeof(SecurityGroup), typeof(SecurityHub)));
                webServer.PagedViewModelTypes.TryAdd("Settings.cshtml", new Tuple<Type, Type>(typeof(Setting), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Results.cshtml", new Tuple<Type, Type>(typeof(Result), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Security.cshtml", new Tuple<Type, Type>(typeof(Security), typeof(DataHub)));

                // Initiate pre-compile of base templates
                if (AssemblyInfo.EntryAssembly.Debuggable)
                {
                    RazorEngine<CSharpDebug>.Default.PreCompile(LogException);
                    RazorEngine<VisualBasicDebug>.Default.PreCompile(LogException);
                }
                else
                {
                    RazorEngine<CSharp>.Default.PreCompile(LogException);
                    RazorEngine<VisualBasic>.Default.PreCompile(LogException);
                }

                Random generator = new Random();
                string[] split = Model.Global.WebHostPortRange.Split('-');
                int minPort = Convert.ToInt32(split[0]);
                int maxPort = Convert.ToInt32(split[1]);
                int port = generator.Next(minPort, maxPort + 1);

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        Model.Global.WebHostURL = $"http://localhost:{port}";

                        LogStatus($"Attempting to initialize web hosting on [{Model.Global.WebHostURL}]...", false);

                        // Create new web application hosting environment
                        m_webAppHost = WebApp.Start<Startup>(Model.Global.WebHostURL);

                        // Open the main page in the user's default browser
                        using (Process.Start(Model.Global.WebHostURL)) { }

                        break;
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                        port = generator.Next(minPort, maxPort + 1);
                    }
                }

                ConfigurationFile.Current.Settings["systemSettings"]["WebHostURL"].Value = Model.Global.WebHostURL;
                ConfigurationFile.Current.Save();
            }
            catch (Exception ex)
            {
                LogException(new InvalidOperationException($"Failed to initialize web hosting: {ex.Message}", ex));
            }
        }

        private void MessagesTextBox_SizeChanged(object sender, EventArgs e)
        {

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            if (MessageBox.Show(this, $"Stopping application will terminate EPRI Drive Configuration Manager web functionality. Are you sure you want to stop the {Text}?", $"Shutdown {Text}...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((object)m_webAppHost != null)
                m_webAppHost.Dispose();

            ErrorLogger.Dispose();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenWebButton_Click(object sender, EventArgs e)
        {
            // Open the main page in the user's default browser
            using (Process.Start(Model.Global.WebHostURL)) { }
        }

        private void WebServer_StatusMessage(object sender, EventArgs<string> e)
        {
            LogStatus(e.Argument, false);
        }

        private void LoggedExceptionHandler(object sender, EventArgs<Exception> e)
        {
            LogException(e.Argument);
        }

        internal void LogStatus(string message, bool pushToHubClients)
        {
            if (pushToHubClients)
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string connectionID = state as string;

                    if (!string.IsNullOrEmpty(connectionID))
                    {
                        Program.HubClients.Client(connectionID).sendInfoMessage(message, 3000);
                    }
                    else
                    {
                        Thread.Sleep(1500);
                        Program.HubClients.All.sendInfoMessage(message, 3000);
                    }
                },
                DataHub.CurrentConnectionID);

            DisplayText(message);
        }

        private void LogException(Exception ex)
        {
            LogException(ex, false);
        }

        internal void LogException(Exception ex, bool pushToHubClients)
        {
            if (pushToHubClients)
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string connectionID = state as string;

                    if (!string.IsNullOrEmpty(connectionID))
                    {
                        Program.HubClients.Client(connectionID).sendErrorMessage(ex.Message, -1);
                    }
                    else
                    {
                        Thread.Sleep(1500);
                        Program.HubClients.All.sendErrorMessage(ex.Message, -1);
                    }
                },
                DataHub.CurrentConnectionID);

            ErrorLogger.Log(ex);

            while (ex is TargetInvocationException)
                ex = ex.InnerException;

            if ((object)ex != null)
                DisplayError(ex.Message);
        }

        private void DisplayText(string text)
        {
            if (InvokeRequired)
            {
                // Invoke UI updates on the UI thread
                BeginInvoke(new Action<string>(DisplayText), text);
                return;
            }

            // Append text to the text box
            MessagesTextBox.AppendText(text + "\n");

            // Truncate old messages when the text
            // exceeds the maximum number of lines
            MessagesTextBox.SelectionStart = 0;

            MessagesTextBox.SelectionLength = MessagesTextBox.Lines
                .Take(MessagesTextBox.Lines.Length - m_maxLines)
                .Aggregate(0, (length, line) => length + line.Length + "\n".Length);

            MessagesTextBox.ReadOnly = false;
            MessagesTextBox.SelectedText = "";
            MessagesTextBox.ReadOnly = true;

            // Scroll to bottom
            MessagesTextBox.SelectionStart = MessagesTextBox.TextLength;
            MessagesTextBox.ScrollToCaret();
        }

        private void DisplayError(string text)
        {
            if (InvokeRequired)
            {
                // Invoke UI updates on the UI thread
                BeginInvoke(new Action<string>(DisplayError), text);
                return;
            }

            // Start selection at the end of the text box
            // in order to set the color of the appended text
            MessagesTextBox.SelectionStart = MessagesTextBox.TextLength;
            MessagesTextBox.SelectionLength = 0;

            // Append text to the text box
            MessagesTextBox.SelectionColor = Color.Red;
            MessagesTextBox.AppendText(text + "\n");
            MessagesTextBox.SelectionColor = ForeColor;

            // Truncate old messages when the text
            // exceeds the maximum number of lines
            MessagesTextBox.SelectionStart = 0;

            MessagesTextBox.SelectionLength = MessagesTextBox.Lines
                .Take(MessagesTextBox.Lines.Length - m_maxLines)
                .Aggregate(0, (length, line) => length + line.Length + "\n".Length);

            MessagesTextBox.ReadOnly = false;
            MessagesTextBox.SelectedText = "";
            MessagesTextBox.ReadOnly = true;

            // Scroll to bottom
            MessagesTextBox.SelectionStart = MessagesTextBox.TextLength;
            MessagesTextBox.ScrollToCaret();
        }

        #endregion


        #region [ Static ]

        // Static Fields
        public static readonly AppModel Model;
        public static CategorizedSettingsElementCollection SystemSettings
        {
            get { return ConfigurationFile.Current.Settings["systemSettings"]; }
        }

        // Static Constructor

        static MainWindow()
        {
            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];

            systemSettings.Add("WebHostURL", "http://localhost:49152", "The web hosting URL for user interface operation. For increased security, only bind to localhost.", false, SettingScope.User);
            systemSettings.Add("WebHostPortRange", "49152-65535", "The port range to use when searching for an available port for the web server.");
            systemSettings.Add("DefaultWebPage", "Index.cshtml", "Determines if cache control is enabled for browser clients.");
            systemSettings.Add("CompanyName", "Grid Protection Alliance", "The name of the company who owns this instance of the openMIC.");
            systemSettings.Add("CompanyAcronym", "GPA", "The acronym representing the company who owns this instance of the openMIC.");
            systemSettings.Add("ProjectName", "EPRI DRIVE Data Loader", "The name of the current project.", false, SettingScope.User);
            systemSettings.Add("DateFormat", "MM/dd/yyyy", "The default date format to use when rendering timestamps.");
            systemSettings.Add("TimeFormat", "HH:mm.ss.fff", "The default time format to use when rendering timestamps.");
            systemSettings.Add("BootstrapTheme", "Content/bootstrap.min.css", "Path to Bootstrap CSS to use for rendering styles.", false, SettingScope.User);
            systemSettings.Add("SubscriptionConnectionString", "server=localhost:6190; interface=0.0.0.0", "Connection string for data subscriptions to Epri Drive server.", false, SettingScope.User);
            systemSettings.Add("DefaultProjectPath", "EpriDrive Projects", "Default path on which to store the user's projects.", false, SettingScope.User);

            Model = new AppModel();
            Model.Global.WebHostURL = systemSettings["WebHostURL"].Value;
            Model.Global.WebHostPortRange = systemSettings["WebHostPortRange"].Value;
            Model.Global.DefaultWebPage = systemSettings["DefaultWebPage"].Value;
            Model.Global.CompanyName = systemSettings["CompanyName"].Value;
            Model.Global.CompanyAcronym = systemSettings["CompanyAcronym"].Value;
            Model.Global.ProjectName = systemSettings["ProjectName"].Value;
            Model.Global.ApplicationName = "Epri Drive Data Modeling Manager";
            Model.Global.ApplicationDescription = "open Extensible Control & Analytics Client";
            Model.Global.ApplicationKeywords = "open source, utility, software, analytics";
            Model.Global.DateFormat = systemSettings["DateFormat"].Value;
            Model.Global.TimeFormat = systemSettings["TimeFormat"].Value;
            Model.Global.DateTimeFormat = $"{Model.Global.DateFormat} {Model.Global.TimeFormat}";
            Model.Global.BootstrapTheme = systemSettings["BootstrapTheme"].Value;
            Model.Global.SubscriptionConnectionString = systemSettings["SubscriptionConnectionString"].Value;
            Model.Global.DefaultProjectPath = FilePath.AddPathSuffix(systemSettings["DefaultProjectPath"].Value);

        }

        #endregion

    }
}
