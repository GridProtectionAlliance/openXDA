using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    [UseEscapedName]
    public class Group
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        public bool Active { get; set; }
    }
}
