using System.ComponentModel;

namespace EDAfilewatcher
{
    partial class frm_Main
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
            this.btn_go = new System.Windows.Forms.Button();
            this.txb_Result = new System.Windows.Forms.TextBox();
            this.btn_LoadXML = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(526, 282);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 23);
            this.btn_go.TabIndex = 0;
            this.btn_go.Text = "Go";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // txb_Result
            // 
            this.txb_Result.Location = new System.Drawing.Point(132, 126);
            this.txb_Result.Multiline = true;
            this.txb_Result.Name = "txb_Result";
            this.txb_Result.Size = new System.Drawing.Size(469, 132);
            this.txb_Result.TabIndex = 6;
            // 
            // btn_LoadXML
            // 
            this.btn_LoadXML.Location = new System.Drawing.Point(132, 282);
            this.btn_LoadXML.Name = "btn_LoadXML";
            this.btn_LoadXML.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadXML.TabIndex = 7;
            this.btn_LoadXML.Text = "Load XML";
            this.btn_LoadXML.UseVisualStyleBackColor = true;
            this.btn_LoadXML.Click += new System.EventHandler(this.btn_LoadXML_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(132, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(280, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "D:\\folder1\\folder2.more\\filename.ext";
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(445, 282);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 9;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 338);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_LoadXML);
            this.Controls.Add(this.txb_Result);
            this.Controls.Add(this.btn_go);
            this.Name = "frm_Main";
            this.Text = "Test openFLE-FW";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.TextBox txb_Result;
        private System.Windows.Forms.Button btn_LoadXML;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_Exit;
    }
}

