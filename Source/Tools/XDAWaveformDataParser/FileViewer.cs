//******************************************************************************************************
//  FileViewer.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/06/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GSF;
using GSF.COMTRADE;
using GSF.EMAX;
using GSF.IO;
using GSF.PQDIF.Logical;
using GSF.SELEventParser;
using Parser = GSF.COMTRADE.Parser;

namespace XDAWaveformDataParser
{
    public partial class FileViewer : Form
    {
        #region [ Members ]

        // Nested Types
        private class ParsedChannel
        {
            public int Index;
            public string Name;
            public List<DateTime> TimeValues; 
            public IList<object> XValues;
            public IList<object> YValues;
        }

        // Fields
        private string m_csvPath;
        private string m_fileName;
        private IList<ParsedChannel> m_channels;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Creates an instance of <see cref="FileViewer"/> class.
        /// </summary>
        public FileViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Methods ]

        private void Form_Resize(object sender, EventArgs e)
        {
            ChannelListBox.Height = Height - 63;
            DataChart.Width = Width - 356;
            DataChart.Height = Height - 63;
        }

        private void COMTRADEButton_Click(object sender, EventArgs e)
        {
            string directory;
            string rootFileName;
            string configurationFileName;

            DateTime? startTime = null;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "COMTRADE Files|*.dat;*.d00|All Files|*.*";
                dialog.Title = "Browse COMTRADE Files";

                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                if (!File.Exists(dialog.FileName))
                    return;

                // Comtrade parsing will require a CFG file, make sure this exists...
                directory = Path.GetDirectoryName(dialog.FileName) ?? string.Empty;
                rootFileName = FilePath.GetFileNameWithoutExtension(dialog.FileName);
                configurationFileName = Path.Combine(directory, rootFileName + ".cfg");

                if (!File.Exists(configurationFileName))
                    return;

                using (Parser parser = new Parser())
                {
                    parser.Schema = new Schema(configurationFileName);
                    parser.FileName = dialog.FileName;
                    parser.InferTimeFromSampleRates = true;

                    // Open COMTRADE data files
                    parser.OpenFiles();

                    // Parse COMTRADE schema into channels
                    m_channels = parser.Schema.AnalogChannels
                        .Select(channel => new ParsedChannel()
                        {
                            Index = channel.Index,
                            Name = ((object)channel.ChannelName != null) ? string.Format("{0} ({1})", channel.StationName, channel.ChannelName) : channel.StationName,
                            TimeValues = new List<DateTime>(),
                            XValues = new List<object>(),
                            YValues = new List<object>()
                        }).ToList();

                    // Read values from COMTRADE data file
                    while (parser.ReadNext())
                    {
                        if ((object)startTime == null)
                            startTime = parser.Timestamp;

                        for (int i = 0; i < m_channels.Count; i++)
                        {
                            m_channels[i].TimeValues.Add(parser.Timestamp);
                            m_channels[i].XValues.Add(parser.Timestamp.Subtract(startTime.Value).TotalSeconds);
                            m_channels[i].YValues.Add(parser.Values[i]);
                        }
                    }
                }

                // Clear the list box and data chart
                ChannelListBox.Items.Clear();
                DataChart.Series.Clear();

                // Populate the list box with channel names
                ChannelListBox.Items.AddRange(m_channels
                    .Select(channel => string.Format("[{0}] {1}", channel.Index, channel.Name))
                    .Cast<object>()
                    .ToArray());

                // Select the first channel in the list
                ChannelListBox.SelectedIndex = 0;

                // Change the title text of the window to show what file the user has open
                m_fileName = dialog.SafeFileName;
                Text = string.Format("COMTRADE - [{0}]", dialog.SafeFileName);
            }
        }

        private void PQDIFButton_Click(object sender, EventArgs e)
        {
            List<ObservationRecord> observationRecords;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "PQDIF Files|*.pqd|All Files|*.*";
                dialog.Title = "Browse PQDIF Files";

                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                if (!File.Exists(dialog.FileName))
                    return;

                // Parse PQDif File Data
                using (LogicalParser logicalParser = new LogicalParser(dialog.FileName))
                {
                    observationRecords = new List<ObservationRecord>();
                    logicalParser.Open();

                    while (logicalParser.HasNextObservationRecord())
                        observationRecords.Add(logicalParser.NextObservationRecord());
                }

                // Convert to common channel format
                m_channels = observationRecords
                    .SelectMany(observation => observation.ChannelInstances)
                    .Where(channel => channel.Definition.QuantityTypeID == QuantityType.WaveForm)
                    .Select(MakeParsedChannel)
                    .ToList();

                // Clear the list box and data chart
                ChannelListBox.Items.Clear();
                DataChart.Series.Clear();

                // Populate the list box with channel names
                ChannelListBox.Items.AddRange(m_channels
                    .Select((channel, index) => string.Format("[{0}] {1}", index, channel.Name))
                    .Cast<object>()
                    .ToArray());

                // Select the first channel in the list
                ChannelListBox.SelectedIndex = 0;

                // Change the title text of the window to show what file the user has open
                m_fileName = dialog.SafeFileName;
                Text = string.Format("PQDIF - [{0}]", dialog.SafeFileName);
            }
        }

        private void SELEVEButton_Click(object sender, EventArgs e)
        {
            EventFile parsedFile;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SEL Event Files|*.eve;*.sel;*.cev;|Text Files|*.txt|All Files|*.*";
                dialog.Title = "Browse SEL Event Files";

                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                if (!File.Exists(dialog.FileName))
                    return;

                // Parse event file
                parsedFile = EventFile.Parse(dialog.FileName);

                // Convert to common channel format
                if(parsedFile.EventReports.Count > 0)
                {
                    m_channels = parsedFile.EventReports
                        .SelectMany(report => report.AnalogSection.AnalogChannels.Select(channel => MakeParsedChannel(report, channel)))
                        .ToList();
                }
                else if( parsedFile.CommaSeparatedEventReports.Count > 0)
                {
                    m_channels = parsedFile.CommaSeparatedEventReports
                        .SelectMany(report => report.AnalogSection.AnalogChannels.Select(channel => MakeParsedChannel(report, channel)))
                        .ToList();
                }

                // Clear the list box and data chart
                ChannelListBox.Items.Clear();
                DataChart.Series.Clear();

                // Populate the list box with channel names
                ChannelListBox.Items.AddRange(m_channels
                    .Select((channel, index) => string.Format("[{0}] {1}", index, channel.Name))
                    .Cast<object>()
                    .ToArray());

                // Select the first channel in the list
                ChannelListBox.SelectedIndex = 0;

                // Change the title text of the window to show what file the user has open
                m_fileName = dialog.SafeFileName;
                Text = string.Format("SEL EVE - [{0}]", dialog.SafeFileName);
            }
        }

        private void EMAXButton_Click(object sender, EventArgs e)
        {
            string directory;
            string rootFileName;
            string controlFileName;

            DateTime? startTime = null;
            DateTime timestamp;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "EMAX Files|*.rcd;*.rcl|All Files|*.*";
                dialog.Title = "Browse EMAX Files";

                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                if (!File.Exists(dialog.FileName))
                    return;

                // EMAX parsing will require a CTL file, make sure this exists...
                directory = Path.GetDirectoryName(dialog.FileName) ?? string.Empty;
                rootFileName = FilePath.GetFileNameWithoutExtension(dialog.FileName);
                controlFileName = Path.Combine(directory, rootFileName + ".ctl");

                if (!File.Exists(controlFileName))
                    return;

                using (CorrectiveParser parser = new CorrectiveParser())
                {
                    parser.ControlFile = new ControlFile(controlFileName);
                    parser.FileName = dialog.FileName;

                    // Open EMAX data file
                    parser.OpenFiles();

                    // Parse EMAX control file into channels
                    m_channels = parser.ControlFile.AnalogChannelSettings.Values
                        .Select(channel => new ParsedChannel()
                        {
                            Index = Convert.ToInt32(channel.chanlnum),
                            Name = channel.title,
                            TimeValues = new List<DateTime>(),
                            XValues = new List<object>(),
                            YValues = new List<object>()
                        })
                        .OrderBy(channel => channel.Index)
                        .ToList();

                    // Read values from EMAX data file
                    while (parser.ReadNext())
                    {
                        timestamp = parser.CalculatedTimestamp;

                        // If this is the first frame, store this frame's
                        // timestamp as the start time of the file
                        if ((object)startTime == null)
                            startTime = timestamp;

                        // Read the values from this frame into
                        // x- and y-value collections for each channel
                        for (int i = 0; i < m_channels.Count; i++)
                        {
                            m_channels[i].TimeValues.Add(timestamp);
                            m_channels[i].XValues.Add(timestamp.Subtract(startTime.Value).TotalSeconds);
                            m_channels[i].YValues.Add(parser.CorrectedValues[i]);
                        }
                    }
                }

                // Clear the list box and data chart
                ChannelListBox.Items.Clear();
                DataChart.Series.Clear();

                // Populate the list box with channel names
                ChannelListBox.Items.AddRange(m_channels
                    .Select(channel => string.Format("[{0}] {1}", channel.Index, channel.Name))
                    .Cast<object>()
                    .ToArray());

                // Select the first channel in the list
                ChannelListBox.SelectedIndex = 0;

                // Change the title text of the window to show what file the user has open
                m_fileName = dialog.SafeFileName;
                Text = string.Format("EMAX - [{0}]", dialog.SafeFileName);
            }
        }

        private void CSVExportButton_Click(object sender, EventArgs e)
        {
            if ((object)m_channels != null)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    // Restore the most recently
                    // selected path for convenience
                    if ((object)m_csvPath != null)
                        dialog.SelectedPath = m_csvPath;

                    // Show the folder browser to the user
                    if (dialog.ShowDialog() == DialogResult.Cancel)
                        return;

                    // Keep track of the most recently
                    // selected path for convenience
                    m_csvPath = dialog.SelectedPath;
                    
                    // Export the data to CSV
                    ExportToCSV(dialog.SelectedPath);
                }
            }
        }

        private void ChannelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParsedChannel parsedChannel;
            IList<object> xValues;
            IList<object> yValues;
            Series chartSeries;

            if (ChannelListBox.SelectedIndex < 0)
                return;

            // Clear the data chart
            DataChart.Series.Clear();

            // Display all selected channels on the chart
            foreach (int selectedIndex in ChannelListBox.SelectedIndices)
            {
                parsedChannel = m_channels[selectedIndex];
                xValues = parsedChannel.XValues;
                yValues = parsedChannel.YValues;

                // Add a series to the chart for this channel
                chartSeries = DataChart.Series.Add(parsedChannel.Name);
                chartSeries.ChartType = SeriesChartType.Line;

                // Go through all the x and y values and
                // add them as points to the chart series
                for (int i = 0; i < xValues.Count && i < yValues.Count; i++)
                    chartSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            RecalculateAxes();
        }

        private void RecalculateAxes()
        {
            double xMin, xMax;
            double yMin, yMax;
            double xScale, yScale;

            // Get the absolute smallest and largest x-values and y-values of the data points on the chart
            xMin = DataChart.Series.Select(series => series.Points.Select(point => point.XValue).Min()).Min();
            xMax = DataChart.Series.Select(series => series.Points.Select(point => point.XValue).Max()).Max();
            yMin = DataChart.Series.Select(series => series.Points.Select(point => point.YValues.Min()).Min()).Min();
            yMax = DataChart.Series.Select(series => series.Points.Select(point => point.YValues.Max()).Max()).Max();

            // Determine scale factor
            xScale = GetChartScale(xMax - xMin);
            yScale = GetChartScale(yMax - yMin);

            // Apply scale to make axis labels more readable
            xMin = xScale * Math.Floor(xMin / xScale);
            xMax = xScale * Math.Ceiling(xMax / xScale);
            yMin = yScale * Math.Floor(yMin / yScale);
            yMax = yScale * Math.Ceiling(yMax / yScale);

            // If the difference between the min an max values is
            // zero, add some space so we do not encounter an error
            if (xMax - xMin == 0.0D)
            {
                xMin -= 0.5D;
                xMax += 0.5D;
            }

            if (yMax - yMin == 0.0D)
            {
                yMin -= 0.5D;
                yMax += 0.5D;
            }

            // Set min, max, and interval of each axis
            DataChart.ChartAreas[0].AxisX.Minimum = xMin;
            DataChart.ChartAreas[0].AxisX.Maximum = xMax;
            DataChart.ChartAreas[0].AxisY.Minimum = yMin;
            DataChart.ChartAreas[0].AxisY.Maximum = yMax;
            DataChart.ChartAreas[0].AxisX.Interval = (xMax - xMin) / 10.0D;
            DataChart.ChartAreas[0].AxisY.Interval = (yMax - yMin) / 10.0D;
        }

        private double GetChartScale(double diff)
        {
            double abs = Math.Abs(diff);
            double log = Math.Log10(abs);
            return (diff == 0.0D) ? 1.0D : Math.Pow(10.0D, Math.Floor(log) - 1.0D);
        }

        private ParsedChannel MakeParsedChannel(ChannelInstance channel)
        {
            // Get the time series and value series for the given channel
            SeriesInstance timeSeries = channel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Time);
            SeriesInstance valuesSeries = channel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Val);

            // Set up parsed channel to be returned
            ParsedChannel parsedChannel = new ParsedChannel()
            {
                Name = channel.Definition.ChannelName,
                YValues = valuesSeries.OriginalValues
            };

            if (timeSeries.Definition.QuantityUnits == QuantityUnits.Seconds)
            {
                // If time series is in seconds from start time of the observation record,
                // TimeValues must be calculated by adding values to start time
                parsedChannel.TimeValues = timeSeries.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .Select(timeSpan => channel.ObservationRecord.StartTime + timeSpan)
                    .ToList();

                parsedChannel.XValues = timeSeries.OriginalValues;
            }
            else if (timeSeries.Definition.QuantityUnits == QuantityUnits.Timestamp)
            {
                // If time series is a collection of absolute time, seconds from start time
                // must be calculated by subtracting the start time of the observation record
                parsedChannel.TimeValues = timeSeries.OriginalValues.Cast<DateTime>().ToList();

                parsedChannel.XValues = timeSeries.OriginalValues
                    .Cast<DateTime>()
                    .Select(time => time - channel.ObservationRecord.StartTime)
                    .Select(timeSpan => timeSpan.TotalSeconds)
                    .Cast<object>()
                    .ToList();
            }

            return parsedChannel;
        }

        private ParsedChannel MakeParsedChannel(EventReport report, Channel<double> channel)
        {
            List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples.ToList();

            List<object> xValues = timeSamples
                .Select(time => time - timeSamples[0])
                .Select(timeSpan => timeSpan.TotalSeconds)
                .Cast<object>()
                .ToList();

            ParsedChannel parsedChannel = new ParsedChannel()
            {
                Name = string.Format("({0}) {1}", report.Command, channel.Name),
                TimeValues = timeSamples,
                XValues = xValues,
                YValues = channel.Samples.Cast<object>().ToList()
            };

            return parsedChannel;
        }

        private ParsedChannel MakeParsedChannel(CommaSeparatedEventReport report, Channel<double> channel)
        {
            List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples.ToList();

            List<object> xValues = timeSamples
                .Select(time => time - timeSamples[0])
                .Select(timeSpan => timeSpan.TotalSeconds)
                .Cast<object>()
                .ToList();

            ParsedChannel parsedChannel = new ParsedChannel()
            {
                Name = string.Format("({0}) {1}", report.Command, channel.Name),
                TimeValues = timeSamples,
                XValues = xValues,
                YValues = channel.Samples.Cast<object>().ToList()
            };

            return parsedChannel;
        }



        private void ExportToCSV(string location)
        {
            IList<IList<ParsedChannel>> groupedChannels = new List<IList<ParsedChannel>>();
            IList<ParsedChannel> selectedGroup;

            foreach (ParsedChannel channel in m_channels)
            {
                // Get the group that this channel belongs to
                selectedGroup = groupedChannels.FirstOrDefault(group => group.First().TimeValues.SequenceEqual(channel.TimeValues));

                // If no group was found, create one
                if ((object)selectedGroup == null)
                {
                    selectedGroup = new List<ParsedChannel>();
                    groupedChannels.Add(selectedGroup);
                }

                // Add the channel to the group
                selectedGroup.Add(channel);
            }

            // Make a separate CSV file for each group of channels
            foreach (IList<ParsedChannel> group in groupedChannels)
                MakeCSVFile(location, group);
        }

        private void MakeCSVFile(string location, IList<ParsedChannel> channels)
        {
            IList<DateTime> timeValues = channels.First().TimeValues;

            // CSV file name is original file name plus time range (yyyy-MM-dd HH!mm!ss.fff format) with .csv extension
            string timeRange = string.Format("{0:yyyy-MM-dd HH!mm!ss.fff}_to_{1:yyyy-MM-dd HH!mm!ss.fff}", timeValues.First(), timeValues.Last());
            string csvFileName = string.Format("{0}_{1}.csv", Path.GetFileNameWithoutExtension(m_fileName), timeRange);
            string csvFilePath = FilePath.GetUniqueFilePathWithBinarySearch(Path.Combine(location, csvFileName));

            // Generate CSV header by joining channel names
            string header = "Time,Ticks," + string.Join(",", channels.Select(channel => channel.Name));

            // It is assumed that all time series in the channels are the same, and that all y-value collections are the same size.
            // Generate CSV data by aligning timestamps and data by index, formatting each time value and joining the corresponding data values together.
            IEnumerable<string> lines = timeValues.Select((time, index) => time.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + "," + (time.Ticks % Ticks.PerSecond) + "," + string.Join(",", channels.Select(channel => Convert.ToDouble(channel.YValues[index]))));

            // Open the file and write in each line
            using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(csvFilePath)))
            {
                fileWriter.WriteLine(header);

                foreach (string line in lines)
                    fileWriter.WriteLine(line);
            }
        }

        #endregion
    }
}
