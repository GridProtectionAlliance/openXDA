using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;


namespace openXDA.Model
{
    public class GroupMeterView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string MeterName { get; set; }

        public int MeterID { get; set; }

        public int GroupID { get; set; }

        public string Location { get; set; }

    }
}