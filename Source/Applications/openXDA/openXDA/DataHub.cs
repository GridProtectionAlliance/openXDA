using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GSF.Data.Model;
using GSF.Web.Model;
using GSF.Web.Security;
using Microsoft.AspNet.SignalR;
using openXDA.Model;

namespace openXDA
{
    public class DataHub: Hub, IRecordOperationsHub
    {
        #region [ Members ]

        // Fields
        private readonly DataContext m_dataContext;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public DataHub()
        {
            m_dataContext = new DataContext();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets <see cref="IRecordOperationsHub.RecordOperationsCache"/> for SignalR hub.
        /// </summary>
        public RecordOperationsCache RecordOperationsCache => s_recordOperationsCache;

        // Gets reference to MiPlan context, creating it if needed


        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DataHub"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                        m_dataContext?.Dispose();
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                    base.Dispose(disposing);    // Call base class Dispose().
                }
            }
        }

        public override Task OnConnected()
        {
            // Store the current connection ID for this thread
            s_connectionID.Value = Context.ConnectionId;
            s_connectCount++;

            //MvcApplication.LogStatusMessage($"DataHub connect by {Context.User?.Identity?.Name ?? "Undefined User"} [{Context.ConnectionId}] - count = {s_connectCount}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                s_connectCount--;
                //MvcApplication.LogStatusMessage($"DataHub disconnect by {Context.User?.Identity?.Name ?? "Undefined User"} [{Context.ConnectionId}] - count = {s_connectCount}");
            }

            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #region [ Static ]

        // Static Properties

        /// <summary>
        /// Gets the hub connection ID for the current thread.
        /// </summary>
        public static string CurrentConnectionID => s_connectionID.Value;

        // Static Fields
        private static volatile int s_connectCount;
        private static readonly ThreadLocal<string> s_connectionID = new ThreadLocal<string>();
        private static readonly RecordOperationsCache s_recordOperationsCache;

        /// <summary>
        /// Gets statically cached instance of <see cref="RecordOperationsCache"/> for <see cref="DataHub"/> instances.
        /// </summary>
        /// <returns>Statically cached instance of <see cref="RecordOperationsCache"/> for <see cref="DataHub"/> instances.</returns>
        public static RecordOperationsCache GetRecordOperationsCache() => s_recordOperationsCache;


        // Static Constructor
        static DataHub()
        {
            // Analyze and cache record operations of security hub
            s_recordOperationsCache = new RecordOperationsCache(typeof(DataHub));
        }

        #endregion

        #region [ Setting ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterString)
        {
            return m_dataContext.Table<Setting>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            m_dataContext.Table<Setting>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return new Setting();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewActionItem(Setting record)
        {
            m_dataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(Setting record)
        {
            m_dataContext.Table<Setting>().UpdateRecord(record);
        }

        #endregion

        #region [ DashSettings ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecordCount)]
        public int QueryDashSettingsCount(string filterString)
        {
            return m_dataContext.Table<DashSettings>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<DashSettings> QueryDashSettingss(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<DashSettings>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.DeleteRecord)]
        public void DeleteDashSettings(int id)
        {
            m_dataContext.Table<DashSettings>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.CreateNewRecord)]
        public DashSettings NewDashSettings()
        {
            return new DashSettings();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.AddNewRecord)]
        public void AddNewActionItem(DashSettings record)
        {
            m_dataContext.Table<DashSettings>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(DashSettings), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(DashSettings record)
        {
            m_dataContext.Table<DashSettings>().UpdateRecord(record);
        }

        #endregion

        #region [ Meter ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecordCount)]
        public int QueryMeterCount(string filterString)
        {
            return m_dataContext.Table<Meter>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable<Meter> QueryMeters(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<Meter>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.DeleteRecord)]
        public void DeleteMeter(int id)
        {
            m_dataContext.Table<Meter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.CreateNewRecord)]
        public Meter NewMeter()
        {
            return new Meter();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.AddNewRecord)]
        public void AddNewMeter(Meter record)
        {
            m_dataContext.Table<Meter>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Meter), RecordOperation.UpdateRecord)]
        public void UpdateMeter(Meter record)
        {
            m_dataContext.Table<Meter>().UpdateRecord(record);
        }

        #endregion

        #region [ MeterLocation ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecordCount)]
        public int QueryMeterLocationCount(string filterString)
        {
            return m_dataContext.Table<MeterLocation>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLocation> QueryMeterLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<MeterLocation>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.DeleteRecord)]
        public void DeleteMeterLocation(int id)
        {
            m_dataContext.Table<MeterLocation>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.CreateNewRecord)]
        public MeterLocation NewMeterLocation()
        {
            return new MeterLocation();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.AddNewRecord)]
        public void AddNewMeterLocation(MeterLocation record)
        {
            m_dataContext.Table<MeterLocation>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.UpdateRecord)]
        public void UpdateMeterLocation(MeterLocation record)
        {
            m_dataContext.Table<MeterLocation>().UpdateRecord(record);
        }

        #endregion

        #region [ Lines ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.QueryRecordCount)]
        public int QueryLinesCount(string filterString)
        {
            return m_dataContext.Table<Lines>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.QueryRecords)]
        public IEnumerable<Lines> QueryLines(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<Lines>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.DeleteRecord)]
        public void DeleteLines(int id)
        {
            m_dataContext.Table<Lines>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.CreateNewRecord)]
        public Lines NewLines()
        {
            return new Lines();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.AddNewRecord)]
        public void AddNewLines(Lines record)
        {
            m_dataContext.Table<Lines>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Lines), RecordOperation.UpdateRecord)]
        public void UpdateLines(Lines record)
        {
            m_dataContext.Table<Lines>().UpdateRecord(record);
        }

        #endregion

        #region [ LineView ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
        public int QueryLineViewCount(string filterString)
        {
            return m_dataContext.Table<LineView>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
        public IEnumerable<LineView> QueryLineView(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<LineView>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
        public void DeleteLineView(int id)
        {
            m_dataContext.Table<Lines>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.CreateNewRecord)]
        public LineView NewLineView()
        {
            return new LineView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.AddNewRecord)]
        public void AddNewLineView(LineView record)
        {
            m_dataContext.Table<Lines>().AddNewRecord(CreateLines(record));
            m_dataContext.Table<LineImpedance>().AddNewRecord(CreateLineImpedance(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
        public void UpdateLineView(LineView record)
        {
            m_dataContext.Table<Lines>().UpdateRecord(CreateLines(record));
            m_dataContext.Table<LineImpedance>().UpdateRecord(CreateLineImpedance(record));

        }

        public Lines CreateLines(LineView record)
        {
            Lines line = NewLines();
            line.AssetKey = record.AssetKey;
            line.Description = record.Description;
            line.Length = record.Length;
            line.ThermalRating = record.ThermalRating;
            line.VoltageKV = record.VoltageKV;
            return line;
        }

        public LineImpedance CreateLineImpedance(LineView record)
        {
            LineImpedance li = new LineImpedance();
            li.R0 = record.R0;
            li.R1 = record.R1;
            li.X0 = record.X0;
            li.X1 = record.X1;
            li.LineID = record.ID;
            return li;
        }

        #endregion


        #region [ MeterLine ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecordCount)]
        public int QueryMeterLineCount(int lineID, int meterID, string filterString)
        {
            string restrictionString = "";
            if(lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if(meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return m_dataContext.Table<MeterLine>().QueryRecordCount(new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLine> QueryMeterLine(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string restrictionString = "";
            if (lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if (meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return m_dataContext.Table<MeterLine>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterLine(int id)
        {
            m_dataContext.Table<MeterLine>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.CreateNewRecord)]
        public MeterLine NewMeterLine()
        {
            return new MeterLine();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.AddNewRecord)]
        public void AddNewMeterLine(MeterLine record)
        {
            m_dataContext.Table<MeterLine>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLine), RecordOperation.UpdateRecord)]
        public void UpdateMeterLine(MeterLine record)
        {
            m_dataContext.Table<MeterLine>().UpdateRecord(record);
        }

        #endregion

        #region [ Channel ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecordCount)]
        public int QueryChannelCount(int lineID, int meterID, string filterString)
        {

            return m_dataContext.Table<Channel>().QueryRecordCount(new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID}"));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<Channel> QueryChannel(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<Channel>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID}"));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.DeleteRecord)]
        public void DeleteChannel(int id)
        {
            m_dataContext.Table<Channel>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.CreateNewRecord)]
        public Channel NewChannel()
        {
            return new Channel();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.AddNewRecord)]
        public void AddNewChannel(Channel record)
        {
            m_dataContext.Table<Channel>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(Channel record)
        {
            m_dataContext.Table<Channel>().UpdateRecord(record);
        }

        #endregion


        #region [ Group ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.QueryRecordCount)]
        public int QueryGroupCount(string filterString)
        {
            return m_dataContext.Table<Group>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.QueryRecords)]
        public IEnumerable<Group> QueryGroups(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<Group>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.DeleteRecord)]
        public void DeleteGroup(int id)
        {
            IEnumerable<GroupMeter> table = m_dataContext.Table<GroupMeter>().QueryRecords(restriction: new RecordRestriction("GroupID = {0}", id));
            foreach (GroupMeter gm in table)
            {
                m_dataContext.Table<GroupMeter>().DeleteRecord(gm.ID);
            }
            m_dataContext.Table<Group>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.CreateNewRecord)]
        public Group NewGroup()
        {
            return new Group();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.AddNewRecord)]
        public void AddNewGroup(Group record)
        {
            m_dataContext.Table<Group>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Group), RecordOperation.UpdateRecord)]
        public void UpdateGroup(Group record)
        {
            m_dataContext.Table<Group>().UpdateRecord(record);
        }

        #endregion

        #region [ GroupMeterView ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.QueryRecordCount)]
        public int QueryGroupMeterViewCount(int groupID, string filterString)
        {
            return m_dataContext.Table<GroupMeterView>().QueryRecordCount(new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.QueryRecords)]
        public IEnumerable<GroupMeterView> QueryGroupMeterViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<GroupMeterView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.DeleteRecord)]
        public void DeleteGroupMeterView(int id)
        {
            m_dataContext.Table<GroupMeter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.CreateNewRecord)]
        public GroupMeterView NewGroupMeterView()
        {
            return new GroupMeterView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.AddNewRecord)]
        public void AddNewGroupMeterView(GroupMeterView record)
        {
            m_dataContext.Table<GroupMeter>().AddNewRecord(CreateNewGroupMeter(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.UpdateRecord)]
        public void UpdateGroupMeterView(GroupMeterView record)
        {
            m_dataContext.Table<GroupMeter>().UpdateRecord(CreateNewGroupMeter(record));
        }

        public GroupMeter CreateNewGroupMeter(GroupMeterView record)
        {
            GroupMeter gm = new GroupMeter();
            gm.GroupID = record.GroupID;
            gm.MeterID = record.MeterID;
            return gm;
        }

        #endregion

        #region [ AlarmRangeLimitView ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryAlarmRangeLimitViewCount(string filterString)
        {
            return m_dataContext.Table<AlarmRangeLimitView>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<AlarmRangeLimitView> QueryAlarmRangeLimitViews( string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<AlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteAlarmRangeLimitView(int id)
        {
            m_dataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public AlarmRangeLimitView NewAlarmRangeLimitView()
        {
            return new AlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            m_dataContext.Table<AlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            m_dataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public AlarmRangeLimit CreateNewAlarmRangeLimit(AlarmRangeLimitView record)
        {
            AlarmRangeLimit arl = new AlarmRangeLimit();
            arl.ID = record.ID;
            arl.ChannelID = record.ChannelID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            arl.Enabled = record.Enabled;
            return arl;
        }

        public void ResetAlarmToDefault(AlarmRangeLimitView record)
        {
            IEnumerable<DefaultAlarmRangeLimit> defaultLimits = m_dataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

            if(defaultLimits.Count() == 1)
            {
                foreach (DefaultAlarmRangeLimit limit in defaultLimits)
                {
                    record.Severity = limit.Severity;
                    record.High = limit.High;
                    record.Low = limit.Low;
                    record.RangeInclusive = limit.RangeInclusive;
                    record.PerUnit = limit.PerUnit;
                }

                m_dataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
            }
        }

        #endregion

        #region [ DefaultAlarmRangeLimitView ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryDefaultAlarmRangeLimitViewCount(string filterString)
        {
            return m_dataContext.Table<DefaultAlarmRangeLimitView>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<DefaultAlarmRangeLimitView> QueryDefaultAlarmRangeLimitViews(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return m_dataContext.Table<DefaultAlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteDefaultAlarmRangeLimitView(int id)
        {
            m_dataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public DefaultAlarmRangeLimitView NewDefaultAlarmRangeLimitView()
        {
            return new DefaultAlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            m_dataContext.Table<DefaultAlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            m_dataContext.Table<DefaultAlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public DefaultAlarmRangeLimit CreateNewAlarmRangeLimit(DefaultAlarmRangeLimitView record)
        {
            DefaultAlarmRangeLimit arl = new DefaultAlarmRangeLimit();
            arl.ID = record.ID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            return arl;
        }

        #endregion

    }
}