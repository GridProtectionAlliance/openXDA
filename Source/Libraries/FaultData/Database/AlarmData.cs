namespace FaultData.Database.AlarmDataTableAdapters
{
    public partial class ChannelAlarmSummaryTableAdapter
    {
        public void Upsert(AlarmData.ChannelAlarmSummaryRow dataRow)
        {
            UpsertQuery(dataRow.ChannelID, dataRow.AlarmTypeID, dataRow.Date, dataRow.AlarmPoints);
        }
    }

    public partial class MeterAlarmSummaryTableAdapter
    {
        public void Upsert(AlarmData.MeterAlarmSummaryRow dataRow)
        {
            UpsertQuery(dataRow.MeterID, dataRow.AlarmTypeID, dataRow.Date, dataRow.AlarmPoints);
        }
    }
}
