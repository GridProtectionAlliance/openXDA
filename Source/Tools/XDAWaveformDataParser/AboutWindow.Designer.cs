namespace XDAWaveformDataParser
{
    partial class AboutWindow
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
            this.AboutTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // AboutTextBox
            // 
            this.AboutTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.AboutTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutTextBox.Location = new System.Drawing.Point(10, 10);
            this.AboutTextBox.Name = "AboutTextBox";
            this.AboutTextBox.ReadOnly = true;
            this.AboutTextBox.Size = new System.Drawing.Size(384, 291);
            this.AboutTextBox.TabIndex = 0;
            this.AboutTextBox.Text = "";
            // 
            // AboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 311);
            this.Controls.Add(this.AboutTextBox);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutWindow";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutWindow";
            this.Load += new System.EventHandler(this.AboutWindow_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AboutWindow_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox AboutTextBox;
    }
}