using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    [UseEscapedName]
    public class User
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}