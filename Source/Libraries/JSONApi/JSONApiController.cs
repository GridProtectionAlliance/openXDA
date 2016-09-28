using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using GSF.Data;
using GSF.Web.Model;

namespace JSONApi
{
    public class JSONApiController : ApiController
    {

        #region [Query Classes]
        public class MeterJSON
        {
            public string AssetKey { get; set; }
            public string ID { get; set; } 
            public string Name { get; set; }
        }

        public class StationJSON
        {
            public string AssetKey { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
        }


        public class LineJSON
        {
            public string AssetKey { get; set; }
            public string ID { get; set; }
        }

        #endregion

        #region [HttpPost Functions]
        [HttpPost]
        public IEnumerable<Meter> GetMeters(MeterJSON json)
        {
            using(DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                string id = (json != null ? (json.ID ?? "%") : "%");
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Meter WHERE AssetKey LIKE {0} AND ID LIKE {1} AND NAME LIKE {2}", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<Meter>().LoadRecord(row));
            }
        }

        [HttpPost]
        public IEnumerable<Line> GetLines(LineJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                string id = (json != null ? (json.ID ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Line WHERE AssetKey LIKE {0} AND ID LIKE {1} ", assetKey, id);
                return table.Select().Select(row => dataContext.Table<Line>().LoadRecord(row));
            }
        }

        [HttpPost]
        public IEnumerable<MeterLocation> GetStations(StationJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                string id = (json != null ? (json.ID ?? "%") : "%");
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM MeterLocation WHERE AssetKey LIKE {0} AND ID LIKE {1} AND NAME LIKE {2} ", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<MeterLocation>().LoadRecord(row));
            }
        }

        [HttpPost]
        public IEnumerable<Channel> GetChannelsByMeter(MeterJSON json)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string assetKey = (json != null ? (json.AssetKey ?? "%") : "%");
                string id = (json != null ? (json.ID ?? "%") : "%");
                string name = (json != null ? (json.Name ?? "%") : "%");

                DataTable table = dataContext.Connection.RetrieveData("Select * FROM Channel WHERE MeterID IN (Select ID FROM Meter WHERE AssetKey LIKE {0} AND ID LIKE {1} AND NAME LIKE {2})", assetKey, id, name);
                return table.Select().Select(row => dataContext.Table<Channel>().LoadRecord(row));
            }
        }

        #endregion
    }
}