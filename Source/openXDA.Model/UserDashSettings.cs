using System;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    [PrimaryLabel("Name")]
    [TableName("UserDashSettings")]
    public class UserDashSettings
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Label("User Account")]
        public Guid UserAccountID { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Value { get; set; }
        public bool Enabled { get; set; }
    }
}