using System;

namespace EDAfilewatcher
{
    public enum FileAction
    {
        None,
        Notify,
        Copy,
        Move
    }

    public enum FileEventWatchType
    {
        None,
        Created,
        Changed,
        Deleted,
        Renamed
    }

    public class WatchFolder
    // An action is specified for each path and filter to watch.
    {
        public string FolderToWatch;
        public string FullPathToWatch;
        public string FileFilter;
        public Boolean WatchSubfolders;
        public WatchAction FileWatcherAction;
        public Meter MeterInfo;
    

        public WatchFolder()
        {
            FolderToWatch = "";
            FullPathToWatch = "";
            FileFilter = "";
            WatchSubfolders = false;
            FileWatcherAction = new WatchAction();
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
        public FileEventWatchType EventType;
        public FileAction Action;
        public string DestinationPath;

        public WatchAction()
        {
            Enabled = false;
            EventType = FileEventWatchType.None;
            Action = FileAction.Notify;
            DestinationPath = "";
        }

    }
    //-------------------------------------------

    
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

}
