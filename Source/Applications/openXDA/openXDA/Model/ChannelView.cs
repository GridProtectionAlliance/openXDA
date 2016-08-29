using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace openXDA.Model
{
    [Table("ChannelView")]
    public class ChannelView: Channel
    {
        public string MeasurementType { get; set; }
        public string MeasurementCharacteristic { get; set; }
    }
}