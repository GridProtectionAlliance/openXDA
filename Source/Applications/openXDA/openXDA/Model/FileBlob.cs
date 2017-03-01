using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FileBlob
    {
        [PrimaryKey(true)]
        public int ID { get; set;}
        public byte[] Blob { get; set; }
        public int DataFileID { get; set; }
    }

    [TableName("DataFile")]
    public class DataFile
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int FileGroupID { get; set; }
        [Searchable]
        public string FilePath { get; set; }
        public int FilePathHash { get; set; }
        public int FileSize { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }

    public class FileGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int Error { get; set; }
        public DateTime DataStartTime { get; set; }
        public DateTime DataEndTime { get; set; }
        public DateTime ProcessingStartTime { get; set; }
        public DateTime ProcessingEndTime { get; set; }
    }

    [TableName("DataReader")]
    public class DataReader
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string FilePattern { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public int LoadOrder { get; set; }
    }
}