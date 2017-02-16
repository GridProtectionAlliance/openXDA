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
}