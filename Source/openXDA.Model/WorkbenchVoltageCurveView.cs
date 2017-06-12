using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    [PrimaryLabel("Name")]
    [TableName("WorkbenchVoltageCurveView")]
    public class WorkbenchVoltageCurveView
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [PrimaryKey(true)]
        public int CurvePointID { get; set; }

        public double PerUnitMagnitude { get; set; }
        public double DurationSeconds { get; set; }
        public int LoadOrder { get; set; }
        public bool Visible { get; set; }
    }
}