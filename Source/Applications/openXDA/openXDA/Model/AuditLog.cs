using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class AuditLog
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string TableName { get; set; }

        [StringLength(200)]
        [Searchable]
        public string PrimaryKeyColumn { get; set; }

        [Searchable]
        public string PrimaryKeyValue { get; set; }

        [StringLength(200)]
        [Searchable]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public bool Deleted { get; set; }

        [StringLength(200)]
        [Searchable]
        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

    }
    public class AuditLogView: AuditLog { }
}