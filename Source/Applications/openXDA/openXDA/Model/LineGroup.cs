using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{

    public class LineGroup
    {
        [Searchable]
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Searchable]
        [StringLength(100)]
        public string Name { get; set; }

    }
}
