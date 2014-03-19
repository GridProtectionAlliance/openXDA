namespace XDAWaveformDataParser
{
    partial class FileViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.DataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SELEVEButton = new System.Windows.Forms.Button();
            this.ChannelListBox = new System.Windows.Forms.ListBox();
            this.PQDIFButton = new System.Windows.Forms.Button();
            this.COMTRADEButton = new System.Windows.Forms.Button();
            this.CSVExportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataChart)).BeginInit();
            this.SuspendLayout();
            // 
            // DataChart
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.DataChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.DataChart.Legends.Add(legend1);
            this.DataChart.Location = new System.Drawing.Point(328, 12);
            this.DataChart.Name = "DataChart";
            this.DataChart.Size = new System.Drawing.Size(544, 537);
            this.DataChart.TabIndex = 4;
            this.DataChart.Text = "DataChart";
            // 
            // SELEVEButton
            // 
            this.SELEVEButton.Location = new System.Drawing.Point(20, 68);
            this.SELEVEButton.Name = "SELEVEButton";
            this.SELEVEButton.Size = new System.Drawing.Size(120, 22);
            this.SELEVEButton.TabIndex = 2;
            this.SELEVEButton.Text = "Open SEL Event...";
            this.SELEVEButton.UseVisualStyleBackColor = true;
            this.SELEVEButton.Click += new System.EventHandler(this.SELEVEButton_Click);
            // 
            // ChannelListBox
            // 
            this.ChannelListBox.FormattingEnabled = true;
            this.ChannelListBox.Location = new System.Drawing.Point(162, 12);
            this.ChannelListBox.Name = "ChannelListBox";
            this.ChannelListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ChannelListBox.Size = new System.Drawing.Size(157, 537);
            this.ChannelListBox.TabIndex = 3;
            this.ChannelListBox.SelectedIndexChanged += new System.EventHandler(this.ChannelListBox_SelectedIndexChanged);
            // 
            // PQDIFButton
            // 
            this.PQDIFButton.Location = new System.Drawing.Point(20, 40);
            this.PQDIFButton.Name = "PQDIFButton";
            this.PQDIFButton.Size = new System.Drawing.Size(120, 22);
            this.PQDIFButton.TabIndex = 1;
            this.PQDIFButton.Text = "Open PQDIF...";
            this.PQDIFButton.UseVisualStyleBackColor = true;
            this.PQDIFButton.Click += new System.EventHandler(this.PQDIFButton_Click);
            // 
            // COMTRADEButton
            // 
            this.COMTRADEButton.Location = new System.Drawing.Point(20, 12);
            this.COMTRADEButton.Name = "COMTRADEButton";
            this.COMTRADEButton.Size = new System.Drawing.Size(120, 22);
            this.COMTRADEButton.TabIndex = 0;
            this.COMTRADEButton.Text = "Open COMTRADE...";
            this.COMTRADEButton.UseVisualStyleBackColor = true;
            this.COMTRADEButton.Click += new System.EventHandler(this.COMTRADEButton_Click);
            // 
            // CSVExportButton
            // 
            this.CSVExportButton.Location = new System.Drawing.Point(20, 120);
            this.CSVExportButton.Name = "CSVExportButton";
            this.CSVExportButton.Size = new System.Drawing.Size(120, 22);
            this.CSVExportButton.TabIndex = 5;
            this.CSVExportButton.Text = "Export to CSV...";
            this.CSVExportButton.UseVisualStyleBackColor = true;
            this.CSVExportButton.Click += new System.EventHandler(this.CSVExportButton_Click);
            // 
            // FileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.CSVExportButton);
            this.Controls.Add(this.COMTRADEButton);
            this.Controls.Add(this.PQDIFButton);
            this.Controls.Add(this.ChannelListBox);
            this.Controls.Add(this.SELEVEButton);
            this.Controls.Add(this.DataChart);
            this.Name = "FileViewer";
            this.Text = "XDA Waveform Data Parser";
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DataChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart DataChart;
        private System.Windows.Forms.Button SELEVEButton;
        private System.Windows.Forms.ListBox ChannelListBox;
        private System.Windows.Forms.Button PQDIFButton;
        private System.Windows.Forms.Button COMTRADEButton;
        private System.Windows.Forms.Button CSVExportButton;
    }
}

