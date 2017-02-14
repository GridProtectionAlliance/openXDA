using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailGroupMeterGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int MeterGroupID { get; set; }
    }

    public class EmailGroupMeterGroupView: EmailGroupMeterGroup
    {
        [Searchable]
        public string EmailGroup { get; set; }
        [Searchable]
        public string MeterGroup { get; set; }
    }
}