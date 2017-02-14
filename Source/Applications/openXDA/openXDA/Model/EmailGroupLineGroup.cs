using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailGroupLineGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int LineGroupID { get; set; }
    }

    public class EmailGroupLineGroupView : EmailGroupLineGroup
    {
        [Searchable]
        public string EmailGroup { get; set; }
        [Searchable]
        public string LineGroup { get; set; }
    }

}