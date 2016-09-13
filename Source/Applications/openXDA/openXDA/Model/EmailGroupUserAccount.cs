using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailGroupUserAccount
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public Guid UserAccountID { get; set; }
    }
}