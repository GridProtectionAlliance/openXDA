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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileViewer));
            this.DataChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SELEVEButton = new System.Windows.Forms.Button();
            this.ChannelListBox = new System.Windows.Forms.ListBox();
            this.PQDIFButton = new System.Windows.Forms.Button();
            this.COMTRADEButton = new System.Windows.Forms.Button();
            this.CSVExportButton = new System.Windows.Forms.Button();
            this.EMAXButton = new System.Windows.Forms.Button();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.ChannelPanel = new System.Windows.Forms.Panel();
            this.ChartPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DataChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.ButtonPanel.SuspendLayout();
            this.ChannelPanel.SuspendLayout();
            this.ChartPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataChart
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.DataChart.ChartAreas.Add(chartArea1);
            this.DataChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.DataChart.Legends.Add(legend1);
            this.DataChart.Location = new System.Drawing.Point(10, 10);
            this.DataChart.Name = "DataChart";
            this.DataChart.Size = new System.Drawing.Size(527, 521);
            this.DataChart.TabIndex = 0;
            this.DataChart.TabStop = false;
            this.DataChart.Text = "DataChart";
            // 
            // SELEVEButton
            // 
            this.SELEVEButton.Location = new System.Drawing.Point(10, 66);
            this.SELEVEButton.Name = "SELEVEButton";
            this.SELEVEButton.Size = new System.Drawing.Size(120, 22);
            this.SELEVEButton.TabIndex = 2;
            this.SELEVEButton.Text = "Open SEL Event...";
            this.SELEVEButton.UseVisualStyleBackColor = true;
            this.SELEVEButton.Click += new System.EventHandler(this.SELEVEButton_Click);
            // 
            // ChannelListBox
            // 
            this.ChannelListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChannelListBox.FormattingEnabled = true;
            this.ChannelListBox.Location = new System.Drawing.Point(10, 10);
            this.ChannelListBox.Name = "ChannelListBox";
            this.ChannelListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ChannelListBox.Size = new System.Drawing.Size(157, 521);
            this.ChannelListBox.TabIndex = 5;
            this.ChannelListBox.SelectedIndexChanged += new System.EventHandler(this.ChannelListBox_SelectedIndexChanged);
            // 
            // PQDIFButton
            // 
            this.PQDIFButton.Location = new System.Drawing.Point(10, 38);
            this.PQDIFButton.Name = "PQDIFButton";
            this.PQDIFButton.Size = new System.Drawing.Size(120, 22);
            this.PQDIFButton.TabIndex = 1;
            this.PQDIFButton.Text = "Open PQDIF...";
            this.PQDIFButton.UseVisualStyleBackColor = true;
            this.PQDIFButton.Click += new System.EventHandler(this.PQDIFButton_Click);
            // 
            // COMTRADEButton
            // 
            this.COMTRADEButton.Location = new System.Drawing.Point(10, 10);
            this.COMTRADEButton.Name = "COMTRADEButton";
            this.COMTRADEButton.Size = new System.Drawing.Size(120, 22);
            this.COMTRADEButton.TabIndex = 0;
            this.COMTRADEButton.Text = "Open COMTRADE...";
            this.COMTRADEButton.UseVisualStyleBackColor = true;
            this.COMTRADEButton.Click += new System.EventHandler(this.COMTRADEButton_Click);
            // 
            // CSVExportButton
            // 
            this.CSVExportButton.Location = new System.Drawing.Point(10, 136);
            this.CSVExportButton.Name = "CSVExportButton";
            this.CSVExportButton.Size = new System.Drawing.Size(120, 22);
            this.CSVExportButton.TabIndex = 4;
            this.CSVExportButton.Text = "Export to CSV...";
            this.CSVExportButton.UseVisualStyleBackColor = true;
            this.CSVExportButton.Click += new System.EventHandler(this.CSVExportButton_Click);
            // 
            // EMAXButton
            // 
            this.EMAXButton.Location = new System.Drawing.Point(10, 94);
            this.EMAXButton.Name = "EMAXButton";
            this.EMAXButton.Size = new System.Drawing.Size(120, 22);
            this.EMAXButton.TabIndex = 3;
            this.EMAXButton.Text = "Open EMAX...";
            this.EMAXButton.UseVisualStyleBackColor = true;
            this.EMAXButton.Click += new System.EventHandler(this.EMAXButton_Click);
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(10, 290);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(10);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(120, 240);
            this.LogoPictureBox.TabIndex = 7;
            this.LogoPictureBox.TabStop = false;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.AutoSize = true;
            this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonPanel.Controls.Add(this.COMTRADEButton);
            this.ButtonPanel.Controls.Add(this.PQDIFButton);
            this.ButtonPanel.Controls.Add(this.SELEVEButton);
            this.ButtonPanel.Controls.Add(this.EMAXButton);
            this.ButtonPanel.Controls.Add(this.CSVExportButton);
            this.ButtonPanel.Controls.Add(this.LogoPictureBox);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonPanel.Location = new System.Drawing.Point(10, 10);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(140, 541);
            this.ButtonPanel.TabIndex = 0;
            // 
            // ChannelPanel
            // 
            this.ChannelPanel.AutoSize = true;
            this.ChannelPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ChannelPanel.Controls.Add(this.ChannelListBox);
            this.ChannelPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChannelPanel.Location = new System.Drawing.Point(150, 10);
            this.ChannelPanel.Name = "ChannelPanel";
            this.ChannelPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ChannelPanel.Size = new System.Drawing.Size(177, 541);
            this.ChannelPanel.TabIndex = 0;
            // 
            // ChartPanel
            // 
            this.ChartPanel.Controls.Add(this.DataChart);
            this.ChartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartPanel.Location = new System.Drawing.Point(327, 10);
            this.ChartPanel.Name = "ChartPanel";
            this.ChartPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ChartPanel.Size = new System.Drawing.Size(547, 541);
            this.ChartPanel.TabIndex = 0;
            // 
            // FileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.ChartPanel);
            this.Controls.Add(this.ChannelPanel);
            this.Controls.Add(this.ButtonPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "FileViewer";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "XDA Waveform Data Parser";
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DataChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ButtonPanel.ResumeLayout(false);
            this.ChannelPanel.ResumeLayout(false);
            this.ChartPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart DataChart;
        private System.Windows.Forms.Button SELEVEButton;
        private System.Windows.Forms.ListBox ChannelListBox;
        private System.Windows.Forms.Button PQDIFButton;
        private System.Windows.Forms.Button COMTRADEButton;
        private System.Windows.Forms.Button CSVExportButton;
        private System.Windows.Forms.Button EMAXButton;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Panel ChannelPanel;
        private System.Windows.Forms.Panel ChartPanel;
    }
}

