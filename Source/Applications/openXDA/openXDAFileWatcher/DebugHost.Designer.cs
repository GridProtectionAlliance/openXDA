﻿//******************************************************************************************************
//  DebugHost.Designer.cs - Gbtc
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
//  09/10/2012 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

namespace openXDAFileWatcher
{
    partial class DebugHost
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugHost));
            this.LabelNotice = new System.Windows.Forms.Label();
            this.m_notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.m_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelNotice
            // 
            this.LabelNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelNotice.Location = new System.Drawing.Point(10, 10);
            this.LabelNotice.Name = "LabelNotice";
            this.LabelNotice.Size = new System.Drawing.Size(324, 53);
            this.LabelNotice.TabIndex = 1;
            this.LabelNotice.Text = resources.GetString("LabelNotice.Text");
            this.LabelNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_notifyIcon
            // 
            this.m_notifyIcon.ContextMenuStrip = this.m_contextMenuStrip;
            this.m_notifyIcon.Text = "{0} (Debug Mode)";
            this.m_notifyIcon.Visible = true;
            // 
            // m_contextMenuStrip
            // 
            this.m_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_showToolStripMenuItem,
            this.m_toolStripSeparator1,
            this.m_exitToolStripMenuItem});
            this.m_contextMenuStrip.Name = "contextMenuStrip";
            this.m_contextMenuStrip.ShowImageMargin = false;
            this.m_contextMenuStrip.Size = new System.Drawing.Size(85, 54);
            // 
            // m_showToolStripMenuItem
            // 
            this.m_showToolStripMenuItem.Name = "m_showToolStripMenuItem";
            this.m_showToolStripMenuItem.Size = new System.Drawing.Size(84, 22);
            this.m_showToolStripMenuItem.Text = "Show";
            this.m_showToolStripMenuItem.Click += new System.EventHandler(this.ShowToolStripMenuItem_Click);
            // 
            // m_toolStripSeparator1
            // 
            this.m_toolStripSeparator1.Name = "m_toolStripSeparator1";
            this.m_toolStripSeparator1.Size = new System.Drawing.Size(81, 6);
            // 
            // m_exitToolStripMenuItem
            // 
            this.m_exitToolStripMenuItem.Name = "m_exitToolStripMenuItem";
            this.m_exitToolStripMenuItem.Size = new System.Drawing.Size(84, 22);
            this.m_exitToolStripMenuItem.Text = "Exit {0}";
            this.m_exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // DebugHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 73);
            this.Controls.Add(this.LabelNotice);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DebugHost";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "{0} (Debug Mode)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugHost_FormClosing);
            this.Load += new System.EventHandler(this.DebugHost_Load);
            this.Resize += new System.EventHandler(this.DebugHost_Resize);
            this.m_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabelNotice;
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private System.Windows.Forms.ContextMenuStrip m_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem m_showToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_exitToolStripMenuItem;
    }
}