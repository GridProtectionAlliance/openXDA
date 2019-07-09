//******************************************************************************************************
//  AllBreakersReport.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  07/05/2019 - Billy Ernest
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
using log4net;
using GSF;
using GSF.Data;

namespace openXDA.Reports
{
    public class AllBreakersReport : Root.Reports.Report
    {
        #region [ Members ]
        public class Point {
            public DateTime Time;
            public double Value;
        }
        // Constants
        private const double PageMarginMillimeters = 12.7D;             // 1-inch margin
        //private const double PageWidthMillimeters = 8.5D * 25.4D;       // 8.5 inch width
        //private const double PageHeightMillimeters = 11.0D * 25.4D;     // 11 inch height
        private const double PageHeightMillimeters = 8.5D * 25.4D;       // 8.5 inch width
        private const double PageWidthMillimeters = 11.0D * 25.4D;     // 11 inch height

        private const double FooterHeightMillimeters = (10.0D / 72.0D) * 25.4D;
        private const double SpacingMillimeters = 6.0D;

        private const string TitleText = "All Breakers Report";

        private const string query =
        @"
            SELECT 
                MAX(BreakerOperation.TripCoilEnergized) as LastOperationDate, 
                BreakerOperation.BreakerNumber,
                COUNT(BreakerOperation.ID) as Total,
                MaximoBreaker.AssetNum,
                MeterLine.LineName,
                -- last timing info
                (SELECT Name from Phase WHERE ID = (SELECT TOP 1 PhaseID FROM BreakerOperation as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized)) as LastPhase,
                (SELECT TOP 1 BreakerTiming FROM BreakerOperation as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastWaveformTiming,
                (SELECT TOP 1 StatusTiming FROM BreakerOperation as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastStatusTiming,
                MaximoBreaker.BreakerSpeed as MfrSpeed,
                (SELECT Name from BreakerOperationType WHERE ID = (SELECT TOP 1 BreakerOperationTypeID FROM BreakerOperation as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized)) as OperationTiming,
                (SELECT TOP 1 CASE WHEN StatusTiming < BreakerTiming THEN 'Status' ELSE 'Waveform' END FROM BreakerOperation as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastMethod,
                -- last slow
                COUNT(BOLate.ID) as TotalLateOperation,
                MAX(BOLate.TripCoilEnergized) as LastLateOperation,
                    -- mfr info
                MaximoBreaker.Manufacturer,
                MaximoBreaker.SerialNum,
                MaximoBreaker.MfrYear,
                MaximoBreaker.ModelNum,
                MaximoBreaker.InterruptCurrentRating,
                MaximoBreaker.ContinuousAmpRating
            FROM 
                BreakerOperation LEFT JOIN
                BreakerOperation as BOLate ON BreakerOperation.ID = BOLate.ID AND BOLate.BreakerOperationTypeID = (SELECT ID FROM BreakerOperationType WHERE Name = 'Late') JOIN
                MeterLine ON (SELECT LineID FROM Channel WHERE ID = (SELECT TOP 1 ChannelID FROM BreakerChannel WHERE BreakerChannel.BreakerNumber = BreakerOperation.BreakerNumber)) = MeterLine.LineID AND (SELECT MeterID FROM Channel WHERE ID = (SELECT TOP 1 ChannelID FROM BreakerChannel WHERE BreakerChannel.BreakerNumber = BreakerOperation.BreakerNumber)) = MeterLine.MeterID LEFT JOIN
                MaximoBreaker ON BreakerOperation.BreakerNumber = SUBSTRING(MaximoBreaker.BreakerNum, PATINDEX('%[^0]%', MaximoBreaker.BreakerNum + '.'), LEN(MaximoBreaker.BreakerNum))
            WHERE
                Cast(BreakerOperation.TripCoilEnergized as Date) BETWEEN {0} AND {1}
            GROUP BY 
                BreakerOperation.BreakerNumber,
                MaximoBreaker.AssetNum,
                MeterLine.LineName,
                MaximoBreaker.Manufacturer,
                MaximoBreaker.SerialNum,
                MaximoBreaker.MfrYear,
                MaximoBreaker.ModelNum,
                MaximoBreaker.InterruptCurrentRating,
                MaximoBreaker.ContinuousAmpRating,
                MaximoBreaker.BreakerSpeed
            ORDER BY 
                LastOperationDate
        ";

        private const string testQuery = @"
            SELECT 
                        MAX(BreakerOperation.TripCoilEnergized) as LastOperationDate, 
                        BreakerOperation.BreakerNumber,
                        COUNT(BreakerOperation.ID) as Total,
                        MaximoBreaker.[Breaker Number] as AssetNum,
                        MaximoBreaker.[Line Name] as LineName,
                        -- last timing info
                        (SELECT Name from Phase WHERE ID = (SELECT TOP 1 PhaseID FROM GTCBreakerOperationsTable as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized)) as LastPhase,
                        (SELECT TOP 1 BreakerTiming FROM GTCBreakerOperationsTable as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastWaveformTiming,
                        (SELECT TOP 1 StatusTiming FROM GTCBreakerOperationsTable as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastStatusTiming,
                        MaximoBreaker.[Breaker Mfr Speed] as MfrSpeed,
                        (SELECT Name from BreakerOperationType WHERE ID = (SELECT TOP 1 BreakerOperationTypeID FROM GTCBreakerOperationsTable as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized)) as OperationTiming,
                        (SELECT TOP 1 CASE WHEN StatusTiming < BreakerTiming THEN 'Status' ELSE 'Waveform' END FROM GTCBreakerOperationsTable as bo WHERE bo.BreakerNumber = BreakerOperation.BreakerNumber ORDER BY TripCoilEnergized) as LastMethod,
                        -- last slow
                        COUNT(BOLate.ID) as TotalLateOperation,
                        MAX(BOLate.TripCoilEnergized) as LastLateOperation,
                            -- mfr info
                        MaximoBreaker.Manufacturer,
                        MaximoBreaker.[Serial Number] as SerialNum,
                        MaximoBreaker.[Mfr Year] as MfrYear,
                        MaximoBreaker.[Model Number] as ModelNum,
                        MaximoBreaker.[Interrupt current Rating (A)] as InterruptCurrentRating,
                        MaximoBreaker.[ Continuous Amp Rating (A)] as ContinuousAmpRating
                    FROM 
                        GTCBreakerOperationsTable as BreakerOperation LEFT JOIN
                        GTCBreakerOperationsTable as BOLate ON BreakerOperation.ID = BOLate.ID AND BOLate.BreakerOperationTypeID = (SELECT ID FROM BreakerOperationType WHERE Name = 'Late') LEFT JOIN
                        MaximoBreakerInfo as MaximoBreaker ON BreakerOperation.BreakerNumber = SUBSTRING(MaximoBreaker.[Breaker Number], PATINDEX('%[^0]%', MaximoBreaker.[Breaker Number] + '.'), LEN(MaximoBreaker.[Breaker Number]))
			        WHERE
                        Cast(BreakerOperation.TripCoilEnergized as Date) BETWEEN {0} AND {1}
                    GROUP BY 
                        BreakerOperation.BreakerNumber,
                        MaximoBreaker.[Breaker Number],
                        MaximoBreaker.[Line Name],
                        MaximoBreaker.Manufacturer,
                        MaximoBreaker.[Serial Number],
                        MaximoBreaker.[Mfr Year],
			            MaximoBreaker.[Breaker Mfr Speed],
                        MaximoBreaker.[Model Number],
                        MaximoBreaker.[Interrupt current Rating (A)],
                        MaximoBreaker.[ Continuous Amp Rating (A)]    
                    ORDER BY 
                        LastOperationDate
                ";

        #endregion

        #region [ Properties ]

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FontDef FontDefinition { get; set; }
        public DataTable DataTable { get; set; }
        #endregion

        #region [ Constructors ]

        public AllBreakersReport(  DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            FontDefinition = new FontDef(this, FontDef.StandardFont.Helvetica);
            
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
#if DEBUG
                DataTable = connection.RetrieveData(testQuery, startTime, endTime);
#else
                DataTable = connection.RetrieveData(query, startTime, endTime);
#endif
            }
        }

        #endregion

        #region [ Methods ]

        public byte[] createPDF()
        {
            try
            {
                GenerateReport();

                using (MemoryStream stream = new MemoryStream())
                {
                    this.formatter.Create(this, stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex) {
                Log.Error(ex.ToString(), ex);
                return null;
            }
        }

        private void GenerateReport()
        {
            CreatePage();
            double verticalMillimeters = InsertHeader();

            verticalMillimeters = CreateTable(verticalMillimeters);

            foreach (Page page in enum_Page)
            {
                InsertFooter(page);
            }
        }

        private double CreateTable(double verticalMillimeters)
        {
            if (DataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(verticalMillimeters, $"   No Breaker Operations during {StartTime.ToString("MM/dd/yyyy")} - {EndTime.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 7.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {

                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) =>
                {
                    verticalMillimeters += tlm.rCurY_MM;
                    verticalMillimeters = NextTablePage(verticalMillimeters, ea);
                };

                List<dynamic> columns = new List<dynamic>()
                {
                    new { Name = "Last Op", Column = "LastOperationDate", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters)*.05 },
                    new { Name = "Breaker", Column = "BreakerNumber", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .04 },
                    new { Name = "# of Ops", Column = "Total", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .04 },
                    new { Name = "Asset", Column = "AssetNum", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 },
                    new { Name = "Line", Column = "LineName", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .10 },
                    new { Name = "Phase",  Column = "LastPhase", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .04 },
                    new { Name = "Timing (wf)", Column = "LastWaveformTiming", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 },
                    new { Name = "Timing (sb)",  Column = "LastStatusTiming", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 },
                    new { Name = "MFR Speed",  Column = "MfrSpeed", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .04 },
                    new { Name = "Operation Timing",  Column = "OperationTiming", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .06 },
                    new { Name = "Method",  Column = "LastMethod", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 },
                    new { Name = "# Late", Column = "TotalLateOperation", Width = (PageWidthMillimeters - 2 * PageMarginMillimeters) * .03 },
                    new { Name = "Last Late",  Column = "LastLateOperation", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 },
                    new { Name = "MFR",  Column = "Manufacturer", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .07 },
                    new { Name = "Serial #",  Column = "SerialNum", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .08 },
                    new { Name = "MFR Year", Column = "MfrYear", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .04 },
                    new { Name = "Model #",  Column = "ModelNum", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .07},
                    new { Name = "ICR (A)",  Column = "InterruptCurrentRating", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .05},
                    new { Name = "CAR (A)",  Column = "ContinuousAmpRating", Width =(PageWidthMillimeters - 2 * PageMarginMillimeters) * .05 }
                };

                foreach (var column in columns)
                    new TlmColumnMM(tlm, column.Name, column.Width);

                List<string> centeredCols = new List<string>(){ "Total", "BreakerNumber", "AssetNum", "LastPhase" };
                foreach (DataRow row in DataTable.Rows)
                {
                    tlm.NewRow();
                    int index = 0;
                    foreach (var column in columns) {

                        if (column.Column == "LastOperationDate" || (column.Column == "LastLateOperation" && row[column.Column].ToString() != string.Empty))
                        {
                            FontProp textProp = new FontProp(FontDefinition, 0);
                            textProp.rSizePoint = 6.0D;
                            string date = DateTime.Parse(row[column.Column].ToString()).ToString("MM/dd/yyyy");
                            string time = DateTime.Parse(row[column.Column].ToString()).ToString("HH:mm:ss");
                            tlm.Add(index, new RepString(textProp, date));
                            tlm.NewLine(index);
                            tlm.Add(index++, new RepString(textProp, time));
                        }
                        else {
                            FontProp textProp = new FontProp(FontDefinition, 0);
                            textProp.rSizePoint = 6.0D;
                            string value = "";

                            value = row[column.Column].ToString();
                            RepString repObj = new RepString(textProp, value);
                            if (centeredCols.Contains(column.Column))
                                repObj.rAlignH = RepObj.rAlignCenter;

                            AddDataColumn(tlm, index, value, textProp, column.Width);
                            index++;

                        }


                    }
                }

                tlm.Commit();

                verticalMillimeters += tlm.rCurY_MM + 10;
            }

            return verticalMillimeters;
        }

        private void AddDataColumn(TlmBase tlm, int index, string value, FontProp fontProp, double width) {
            double initialWidth =  fontProp.rGetTextWidthMM(value);
            string remainingString = value.Trim();
            while (initialWidth > width)
            {
                string[] tokens = remainingString.Split(' ');
                string splitValue = tokens.First();

                foreach (string token in tokens.Skip(1)) {
                    if (fontProp.rGetTextWidthMM(splitValue + token) < width)
                        splitValue = splitValue + ' ' + token;
                    else
                        break;
                }

                remainingString = remainingString.Replace(splitValue, "");
                tlm.Add(index, new RepString(fontProp, splitValue));
                tlm.NewLine(index);
                initialWidth = fontProp.rGetTextWidthMM(remainingString);
            }
            tlm.Add(index, new RepString(fontProp, remainingString));
        }

        // Creates a page and sets width and height to standard 8.5x11 inches.
        private Page CreatePage()
        {
            Page page = new Page(this);
            page.rWidthMM = PageWidthMillimeters;
            page.rHeightMM = PageHeightMillimeters;
            return page;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertFooter(Page page)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;
            page.AddMM(PageWidthMillimeters - PageMarginMillimeters - font.rGetTextWidthMM(page.iPageNo.ToString()), PageHeightMillimeters - 5, new RepString(font, page.iPageNo.ToString()));
            return font.rSizeMM;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertHeader()
        {
            string ReportIdentifier = "All Breakers Report - " + StartTime.ToString("MM/dd/yyyy") + " - " + EndTime.ToString("MM/dd/yyyy")  + "";

            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;

            FontProp meterNameFont = new FontProp(FontDefinition, 0.0D);
            meterNameFont.rSizePoint = 12.0D;

            int height = 6;

            double reportIdentifierHorizontalPosition = PageWidthMillimeters / 2 - font.rGetTextWidthMM(ReportIdentifier) / 2;

            page_Cur.AddMM(reportIdentifierHorizontalPosition, height, new RepString(font, ReportIdentifier));
            page_Cur.AddMM(0, 10, new RepRectMM(new BrushProp(this, Color.Black), PageWidthMillimeters, 0.1D));

            return 20;
        }


        // Inserts the given text as a section header (16-pt, bold).
        private double InsertItalicText(double verticalMillimeters, string text)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 14.0D;
            font.bBold = false;
            font.bItalic = true;
            page_Cur.AddMM(PageMarginMillimeters, verticalMillimeters, new RepString(font, text));
            return font.rSizeMM + 5;
        }

        private double NextTablePage( double verticalMillimeters, TlmBase.NewContainerEventArgs ea)
        {
            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                CreatePage();
                verticalMillimeters = InsertHeader();
            }

            ea.container.rHeightMM = PageHeightMillimeters - verticalMillimeters - PageMarginMillimeters;
            page_Cur.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);

            return verticalMillimeters;
        }

        #endregion

        #region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(PQReport));

        #endregion
    }
}
