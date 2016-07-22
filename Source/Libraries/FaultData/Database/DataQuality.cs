namespace FaultData.Database.DataQualityTableAdapters
{
    public partial class MeterDataQualitySummaryTableAdapter
    {
        public void Upsert(DataQuality.MeterDataQualitySummaryRow dataRow)
        {
            UpsertQuery(dataRow.MeterID, dataRow.Date, dataRow.ExpectedPoints, dataRow.GoodPoints, dataRow.LatchedPoints, dataRow.UnreasonablePoints, dataRow.NoncongruentPoints, dataRow.DuplicatePoints);
        }
    }

    public partial class ChannelDataQualitySummaryTableAdapter
    {
        public void Upsert(DataQuality.ChannelDataQualitySummaryRow dataRow)
        {
            UpsertQuery(dataRow.ChannelID, dataRow.Date, dataRow.ExpectedPoints, dataRow.GoodPoints, dataRow.LatchedPoints, dataRow.UnreasonablePoints, dataRow.NoncongruentPoints, dataRow.DuplicatePoints);
        }
    }
}
