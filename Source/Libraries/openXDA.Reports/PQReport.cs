//******************************************************************************************************
//  PQReport.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/12/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using Root.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using ChartSeries = System.Windows.Forms.DataVisualization.Charting.Series;

namespace openXDA.Reports
{
    public class PQReport : Root.Reports.Report
    {
        #region [ Members ]

        // Constants
        private const double PageMarginMillimeters = 25.4D;             // 1-inch margin
        private const double PageWidthMillimeters = 8.5D * 25.4D;       // 8.5 inch width
        private const double PageHeightMillimeters = 11.0D * 25.4D;     // 11 inch height

        private const double FooterHeightMillimeters = (10.0D / 72.0D) * 25.4D;
        private const double SpacingMillimeters = 6.0D;

        private const int NumBuckets = 45;

        private const string TitleText = "Monthly PQ Compliance Report";

        private class PointGroup
        {
            public string Name { get; set; }
            public List<openHistorian.XDALink.TrendingDataPoint> Data { get; set; }
            public Color Color { get; set; }
        }

        private class SummaryResults
        {
            public SummaryResults()
            {
                Frequency = new FrequencyData();
                VoltageLN = new VoltageData();
                VoltageLL = new VoltageData();
                Flicker = new FlickerData();
                Imbalance = new ImbalanceData();
                THD = new THDData();
                Harmonics = new HarmonicsData();
                Sags = new SagsData();
                Swells = new SwellsData();
                Interruptions = new InterruptionsData();
                Faults = new FaultsData();
            }

            public FrequencyData Frequency { get; set; }
            public VoltageData VoltageLN { get; set; }
            public VoltageData VoltageLL { get; set; }
            public FlickerData Flicker { get; set; }
            public ImbalanceData Imbalance { get; set; }
            public THDData THD { get; set; }
            public HarmonicsData Harmonics { get; set; }
            public SagsData Sags { get; set; }
            public SwellsData Swells { get; set; }
            public InterruptionsData Interruptions { get; set; }
            public FaultsData Faults { get; set; }

            public class FrequencyData
            {
                public double Min { get; set; }
                public double Avg { get; set; }
                public double Max { get; set; }
                public string Compliance { get; set; }
                public double Nominal { get; set; }
            }
            public class VoltageData
            {
                public double Min { get; set; }
                public double Avg { get; set; }
                public double Max { get; set; }
                public string Compliance { get; set; }
                public double Nominal { get; set; }
            }
            public class FlickerData
            {
                public double Max { get; set; }
                public string Compliance { get; set; }
            }
            public class ImbalanceData
            {
                public double Avg { get; set; }
                public string Compliance { get; set; }
            }
            public class THDData
            {
                public double Min { get; set; }
                public double Avg { get; set; }
                public double Max { get; set; }
                public string Compliance { get; set; }
            }

            public class HarmonicsData
            {
                public string Compliance { get; set; }
            }

            public class SagsData
            {
                public int Count { get; set; }
            }

            public class SwellsData
            {
                public int Count { get; set; }
            }

            public class InterruptionsData
            {
                public int Count { get; set; }
            }

            public class FaultsData
            {
                public int Count { get; set; }
            }


        }

        private class Curves
        {
            public static List<Tuple<double, double>> IticUpperCurve { get; set; }
            public static List<Tuple<double, double>> IticLowerCurve { get; set; }

            static Curves()
            {
                IticUpperCurve = new List<Tuple<double, double>>()
                {
                    Tuple.Create(0.0001666667D, 5.0D),
                    Tuple.Create(0.001D, 2.0D),
                    Tuple.Create(0.003D, 1.4D),
                    Tuple.Create(0.003D, 1.2D),
                    Tuple.Create(0.5D, 1.2D),
                    Tuple.Create(0.5D, 1.1D),
                    Tuple.Create(10.0D, 1.1D)
                };
                IticLowerCurve = new List<Tuple<double, double>>()
                {
                    Tuple.Create(0.02D, 0.0D),
                    Tuple.Create(0.02D, 0.7D),
                    Tuple.Create(0.5D, 0.7D),
                    Tuple.Create(0.5D, 0.8D),
                    Tuple.Create(10.0D, 0.8D),
                    Tuple.Create(10.0D, 0.9D)
                };
            }
        }

        #endregion

        #region [ Properties ]
        public Meter Meter { get; set; }
        public DateTime FirstOfMonth { get; set; }
        public DateTime EndOfMonth { get; set; }
        public DataContext DataContext { get; set; }
        public string Result { get; set; }
        public FontDef FontDefinition { get; set; }
        private SummaryResults Summary { get; set; }
        #endregion

        #region [ Constructors ]
        public PQReport(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext)
        {
            Meter = meter;
            FirstOfMonth = firstOfMonth;
            EndOfMonth = endOfMonth;
            DataContext = dataContext;
            FontDefinition = new FontDef(this, "Helvetica");
            Summary = new SummaryResults();

        }

        #endregion

        #region [ Methods ]
        public byte[] createPDF()
        {
            GenerateReport();

            using (MemoryStream stream = new MemoryStream())
            {
                this.formatter.Create(this, stream);
                return stream.ToArray();
            }
        }

        private void GenerateReport()
        {

            DateTime now = DateTime.Now;

            // Build Report
            Page coverPage = CreatePage();
            Page pageTwo = CreatePage();
            CreateFrequencyPage();
            CreateVoltageLNPage();
            CreateVoltageLLPage();
            CreateFlickerPage();
            CreateImbalancePage();
            CreateTHDPage();
            CreateHarmonicsPage();
            CreateMagDurPage();
            double verticalMillimeters = CreateInteruptionsPage();
            verticalMillimeters = CreateSagsPage(verticalMillimeters);
            verticalMillimeters = CreateSwellsPage(verticalMillimeters);
            verticalMillimeters = CreateFaultsPage(verticalMillimeters);
            CreateSummaryPage(pageTwo);

            Result = (
                Summary.Frequency.Compliance == "Pass" &&
                Summary.VoltageLN.Compliance == "Pass" &&
                Summary.VoltageLL.Compliance == "Pass" &&
                Summary.Flicker.Compliance == "Pass" &&
                Summary.Imbalance.Compliance == "Pass" &&
                Summary.THD.Compliance == "Pass" &&
                Summary.Harmonics.Compliance == "Pass"
                ? "Pass" : "Fail");

            CreateCoverPage(coverPage);

            foreach (Page page in enum_Page)
            {
                if (page.iPageNo != 1)
                    InsertFooter(page);
            }
        }

        private void CreateCoverPage(Page page)
        {
            double verticalMillimeters = PageMarginMillimeters;
            verticalMillimeters += InsertTitle(page, verticalMillimeters) + SpacingMillimeters;

        }

        private void CreateFrequencyPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 1: Power Frequency");
            verticalMillimeters += InsertNominal(page, verticalMillimeters, "Frequency", "60.00", "Hz");
            verticalMillimeters += InsertFrequencyPage(page, verticalMillimeters);

        }

        private void CreateVoltageLNPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 2a: Supply Voltage (L-N)");
            verticalMillimeters += InsertVoltageLNPage(page, verticalMillimeters);
        }

        private void CreateVoltageLLPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 2b: Supply Voltage (L-L)");
            verticalMillimeters += InsertVoltageLLPage(page, verticalMillimeters);

        }

        private void CreateFlickerPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 3: Flicker Severity");
            verticalMillimeters += InsertFlickerPage(page, verticalMillimeters);
        }

        private void CreateImbalancePage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 4: Voltage Unbalance");
            verticalMillimeters += InsertImbalancePage(page, verticalMillimeters);
        }

        private void CreateTHDPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 5: Voltage THD");
            verticalMillimeters += InsertTHDPage(page, verticalMillimeters);
        }

        private void CreateHarmonicsPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 6: Harmonics");
        }

        private double CreateInteruptionsPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 8: Interruptions");

            DataTable dataTable = DataContext.Connection.RetrieveData(@"
                    Select 
	                    cast(Disturbance.StartTime as Date) as Date,
	                    cast(Disturbance.StartTime as Time) as Time,
	                    Disturbance.PerUnitMagnitude as Depth,
	                    Disturbance.DurationSeconds as Duration 
                    from 
	                    Event Join
	                    Disturbance ON Event.ID = Disturbance.EventID
                    WHERE 
	                    Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
	                    Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Interruption') AND 
                        Disturbance.StartTime BETWEEN {0} AND {1} AND
	                    Event.MeterID = {2}
            ", FirstOfMonth, EndOfMonth, Meter.ID);

            Summary.Interruptions.Count = dataTable.Rows.Count;

            if (dataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText( page, verticalMillimeters, $"   No Interruptions during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);

                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Date", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Time", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Depth", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Duration", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);

                foreach (DataRow row in dataTable.Rows)
                {
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, row["Date"].ToString()));
                    tlm.Add(1, new RepString(textProp, row["Time"].ToString()));
                    tlm.Add(2, new RepString(textProp, row["Depth"].ToString()));
                    tlm.Add(3, new RepString(textProp, row["Duration"].ToString()));
                }

                verticalMillimeters += tlm.rContainerHeightMM;
                return verticalMillimeters;
            }

        }

        private double CreateSagsPage( double verticalMillimeters)
        {
            Page page = page_Cur;
            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                page = CreatePage();
                verticalMillimeters = InsertHeader(page);
            }

            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 9: Sags");

            DataTable dataTable = DataContext.Connection.RetrieveData(@"
                    Select 
	                    cast(Disturbance.StartTime as Date) as Date,
	                    cast(Disturbance.StartTime as Time) as Time,
	                    Disturbance.PerUnitMagnitude as Depth,
	                    Disturbance.DurationSeconds as Duration 
                    from 
	                    Event Join
	                    Disturbance ON Event.ID = Disturbance.EventID
                    WHERE 
	                    Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
	                    Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Sag') AND 
                        Disturbance.StartTime BETWEEN {0} AND {1} AND
	                    Event.MeterID = {2}
            ", FirstOfMonth, EndOfMonth, Meter.ID);

            Summary.Sags.Count = dataTable.Rows.Count;

            if (dataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Faults during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);
                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Date", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Time", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Depth", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Duration (seconds)", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);

                foreach (DataRow row in dataTable.Rows)
                {
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, ((DateTime)row["Date"]).ToString("MM/dd/yyyy")));
                    tlm.Add(1, new RepString(textProp, row["Time"].ToString()));
                    tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", double.Parse(row["Depth"].ToString()) * 100)));
                    tlm.Add(3, new RepString(textProp, string.Format("{0:N3}", double.Parse(row["Duration"].ToString()))));
                }

                verticalMillimeters += tlm.rContainerHeightMM;

            }

            return verticalMillimeters;

        }

        private double CreateSwellsPage( double verticalMillimeters)
        {
            Page page = page_Cur;
            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                page = CreatePage();
                verticalMillimeters = InsertHeader(page);
            }

            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 10: Swells");
            DataTable dataTable = DataContext.Connection.RetrieveData(@"
                    Select 
	                    cast(Disturbance.StartTime as Date) as Date,
	                    cast(Disturbance.StartTime as Time) as Time,
	                    Disturbance.PerUnitMagnitude as Depth,
	                    Disturbance.DurationSeconds as Duration 
                    from 
	                    Event Join
	                    Disturbance ON Event.ID = Disturbance.EventID
                    WHERE 
	                    Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
	                    Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Swell') AND 
                        Disturbance.StartTime BETWEEN {0} AND {1} AND
	                    Event.MeterID = {2}
            ", FirstOfMonth, EndOfMonth, Meter.ID);

            Summary.Swells.Count = dataTable.Rows.Count;


            if (dataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Swells during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);
                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Date", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Time", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Depth", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Duration", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);

                foreach (DataRow row in dataTable.Rows)
                {
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, row["Date"].ToString()));
                    tlm.Add(1, new RepString(textProp, row["Time"].ToString()));
                    tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", double.Parse(row["Depth"].ToString()) * 100)));
                    tlm.Add(3, new RepString(textProp, string.Format("{0:N3}", double.Parse(row["Duration"].ToString()))));
                }

                verticalMillimeters += tlm.rContainerHeightMM;

            }
            return verticalMillimeters;

        }

        private void CreateMagDurPage( )
        {
            Page page = CreatePage();
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader( page, verticalMillimeters, "Section 7: Mag-Dur Chart");

            DataTable dataTable = DataContext.Connection.RetrieveData(@"
                    Select 
	                    Disturbance.PerUnitMagnitude as Depth,
	                    Disturbance.DurationSeconds as Duration 
                    from 
	                    Event Join
	                    Disturbance ON Event.ID = Disturbance.EventID
                    WHERE 
	                    Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
	                    (
							Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Sag')  OR
							Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Swell') OR
							Event.EventTypeID = (SELECT ID FROM EventType WHERE Name ='Interruption')
						) AND 
                        Disturbance.StartTime BETWEEN {0} AND {1} AND
	                    Event.MeterID = {2}
            ", FirstOfMonth, EndOfMonth, Meter.ID);

            Chart chart = GenerateMagDurChart(dataTable.Select().Select(row => new Tuple<double, double>(double.Parse(row["Duration"].ToString()), double.Parse(row["Depth"].ToString()))).ToList(), "0.0000 s", "0.0 %");
            verticalMillimeters += 150;

            page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 150));

        }

        private double CreateFaultsPage( double verticalMillimeters)
        {
            Page page = page_Cur;
            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                page = CreatePage();
                verticalMillimeters = InsertHeader(page);
            }

            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 11: Faults");
            DataTable dataTable = DataContext.Connection.RetrieveData(@"
                SELECT 
	                Cast(FaultSummary.Inception as Date) as Date,
	                Cast(FaultSummary.Inception as Time) as Time,
	                FaultSummary.Distance, 
	                FaultSummary.DurationSeconds as Duration
                FROM
	                Event JOIN
	                FaultSummary ON Event.ID = FaultSummary.EventID
                WHERE 
	                FaultSummary.IsSelectedAlgorithm = 1 AND
	                FaultSummary.IsSuppressed = 0 AND
	                FaultSummary.IsValid = 1 AND 
                    FaultSummary.Inception BETWEEN {0} AND {1} AND
	                Event.MeterID = {2}
            ", FirstOfMonth, EndOfMonth, Meter.ID);

            Summary.Faults.Count = dataTable.Rows.Count;

            if (dataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Faults during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);
                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Date", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Time", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Distance", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);
                col = new TlmColumnMM(tlm, "Duration", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.25);

                foreach (DataRow row in dataTable.Rows)
                {
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, row["Date"].ToString()));
                    tlm.Add(1, new RepString(textProp, row["Time"].ToString()));
                    tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", double.Parse(row["Distance"].ToString()) * 100)));
                    tlm.Add(3, new RepString(textProp, string.Format("{0:N3}", double.Parse(row["Duration"].ToString()))));
                }

                verticalMillimeters += tlm.rContainerHeightMM;

            }
            return verticalMillimeters;

        }

        private void CreateSummaryPage(Page page)
        {
            double verticalMillimeters = InsertHeader(page);
            verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Summary of Results");

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);
                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Measurement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.35);
                col = new TlmColumnMM(tlm, "Min", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.15);
                col = new TlmColumnMM(tlm, "Avg", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.15);
                col = new TlmColumnMM(tlm, "Max", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.15);
                col = new TlmColumnMM(tlm, "Compliance", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"1. Frequency ({Summary.Frequency.Nominal} Hz)"));
                tlm.Add(1, new RepString(textProp, string.Format("{0:N2}", Summary.Frequency.Min)));
                tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", Summary.Frequency.Avg)));
                tlm.Add(3, new RepString(textProp, string.Format("{0:N2}", Summary.Frequency.Max)));
                tlm.Add(4, new RepString(textProp, Summary.Frequency.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"2a. Voltage L-N ({string.Format("{0:N0}", Summary.VoltageLN.Nominal)} V)"));
                tlm.Add(1, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLN.Min)));
                tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLN.Avg)));
                tlm.Add(3, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLN.Max)));
                tlm.Add(4, new RepString(textProp, Summary.VoltageLN.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"2b. Voltage L-L ({string.Format("{0:N0}", Summary.VoltageLL.Nominal)} V)"));
                tlm.Add(1, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLL.Min)));
                tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLL.Avg)));
                tlm.Add(3, new RepString(textProp, string.Format("{0:N2}", Summary.VoltageLL.Max)));
                tlm.Add(4, new RepString(textProp, Summary.VoltageLL.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"3. Flicker"));
                tlm.Add(1, new RepString(textProp, ""));
                tlm.Add(2, new RepString(textProp, ""));
                tlm.Add(3, new RepString(textProp, string.Format("{0:N2}", Summary.Flicker.Max)));
                tlm.Add(4, new RepString(textProp, Summary.Flicker.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"4. Imbalance"));
                tlm.Add(1, new RepString(textProp, ""));
                tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", Summary.Imbalance.Avg)));
                tlm.Add(3, new RepString(textProp, ""));
                tlm.Add(4, new RepString(textProp, Summary.Imbalance.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"5. THD"));
                tlm.Add(1, new RepString(textProp, string.Format("{0:N2}", Summary.THD.Min)));
                tlm.Add(2, new RepString(textProp, string.Format("{0:N2}", Summary.THD.Avg)));
                tlm.Add(3, new RepString(textProp, string.Format("{0:N2}", Summary.THD.Max)));
                tlm.Add(4, new RepString(textProp, Summary.THD.Compliance));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"6. Harmonics"));
                tlm.Add(1, new RepString(textProp, ""));
                tlm.Add(2, new RepString(textProp, ""));
                tlm.Add(3, new RepString(textProp, ""));
                tlm.Add(4, new RepString(textProp, Summary.Harmonics.Compliance));

                verticalMillimeters += tlm.rContainerHeightMM;

            }

            verticalMillimeters += 20;

            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.rContainerHeightMM = 3 * 12;  // set height of table
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) => NextTablePage(page, verticalMillimeters, ea);
                // define columns
                TlmColumn col;
                col = new TlmColumnMM(tlm, "Event Type", (PageWidthMillimeters / 2 - PageMarginMillimeters) * 0.65);
                col = new TlmColumnMM(tlm, "Count", (PageWidthMillimeters / 2 - PageMarginMillimeters) * 0.35);

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"8. Interruptions"));
                tlm.Add(1, new RepString(textProp, string.Format("{0}", Summary.Interruptions.Count)));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"9. Sags"));
                tlm.Add(1, new RepString(textProp, string.Format("{0}", Summary.Sags.Count)));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"10. Swells"));
                tlm.Add(1, new RepString(textProp, string.Format("{0}", Summary.Swells.Count)));

                tlm.NewRow();
                tlm.Add(0, new RepString(textProp, $"11. Faults"));
                tlm.Add(1, new RepString(textProp, string.Format("{0}", Summary.Faults.Count)));


                verticalMillimeters += tlm.rContainerHeightMM;

            }

        }

        // Creates a page and sets width and height to standard 8.5x11 inches.
        private Page CreatePage()
        {
            Page page = new Page(this);
            page.rWidthMM = PageWidthMillimeters;
            page.rHeightMM = PageHeightMillimeters;
            return page;
        }

        // Inserts the title and company text on the given page.
        private double InsertTitle(Page page, double verticalMillimeters)
        {
            FontProp titleFont = new FontProp(FontDefinition, 0.0D);
            FontProp companyFont = new FontProp(FontDefinition, 0.0D);

            titleFont.rSizePoint = 20.0D;
            companyFont.rSizePoint = 14.0D;
            titleFont.bBold = true;
            companyFont.bBold = true;

            // Title
            page.AddCB_MM(verticalMillimeters + titleFont.rSizeMM, new RepString(titleFont, TitleText));
            verticalMillimeters += 1.5D * titleFont.rSizeMM;
            verticalMillimeters += 5;

            // Company
            page.AddCB_MM(verticalMillimeters + companyFont.rSizeMM, new RepString(companyFont, $"{DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'CompanyName'")}"));

            verticalMillimeters += 1.5D * titleFont.rSizeMM;

            // openXDA
            page.AddCB_MM(verticalMillimeters + companyFont.rSizeMM, new RepString(companyFont, $"openXDA"));

            verticalMillimeters += 1.5D * titleFont.rSizeMM;

            // Date
            page.AddCB_MM(verticalMillimeters + companyFont.rSizeMM, new RepString(companyFont, $"{FirstOfMonth.ToString("MMMM yyyy")}"));

            verticalMillimeters += 1.5D * titleFont.rSizeMM;

            // Meter Name
            page.AddCB_MM(verticalMillimeters + companyFont.rSizeMM, new RepString(companyFont, $"{Meter.Name} - {Result}"));

            verticalMillimeters += 1.5D * companyFont.rSizeMM;


            return verticalMillimeters;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertFooter(Page page)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;
            page.AddMM(PageWidthMillimeters - PageMarginMillimeters - font.rGetTextWidthMM(page.iPageNo.ToString()), PageHeightMillimeters - 15, new RepString(font, page.iPageNo.ToString()));
            return font.rSizeMM;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertHeader(Page page)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;

            int height = 6;

            page.AddMM(PageMarginMillimeters, height, new RepString(font, Meter.Name));
            page.AddMM(PageWidthMillimeters / 2 - font.rGetTextWidthMM("PQ Report - openXDA") / 2, height, new RepString(font, "PQ Report - openXDA"));
            page.AddMM(PageWidthMillimeters - PageMarginMillimeters - font.rGetTextWidthMM(FirstOfMonth.ToString("MMMM yyyy")), height, new RepString(font, FirstOfMonth.ToString("MMMM yyyy")));
            page.AddMM(0, 10, new RepRectMM(new BrushProp(this, Color.Black), PageWidthMillimeters, 0.1D));

            return 20;
        }


        // Inserts the given text as a section header (16-pt, bold).
        private double InsertSectionHeader(Page page, double verticalMillimeters, string text)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 14.0D;
            font.bBold = true;
            page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepString(font, text));
            return font.rSizeMM + 5;
        }

        // Inserts the given text as a section header (16-pt, bold).
        private double InsertItalicText(Page page, double verticalMillimeters, string text)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 14.0D;
            font.bBold = false;
            font.bItalic = true;
            page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepString(font, text));
            return font.rSizeMM + 5;
        }

        // Inserts the given text as a section header (16-pt, bold).
        private double InsertNominal(Page page, double verticalMillimeters, string type, string value, string units)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 10.0D;
            page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepString(font, $"Nominal {type}: {value} {units}"));
            return font.rSizeMM + 5;
        }

        private void NextTablePage(Page page,  double verticalMillimeters, TlmBase.NewContainerEventArgs ea)
        {

            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                page = CreatePage();
                verticalMillimeters = InsertHeader(page);
                verticalMillimeters += InsertSectionHeader(page, verticalMillimeters, "Section 8: Sags (cont.)");

            }
            ea.container.rHeightMM = PageHeightMillimeters - verticalMillimeters - PageMarginMillimeters;
            page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
            verticalMillimeters += ea.container.rHeightMM;
        }

        private double InsertFrequencyPage(Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Frequency') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", Meter.ID);

            if (!channels.Any())
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Frequency channels setup on this meter.");
                Summary.Frequency.Compliance = "Pass";
                return verticalMillimeters;
            }


            double nominal = Summary.Frequency.Nominal = 60.0D;
            double testOneMultiplier = 0.003;
            double testTwoMultiplier = 0.005;

            double lowThreshOne = nominal - (nominal * testOneMultiplier);
            double highThreshOne = nominal + (nominal * testOneMultiplier);
            double lowThreshTwo = nominal - (nominal * testTwoMultiplier);
            double highThreshTwo = nominal + (nominal * testTwoMultiplier);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> points = historian.Read(channels.Select(x => x.ID), FirstOfMonth, EndOfMonth).ToList();
                if (!points.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Frequency data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    return verticalMillimeters;
                }

                List<openHistorian.XDALink.TrendingDataPoint> avg = points.Where(point => point.SeriesID == SeriesID.Average).OrderBy(point => point.Value).ToList();

                double maxValue = Summary.Frequency.Max = points.Where(point => point.SeriesID == SeriesID.Maximum).Select(point => point.Value).Max();
                double minValue = Summary.Frequency.Min = points.Where(point => point.SeriesID == SeriesID.Minimum).Select(point => point.Value).Min();
                double avgValue = Summary.Frequency.Avg = avg.Select(point => point.Value).Average();

                bool testOne = (double)avg.Where(x => x.Value >= lowThreshOne && x.Value <= highThreshOne).Count() / (avg.Any() ? avg.Count() : 1) > 0.99;
                bool testTwo = (double)avg.Where(x => x.Value >= lowThreshTwo && x.Value <= highThreshTwo).Count() / (avg.Any() ? avg.Count() : 1) > 0.9995;

                Summary.Frequency.Compliance = (testOne && testTwo ? "Pass" : "Fail");
                int oneCount = (avg.Count() - (int)(avg.Count() * 0.992)) / 2;
                List<double> oneList = avg.Where((point, i) => i >= oneCount && i <= (avg.Count() - oneCount)).Select(point => point.Value).ToList();
                int twoCount = (avg.Count() - (int)(avg.Count() * 0.9995)) / 2;
                List<double> twoList = avg.Where((point, i) => i >= twoCount && i <= (avg.Count() - twoCount)).Select(point => point.Value).ToList();

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.40);
                    col = new TlmColumnMM(tlm, "Measured Frequency", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.40);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99% of the time: {string.Format("{0:N2}", lowThreshOne)} Hz - {string.Format("{0:N2}", highThreshOne)} Hz"));
                    tlm.Add(1, new RepString(textProp, (avg.Any() ? $"{string.Format("{0:N2}", oneList.First())} - {string.Format("{0:N2}", oneList.Last())}" : "No Data Provided")));
                    tlm.Add(2, new RepString(textProp, (testOne ? "Pass" : "Fail")));
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99.95% of the time: {string.Format("{0:N2}", lowThreshTwo)} Hz - {string.Format("{0:N2}", highThreshTwo)} Hz"));
                    tlm.Add(1, new RepString(textProp, (avg.Any() ? $"{string.Format("{0:N2}", twoList.First())} - {string.Format("{0:N2}", twoList.Last())}" : "No Data Provided")));
                    tlm.Add(2, new RepString(textProp, (testTwo ? "Pass" : "Fail")));
                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "Frequency",
                    Color = Color.DarkBlue,
                    Data = points.Where(point => point.SeriesID == SeriesID.Average).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "Frequency Max",
                    Color = Color.DarkGreen,
                    Data = points.Where(point => point.SeriesID == SeriesID.Maximum).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "Frequency Min",
                    Color = Color.Purple,
                    Data = points.Where(point => point.SeriesID == SeriesID.Minimum).ToList()
                });

                Chart chart = GenerateLineChart("Hz", pointGroups, highThreshTwo + (highThreshTwo - lowThreshTwo) * .10, lowThreshTwo - (highThreshTwo - lowThreshTwo) * .10, "MM/dd", "0.00", "Frequency", highThreshTwo, highThreshOne, lowThreshTwo, lowThreshOne, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                verticalMillimeters += 75;

                chart = GenerateBarChart("0.00 Hz", avg.Select(x => x.Value), maxValue, minValue);

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                return verticalMillimeters;

            }

        }

        private double InsertVoltageLNPage( Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            Channel line1 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'AN')", Meter.ID);
            Channel line2 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'BN')", Meter.ID);
            Channel line3 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'CN')", Meter.ID);

            int lineId;
            if (line1 == null && line2 == null && line3 == null)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No L-N Voltage channels setup on this meter.");
                Summary.VoltageLN.Compliance = "Pass";
                return verticalMillimeters;
            }
            else if (line1 != null) lineId = line1.LineID;
            else if (line2 != null) lineId = line2.LineID;
            else lineId = line3.LineID;

            double nominal = Summary.VoltageLN.Nominal = DataContext.Connection.ExecuteScalar<double>("SELECT VoltageKV FROM Line WHERE ID = {0}", lineId) * 1000 / Math.Sqrt(3);
            verticalMillimeters += InsertNominal(page, verticalMillimeters, "Voltage", string.Format("{0:N0}", nominal), "V L-N");

            double testOneMultiplier = 0.003;
            double testTwoMultiplier = 0.005;

            double lowThreshOne = nominal - (nominal * testOneMultiplier);
            double highThreshOne = nominal + (nominal * testOneMultiplier);
            double lowThreshTwo = nominal - (nominal * testTwoMultiplier);
            double highThreshTwo = nominal + (nominal * testTwoMultiplier);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new List<int>() { line1.ID, line2.ID, line3.ID }, FirstOfMonth, EndOfMonth).ToList();
                List<openHistorian.XDALink.TrendingDataPoint> avg = data.Where(x => x.SeriesID == SeriesID.Average).ToList();

                if (!avg.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No L-N Voltage data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    Summary.VoltageLN.Compliance = "Pass";
                    return verticalMillimeters;
                }

                Summary.VoltageLN.Min = data.Where(point => point.SeriesID == SeriesID.Minimum).Select(point => point.Value).Min();
                Summary.VoltageLN.Avg = data.Where(point => point.SeriesID == SeriesID.Average).Select(point => point.Value).Average();
                Summary.VoltageLN.Max = data.Where(point => point.SeriesID == SeriesID.Maximum).Select(point => point.Value).Max();


                int oneCount = avg.Where(point => point.ChannelID == line1.ID).Count();
                int oneIndex99 = (oneCount - (int)(oneCount * 0.99)) / 2;
                int oneIndex9995 = (oneCount - (int)(oneCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> oneData = avg.Where((point, index) => point.ChannelID == line1.ID).ToList();
                double? line1value99High = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex99 && index <= oneCount - oneIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line1value99Low = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex99 && index <= oneCount - oneIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line1value9995High = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex9995 && index <= oneCount - oneIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line1value9995Low = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex9995 && index <= oneCount - oneIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test199 = (double)oneData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (oneData.Any() ? oneData.Count() : 1) > 0.99;
                bool test19995 = (double)oneData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (oneData.Any() ? oneData.Count() : 1) > 0.9995;

                int twoCount = avg.Where(point => point.ChannelID == line2.ID).Count();
                int TwoIndex99 = (twoCount - (int)(twoCount * 0.99)) / 2;
                int TwoIndex9995 = (twoCount - (int)(twoCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> twoData = avg.Where((point, index) => point.ChannelID == line2.ID).ToList();
                double? line2value99High = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex99 && index <= twoCount - TwoIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line2value99Low = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex99 && index <= twoCount - TwoIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line2value9995High = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex9995 && index <= twoCount - TwoIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line2value9995Low = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex9995 && index <= twoCount - TwoIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test299 = (double)twoData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (twoData.Any() ? twoData.Count() : 1) > 0.99;
                bool test29995 = (double)twoData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (twoData.Any() ? twoData.Count() : 1) > 0.9995;

                int threeCount = avg.Where(point => point.ChannelID == line3.ID).Count();
                int threeIndex99 = (threeCount - (int)(threeCount * 0.99)) / 2;
                int threeIndex9995 = (threeCount - (int)(threeCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> threeData = avg.Where((point, index) => point.ChannelID == line3.ID).ToList();
                double? line3value99High = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex99 && index <= threeCount - threeIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line3value99Low = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex99 && index <= threeCount - threeIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line3value9995High = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex9995 && index <= threeCount - threeIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line3value9995Low = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex9995 && index <= threeCount - threeIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test399 = (double)threeData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (threeData.Any() ? threeData.Count() : 1) > 0.99;
                bool test39995 = (double)threeData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (threeData.Any() ? threeData.Count() : 1) > 0.9995;

                bool test1 = test199 && test299 && test399;
                bool test2 = test19995 && test29995 && test39995;

                Summary.VoltageLN.Compliance = (test1 && test2 ? "Pass" : "Fail");

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.30);
                    col = new TlmColumnMM(tlm, "Measured L1 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L2 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L3 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.10);

                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99% of the time: {string.Format("{0:N1}", lowThreshOne)} V - {string.Format("{0:N1}", highThreshOne)} V"));
                    tlm.Add(1, new RepString(textProp, (line1value99Low != null ? $"{string.Format("{0:N1}", line1value99Low)}V - {string.Format("{0:N1}", line1value99High)}V" : "No data for time period")));
                    tlm.Add(2, new RepString(textProp, (line2value99Low != null ? $"{string.Format("{0:N1}", line2value99Low)}V - {string.Format("{0:N1}", line2value99High)}V" : "No data for time period")));
                    tlm.Add(3, new RepString(textProp, (line3value99Low != null ? $"{string.Format("{0:N1}", line3value99Low)}V - {string.Format("{0:N1}", line3value99High)}V" : "No data for time period")));
                    tlm.Add(4, new RepString(textProp, (test1 ? "Pass" : "Fail")));

                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99% of the time: {string.Format("{0:N1}", lowThreshOne)} V - {string.Format("{0:N1}", highThreshOne)} V"));
                    tlm.Add(1, new RepString(textProp, (line1value9995Low != null ? $"{string.Format("{0:N1}", line1value9995Low)}V - {string.Format("{0:N1}", line1value9995High)}V" : "No data for time period")));
                    tlm.Add(2, new RepString(textProp, (line2value9995Low != null ? $"{string.Format("{0:N1}", line2value9995Low)}V - {string.Format("{0:N1}", line2value9995High)}V" : "No data for time period")));
                    tlm.Add(3, new RepString(textProp, (line3value9995Low != null ? $"{string.Format("{0:N1}", line3value9995Low)}V - {string.Format("{0:N1}", line3value9995High)}V" : "No data for time period")));
                    tlm.Add(4, new RepString(textProp, (test2 ? "Pass" : "Fail")));

                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "L1-N",
                    Color = Color.DarkBlue,
                    Data = oneData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2-N",
                    Color = Color.DarkGreen,
                    Data = twoData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3-N",
                    Color = Color.Purple,
                    Data = threeData
                });

                double maxValue = avg.Select(point => point.Value).Max();
                double minValue = avg.Select(point => point.Value).Min();

                double chartHigh = (maxValue > highThreshTwo ? maxValue : highThreshTwo);
                double chartLow = (minValue < lowThreshTwo ? minValue : lowThreshTwo);

                double chartMax = chartHigh + (chartHigh - chartLow) * .10;
                double chartMin = chartLow - (chartHigh - chartLow) * .10;

                Chart chart = GenerateLineChart("V", pointGroups, chartMax, chartMin, "MM/dd", "0.00", "Voltage L-L", highThreshTwo, highThreshOne, lowThreshTwo, lowThreshOne, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                int numBuckets = 45;
                double step = (maxValue - minValue) / numBuckets;
                verticalMillimeters += 75;

                chart = GenerateBarChart("0", avg.Select(x => x.Value), maxValue, minValue);
                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));



                return verticalMillimeters;

            }
        }

        private double InsertVoltageLLPage(Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", Meter.ID);

            Channel line1 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'AB')", Meter.ID);
            Channel line2 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'BC')", Meter.ID);
            Channel line3 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'CA')", Meter.ID);

            int lineId;
            if (line1 == null && line2 == null && line3 == null)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No L-L Voltage channels setup on this meter.");
                Summary.VoltageLL.Compliance = "Pass";
                return verticalMillimeters;
            }
            else if (line1 != null) lineId = line1.LineID;
            else if (line2 != null) lineId = line2.LineID;
            else lineId = line3.LineID;

            double nominal = Summary.VoltageLL.Nominal = DataContext.Connection.ExecuteScalar<double>("SELECT VoltageKV FROM Line WHERE ID = {0}", lineId) * 1000;
            verticalMillimeters += InsertNominal(page, verticalMillimeters, "Voltage", string.Format("{0:N0}", nominal), "V L-L");

            double testOneMultiplier = 0.003;
            double testTwoMultiplier = 0.005;

            double lowThreshOne = nominal - (nominal * testOneMultiplier);
            double highThreshOne = nominal + (nominal * testOneMultiplier);
            double lowThreshTwo = nominal - (nominal * testTwoMultiplier);
            double highThreshTwo = nominal + (nominal * testTwoMultiplier);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new List<int>() { line1.ID, line2.ID, line3.ID }, FirstOfMonth, EndOfMonth).ToList();
                List<openHistorian.XDALink.TrendingDataPoint> avg = data.Where(x => x.SeriesID == SeriesID.Average).ToList();

                if (!avg.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No L-L Voltage data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    return verticalMillimeters;
                }

                Summary.VoltageLL.Min = data.Where(point => point.SeriesID == SeriesID.Minimum).Select(point => point.Value).Min();
                Summary.VoltageLL.Avg = data.Where(point => point.SeriesID == SeriesID.Average).Select(point => point.Value).Average();
                Summary.VoltageLL.Max = data.Where(point => point.SeriesID == SeriesID.Maximum).Select(point => point.Value).Max();

                int oneCount = avg.Where(point => point.ChannelID == line1.ID).Count();
                int oneIndex99 = (oneCount - (int)(oneCount * 0.99)) / 2;
                int oneIndex9995 = (oneCount - (int)(oneCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> oneData = avg.Where((point, index) => point.ChannelID == line1.ID).ToList();
                double? line1value99High = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex99 && index <= oneCount - oneIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line1value99Low = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex99 && index <= oneCount - oneIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line1value9995High = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex9995 && index <= oneCount - oneIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line1value9995Low = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex9995 && index <= oneCount - oneIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test199 = (double)oneData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (oneData.Any() ? oneData.Count() : 1) > 0.99;
                bool test19995 = (double)oneData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (oneData.Any() ? oneData.Count() : 1) > 0.9995;

                int twoCount = avg.Where(point => point.ChannelID == line2.ID).Count();
                int TwoIndex99 = (twoCount - (int)(twoCount * 0.99)) / 2;
                int TwoIndex9995 = (twoCount - (int)(twoCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> twoData = avg.Where((point, index) => point.ChannelID == line2.ID).ToList();
                double? line2value99High = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex99 && index <= twoCount - TwoIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line2value99Low = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex99 && index <= twoCount - TwoIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line2value9995High = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex9995 && index <= twoCount - TwoIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line2value9995Low = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => index >= TwoIndex9995 && index <= twoCount - TwoIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test299 = (double)twoData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (twoData.Any() ? twoData.Count() : 1) > 0.99;
                bool test29995 = (double)twoData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (twoData.Any() ? twoData.Count() : 1) > 0.9995;

                int threeCount = avg.Where(point => point.ChannelID == line3.ID).Count();
                int threeIndex99 = (threeCount - (int)(threeCount * 0.99)) / 2;
                int threeIndex9995 = (threeCount - (int)(threeCount * 0.99995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> threeData = avg.Where((point, index) => point.ChannelID == line3.ID).ToList();
                double? line3value99High = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex99 && index <= threeCount - threeIndex99).Select(point => point.Value).Max() : (double?)null);
                double? line3value99Low = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex99 && index <= threeCount - threeIndex99).Select(point => point.Value).Min() : (double?)null);
                double? line3value9995High = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex9995 && index <= threeCount - threeIndex9995).Select(point => point.Value).Max() : (double?)null);
                double? line3value9995Low = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex9995 && index <= threeCount - threeIndex9995).Select(point => point.Value).Min() : (double?)null);
                bool test399 = (double)threeData.Where(point => point.Value >= lowThreshOne && point.Value <= highThreshOne).Count() / (threeData.Any() ? threeData.Count() : 1) > 0.99;
                bool test39995 = (double)threeData.Where(point => point.Value >= lowThreshTwo && point.Value <= highThreshTwo).Count() / (threeData.Any() ? threeData.Count() : 1) > 0.9995;

                bool test1 = test199 && test299 && test399;
                bool test2 = test19995 && test29995 && test39995;

                Summary.VoltageLL.Compliance = (test1 && test2 ? "Pass" : "Fail");

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.30);
                    col = new TlmColumnMM(tlm, "Measured L1 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L2 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L3 Voltage", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.10);

                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99% of the time: {string.Format("{0:N2}", lowThreshOne)} V - {string.Format("{0:N2}", highThreshOne)} V"));
                    tlm.Add(1, new RepString(textProp, (line1value99Low != null ? $"{string.Format("{0:N2}", line1value99Low)}V - {string.Format("{0:N2}", line1value99High)}V" : "No data for time period")));
                    tlm.Add(2, new RepString(textProp, (line2value99Low != null ? $"{string.Format("{0:N2}", line2value99Low)}V - {string.Format("{0:N2}", line2value99High)}V" : "No data for time period")));
                    tlm.Add(3, new RepString(textProp, (line3value99Low != null ? $"{string.Format("{0:N2}", line3value99Low)}V - {string.Format("{0:N2}", line3value99High)}V" : "No data for time period")));
                    tlm.Add(4, new RepString(textProp, (test1 ? "Pass" : "Fail")));

                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99.95% of the time: {string.Format("{0:N2}", lowThreshTwo)} V - {string.Format("{0:N2}", highThreshTwo)} V"));
                    tlm.Add(1, new RepString(textProp, (line1value9995Low != null ? $"{string.Format("{0:N2}", line1value9995Low)}V - {string.Format("{0:N2}", line1value9995High)}V" : "No data for time period")));
                    tlm.Add(2, new RepString(textProp, (line2value9995Low != null ? $"{string.Format("{0:N2}", line2value9995Low)}V - {string.Format("{0:N2}", line2value9995High)}V" : "No data for time period")));
                    tlm.Add(3, new RepString(textProp, (line3value9995Low != null ? $"{string.Format("{0:N2}", line3value9995Low)}V - {string.Format("{0:N2}", line3value9995High)}V" : "No data for time period")));
                    tlm.Add(4, new RepString(textProp, (test2 ? "Pass" : "Fail")));

                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "L1-L2",
                    Color = Color.DarkBlue,
                    Data = oneData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2-L3",
                    Color = Color.DarkGreen,
                    Data = twoData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3-L1",
                    Color = Color.Purple,
                    Data = threeData
                });

                double maxValue = avg.Select(point => point.Value).Max();
                double minValue = avg.Select(point => point.Value).Min();

                double chartHigh = (maxValue > highThreshTwo ? maxValue : highThreshTwo);
                double chartLow = (minValue < lowThreshTwo ? minValue : lowThreshTwo);

                double chartMax = chartHigh + (chartHigh - chartLow) * .10;
                double chartMin = chartLow - (chartHigh - chartLow) * .10;

                Chart chart = GenerateLineChart("V", pointGroups, chartMax, chartMin, "MM/dd", "0.00", "Voltage L-L", highThreshTwo, highThreshOne, lowThreshTwo, lowThreshOne, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                verticalMillimeters += 75;

                chart = GenerateBarChart("0", avg.Select(x => x.Value), maxValue, minValue);
                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));



                return verticalMillimeters;

            }
        }

        private double InsertFlickerPage(Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            Channel line1 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'AN')", Meter.ID);
            Channel line2 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'BN')", Meter.ID);
            Channel line3 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'CN')", Meter.ID);

            if (line1 == null && line2 == null && line3 == null)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Flicker channels setup on this meter.");
                Summary.Flicker.Compliance = "Pass";
                return verticalMillimeters;
            }

            double nominal = 1;

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> max = historian.Read(new List<int>() { line1.ID, line2.ID, line3.ID }, FirstOfMonth, EndOfMonth).Where(x => x.SeriesID == SeriesID.Maximum).ToList();
                if (!max.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Flicker data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    return verticalMillimeters;
                }

                Summary.Flicker.Max = max.Select(point => point.Value).Max();


                int oneCount = max.Where(point => point.ChannelID == line1.ID).Count();
                int oneIndex = (oneCount - (int)(oneCount * 0.985)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> oneData = max.Where((point, index) => point.ChannelID == line1.ID).ToList();
                double line1value = oneData.OrderBy(point => point.Value).Where((point, index) => index >= oneIndex && index <= oneCount - oneIndex).Select(point => point.Value).Max();
                bool test1 = (double)oneData.Where(point => point.Value <= nominal).Count() / (oneData.Any() ? oneData.Count() : 1) > 0.985;

                int twoCount = max.Where(point => point.ChannelID == line2.ID).Count();
                int twoIndex = (twoCount - (int)(twoCount * 0.985)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> twoData = max.Where((point, index) => point.ChannelID == line2.ID).ToList();
                double line2value = twoData.OrderBy(point => point.Value).Where((point, index) => index >= twoIndex && index <= twoCount - twoIndex).Select(point => point.Value).Max();
                bool test2 = (double)twoData.Where(point => point.Value <= nominal).Count() / (twoData.Any() ? twoData.Count() : 1) > 0.985;

                int threeCount = max.Where(point => point.ChannelID == line3.ID).Count();
                int threeIndex = (threeCount - (int)(threeCount * 0.985)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> threeData = max.Where((point, index) => point.ChannelID == line3.ID).ToList();
                double line3value = threeData.OrderBy(point => point.Value).Where((point, index) => index >= threeIndex && index <= threeCount - threeIndex).Select(point => point.Value).Max();
                bool test3 = (double)threeData.Where(point => point.Value <= nominal).Count() / (threeData.Any() ? threeData.Count() : 1) > 0.985;

                bool test = test1 && test2 && test3;

                Summary.Flicker.Compliance = (test ? "Pass" : "Fail");

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.30);
                    col = new TlmColumnMM(tlm, "Measured L1 Plt", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L2 Plt", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L3 Plt", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.10);
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, "98.50% of the time: Plt <= 1"));
                    tlm.Add(1, new RepString(textProp, $"{string.Format("{0:N2}", line1value)}"));
                    tlm.Add(2, new RepString(textProp, $"{string.Format("{0:N2}", line2value)}"));
                    tlm.Add(3, new RepString(textProp, $"{string.Format("{0:N2}", line3value)}"));
                    tlm.Add(4, new RepString(textProp, (test ? "Pass" : "Fail")));
                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "L1Max",
                    Color = Color.DarkBlue,
                    Data = oneData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2Max",
                    Color = Color.DarkGreen,
                    Data = twoData
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3Max",
                    Color = Color.Purple,
                    Data = threeData
                });

                double maxValue = max.Select(point => point.Value).Max();
                Chart chart = GenerateLineChart("", pointGroups, maxValue * 1.1, 0, "MM/dd", "0.00", "Flicker", 1, null, null, null, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                int numBuckets = 45;
                double step = maxValue / numBuckets;
                verticalMillimeters += 75;

                chart = GenerateBarChart("0.00", max.Select(x => x.Value), maxValue, 0);
                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));



                return verticalMillimeters;

            }
        }

        private double InsertImbalancePage(Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            Channel channel = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AvgImbal') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", Meter.ID);

            if (channel == null)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Imbalance channel setup on this meter.");
                Summary.Imbalance.Compliance = "Pass";
                return verticalMillimeters;
            }

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> avg = historian.Read(new List<int>() { channel.ID }, FirstOfMonth, EndOfMonth).Where(x => x.SeriesID == SeriesID.Average).ToList();
                if (!avg.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No Imbalance data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    return verticalMillimeters;
                }

                Summary.Imbalance.Avg = avg.Select(point => point.Value).Average();

                int count = avg.Count();
                int index = (count - (int)(count * 0.9995)) / 2;
                double value = avg.OrderBy(point => point.Value).Where((point, i) => i >= index && i <= count - index).Select(point => point.Value).Max();
                double minValue = avg.OrderBy(point => point.Value).Where((point, i) => i >= index && i <= count - index).Select(point => point.Value).Min();
                bool test = (double)avg.Where(point => point.Value >= 0 && point.Value <= 2).Count() / (avg.Any() ? avg.Count() : 1) > 0.9995;

                Summary.Imbalance.Compliance = (test ? "Pass" : "Fail");

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.40);
                    col = new TlmColumnMM(tlm, "Measured Unbalance u2", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.40);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, "99.95% of the time: 0% ~ 2% u2"));
                    tlm.Add(1, new RepString(textProp, $"{string.Format("{0:N2}", value)}"));
                    tlm.Add(2, new RepString(textProp, (test ? "Pass" : "Fail")));
                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "Unbalance",
                    Color = Color.DarkBlue,
                    Data = avg
                });

                Chart chart = GenerateLineChart("", pointGroups, (value > 2 ? value : 2) * 1.1, (minValue < 0 ? minValue * 0.9 : 0), "MM/dd", "0.00", "Flicker", 2, null, 0, null, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                int numBuckets = 45;
                double step = (value - minValue) / numBuckets;
                verticalMillimeters += 75;

                chart = GenerateBarChart("0.00", avg.Select(x => x.Value), value, minValue);
                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));



                return verticalMillimeters;

            }
        }

        private double InsertTHDPage(Page page, double verticalMillimeters)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            Channel line1 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'AN')", Meter.ID);
            Channel line2 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'BN')", Meter.ID);
            Channel line3 = DataContext.Table<Channel>().QueryRecordWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'CN')", Meter.ID);

            if (line1 == null && line2 == null && line3 == null)
            {
                verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No THD channels setup on this meter.");
                Summary.THD.Compliance = "Pass";
                return verticalMillimeters;
            }

            double nominal = 8;

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new List<int>() { line1.ID, line2.ID, line3.ID }, FirstOfMonth, EndOfMonth).ToList();

                if (!data.Any())
                {
                    verticalMillimeters += InsertItalicText(page, verticalMillimeters, $"   No THD data during {FirstOfMonth.ToString("MM/dd/yyyy")} - {EndOfMonth.ToString("MM/dd/yyyy")}");
                    return verticalMillimeters;
                }

                Summary.THD.Min = data.Where(point => point.SeriesID == SeriesID.Minimum).Select(point => point.Value).Min();
                Summary.THD.Avg = data.Where(point => point.SeriesID == SeriesID.Average).Select(point => point.Value).Average();
                Summary.THD.Max = data.Where(point => point.SeriesID == SeriesID.Maximum).Select(point => point.Value).Max();

                int oneCount = data.Where(point => point.SeriesID == SeriesID.Maximum && point.ChannelID == line1.ID).Count();
                int oneIndex = (oneCount - (int)(oneCount * 0.9995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> oneData = data.Where((point, index) => point.ChannelID == line1.ID).ToList();
                double? line1valueHigh = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= oneIndex && index <= oneCount - oneIndex).Select(point => point.Value).Max() : (double?)null);
                double? line1valueLow = (oneData.Any() ? oneData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= oneIndex && index <= oneCount - oneIndex).Select(point => point.Value).Min() : (double?)null);
                bool test1 = (double)oneData.Where(point => point.SeriesID == SeriesID.Maximum && point.Value <= nominal).Count() / (oneData.Where(point => point.SeriesID == SeriesID.Maximum).Any() ? oneData.Where(point => point.SeriesID == SeriesID.Maximum).Count() : 1) > 0.9995;

                int twoCount = data.Where(point => point.SeriesID == SeriesID.Maximum && point.ChannelID == line2.ID).Count();
                int TwoIndex = (twoCount - (int)(twoCount * 0.9995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> twoData = data.Where((point, index) => point.ChannelID == line2.ID).ToList();
                double? line2valueHigh = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= TwoIndex && index <= twoCount - TwoIndex).Select(point => point.Value).Max() : (double?)null);
                double? line2valueLow = (twoData.Any() ? twoData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= TwoIndex && index <= twoCount - TwoIndex).Select(point => point.Value).Min() : (double?)null);
                bool test2 = (double)twoData.Where(point => point.SeriesID == SeriesID.Maximum && point.Value <= nominal).Count() / (twoData.Where(point => point.SeriesID == SeriesID.Maximum).Any() ? twoData.Where(point => point.SeriesID == SeriesID.Maximum).Count() : 1) > 0.9995;

                int threeCount = data.Where(point => point.SeriesID == SeriesID.Maximum && point.ChannelID == line3.ID).Count();
                int threeIndex = (threeCount - (int)(threeCount * 0.9995)) / 2;
                List<openHistorian.XDALink.TrendingDataPoint> threeData = data.Where((point, index) => point.ChannelID == line3.ID).ToList();
                double? line3valueHigh = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= threeIndex && index <= threeCount - threeIndex).Select(point => point.Value).Max() : (double?)null);
                double? line3valueLow = (threeData.Any() ? threeData.OrderBy(point => point.Value).Where((point, index) => point.SeriesID == SeriesID.Maximum && index >= threeIndex && index <= threeCount - threeIndex).Select(point => point.Value).Min() : (double?)null);
                bool test3 = (double)threeData.Where(point => point.SeriesID == SeriesID.Maximum && point.Value <= nominal).Count() / (threeData.Where(point => point.SeriesID == SeriesID.Maximum).Any() ? threeData.Where(point => point.SeriesID == SeriesID.Maximum).Count() : 1) > 0.9995;

                bool test = test1 && test2 && test3;

                Summary.THD.Compliance = (test ? "Pass" : "Fail");

                FontProp headerProp = new FontProp(FontDefinition, 0);
                headerProp.rSizePoint = 10.0D;
                using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
                {
                    FontProp textProp = new FontProp(FontDefinition, 0);
                    textProp.rSizePoint = 8.0D;
                    tlm.rContainerHeightMM = 3 * 12;  // set height of table
                    tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                    tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                    tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                    tlm.eNewContainer += (oSender, ea) => page.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);
                    // define columns
                    TlmColumn col;
                    col = new TlmColumnMM(tlm, "Monthly PQ Report Requirement", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.30);
                    col = new TlmColumnMM(tlm, "Measured L1 THD", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L2 THD", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Measured L3 THD", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.20);
                    col = new TlmColumnMM(tlm, "Result", (PageWidthMillimeters - 2 * PageMarginMillimeters) * 0.10);

                    tlm.NewRow();
                    tlm.Add(0, new RepString(textProp, $"99.95% of the time: THD <= 8%"));
                    tlm.Add(1, new RepString(textProp, (line1valueLow != null ? $"{string.Format("{0:N2}", line1valueLow)}V - {string.Format("{0:N2}", line1valueHigh)}V" : "No data for time period")));
                    tlm.Add(2, new RepString(textProp, (line2valueLow != null ? $"{string.Format("{0:N2}", line2valueLow)}V - {string.Format("{0:N2}", line2valueHigh)}V" : "No data for time period")));
                    tlm.Add(3, new RepString(textProp, (line3valueLow != null ? $"{string.Format("{0:N2}", line3valueLow)}V - {string.Format("{0:N2}", line3valueHigh)}V" : "No data for time period")));
                    tlm.Add(4, new RepString(textProp, (test1 ? "Pass" : "Fail")));

                    verticalMillimeters += tlm.rContainerHeightMM;

                }

                List<PointGroup> pointGroups = new List<PointGroup>();
                pointGroups.Add(new PointGroup()
                {
                    Name = "L1",
                    Color = Color.DarkBlue,
                    Data = oneData.Where(point => point.SeriesID == SeriesID.Average).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2",
                    Color = Color.DarkGreen,
                    Data = twoData.Where(point => point.SeriesID == SeriesID.Average).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3",
                    Color = Color.DarkMagenta,
                    Data = threeData.Where(point => point.SeriesID == SeriesID.Average).ToList()
                });

                pointGroups.Add(new PointGroup()
                {
                    Name = "L1-Max",
                    Color = Color.Blue,
                    Data = oneData.Where(point => point.SeriesID == SeriesID.Maximum).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2-Max",
                    Color = Color.Green,
                    Data = twoData.Where(point => point.SeriesID == SeriesID.Maximum).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3-Max",
                    Color = Color.Magenta,
                    Data = threeData.Where(point => point.SeriesID == SeriesID.Maximum).ToList()
                });

                pointGroups.Add(new PointGroup()
                {
                    Name = "L1-Min",
                    Color = Color.LightBlue,
                    Data = oneData.Where(point => point.SeriesID == SeriesID.Minimum).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L2-Min",
                    Color = Color.LightGreen,
                    Data = twoData.Where(point => point.SeriesID == SeriesID.Minimum).ToList()
                });
                pointGroups.Add(new PointGroup()
                {
                    Name = "L3-Min",
                    Color = Color.LightPink,
                    Data = threeData.Where(point => point.SeriesID == SeriesID.Minimum).ToList()
                });

                double maxValue = data.Select(point => point.Value).Max();
                double minValue = data.Select(point => point.Value).Min();

                double chartHigh = (maxValue > 8 ? maxValue : 8);
                double chartLow = (minValue < 0 ? minValue : 0);

                double chartMax = chartHigh + (chartHigh - chartLow) * .10;
                double chartMin = chartLow - (chartHigh - chartLow) * .10;

                Chart chart = GenerateLineChart("V", pointGroups, chartMax, chartMin, "MM/dd", "0.00", "Voltage L-L", 8, null, null, null, FirstOfMonth, EndOfMonth);
                verticalMillimeters += 75;

                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

                int numBuckets = 45;
                double step = (maxValue - 0) / numBuckets;
                verticalMillimeters += 75;

                chart = GenerateBarChart("0.00", data.Select(x => x.Value), maxValue, minValue);
                page.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));



                return verticalMillimeters;

            }
        }

        private Chart GenerateLineChart(string units, List<PointGroup> pointsGroup, double max, double min, string xFormat, string yFormat, string title, double? highThreshold, double? highWarning, double? lowThreshold, double? lowWarning, DateTime start, DateTime end)
        {

            Chart chart;
            ChartArea area;
            ChartSeries series;
            area = new ChartArea();
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Format = xFormat;
            area.AxisY.Title = units;
            area.AxisY.LabelStyle.Format = yFormat;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.Maximum = max;
            area.AxisY.Minimum = min;

            chart = new Chart();
            chart.Width = 1200;
            chart.Height = 500;
            chart.Name = title;

            chart.Legends.Add(new Legend());
            chart.ChartAreas.Add(area);

            foreach (var group in pointsGroup)
            {

                series = new ChartSeries(group.Name);
                series.ChartType = SeriesChartType.FastLine;
                series.BorderWidth = 2;
                series.Color = group.Color;

                foreach (var point in group.Data)
                {
                    series.Points.AddXY(point.Timestamp, point.Value);
                }

                chart.Series.Add(series);
            }

            if (highThreshold.HasValue)
                chart.Series.Add(MakeLimitSeries((double)highThreshold, Color.Red, "High Threshold", start, end));
            if (lowThreshold.HasValue)
                chart.Series.Add(MakeLimitSeries((double)lowThreshold, Color.Red, "Low Threshold", start, end));
            if (highWarning.HasValue)
                chart.Series.Add(MakeLimitSeries((double)highWarning, Color.Orange, "High Warning", start, end));
            if (lowWarning.HasValue)
                chart.Series.Add(MakeLimitSeries((double)lowWarning, Color.Orange, "Low Warning", start, end));

            return chart;

        }

        private Chart GenerateBarChart(string units, IEnumerable<double> points, double max, double min)
        {
            Chart chart;
            ChartArea area;
            ChartSeries series;
            area = new ChartArea();
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.Enabled = AxisEnabled.False;
            area.AxisX.LabelStyle.Format = units;
            area.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.DecreaseFont;
            chart = new Chart();
            chart.Width = 1200;
            chart.Height = 500;
            chart.ChartAreas.Add(area);

            double step = (max - min) / NumBuckets;
            IEnumerable<IGrouping<int, double>> groups = points.GroupBy(x => (int)(x / step)).Where(x => x.Key > 0).OrderBy(x => x.Key);

            double index = min;
            Dictionary<string, int> buckets = new Dictionary<string, int>();
            while (index < max)
            {
                IGrouping<int, double> group = groups.Where(x => x.Key * step <= index && x.Key * step + step > index).FirstOrDefault();

                buckets.Add(index.ToString(), group?.Count() ?? 0);
                index += step;
            }

            foreach (var key in buckets)
            {

                series = new ChartSeries(key.Key);
                series.ChartType = SeriesChartType.Column;
                series.BorderWidth = 20;
                series.Color = Color.DarkBlue;
                series.Points.AddXY(double.Parse(key.Key), key.Value);

                chart.Series.Add(series);
            }

            return chart;

        }

        private Chart GenerateMagDurChart(List<Tuple<double, double>> events, string xFormat, string yFormat)
        {

            Chart chart;
            ChartArea area;
            ChartSeries series;
            area = new ChartArea();
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Format = xFormat;
            area.AxisX.Maximum = 10;
            area.AxisX.Minimum = 0.00001;
            area.AxisX.IsLogarithmic = true;
            area.AxisY.LabelStyle.Format = yFormat;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.Maximum = 6;
            area.AxisY.Minimum = 0;

            chart = new Chart();
            chart.Width = 1000;
            chart.Height = 1000;
            chart.Name = "ITIC Curve (Interruptions, Sags, Swells)";
            chart.ChartAreas.Add(area);

            series = new ChartSeries("Points");
            series.ChartType = SeriesChartType.Point;
            series.BorderWidth = 2;
            series.Color = Color.DarkBlue;

            foreach (Tuple<double, double> evt in events)
            {
                series.Points.AddXY(evt.Item1, evt.Item2);
            }

            chart.Series.Add(series);

            series = new ChartSeries("ITIC Upper");
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.Color = Color.Red;

            foreach (Tuple<double, double> point in Curves.IticUpperCurve)
            {
                series.Points.AddXY(point.Item1, point.Item2);
            }

            chart.Series.Add(series);

            series = new ChartSeries("ITIC Lower");
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.Color = Color.Red;

            foreach (Tuple<double, double> point in Curves.IticLowerCurve)
            {
                series.Points.AddXY(point.Item1, point.Item2);
            }

            chart.Series.Add(series);

            return chart;

        }

        private ChartSeries MakeLimitSeries(double value, Color color, string name, DateTime start, DateTime end)
        {
            ChartSeries cs = new ChartSeries(name);
            cs.ChartType = SeriesChartType.FastLine;
            cs.BorderWidth = 1;

            cs.Color = color;
            cs.Points.AddXY(start, value);
            cs.Points.AddXY(end, value);
            return cs;
        }

        private Stream ChartToImage(Chart chart)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
