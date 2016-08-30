using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    [Table("UserGroupView")]
    public class UserGroupView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public string GroupName { get; set; }
    }
}