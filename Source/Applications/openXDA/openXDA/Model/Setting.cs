using GSF.Data.Model;

namespace openXDA.Model
{
    public class Setting
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
         
    }
}