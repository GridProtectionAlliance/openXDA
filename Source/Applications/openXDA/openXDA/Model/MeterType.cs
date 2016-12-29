using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    [PrimaryLabel("Name")]
    [TableName("MeterType")]
    public class MeterType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}