using System;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace openXDA.Model
{
    [PrimaryLabel("Name")]
    [TableName("UserDashSettings")]
    public class UserDashSettings
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [Label("User Account")]
        public Guid UserAccountID { get; set; }

        [Required]
        [Searchable]
        public string Name { get; set; }

        [Required]
        [Searchable]
        public string Value { get; set; }
        public bool Enabled { get; set; }
    }
}