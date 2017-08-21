using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace openXDA.Model
{
    public class WorkbenchFilter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid? UserID { get; set; }
        public string TimeRange { get; set; }
        public string Meters { get; set; }
        public string Lines { get; set; }
        public string EventTypes { get; set; }
        public bool IsDefault { get; set; }
    }
}