namespace FaultData.Database
{
    public partial class MeterData
    {
    }
}

namespace FaultData.Database.MeterDataTableAdapters
{
    public partial class DailyTrendingSummaryTableAdapter
    {
        public void Upsert(MeterData.DailyTrendingSummaryRow row)
        {
            Upsert(row.ChannelID, row.Date, row.Minimum, row.Maximum, row.Average, row.ValidCount, row.InvalidCount);
        }
    }
}
