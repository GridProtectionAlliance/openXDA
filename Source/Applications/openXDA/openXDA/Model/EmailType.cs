using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openXDA.Model
{
    public class EmailType
    {
        public int ID { get; set; }
        public int EmailCategoryID { get; set; }
        public int XSLTemplateID { get; set; }
    }
}