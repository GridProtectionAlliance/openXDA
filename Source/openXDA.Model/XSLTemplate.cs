using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace openXDA.Model
{
    public class XSLTemplate
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [Searchable]
        public string Name { get; set; }

        [Required]
        [Searchable]
        public string Template { get; set; }
    }
}