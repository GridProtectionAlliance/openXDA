using System;
using System.Collections.Generic;
using System.Linq;

namespace FaultData.Database
{
    public class ChannelKey : IEquatable<ChannelKey>
    {
        #region [ Members ]

        // Fields
        private readonly Tuple<int, int, string, string, string, string> m_tuple;

        #endregion

        #region [ Constructors ]

        public ChannelKey(int lineID, int harmonicGroup, string name, string measurementType, string measurementCharacteristic, string phase)
        {
            m_tuple = Tuple.Create(lineID, harmonicGroup, name, measurementType, measurementCharacteristic, phase);
        }

        public ChannelKey(Channel channel)
            : this(channel.Line.ID, channel.HarmonicGroup, channel.Name, channel.MeasurementType.Name, channel.MeasurementCharacteristic.Name, channel.Phase.Name)
        {
        }

        #endregion

        #region [ Properties ]

        public int LineID
        {
            get
            {
                return m_tuple.Item1;
            }
        }

        public int HarmonicGroup
        {
            get
            {
                return m_tuple.Item2;
            }
        }

        public string Name
        {
            get
            {
                return m_tuple.Item3;
            }
        }

        public string MeasurementType
        {
            get
            {
                return m_tuple.Item4;
            }
        }

        public string MeasurementCharacteristic
        {
            get
            {
                return m_tuple.Item5;
            }
        }

        public string Phase
        {
            get
            {
                return m_tuple.Item6;
            }
        }

        #endregion

        #region [ Methods ]

        public override int GetHashCode()
        {
            return m_tuple.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChannelKey);
        }

        public bool Equals(ChannelKey other)
        {
            if ((object)other == null)
                return false;

            return m_tuple.Equals(other.m_tuple);
        }

        #endregion
    }

    public class SeriesKey : IEquatable<SeriesKey>
    {
        #region [ Members ]

        // Fields
        private Tuple<ChannelKey, string> m_tuple;

        #endregion

        #region [ Constructors ]

        public SeriesKey(ChannelKey channelKey, string seriesType)
        {
            m_tuple = Tuple.Create(channelKey, seriesType);
        }

        public SeriesKey(Series series)
            : this(new ChannelKey(series.Channel), series.SeriesType.Name)
        {
        }

        #endregion

        #region [ Properties ]

        public ChannelKey ChannelKey
        {
            get
            {
                return m_tuple.Item1;
            }
        }

        public string SeriesType
        {
            get
            {
                return m_tuple.Item2;
            }
        }

        #endregion

        #region [ Methods ]

        public override int GetHashCode()
        {
            return m_tuple.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SeriesKey);
        }

        public bool Equals(SeriesKey other)
        {
            if ((object)other == null)
                return false;

            return m_tuple.Equals(other.m_tuple);
        }

        #endregion
    }

    partial class Meter
    {
        public TimeZoneInfo GetTimeZoneInfo(TimeZoneInfo defaultTimeZone)
        {
            if (!string.IsNullOrEmpty(TimeZone))
                return TimeZoneInfo.FindSystemTimeZoneById(TimeZone);

            return defaultTimeZone;
        }
    }

    partial class MeasurementType
    {
        public MeasurementType Clone()
        {
            return new MeasurementType()
            {
                Name = Name,
                Description = Description
            };
        }
    }

    partial class MeasurementCharacteristic
    {
        public MeasurementCharacteristic Clone()
        {
            return new MeasurementCharacteristic()
            {
                Name = Name,
                Description = Description
            };
        }
    }

    partial class Phase
    {
        public Phase Clone()
        {
            return new Phase()
            {
                Name = Name,
                Description = Description
            };
        }
    }

    partial class Channel
    {
        public Channel Clone()
        {
            return new Channel()
            {
                Meter = Meter,
                Line = Line,
                MeasurementType = MeasurementType,
                MeasurementCharacteristic = MeasurementCharacteristic,
                Phase = Phase,
                Name = Name,
                HarmonicGroup = HarmonicGroup,
                SamplesPerHour = SamplesPerHour,
                PerUnitValue = PerUnitValue,
                Description = Description
            };
        }
    }

    partial class SeriesType
    {
        public SeriesType Clone()
        {
            return new SeriesType()
            {
                Name = Name,
                Description = Description
            };
        }
    }

    partial class Series
    {
        public Series Clone()
        {
            return new Series()
            {
                SeriesType = SeriesType,
                Channel = Channel,
                SourceIndexes = SourceIndexes
            };
        }
    }

    partial class Line
    {
        public IEnumerable<MeterLocation> MeterLocations
        {
            get
            {
                return MeterLocationLines.Select(mll => mll.MeterLocation);
            }
        }
    }

    partial class MeterLocation
    {
        public IEnumerable<Line> Lines
        {
            get
            {
                return MeterLocationLines.Select(mll => mll.Line);
            }
        }
    }
}
