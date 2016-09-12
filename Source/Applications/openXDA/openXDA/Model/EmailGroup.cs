using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}