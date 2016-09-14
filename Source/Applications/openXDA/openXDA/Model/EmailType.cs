using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EmailType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailCategoryID { get; set; }
        public int XSLTemplateID { get; set; }
    }
}