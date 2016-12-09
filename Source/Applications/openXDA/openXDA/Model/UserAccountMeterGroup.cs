using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class UserAccountMeterGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public Guid UserAccountID { get; set; }
        public int MeterGroupID { get; set; }
    }

    [PrimaryLabel("Username")]
    public class UserAccountMeterGroupView 
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public Guid UserAccountID { get; set; }
        public int MeterGroupID { get; set; }
        public string Username { get; set; }
        public string GroupName { get; set; }
    }
}