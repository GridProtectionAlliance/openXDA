//******************************************************************************************************
//  Filewatcher.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/02/2013 - F. Russell Robertson
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using GSF;
using System.Xml;
using System.Threading;
using System.Reflection;
using GSF.IO;
using GSF.Security.Cryptography;
using Timer = System.Timers.Timer;

namespace XDAFileWatcher
{
    public enum FileAction
    {
        None,
        Notify,
        Copy,
        Move
    }

    public enum FilePatternStatus
    {
        NoneSet,
        StaticSet,
        Complete
    }

    public enum FileEventType
    {
        None,
        Created,
        Changed,
        Deleted,
        Renamed
    }

    public class FileShare
    {
        public string Name;
        public string Domain;
        public string Username;
        public string Password;

        public FileShare()
        {
            Name = "";
            Domain = "";
            Username = "";
            Password = "";
        }
    }

    public class WatchFolder
        // An action is specified for each path and filter to watch.
    {
        public string FolderToWatch;
        public string FullPathToWatch;
        public string FileFilter;
        public Boolean WatchSubfolders;
        public Boolean ProduceResultsFile;
        public WatchAction WatchFolderAction;
        public Meter MeterInfo;

        public WatchFolder()
        {
            FolderToWatch = "";
            FullPathToWatch = "";
            FileFilter = "";
            WatchSubfolders = false;
            ProduceResultsFile = true; //note default is to product .fwr xml files
            WatchFolderAction = new WatchAction();
            MeterInfo = new Meter();
        }
    }

    public class Meter
    {
        public string SN;
        public string AssetID;
        public string Alias;
        public string Vendor;
        public string Model;
        public string Name;
        public string StationID;
        public string StationName;

        public Meter()
        {
            SN = "";
            AssetID = "";
            Vendor = "";
            Model = "";
            Alias = "";
            Name = "";
            StationID = "";
            StationName = "";
        }
    }

    public class WatchAction
        //an action is designated by type,
    {
        public bool Enabled;
        public FileEventType EventType;
        public FileAction Action;
        public FilePatternStatus OutputPatternStatus;
        public DestinationFilePattern[] OutputFolderPattern;
        public DestinationFilePattern[] OutputFilePattern;

        public WatchAction()
        {
            Enabled = false;
            EventType = FileEventType.None;
            OutputPatternStatus = FilePatternStatus.NoneSet;
            Action = FileAction.Notify;
            OutputFolderPattern = new DestinationFilePattern[1];
            OutputFilePattern = new DestinationFilePattern[1];
        }
    }

    public class FileWatcherParms
    {
        public string ProgramName;
        public string LogFolder;

        public FileWatcherParms()
        {
            ProgramName = "";
            LogFolder = "";
        }
    }

    public class DestinationFilePattern
    {
        public bool IsField;
        public string Element;
        public string PathPieceText;

        public DestinationFilePattern()
        {
            IsField = false;
            Element = null;
            PathPieceText = "";
        }
    }

    public class XDAFileWatcher : IDisposable
    {
        private Timer m_watcherMonitor;
        private FileSystemWatcher m_fileSystemWatcher;
        private volatile List<WatchFolder> m_watchFolders;
        //private DestinationFilePattern[] _outputDestinationFileElements;
        private bool m_disposed;

        public bool OutputFilePatternIsSet
        {
            get;
            set;
        }

        //Note if set externally, this is reset by the config file
        public ISet<FileShare> FileShares
        {
            get;
            set;
        }

        public ISet<FileShare> UnauthenticatedFileShares
        {
            get;
            set;
        }

        public string RootPathToWatch
        {
            get;
            set;
        }

        private FileSystemWatcher FileSystemWatcher
        {
            get
            {
                return m_fileSystemWatcher;
            }
            set
            {
                using (FileSystemWatcher fileSystemWatcher = m_fileSystemWatcher)
                {
                    m_fileSystemWatcher = value;
                }
            }
        }

        private char[] _separators = { ',', ';' };

        public XDAFileWatcher()
        {
            m_watcherMonitor = new Timer(1000);
            m_watcherMonitor.AutoReset = false;
            m_watcherMonitor.Elapsed += WatcherMonitorElapsed;

            m_watchFolders = new List<WatchFolder>();

            OutputFilePatternIsSet = false;
        }

        /// <summary>
        /// Releases the unmanaged resources before the <see cref="XDAFileWatcher"/> object is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~XDAFileWatcher()
        {
            Dispose(false);
        }

        public void StartWatching()
        {
            new Thread(CreateAndValidateNewWatcher).Start();
            Log.Instance.Info("StartWatching: EventType Tread Started for root folder = " + RootPathToWatch.QuoteWrap());
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadConfigFile(string configurationFilePath)
        {
            List<WatchFolder> watchFolders;

            if (string.IsNullOrEmpty(configurationFilePath))
            {
                Log.Instance.Error("ReadConfigFile:  Configuration file is missing or empty.");
                return;
            }

            watchFolders = new List<WatchFolder>();

            Log.Instance.Debug("ReadConfigFile: _watcherFolders and _outsideFileElements redimensioned and cleared.");

            if (ReadConfigXml(configurationFilePath, watchFolders))
            {
                Log.Instance.Debug("ReadConfigFile: ReadXML Successful");
                Log.Instance.Info("ReadConfigFile: Count of Folders to EventType = " + watchFolders.Count.ToString());

                int i = 0;
                foreach (WatchFolder f in watchFolders)
                {
                    i++;

                    Log.Instance.Info("watchFolder " + i.ToString() + ": FolderToWatch = " + f.FolderToWatch.QuoteWrap() +
                                      " FileFilter = " + f.FileFilter.QuoteWrap() + " EventType = " + f.WatchFolderAction.EventType.ToString() + " Action = " + f.WatchFolderAction.Action.ToString());

                    f.WatchFolderAction.OutputFolderPattern = CreateStaticElements(f, f.WatchFolderAction.OutputFolderPattern);
                    LogPatternString(f.WatchFolderAction.OutputFolderPattern, true, i);

                    f.WatchFolderAction.OutputFilePattern = CreateStaticElements(f, f.WatchFolderAction.OutputFilePattern);
                    LogPatternString(f.WatchFolderAction.OutputFilePattern, false, i);

                    f.WatchFolderAction.OutputPatternStatus = FilePatternStatus.StaticSet;
                }
            }

            m_watchFolders = watchFolders;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LogPatternString(DestinationFilePattern[] pattern, bool isFolderPathPiece, int folderID)
        {
            StringBuilder sb = new StringBuilder();
            int j = 0;
            foreach (DestinationFilePattern p in pattern)
            {
                j++;
                sb.Append("Element ");
                sb.Append(j.ToString());
                sb.Append(": ");
                if (p.IsField)
                {
                    sb.Append("{" + p.Element + "}");
                }
                else
                {
                    sb.Append(p.PathPieceText.QuoteWrap());
                }
                sb.Append(";  ");
            }

            string message = "File";
            if (isFolderPathPiece) message = "Folder";

            Log.Instance.Info("watchFolder " + folderID.ToString() + ": Destination " + message + " Parms = " + sb.ToString());
        }

        #region [ Private Processing Methods ]

        private void CreateAndValidateNewWatcher()
        {
            if (!Path.GetInvalidPathChars().Any(c => RootPathToWatch.Contains(c)) && Directory.Exists(RootPathToWatch))
                CreateNewWatcher();
            else
                Log.Instance.Error("CreateNewWatcher: CANNOT START FILEWATCHER.  Root path to watch does not exist or is invalid.");

            // Start the timer to monitor the file system watcher
            m_watcherMonitor.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateNewWatcher()
        {
            FileSystemWatcher fileSystemWatcher = null;

            try
            {
                fileSystemWatcher = new FileSystemWatcher();

                fileSystemWatcher.Path = RootPathToWatch;
                fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                    NotifyFilters.DirectoryName;

                //watch all files in the path 
                fileSystemWatcher.Filter = "*.*";

                //watch sub dir as default
                fileSystemWatcher.IncludeSubdirectories = true;

                fileSystemWatcher.Created += OnCreated;
                Log.Instance.Debug("CreateNewWatcher: EventType CREATE Handler Loaded");

                fileSystemWatcher.Deleted += OnDeleted;
                Log.Instance.Debug("CreateNewWatcher: EventType DELETE Handler Loaded");

                fileSystemWatcher.Renamed += OnRenamed;
                Log.Instance.Debug("CreateNewWatcher: EventType RENAME Handler Loaded");

                fileSystemWatcher.EnableRaisingEvents = true;
                Log.Instance.Info("CreateNewWatcher: FileWatcher Serivce Successfully Started  *********  WAITING for FILE CHANGES ******** ");

                FileSystemWatcher = fileSystemWatcher;
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorException("CreateNewWatcher: CANNOT START FILEWATCHER.", ex);

                if ((object)fileSystemWatcher != null)
                    fileSystemWatcher.Dispose();
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Log.Instance.Info("CREATED event returned by FileWatcher for " + e.FullPath.QuoteWrap());
            ProcessEvent(e.FullPath, FileEventType.Created);
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Log.Instance.Info("DELETED event returned by FileWatcher for " + e.FullPath.QuoteWrap());
            ProcessEvent(e.FullPath, FileEventType.Deleted);
        }

        private void OnRenamed(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Log.Instance.Info("RENAMED event returned by FileWatcher for " + e.FullPath.QuoteWrap());
            ProcessEvent(e.FullPath, FileEventType.Renamed);
        }

        private void WatcherMonitorElapsed(object sender, ElapsedEventArgs e)
        {
            // First, check for error conditions that would
            // require recreating the file system watcher
            try
            {
                if ((object)m_fileSystemWatcher != null)
                {
                    if (!Directory.Exists(RootPathToWatch))
                    {
                        Log.Instance.Error("No longer able to find root path. Attempting to reenable the file system watcher...");
                        FileSystemWatcher = null;
                    }
                    else if (!m_fileSystemWatcher.EnableRaisingEvents)
                    {
                        Log.Instance.Error("The file system watcher has stopped unexpectedly. Attempting to reenable the file system watcher...");
                        FileSystemWatcher = null;
                    }
                }
            }
            catch
            {
                Log.Instance.Error("A strange and unexpected error has occurred while monitoring the file the system watcher. Attempting to reenable the file system watcher -- just in case...");
                FileSystemWatcher = null;
            }

            // Attempt to reconnect to file shares that we have yet to authenticate to
            foreach (FileShare share in FileShares.Where(s => (object)s.Password != null))
            {
                if (TryConnectToFileShare(share))
                    share.Password = null;
            }

            // If the file system watcher is null, attempt to recreate it
            if ((object)m_fileSystemWatcher == null)
                CreateNewWatcher();

            if ((object)m_watcherMonitor != null)
                m_watcherMonitor.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessEvent(string sourceFullPath, FileEventType eventType)
        {
            DateTime processStartTime;
            List<WatchFolder> watchFolders;

            Log.Instance.Debug("ProcessEvent: There was a " + eventType.ToString().ToUpper() + " event in " + sourceFullPath);

            if (string.IsNullOrEmpty(sourceFullPath))
                return;

            processStartTime = DateTime.UtcNow;
            watchFolders = m_watchFolders;

            // Full Path may or may not be a folder of interst to watch. (Need to loop through all -- folder may appear more than once)
            foreach (WatchFolder f in watchFolders)
            {
                Log.Instance.Debug("ProcessEvent: Is this event under at the level " + f.FullPathToWatch.QuoteWrap() + " (or below if subfolders=true) ?");

                if (ProcessThisFolder(sourceFullPath, f) && ProcessThisFilename(sourceFullPath, f))
                {
                    Log.Instance.Debug("ProcessEvent: Yes.");
                    Log.Instance.Debug("ProcessEvent: Filename matches filter.  Folder is within scope.");

                    // Does the action match a watch action for this folder?
                    Log.Instance.Debug("ProcessEvent: Is the event type the expected " + f.WatchFolderAction.EventType.ToString().ToUpper() + "?");
                    if (f.WatchFolderAction.EventType == eventType)
                    {
                        Log.Instance.Debug("ProcessEvent: Yes.(Event Type = " + eventType.ToString() + ")");

                        // Determine destination filename
                        string destinationFileName = BuildDestinationName(sourceFullPath, f.WatchFolderAction.OutputFilePattern, false);
                        destinationFileName = string.Concat(destinationFileName, FilePath.GetExtension(sourceFullPath));

                        // Determine destination foldername
                        string destinationFolderName = FilePath.GetAbsolutePath(BuildDestinationName(sourceFullPath, f.WatchFolderAction.OutputFolderPattern, true));

                        // Determine destination sourceFullPath
                        string destinationPathName = FilePath.GetUniqueFilePath(Path.Combine(destinationFolderName, destinationFileName));

                        Log.Instance.Debug("ProcessEvent: Destination FullPath = " + destinationPathName.QuoteWrap());

                        if (string.IsNullOrEmpty(destinationFileName))
                        {
                            Log.Instance.Error("ProcessEvent: Destination filename is missing or empty.  Processing continuting to next watch folder");
                            break;
                        }

                        switch (f.WatchFolderAction.Action)
                        {
                            case FileAction.Copy:
                                Log.Instance.Debug("ProcessEvent: Try COPY File " + FilePath.GetFileName(sourceFullPath).QuoteWrap() + " to " + destinationPathName.QuoteWrap());

                                try
                                {
                                    if (!Directory.Exists(destinationFolderName))
                                        Directory.CreateDirectory(destinationFolderName);

                                    FilePath.WaitForReadLock(sourceFullPath);
                                    File.Copy(sourceFullPath, destinationPathName, true);

                                    TimeSpan duration = processStartTime.Subtract(DateTime.UtcNow).Negate();
                                    Log.Instance.Info("ProcessEvent: File COPY successsful to " + destinationPathName.QuoteWrap() + " Duration = " + duration.ToFormattedString());

                                    if (f.ProduceResultsFile)
                                    {
                                        XmlDocument resultsXml = BuildResultsXml(sourceFullPath, destinationPathName, processStartTime, duration);
                                        Log.Instance.Debug("ProcessEvent: Successfully produced stats XML document.");
                                        writeXml(resultsXml, destinationPathName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Instance.ErrorException("ProcessEvent: Cannot COPY file to " + destinationPathName.QuoteWrap(), ex);
                                }

                                break;

                            case FileAction.Move:
                                Log.Instance.Debug("ProcessEvent: Try MOVE File " + Path.GetFileName(sourceFullPath).QuoteWrap() + " to " + destinationPathName.QuoteWrap());

                                try
                                {
                                    if (!Directory.Exists(destinationFolderName))
                                        Directory.CreateDirectory(destinationFolderName);

                                    FilePath.WaitForReadLock(sourceFullPath);
                                    File.Move(sourceFullPath, destinationPathName);

                                    TimeSpan duration = processStartTime.Subtract(DateTime.UtcNow).Negate();
                                    Log.Instance.Info("ProcessEvent: File MOVE successsful to " + destinationPathName.QuoteWrap() + " Duration = " + duration.ToFormattedString());

                                    if (f.ProduceResultsFile)
                                    {
                                        XmlDocument resultsXml = BuildResultsXml(sourceFullPath, destinationPathName, processStartTime, duration);
                                        Log.Instance.Debug("ProcessEvent: Successfully produced stats XML document.");
                                        writeXml(resultsXml, destinationPathName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Instance.ErrorException("ProcessEvent: Cannot MOVE file to " + destinationPathName.QuoteWrap(), ex);
                                }

                                break;

                            default:
                                Log.Instance.Info("ProcessEvent: Default Action - Logging an event on file: " + sourceFullPath.QuoteWrap());
                                break;
                        }
                    }
                    else
                    {
                        Log.Instance.Debug("ProcessEvent: No.");
                    }
                }
                else
                {
                    Log.Instance.Debug("ProcessEvent: No.");
                }
            }
        }

        private XmlDocument BuildResultsXml(string sourceFullPath, string destinationFullPath, DateTime startTime, TimeSpan duration)
        {
            if (string.IsNullOrEmpty(sourceFullPath)) return null;
            if (string.IsNullOrEmpty((destinationFullPath))) destinationFullPath = "";
            XmlDocument resultsXml = new XmlDocument();
            FileInfo fi = new FileInfo(sourceFullPath);

            //Declaration
            XmlNode xmlDeclaration = resultsXml.CreateNode(XmlNodeType.XmlDeclaration, "1.0", "UTF-8", null);
            resultsXml.AppendChild(xmlDeclaration);

            //Root element
            XmlElement xmlRoot = resultsXml.CreateElement("fileWatcherResults");
            resultsXml.AppendChild(xmlRoot);

            //Children of Root
            XmlElement xmlSource = resultsXml.CreateElement("sourceFullPath");
            xmlSource.InnerText = sourceFullPath;

            XmlAttribute attrSize = resultsXml.CreateAttribute("Size");
            attrSize.Value = fi.Length.ToString();
            xmlSource.Attributes.Append(attrSize);

            XmlAttribute attrCT = resultsXml.CreateAttribute("CreationTime");
            attrCT.Value = fi.CreationTime.ToString();
            xmlSource.Attributes.Append(attrCT);

            XmlAttribute attrWT = resultsXml.CreateAttribute("LastWriteTime");
            attrWT.Value = fi.LastWriteTime.ToString();
            xmlSource.Attributes.Append(attrWT);

            XmlAttribute attrAT = resultsXml.CreateAttribute("LastAccessTime");
            attrAT.Value = fi.LastAccessTime.ToString();
            xmlSource.Attributes.Append(attrAT);

            xmlRoot.AppendChild(xmlSource);

            XmlElement xmlDestination = resultsXml.CreateElement("DestinationFullPath");
            xmlDestination.InnerText = destinationFullPath;
            xmlRoot.AppendChild(xmlDestination);

            XmlElement xmlStats = resultsXml.CreateElement("Stats");
            XmlAttribute attrDuration = resultsXml.CreateAttribute("ProcessingTime");
            attrDuration.Value = duration.ToString();
            xmlStats.Attributes.Append(attrDuration);

            XmlAttribute attrStart = resultsXml.CreateAttribute("StartTime");
            attrStart.Value = startTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            xmlStats.Attributes.Append(attrStart);


            xmlRoot.AppendChild(xmlStats);

            return resultsXml;
        }

        private void writeXml(XmlDocument xmlDoc, string destinationFullPath)
        {
            if (xmlDoc == null || string.IsNullOrEmpty(destinationFullPath))
            {
                Log.Instance.Warn("ProcessEvent: Null or empty stats document.");
                return;
            }

            destinationFullPath += ".fwr";

            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(destinationFullPath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    xmlDoc.Save(writer);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorException("ProcessEvent: Could not save stats xml file: " + destinationFullPath.QuoteWrap(), ex);
            }

            Log.Instance.Info("ProcessEvent: XmlFile RESULTS (.fwr) creation successful: " + destinationFullPath.QuoteWrap());

        }

        private string BuildDestinationName(string sourceFullPath, DestinationFilePattern[] patternSet, bool isFolderPathPiece)
        {
            StringBuilder outString = new StringBuilder();

            string[] folders = FilePath.DropPathRoot(FilePath.GetAbsolutePath(sourceFullPath))
                .Split(Path.DirectorySeparatorChar)
                .Where(f => !string.IsNullOrEmpty(f))
                .ToArray();

            int level;

            foreach (DestinationFilePattern p in patternSet)
            {
                if (p.IsField)
                {
                    switch (p.Element)
                    {
                        case "FOLDER":
                            outString.Append(folders[folders.Length - 1]);
                            break;

                        case "FILEDATETIME":
                            outString.Append(Directory.GetCreationTime(sourceFullPath).ToShortDateString().Replace('/', '-').RemoveCharacters(c => c == ':'));
                            outString.Append(" ");
                            outString.Append(Directory.GetCreationTime(sourceFullPath).ToShortTimeString().RemoveCharacters(c => c == ':'));
                            break;

                        case "FILEDATE":
                            outString.Append(Directory.GetCreationTime(sourceFullPath).ToShortDateString().Replace('/', '-'));
                            break;

                        case "FILETIME":
                            outString.Append(Directory.GetCreationTime(sourceFullPath).ToShortTimeString().RemoveCharacters(c => c == ':'));
                            break;

                        case "CURRENTDATETIME":
                            outString.Append(DateTime.UtcNow.ToShortDateString().Replace('/', '-').RemoveCharacters(c => c == ':'));
                            outString.Append(" ");
                            outString.Append(DateTime.UtcNow.ToShortTimeString().RemoveCharacters(c => c == ':'));
                            break;

                        case "CURRENTDATE":
                            outString.Append(DateTime.UtcNow.ToShortDateString().Replace('/', '-'));
                            break;

                        case "CURRENTTIME":
                            outString.Append(DateTime.UtcNow.ToShortTimeString().RemoveCharacters(c => c == ':'));
                            break;

                        case "EXISTINGFILENAME":
                            outString.Append(FilePath.GetFileNameWithoutExtension(sourceFullPath));
                            break;

                        default:
                            if (p.Element.StartsWith("FOLDER") && int.TryParse(p.Element.Substring("FOLDER".Length).Trim(), out level))
                                outString.Append(folders[folders.Length - (level + 1)]);

                            break;
                    }
                }
                else if (isFolderPathPiece)
                {
                    string pathPieceText = p.PathPieceText.Trim();

                    if (pathPieceText != "\\" && pathPieceText.StartsWith("\\") || pathPieceText.EndsWith("\\"))
                        outString = new StringBuilder(Path.Combine(outString.ToString(), pathPieceText));
                    else
                        outString.Append(pathPieceText);
                }
                else
                {
                    outString.Append(p.PathPieceText.Trim());
                }
            }

            return outString.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filespec"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool ProcessThisFilename(string filespec, WatchFolder f)
        {
            string filename = FilePath.GetFileName(filespec);
            string[] patterns;
            string regex;

            if (string.IsNullOrEmpty(filename))
                return false;

            patterns = f.FileFilter.Split(_separators);

            foreach (string pattern in patterns)
            {
                regex = Regex.Escape(pattern.Trim()).Replace("\\*", ".*").Replace("\\?", ".");

                if (Regex.IsMatch(filename, regex, RegexOptions.IgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns TRUE if actions in this folder should be processed
        /// </summary>
        /// <param name="fileSpec">The full file path of this folder</param>
        /// <param name="f">The WatchFolder parms for his candidate action</param>
        /// <returns></returns>
        private bool ProcessThisFolder(string fileSpec, WatchFolder f)
        {
            string folderSpec = Path.GetFullPath(Path.GetDirectoryName(fileSpec).ToNonNullString()).TrimEnd('\\');
            string pathToWatch = Path.GetFullPath(f.FullPathToWatch).TrimEnd('\\');

            if (string.IsNullOrEmpty(folderSpec))
                return false;

            // If watching sub folders, we trim folderSpec down to the same
            // number of characters as the path to watch before we compare
            if (f.WatchSubfolders && (folderSpec.Length > pathToWatch.Length))
                folderSpec = folderSpec.Substring(0, pathToWatch.Length + 1).TrimEnd('\\');

            return (string.Compare(folderSpec, pathToWatch, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        #endregion

        #region [ Private Configuration Methods ]

        /// <summary>
        /// Reads the XML file as an "XMLDocument"and calls "ExtractParms" to load them 
        /// </summary>
        private bool ReadConfigXml(string configurationFileFullPath, List<WatchFolder> watchFolders)
        {
            string xmlfilestring = "";
            if (string.IsNullOrEmpty(configurationFileFullPath))
            {
                Log.Instance.Error("ReadConfigXml: No file name provided for EDAFileWatcher Configuration File.");
                return false;
            }

            if (File.Exists(configurationFileFullPath))
            {
                try
                {
                    xmlfilestring = File.ReadAllText(configurationFileFullPath);
                }
                catch (Exception ex)
                {
                    Log.Instance.ErrorException("ReadXML: Cannot read configuration file.", ex);
                    return false;
                }

                if (string.IsNullOrEmpty(xmlfilestring))
                {
                    Log.Instance.Warn("ReadXML: XML configuration file " + configurationFileFullPath + " is empty.");
                    return false;
                }
                else
                {
                    Log.Instance.Info("ReadXML: Successfully read text from configuration XML " + configurationFileFullPath);
                }
            }
            else
            {
                Log.Instance.Warn("ReadXML: XML configuration file " + configurationFileFullPath + " does not exist.");
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlfilestring);
                Log.Instance.Info("ReadXML: Successfully parsed configuation XML.");
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorException("ReadXML: Cannot parse XML configuraiton file.", ex);
                return false;
                //throw ex;
            }

            if (ExtractParms(xmlDoc, watchFolders))
            {
                Log.Instance.Info("ReadXML: Successfully set configuration parameters.");
                return true;
            }
            return false;
        }


        /// <summary>
        /// Loads all the configuration information for the XML file into public fields
        /// </summary>
        private bool ExtractParms(XmlDocument configXml, List<WatchFolder> watchFolders)
        {

            try
            {
                XmlNodeList rootParms = configXml.SelectNodes("EDAfilewatcher");
                WatchFolder watchFolder;

                FileShares = new HashSet<FileShare>();
                UnauthenticatedFileShares = new HashSet<FileShare>();

                if (rootParms == null)
                {
                    Log.Instance.Warn("ExtractParms: No Parms found to process in XML file.");
                    return false;
                }


                foreach (XmlNode xmlNode in rootParms)
                {
                    foreach (XmlNode childNode in xmlNode)
                    {
                        if (childNode.Name.ToUpper() != "FILESHARE")
                            continue;

                        FileShare share = new FileShare();

                        foreach (XmlNode fileShareNode in childNode)
                        {
                            switch (fileShareNode.Name.ToUpper())
                            {
                                case "NAME":
                                    share.Name = fileShareNode.InnerText;
                                    break;

                                case "DOMAIN":
                                    share.Domain = fileShareNode.InnerText;
                                    break;

                                case "USERNAME":
                                    string[] splitUsername = fileShareNode.InnerText.Split('\\');

                                    if (splitUsername.Length > 1)
                                    {
                                        share.Domain = splitUsername[0];
                                        share.Username = splitUsername[1];
                                    }
                                    else
                                    {
                                        share.Username = splitUsername[0];
                                    }

                                    break;

                                case "PASSWORD":
                                    bool encrypted = fileShareNode.Attributes != null && fileShareNode.Attributes.Cast<XmlAttribute>().Any(attr => attr.Name.ToUpper() == "ENCRYPTED" && attr.Value.ParseBoolean());

                                    if (encrypted)
                                        share.Password = fileShareNode.InnerText.Decrypt("0679d9ae-aca5-4702-a3f5-604415096987", CipherStrength.Aes256);
                                    else
                                        share.Password = fileShareNode.InnerText;

                                    break;
                            }
                        }

                        if (TryConnectToFileShare(share))
                            share.Password = null;

                        FileShares.Add(share);
                    }

                    foreach (XmlNode childNode in xmlNode)
                    {
                        switch (childNode.Name.ToUpper())
                        {
                            case "SOURCEROOTPATH":
                                if (!string.IsNullOrEmpty(childNode.InnerText))
                                {
                                    RootPathToWatch = FilePath.GetAbsolutePath(childNode.InnerText.EnsureEnd("\\"));

                                    if (Directory.Exists(RootPathToWatch))
                                    {
                                        Log.Instance.Info("ExtractParms:  Root watch folder " + RootPathToWatch.QuoteWrap() + " is valid and exists.");
                                    }
                                    else
                                    {
                                        if (Path.GetInvalidPathChars().Any(c => RootPathToWatch.Contains(c)))
                                            Log.Instance.Error("ExtractParms:  Root watch folder " + RootPathToWatch.QuoteWrap() + " is not a valid folder name.  Many errors to follow.");
                                        else
                                            Log.Instance.Error("ExtractParms:  Root watch folder " + RootPathToWatch.QuoteWrap() + " does not exist or is not accessable.");
                                    }
                                }
                                else
                                {
                                    RootPathToWatch = "";
                                    Log.Instance.Error("ExtractParms:  Root watch folder is empty.  Many errors to follow.");
                                }
                                break;

                                //case "OUTPUTFILENAMEPATTERN":
                                //    if (!string.IsNullOrEmpty(childNode.InnerText))
                                //    {
                                //        Log.Instance.Debug("ExtractParms: Format Pattern to Parse = " + childNode.InnerText.QuoteWrap());
                                //        OutputFilePatternIsSet = CreateOutputPattern(childNode.InnerText);
                                //    }
                                //    else
                                //    {
                                //        OutputFilePatternIsSet = false;
                                //        Log.Instance.Debug("ExtractParms: No Format Pattern to Parse.");
                                //    }
                                //    if (OutputFilePatternIsSet)
                                //    {
                                //        Log.Instance.Info("ExtractParms:  Output File Pattern = " +
                                //                          childNode.InnerText.QuoteWrap() + " " +
                                //                          _outputDestinationFileElements.Length.ToString() + " elements found.");

                                //    }
                                //    else
                                //    {
                                //        Log.Instance.Warn("ExtractParms:  No or invalid file inPatternString for creating of output files.  Pattern set to default.");
                                //        OutputFilePatternIsSet = CreateOutputPattern("{meterAssetID}_{filename}");
                                //        //if (!OutputFilePatternIsSet)
                                //        //    Log.Instance.Error("ExtractParms: Did not sucessfully parse defalut file inPatternString");

                                //    }
                                //    break;

                                //case "LOGFILEFOLDER":
                                //    if (!string.IsNullOrEmpty(childNode.InnerText))
                                //    {
                                //        _logFolder = childNode.InnerText;

                                //    }
                                //    else
                                //    {
                                //        _logFolder = "ApplicationPath";
                                //    }
                                //   Log.Instance.Info("ExtractParms:  Log file foldername = " + RootPathToWatch);
                                //    break;
                                //case "LOGDETAIL":
                                //    if (!string.IsNullOrEmpty(childNode.InnerText) &&
                                //        childNode.InnerText.ToUpper().Trim() == "VERBOSE")
                                //    {
                                //        _logDetail = LogDetail.Verbose;
                                //    }
                                //    else if (!string.IsNullOrEmpty(childNode.InnerText) &&
                                //             childNode.InnerText.ToUpper().Trim() == "NONE")
                                //    {
                                //        _logDetail = LogDetail.None;
                                //    }
                                //    else if (!string.IsNullOrEmpty(childNode.InnerText) &&
                                //            childNode.InnerText.ToUpper().Trim() == "NORMAL")
                                //    {
                                //        _logDetail = LogDetail.Normal;
                                //    }
                                //    Log.Instance.Info("ExtractParms:  Log Detail = " + _logDetail.ToString());
                                //    break;
                            default:
                                break;
                        }
                    }
                }

                XmlNodeList actionSet = configXml.SelectNodes("EDAfilewatcher/watchActions");
                if (actionSet == null)
                {
                    Log.Instance.Warn("ExtractParms: No watchActions found to process in XML file.");
                    return false;
                }
                foreach (XmlNode action in actionSet)
                {

                    foreach (XmlNode folder in action.ChildNodes)
                    {

                        if (folder.Name.ToUpper() == "FOLDER" && folder.Attributes != null && folder.Attributes.Count >= 1)
                            //each folder tag must have at least one attribute, the "name" of the folder.
                        {
                            Log.Instance.Debug("ExtractParms:  Proessing XML for folder = " + folder.Attributes[0].Value.Trim());

                            string filter = "";
                            string folderToProcess = "";
                            bool subfolders = false;
                            bool createFwrXml = true; //default is true.
                            Meter nodeMeter = new Meter();

                            foreach (XmlAttribute a in folder.Attributes)
                            {
                                switch (a.Name.ToUpper())
                                {
                                    case "NAME":
                                        folderToProcess = a.Value;
                                        break;
                                }
                            }

                            //XML can be in any order, need the static filter and subfolders info first (neither is necessary)
                            foreach (XmlNode xmlNode in folder.ChildNodes)
                            {
                                Log.Instance.Debug("ExtractParms:  Proessing Node " + xmlNode.Name.Trim() + "   InnerText = " + xmlNode.InnerText.Trim().QuoteWrap());
                                switch (xmlNode.Name.ToUpper())
                                {
                                    case "FILEFILTER":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            filter = xmlNode.InnerText.Trim();
                                        }
                                        else
                                        {
                                            filter = "";
                                        }
                                        break;
                                    case "INCLUDESUBFOLDERS":
                                        if (xmlNode.InnerText.ParseBoolean())
                                        {
                                            subfolders = true;
                                        }
                                        else
                                        {
                                            subfolders = false;
                                        }
                                        break;
                                        break;
                                    case "PRODUCERESULTSXML":
                                        if (xmlNode.InnerText.ParseBoolean())
                                        {
                                            createFwrXml = true;
                                        }
                                        else
                                        {
                                            createFwrXml = false;
                                        }
                                        break;
                                    case "METERNAME":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.Name = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERSN":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.SN = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERALIAS":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.Alias = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERASSETID":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.AssetID = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERVENDOR":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.Vendor = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERMODEL":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.Model = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERSUBSTATIONID":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.StationID = xmlNode.InnerText.Trim();
                                        }
                                        break;
                                    case "METERSUBSTATIONNAME":
                                        if (!string.IsNullOrEmpty(xmlNode.InnerText))
                                        {
                                            nodeMeter.StationName = xmlNode.InnerText.Trim();
                                        }
                                        break;

                                }
                            }

                            //now process each action
                            foreach (XmlNode actionParm in folder.ChildNodes)
                            {
                                switch (actionParm.Name.ToUpper())
                                {
                                    case "WATCHCREATED":
                                    case "WATCHRENAMED":
                                    case "WATCHDELETED":
                                    case "WATCHCHANGED":
                                        watchFolder = new WatchFolder();

                                        foreach (XmlAttribute a in actionParm.Attributes)
                                        {
                                            switch (a.Name.ToUpper())
                                            {
                                                case "ACTION":
                                                    if (a.Value.ToUpper() == "NOTIFY")
                                                        watchFolder.WatchFolderAction.Action = FileAction.Notify;
                                                    else if (a.Value.ToUpper() == "COPY")
                                                        watchFolder.WatchFolderAction.Action = FileAction.Copy;
                                                    else if (a.Value.ToUpper() == "MOVE")
                                                        watchFolder.WatchFolderAction.Action = FileAction.Move;
                                                    else
                                                        watchFolder.WatchFolderAction.Action = FileAction.None;
                                                    Log.Instance.Debug("ExtractParms: Action Parsed " + a.Name + " = " + a.Value);
                                                    break;
                                                case "DESTINATIONFOLDER":
                                                    watchFolder.WatchFolderAction.OutputFolderPattern = CreateOutputPattern(a.Value);
                                                    if (watchFolder.WatchFolderAction.OutputFolderPattern == null)
                                                    {
                                                        Log.Instance.Warn("ExtractParms: Destination Folder Not Parsed Successfully for " + a.Name + " = " + a.Value.QuoteWrap() + "for "
                                                                          + actionParm.Name);
                                                    }
                                                    else
                                                    {
                                                        Log.Instance.Debug("ExtractParms: Destination Folder Parsed " + a.Name + " = " + a.Value.QuoteWrap());
                                                    }

                                                    break;
                                                case "DESTINATIONFILENAME":
                                                    watchFolder.WatchFolderAction.OutputFilePattern = CreateOutputPattern(a.Value);
                                                    if (watchFolder.WatchFolderAction.OutputFilePattern == null)
                                                    {
                                                        Log.Instance.Warn("ExtractParms: Destination Filename Not Parsed Successfully for " + a.Name + " = " + a.Value.QuoteWrap() + "for "
                                                                          + actionParm.Name);
                                                    }
                                                    else
                                                    {
                                                        Log.Instance.Debug("ExtractParms: Destination FileName Parsed " + a.Name + " = " + a.Value.QuoteWrap());
                                                    }
                                                    break;
                                            }
                                        }

                                        watchFolder.WatchFolderAction.Enabled = actionParm.InnerText.ParseBoolean();
                                        watchFolder.WatchSubfolders = subfolders;
                                        watchFolder.ProduceResultsFile = createFwrXml;

                                        watchFolder.FileFilter = filter;
                                        watchFolder.MeterInfo = nodeMeter;

                                        watchFolder.FolderToWatch = folderToProcess;

                                        if (folderToProcess.ToUpper() == "SOURCEROOT")
                                            watchFolder.FullPathToWatch = RootPathToWatch.EnsureEnd("\\");
                                        else
                                            watchFolder.FullPathToWatch = Path.Combine(RootPathToWatch, folderToProcess).EnsureEnd("\\");

                                        if (Path.GetInvalidPathChars().Any(c => watchFolder.FullPathToWatch.Contains(c)))
                                        {
                                            watchFolder.WatchFolderAction.Enabled = false;
                                            Log.Instance.Warn("ExtractParms: Folder specified to watch " + folderToProcess.QuoteWrap() + " is not valid path name.  Folder action skipped.");
                                        }

                                            if (actionParm.Name.ToUpper().IndexOf("CREATED") > 0)
                                            {
                                                watchFolder.WatchFolderAction.EventType =
                                                    FileEventType.Created;
                                            }
                                            else if (actionParm.Name.ToUpper().IndexOf("CHANGED") > 0)
                                            {
                                                watchFolder.WatchFolderAction.EventType =
                                                    FileEventType.Changed;
                                            }
                                            else if (actionParm.Name.ToUpper().IndexOf("DELETED") > 0)
                                            {
                                                watchFolder.WatchFolderAction.EventType =
                                                    FileEventType.Deleted;
                                            }
                                            else if (actionParm.Name.ToUpper().IndexOf("RENAMED") > 0)
                                            {
                                                watchFolder.WatchFolderAction.EventType =
                                                    FileEventType.Renamed;
                                            }

                                        if (actionParm.Attributes != null && actionParm.Attributes.Count >= 1)
                                        {
                                            foreach (XmlAttribute attrib in actionParm.Attributes)
                                                switch (attrib.Name.ToUpper())
                                                {
                                                    case "ACTION":
                                                        if (attrib.Value.ToUpper().IndexOf("NOTIFY") > -1)
                                                            watchFolder.WatchFolderAction.Action = FileAction.Notify;
                                                        else if (attrib.Value.ToUpper().IndexOf("MOVE") > -1)
                                                            watchFolder.WatchFolderAction.Action = FileAction.Move;
                                                        else if (attrib.Value.ToUpper().IndexOf("COPY") > -1)
                                                            watchFolder.WatchFolderAction.Action = FileAction.Copy;
                                                        else
                                                            watchFolder.WatchFolderAction.Enabled = false;

                                                        break;

                                                    case "DESTINATTION":
                                                        // ??  Investigate  Why is this here?
                                                        watchFolder.WatchFolderAction.OutputFolderPattern = CreateOutputPattern(attrib.Value);
                                                        break;
                                                }
                                        }
                                        else
                                        {
                                            watchFolder.WatchFolderAction.Enabled = false;
                                        }

                                        watchFolders.Add(watchFolder);

                                        break;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorException("ExtractParms: Cannot parse watchActions in XML configuration file. ", ex);
                return false;
            }
        }

        /// <summary>
        /// For parsing destination string within "WatchCreated" tag
        /// </summary>
        private string GetNextElement(ref string inString, ref bool isField)
        {
            int location1;
            int location2;
            string payload;

            if (string.IsNullOrEmpty(inString))
            {
                inString = "";
                return "";
            }

            inString = inString.Trim();
            location1 = inString.IndexOf('{');
            isField = false;

            if (location1 >= 0)
            {
                if (location1 == 0)
                {
                    // Open bracket is at beginning of inString, try to parse field
                    location2 = inString.IndexOf('}', ++location1);
                    payload = (location2 >= 0) ? inString.Substring(location1, location2 - location1).Trim() : null;

                    if ((object)payload == null)
                    {
                        // No matching close token, payload is the rest of the inString
                        payload = inString.Substring(location1).Trim();
                        inString = string.Empty;
                    }
                    else if (payload.Length == 0)
                    {
                        // Empty field, adjust instring
                        inString = inString.Substring(location2 + 1).Trim();
                    }
                    else
                    {
                        // Valid field to process
                        inString = inString.Substring(location2 + 1).Trim();
                        isField = true;
                    }
                }
                else
                {
                    // Location1 > 0, there is a non-field item to be handled
                    payload = inString.Substring(0, location1).Trim();
                    inString = inString.Substring(location1).Trim();
                }
            }
            else
            {
                // There are no fields remaining
                payload = inString.Trim();
                inString = string.Empty;
            }

            return payload;
        }

        /// <summary>
        /// Parses the output file inPatternString from the destination parameter in each action tag
        /// </summary>
        private DestinationFilePattern[] CreateOutputPattern(string inPatternString)
        {
            List<DestinationFilePattern> outputPatterns;
            DestinationFilePattern outputPattern;

            string element;
            bool isField;

            if (string.IsNullOrEmpty(inPatternString))
                return null;

            outputPatterns = new List<DestinationFilePattern>();
            isField = false;

            element = GetNextElement(ref inPatternString, ref isField);

            while (inPatternString.Length > 0 || element.Length > 0)
            {
                if (isField)
                {
                    // Case insensitive
                    element = element.ToUpper();
                }
                else
                {
                    // Can't let element form improper filename
                    element = element.RemoveCharacters(c => Path.GetInvalidPathChars().Contains(c));
                }

                if (element.Length > 0)
                {
                    outputPattern = new DestinationFilePattern();
                    outputPattern.Element = element;
                    outputPattern.IsField = isField;
                    Log.Instance.Debug("CreateOutputFileNamePattern: Index = " + outputPatterns.Count + " IsField = " + isField.ToString() + " Element = " + element.QuoteWrap());
                    outputPatterns.Add(outputPattern);
                }

                element = GetNextElement(ref inPatternString, ref isField);
            }

            return (outputPatterns.Count > 0) ? outputPatterns.ToArray() : null;

        }

        /// <summary>
        /// 
        /// </summary>
        private DestinationFilePattern[] CreateStaticElements(WatchFolder wf, DestinationFilePattern[] pattern)
        {
            if (wf == null) return null;

            // There is only one WatchAction per WatchFolder

            Log.Instance.Debug("CreateStaticFileNameElements: Processing " + wf.FolderToWatch.QuoteWrap() + " destination folder pattern.");

            foreach (DestinationFilePattern fp in pattern)
            {
                if (fp.IsField)
                {
                    switch (fp.Element)
                    {
                        case "METERNAME":
                            if (string.IsNullOrEmpty(wf.MeterInfo.Name))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.Name;
                            fp.IsField = false;
                            break;
                        case "METERSN":
                            if (string.IsNullOrEmpty(wf.MeterInfo.SN))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.SN;
                            fp.IsField = false;
                            break;
                        case "METERASSETID":
                            if (string.IsNullOrEmpty(wf.MeterInfo.AssetID))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.AssetID;
                            fp.IsField = false;
                            break;
                        case "METERALIAS":
                            if (string.IsNullOrEmpty(wf.MeterInfo.Alias))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.Alias;
                            fp.IsField = false;
                            break;
                        case "METERSUBSTATATIONNAME":
                            if (string.IsNullOrEmpty(wf.MeterInfo.StationName))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.StationName;
                            fp.IsField = false;
                            break;
                        case "METERVENDOR":
                            if (string.IsNullOrEmpty(wf.MeterInfo.Vendor))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.Vendor;
                            fp.IsField = false;
                            break;
                        case "METERSUBSTATIONID":
                            if (string.IsNullOrEmpty(wf.MeterInfo.StationID))
                                fp.PathPieceText = "";
                            else
                                fp.PathPieceText = wf.MeterInfo.StationID;
                            fp.IsField = false;
                            break;
                        case "APPPATH":
                            fp.PathPieceText = Path.GetDirectoryName(Assembly.GetAssembly(typeof(XDAFileWatcher)).CodeBase);
                            fp.IsField = false;
                            break;
                    }
                }
                else
                {
                    // Not a field
                    fp.PathPieceText = fp.Element;
                }
            }
            return pattern;
        }

        private bool TryConnectToFileShare(FileShare share)
        {
            try
            {
                if (!Directory.Exists(share.Name))
                {
                    FilePath.ConnectToNetworkShare(share.Name, share.Username, share.Password, share.Domain);
                    Log.Instance.Debug("Successfully connected to file share " + share.Name.QuoteWrap() + ".");
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error connecting to file share " + share.Name.QuoteWrap() + ":" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Releases all the resources used by the <see cref="XDAFileWatcher"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="XDAFileWatcher"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    Log.Instance.Info("***** FileWatcher Terminated *****");
                    Log.FlushLog();

                    if (disposing)
                    {
                        if ((object)m_watcherMonitor != null)
                        {
                            m_watcherMonitor.Dispose();
                            m_watcherMonitor = null;
                        }

                        if ((object)m_fileSystemWatcher != null)
                        {
                            m_fileSystemWatcher.Dispose();
                            m_fileSystemWatcher = null;
                        }
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }


        #endregion
    }
}
