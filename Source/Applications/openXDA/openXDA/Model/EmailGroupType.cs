using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailGroupType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int EmailTypeID { get; set; }
    }
    public class EmailGroupTypeView : EmailGroupType
    {
        [Searchable]
        public string GroupName { get; set; }
        [Searchable]
        public string TypeName { get; set; }
    }
}