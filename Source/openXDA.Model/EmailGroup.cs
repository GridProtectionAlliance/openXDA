using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace openXDA.Model
{
    public class EmailGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [Searchable]
        public string Name { get; set; }
    }

    public class EmailGroupMeterGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int MeterGroupID { get; set; }
    }

    public class EmailGroupMeterGroupView : EmailGroupMeterGroup
    {
        [Searchable]
        public string EmailGroup { get; set; }
        [Searchable]
        public string MeterGroup { get; set; }
    }

    public class EmailGroupLineGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int LineGroupID { get; set; }
    }

    public class EmailGroupLineGroupView : EmailGroupLineGroup
    {
        [Searchable]
        public string EmailGroup { get; set; }
        [Searchable]
        public string LineGroup { get; set; }
    }

    public class EmailGroupSecurityGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int SecurityGroupID { get; set; }
    }

    public class EmailGroupType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int EmailTypeID { get; set; }
    }

    public class EmailGroupTypeView : EmailGroupType
    {
        [Searchable]
        public string GroupName { get; set; }
        [Searchable]
        public string TypeName { get; set; }
    }

    public class EmailGroupUserAccount
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public Guid UserAccountID { get; set; }
    }

    public class EmailGroupUserAccountView : EmailGroupUserAccount
    {
        [Searchable]
        public string EmailGroup { get; set; }
        [Searchable]
        public string UserName { get; set; }
    }


}