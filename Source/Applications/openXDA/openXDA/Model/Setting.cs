using GSF.Data.Model;

namespace openXDA.Model
{
    public class Setting
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Value { get; set; }
        [Searchable]
        public string DefaultValue { get; set; }
         
    }
}