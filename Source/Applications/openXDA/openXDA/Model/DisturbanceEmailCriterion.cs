using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openXDA.Model
{
    public class DisturbanceEmailCriterion
    {
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int SeverityCode { get; set; }
    }
}