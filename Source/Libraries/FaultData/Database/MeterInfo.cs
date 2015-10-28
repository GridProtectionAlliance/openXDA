using System.Collections.Generic;
using System.Linq;

namespace FaultData.Database
{
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
