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
        public void AddNewActionItem(Meter record)
        {
            m_dataContext.Table<Meter>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Meter), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(Meter record)
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
        public void AddNewActionItem(MeterLocation record)
        {
            m_dataContext.Table<MeterLocation>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(MeterLocation record)
        {
            m_dataContext.Table<MeterLocation>().UpdateRecord(record);
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
        public void AddNewActionItem(Group record)
        {
            m_dataContext.Table<Group>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Group), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(Group record)
        {
            m_dataContext.Table<Group>().UpdateRecord(record);
        }

        #endregion


    }
}