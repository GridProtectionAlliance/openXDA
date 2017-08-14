using GSF.Data.Model;

namespace openXDA.Model
{
    public class PQMarkAggregate
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
	    public int MeterID { get; set;}
	    public int Year { get; set;}
	    public int Month { get; set;}
	    public int ITIC { get; set;}
	    public int SEMI { get; set;}
	    public int SARFI90 { get; set;}
	    public int SARFI70 { get; set;}
	    public int SARFI50 { get; set;}
	    public int SARFI10 { get; set;}
	    public string THDJson { get; set; }
    }
}
