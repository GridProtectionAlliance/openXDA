namespace XDACloudDataPusher
{
    partial class MainForm
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
            this.EventQueryGroupBox = new System.Windows.Forms.GroupBox();
            this.UnselectAllEventsButton = new System.Windows.Forms.Button();
            this.EndDateTimeLabel = new System.Windows.Forms.Label();
            this.StartDateTimeLabel = new System.Windows.Forms.Label();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SelectAllEventsButton = new System.Windows.Forms.Button();
            this.LoadSourcesButton = new System.Windows.Forms.Button();
            this.SelectSourcesLabel = new System.Windows.Forms.Label();
            this.XDAUrlLabel = new System.Windows.Forms.Label();
            this.XDAUrlTextBox = new System.Windows.Forms.TextBox();
            this.SourceTypeLabel = new System.Windows.Forms.Label();
            this.QueryLinesRadioButton = new System.Windows.Forms.RadioButton();
            this.QueryMetersRadioButton = new System.Windows.Forms.RadioButton();
            this.SelectedSourcesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.CloudRepositoryGroupBox = new System.Windows.Forms.GroupBox();
            this.ExportFrequencyDomainDataCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportWaveformDataCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportBreakerOperationCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportDisturbanceDataCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportFaultDataCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportEventDataCheckBox = new System.Windows.Forms.CheckBox();
            this.PushToCloudButton = new System.Windows.Forms.Button();
            this.AWSRadioButton = new System.Windows.Forms.RadioButton();
            this.RepositoryTypeLabel = new System.Windows.Forms.Label();
            this.AzureRadioButton = new System.Windows.Forms.RadioButton();
            this.ConnectionStringLabel = new System.Windows.Forms.Label();
            this.ConnectionStringTextBox = new System.Windows.Forms.TextBox();
            this.MessageGroupBox = new System.Windows.Forms.GroupBox();
            this.MessageOutputTextBox = new System.Windows.Forms.TextBox();
            this.ExportProgressBar = new System.Windows.Forms.ProgressBar();
            this.CancelButton = new System.Windows.Forms.Button();
            this.EventQueryGroupBox.SuspendLayout();
            this.CloudRepositoryGroupBox.SuspendLayout();
            this.MessageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventQueryGroupBox
            // 
            this.EventQueryGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventQueryGroupBox.Controls.Add(this.UnselectAllEventsButton);
            this.EventQueryGroupBox.Controls.Add(this.EndDateTimeLabel);
            this.EventQueryGroupBox.Controls.Add(this.StartDateTimeLabel);
            this.EventQueryGroupBox.Controls.Add(this.EndDateTimePicker);
            this.EventQueryGroupBox.Controls.Add(this.StartDateTimePicker);
            this.EventQueryGroupBox.Controls.Add(this.SelectAllEventsButton);
            this.EventQueryGroupBox.Controls.Add(this.LoadSourcesButton);
            this.EventQueryGroupBox.Controls.Add(this.SelectSourcesLabel);
            this.EventQueryGroupBox.Controls.Add(this.XDAUrlLabel);
            this.EventQueryGroupBox.Controls.Add(this.XDAUrlTextBox);
            this.EventQueryGroupBox.Controls.Add(this.SourceTypeLabel);
            this.EventQueryGroupBox.Controls.Add(this.QueryLinesRadioButton);
            this.EventQueryGroupBox.Controls.Add(this.QueryMetersRadioButton);
            this.EventQueryGroupBox.Controls.Add(this.SelectedSourcesCheckedListBox);
            this.EventQueryGroupBox.Location = new System.Drawing.Point(12, 12);
            this.EventQueryGroupBox.Name = "EventQueryGroupBox";
            this.EventQueryGroupBox.Size = new System.Drawing.Size(458, 328);
            this.EventQueryGroupBox.TabIndex = 0;
            this.EventQueryGroupBox.TabStop = false;
            this.EventQueryGroupBox.Text = "Event Query";
            // 
            // UnselectAllEventsButton
            // 
            this.UnselectAllEventsButton.Location = new System.Drawing.Point(100, 238);
            this.UnselectAllEventsButton.Name = "UnselectAllEventsButton";
            this.UnselectAllEventsButton.Size = new System.Drawing.Size(73, 22);
            this.UnselectAllEventsButton.TabIndex = 9;
            this.UnselectAllEventsButton.Text = "U&nselect All";
            this.UnselectAllEventsButton.UseVisualStyleBackColor = true;
            // 
            // EndDateTimeLabel
            // 
            this.EndDateTimeLabel.AutoSize = true;
            this.EndDateTimeLabel.Location = new System.Drawing.Point(240, 275);
            this.EndDateTimeLabel.Name = "EndDateTimeLabel";
            this.EndDateTimeLabel.Size = new System.Drawing.Size(129, 13);
            this.EndDateTimeLabel.TabIndex = 12;
            this.EndDateTimeLabel.Text = "&End Date/Time for Query:";
            // 
            // StartDateTimeLabel
            // 
            this.StartDateTimeLabel.AutoSize = true;
            this.StartDateTimeLabel.Location = new System.Drawing.Point(18, 275);
            this.StartDateTimeLabel.Name = "StartDateTimeLabel";
            this.StartDateTimeLabel.Size = new System.Drawing.Size(132, 13);
            this.StartDateTimeLabel.TabIndex = 10;
            this.StartDateTimeLabel.Text = "&Start Date/Time for Query:";
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Location = new System.Drawing.Point(243, 291);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 13;
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Location = new System.Drawing.Point(21, 291);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 11;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // SelectAllEventsButton
            // 
            this.SelectAllEventsButton.Location = new System.Drawing.Point(21, 238);
            this.SelectAllEventsButton.Name = "SelectAllEventsButton";
            this.SelectAllEventsButton.Size = new System.Drawing.Size(73, 22);
            this.SelectAllEventsButton.TabIndex = 8;
            this.SelectAllEventsButton.Text = "Select &All";
            this.SelectAllEventsButton.UseVisualStyleBackColor = true;
            // 
            // LoadSourcesButton
            // 
            this.LoadSourcesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadSourcesButton.Location = new System.Drawing.Point(370, 53);
            this.LoadSourcesButton.Name = "LoadSourcesButton";
            this.LoadSourcesButton.Size = new System.Drawing.Size(73, 22);
            this.LoadSourcesButton.TabIndex = 5;
            this.LoadSourcesButton.Text = "L&oad";
            this.LoadSourcesButton.UseVisualStyleBackColor = true;
            // 
            // SelectSourcesLabel
            // 
            this.SelectSourcesLabel.AutoSize = true;
            this.SelectSourcesLabel.Location = new System.Drawing.Point(18, 77);
            this.SelectSourcesLabel.Name = "SelectSourcesLabel";
            this.SelectSourcesLabel.Size = new System.Drawing.Size(82, 13);
            this.SelectSourcesLabel.TabIndex = 6;
            this.SelectSourcesLabel.Tag = "";
            this.SelectSourcesLabel.Text = "Select Sou&rces:";
            // 
            // XDAUrlLabel
            // 
            this.XDAUrlLabel.AutoSize = true;
            this.XDAUrlLabel.Location = new System.Drawing.Point(21, 25);
            this.XDAUrlLabel.Name = "XDAUrlLabel";
            this.XDAUrlLabel.Size = new System.Drawing.Size(81, 13);
            this.XDAUrlLabel.TabIndex = 0;
            this.XDAUrlLabel.Text = "openXDA &URL:";
            this.XDAUrlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // XDAUrlTextBox
            // 
            this.XDAUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XDAUrlTextBox.Location = new System.Drawing.Point(105, 22);
            this.XDAUrlTextBox.Name = "XDAUrlTextBox";
            this.XDAUrlTextBox.Size = new System.Drawing.Size(338, 20);
            this.XDAUrlTextBox.TabIndex = 1;
            this.XDAUrlTextBox.TextChanged += new System.EventHandler(this.XDAUrlTextBox_TextChanged);
            // 
            // SourceTypeLabel
            // 
            this.SourceTypeLabel.AutoSize = true;
            this.SourceTypeLabel.Location = new System.Drawing.Point(31, 53);
            this.SourceTypeLabel.Name = "SourceTypeLabel";
            this.SourceTypeLabel.Size = new System.Drawing.Size(71, 13);
            this.SourceTypeLabel.TabIndex = 2;
            this.SourceTypeLabel.Text = "Source Type:";
            this.SourceTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // QueryLinesRadioButton
            // 
            this.QueryLinesRadioButton.AutoSize = true;
            this.QueryLinesRadioButton.Checked = true;
            this.QueryLinesRadioButton.Location = new System.Drawing.Point(105, 51);
            this.QueryLinesRadioButton.Name = "QueryLinesRadioButton";
            this.QueryLinesRadioButton.Size = new System.Drawing.Size(50, 17);
            this.QueryLinesRadioButton.TabIndex = 3;
            this.QueryLinesRadioButton.TabStop = true;
            this.QueryLinesRadioButton.Text = "&Lines";
            this.QueryLinesRadioButton.UseVisualStyleBackColor = true;
            this.QueryLinesRadioButton.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // QueryMetersRadioButton
            // 
            this.QueryMetersRadioButton.AutoSize = true;
            this.QueryMetersRadioButton.Location = new System.Drawing.Point(164, 51);
            this.QueryMetersRadioButton.Name = "QueryMetersRadioButton";
            this.QueryMetersRadioButton.Size = new System.Drawing.Size(57, 17);
            this.QueryMetersRadioButton.TabIndex = 4;
            this.QueryMetersRadioButton.Text = "&Meters";
            this.QueryMetersRadioButton.UseVisualStyleBackColor = true;
            this.QueryMetersRadioButton.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // SelectedSourcesCheckedListBox
            // 
            this.SelectedSourcesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedSourcesCheckedListBox.FormattingEnabled = true;
            this.SelectedSourcesCheckedListBox.Location = new System.Drawing.Point(21, 93);
            this.SelectedSourcesCheckedListBox.Name = "SelectedSourcesCheckedListBox";
            this.SelectedSourcesCheckedListBox.ScrollAlwaysVisible = true;
            this.SelectedSourcesCheckedListBox.Size = new System.Drawing.Size(422, 139);
            this.SelectedSourcesCheckedListBox.TabIndex = 7;
            // 
            // CloudRepositoryGroupBox
            // 
            this.CloudRepositoryGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportFrequencyDomainDataCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportWaveformDataCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportBreakerOperationCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportDisturbanceDataCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportFaultDataCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.ExportEventDataCheckBox);
            this.CloudRepositoryGroupBox.Controls.Add(this.PushToCloudButton);
            this.CloudRepositoryGroupBox.Controls.Add(this.AWSRadioButton);
            this.CloudRepositoryGroupBox.Controls.Add(this.RepositoryTypeLabel);
            this.CloudRepositoryGroupBox.Controls.Add(this.AzureRadioButton);
            this.CloudRepositoryGroupBox.Controls.Add(this.ConnectionStringLabel);
            this.CloudRepositoryGroupBox.Controls.Add(this.ConnectionStringTextBox);
            this.CloudRepositoryGroupBox.Location = new System.Drawing.Point(476, 12);
            this.CloudRepositoryGroupBox.Name = "CloudRepositoryGroupBox";
            this.CloudRepositoryGroupBox.Size = new System.Drawing.Size(367, 328);
            this.CloudRepositoryGroupBox.TabIndex = 1;
            this.CloudRepositoryGroupBox.TabStop = false;
            this.CloudRepositoryGroupBox.Text = "Cloud Repository";
            // 
            // ExportFrequencyDomainDataCheckBox
            // 
            this.ExportFrequencyDomainDataCheckBox.AutoSize = true;
            this.ExportFrequencyDomainDataCheckBox.Checked = true;
            this.ExportFrequencyDomainDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportFrequencyDomainDataCheckBox.Location = new System.Drawing.Point(13, 261);
            this.ExportFrequencyDomainDataCheckBox.Name = "ExportFrequencyDomainDataCheckBox";
            this.ExportFrequencyDomainDataCheckBox.Size = new System.Drawing.Size(174, 17);
            this.ExportFrequencyDomainDataCheckBox.TabIndex = 10;
            this.ExportFrequencyDomainDataCheckBox.Text = "Export Fre&quency Domain Data";
            this.ExportFrequencyDomainDataCheckBox.UseVisualStyleBackColor = true;
            this.ExportFrequencyDomainDataCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ExportWaveformDataCheckBox
            // 
            this.ExportWaveformDataCheckBox.AutoSize = true;
            this.ExportWaveformDataCheckBox.Checked = true;
            this.ExportWaveformDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportWaveformDataCheckBox.Location = new System.Drawing.Point(13, 238);
            this.ExportWaveformDataCheckBox.Name = "ExportWaveformDataCheckBox";
            this.ExportWaveformDataCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ExportWaveformDataCheckBox.TabIndex = 9;
            this.ExportWaveformDataCheckBox.Text = "Export &Waveform Data";
            this.ExportWaveformDataCheckBox.UseVisualStyleBackColor = true;
            this.ExportWaveformDataCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ExportBreakerOperationCheckBox
            // 
            this.ExportBreakerOperationCheckBox.AutoSize = true;
            this.ExportBreakerOperationCheckBox.Checked = true;
            this.ExportBreakerOperationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportBreakerOperationCheckBox.Location = new System.Drawing.Point(13, 215);
            this.ExportBreakerOperationCheckBox.Name = "ExportBreakerOperationCheckBox";
            this.ExportBreakerOperationCheckBox.Size = new System.Drawing.Size(171, 17);
            this.ExportBreakerOperationCheckBox.TabIndex = 8;
            this.ExportBreakerOperationCheckBox.Text = "Export &Breaker Operation Data";
            this.ExportBreakerOperationCheckBox.UseVisualStyleBackColor = true;
            this.ExportBreakerOperationCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ExportDisturbanceDataCheckBox
            // 
            this.ExportDisturbanceDataCheckBox.AutoSize = true;
            this.ExportDisturbanceDataCheckBox.Checked = true;
            this.ExportDisturbanceDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportDisturbanceDataCheckBox.Location = new System.Drawing.Point(13, 192);
            this.ExportDisturbanceDataCheckBox.Name = "ExportDisturbanceDataCheckBox";
            this.ExportDisturbanceDataCheckBox.Size = new System.Drawing.Size(142, 17);
            this.ExportDisturbanceDataCheckBox.TabIndex = 7;
            this.ExportDisturbanceDataCheckBox.Text = "Export &Disturbance Data";
            this.ExportDisturbanceDataCheckBox.UseVisualStyleBackColor = true;
            this.ExportDisturbanceDataCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ExportFaultDataCheckBox
            // 
            this.ExportFaultDataCheckBox.AutoSize = true;
            this.ExportFaultDataCheckBox.Checked = true;
            this.ExportFaultDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportFaultDataCheckBox.Location = new System.Drawing.Point(13, 169);
            this.ExportFaultDataCheckBox.Name = "ExportFaultDataCheckBox";
            this.ExportFaultDataCheckBox.Size = new System.Drawing.Size(108, 17);
            this.ExportFaultDataCheckBox.TabIndex = 6;
            this.ExportFaultDataCheckBox.Text = "Export &Fault Data";
            this.ExportFaultDataCheckBox.UseVisualStyleBackColor = true;
            this.ExportFaultDataCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ExportEventDataCheckBox
            // 
            this.ExportEventDataCheckBox.AutoSize = true;
            this.ExportEventDataCheckBox.Checked = true;
            this.ExportEventDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportEventDataCheckBox.Location = new System.Drawing.Point(13, 146);
            this.ExportEventDataCheckBox.Name = "ExportEventDataCheckBox";
            this.ExportEventDataCheckBox.Size = new System.Drawing.Size(113, 17);
            this.ExportEventDataCheckBox.TabIndex = 5;
            this.ExportEventDataCheckBox.Text = "Export E&vent Data";
            this.ExportEventDataCheckBox.UseVisualStyleBackColor = true;
            this.ExportEventDataCheckBox.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // PushToCloudButton
            // 
            this.PushToCloudButton.Location = new System.Drawing.Point(279, 288);
            this.PushToCloudButton.Name = "PushToCloudButton";
            this.PushToCloudButton.Size = new System.Drawing.Size(75, 23);
            this.PushToCloudButton.TabIndex = 11;
            this.PushToCloudButton.Text = "&Push";
            this.PushToCloudButton.UseVisualStyleBackColor = true;
            this.PushToCloudButton.Click += new System.EventHandler(this.PushToCloudButton_Click);
            // 
            // AWSRadioButton
            // 
            this.AWSRadioButton.AutoSize = true;
            this.AWSRadioButton.Enabled = false;
            this.AWSRadioButton.Location = new System.Drawing.Point(215, 19);
            this.AWSRadioButton.Name = "AWSRadioButton";
            this.AWSRadioButton.Size = new System.Drawing.Size(99, 17);
            this.AWSRadioButton.TabIndex = 2;
            this.AWSRadioButton.Text = "Amazon &Kinesis";
            this.AWSRadioButton.UseVisualStyleBackColor = true;
            this.AWSRadioButton.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // RepositoryTypeLabel
            // 
            this.RepositoryTypeLabel.AutoSize = true;
            this.RepositoryTypeLabel.Location = new System.Drawing.Point(10, 21);
            this.RepositoryTypeLabel.Name = "RepositoryTypeLabel";
            this.RepositoryTypeLabel.Size = new System.Drawing.Size(87, 13);
            this.RepositoryTypeLabel.TabIndex = 0;
            this.RepositoryTypeLabel.Text = "Repository Type:";
            this.RepositoryTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AzureRadioButton
            // 
            this.AzureRadioButton.AutoSize = true;
            this.AzureRadioButton.Checked = true;
            this.AzureRadioButton.Location = new System.Drawing.Point(103, 19);
            this.AzureRadioButton.Name = "AzureRadioButton";
            this.AzureRadioButton.Size = new System.Drawing.Size(106, 17);
            this.AzureRadioButton.TabIndex = 1;
            this.AzureRadioButton.TabStop = true;
            this.AzureRadioButton.Text = "A&zure Event Hub";
            this.AzureRadioButton.UseVisualStyleBackColor = true;
            this.AzureRadioButton.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // ConnectionStringLabel
            // 
            this.ConnectionStringLabel.AutoSize = true;
            this.ConnectionStringLabel.Location = new System.Drawing.Point(10, 44);
            this.ConnectionStringLabel.Name = "ConnectionStringLabel";
            this.ConnectionStringLabel.Size = new System.Drawing.Size(163, 13);
            this.ConnectionStringLabel.TabIndex = 3;
            this.ConnectionStringLabel.Text = "Cloud Service &Connection String:";
            // 
            // ConnectionStringTextBox
            // 
            this.ConnectionStringTextBox.Location = new System.Drawing.Point(13, 60);
            this.ConnectionStringTextBox.Multiline = true;
            this.ConnectionStringTextBox.Name = "ConnectionStringTextBox";
            this.ConnectionStringTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConnectionStringTextBox.Size = new System.Drawing.Size(341, 70);
            this.ConnectionStringTextBox.TabIndex = 4;
            this.ConnectionStringTextBox.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // MessageGroupBox
            // 
            this.MessageGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageGroupBox.Controls.Add(this.MessageOutputTextBox);
            this.MessageGroupBox.Location = new System.Drawing.Point(12, 345);
            this.MessageGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.MessageGroupBox.Name = "MessageGroupBox";
            this.MessageGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.MessageGroupBox.Size = new System.Drawing.Size(831, 261);
            this.MessageGroupBox.TabIndex = 2;
            this.MessageGroupBox.TabStop = false;
            this.MessageGroupBox.Text = "Messages";
            // 
            // MessageOutputTextBox
            // 
            this.MessageOutputTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.MessageOutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageOutputTextBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageOutputTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.MessageOutputTextBox.Location = new System.Drawing.Point(2, 15);
            this.MessageOutputTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.MessageOutputTextBox.Multiline = true;
            this.MessageOutputTextBox.Name = "MessageOutputTextBox";
            this.MessageOutputTextBox.ReadOnly = true;
            this.MessageOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageOutputTextBox.Size = new System.Drawing.Size(827, 244);
            this.MessageOutputTextBox.TabIndex = 0;
            this.MessageOutputTextBox.TabStop = false;
            // 
            // ExportProgressBar
            // 
            this.ExportProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportProgressBar.Location = new System.Drawing.Point(14, 606);
            this.ExportProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.ExportProgressBar.Name = "ExportProgressBar";
            this.ExportProgressBar.Size = new System.Drawing.Size(749, 27);
            this.ExportProgressBar.TabIndex = 3;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Enabled = false;
            this.CancelButton.Location = new System.Drawing.Point(768, 608);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 636);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ExportProgressBar);
            this.Controls.Add(this.MessageGroupBox);
            this.Controls.Add(this.CloudRepositoryGroupBox);
            this.Controls.Add(this.EventQueryGroupBox);
            this.MinimumSize = new System.Drawing.Size(865, 675);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "openXDA Cloud Data Pusher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.EventQueryGroupBox.ResumeLayout(false);
            this.EventQueryGroupBox.PerformLayout();
            this.CloudRepositoryGroupBox.ResumeLayout(false);
            this.CloudRepositoryGroupBox.PerformLayout();
            this.MessageGroupBox.ResumeLayout(false);
            this.MessageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox EventQueryGroupBox;
        private System.Windows.Forms.Label EndDateTimeLabel;
        private System.Windows.Forms.Label StartDateTimeLabel;
        private System.Windows.Forms.Button SelectAllEventsButton;
        private System.Windows.Forms.Button LoadSourcesButton;
        private System.Windows.Forms.Label SelectSourcesLabel;
        private System.Windows.Forms.Label XDAUrlLabel;
        private System.Windows.Forms.Label SourceTypeLabel;
        private System.Windows.Forms.CheckedListBox SelectedSourcesCheckedListBox;
        private System.Windows.Forms.GroupBox CloudRepositoryGroupBox;
        private System.Windows.Forms.Button PushToCloudButton;
        private System.Windows.Forms.Label RepositoryTypeLabel;
        private System.Windows.Forms.Label ConnectionStringLabel;
        private System.Windows.Forms.Button UnselectAllEventsButton;
        public System.Windows.Forms.TextBox XDAUrlTextBox;
        public System.Windows.Forms.DateTimePicker EndDateTimePicker;
        public System.Windows.Forms.DateTimePicker StartDateTimePicker;
        public System.Windows.Forms.RadioButton QueryLinesRadioButton;
        public System.Windows.Forms.RadioButton QueryMetersRadioButton;
        public System.Windows.Forms.RadioButton AWSRadioButton;
        public System.Windows.Forms.RadioButton AzureRadioButton;
        public System.Windows.Forms.TextBox ConnectionStringTextBox;
        public System.Windows.Forms.CheckBox ExportFrequencyDomainDataCheckBox;
        public System.Windows.Forms.CheckBox ExportWaveformDataCheckBox;
        public System.Windows.Forms.CheckBox ExportBreakerOperationCheckBox;
        public System.Windows.Forms.CheckBox ExportDisturbanceDataCheckBox;
        public System.Windows.Forms.CheckBox ExportFaultDataCheckBox;
        public System.Windows.Forms.CheckBox ExportEventDataCheckBox;
        private System.Windows.Forms.GroupBox MessageGroupBox;
        private System.Windows.Forms.TextBox MessageOutputTextBox;
        private System.Windows.Forms.ProgressBar ExportProgressBar;
        private System.Windows.Forms.Button CancelButton;
    }
}

