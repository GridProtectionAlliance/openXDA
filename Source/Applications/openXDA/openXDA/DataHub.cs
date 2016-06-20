using System.Collections.Generic;
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
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
        public void UpdateLineView(LineView record)
        {
            m_dataContext.Table<Lines>().UpdateRecord(CreateLines(record));
        }

        public Lines CreateLines(LineView record)
        {
            Lines line = NewLineView();
            line.AssetKey = record.AssetKey;
            line.Description = record.Description;
            line.Length = record.Length;
            line.ThermalRating = record.ThermalRating;
            line.VoltageKV = record.VoltageKV;
            return line;
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


    }
}