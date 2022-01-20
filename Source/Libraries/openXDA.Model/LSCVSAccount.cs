using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCenter.Model
{
    public class LSCVSAccount
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string AccountID { get; set; }

        public int CustomerID { get; set; }
    }
}
