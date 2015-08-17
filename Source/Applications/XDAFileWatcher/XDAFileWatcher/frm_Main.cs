using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GSF;
using GSF.IO;


namespace EDAfilewatcher
{
    public partial class frm_Main : Form
    {
       
        
      

        public frm_Main()
        {
            InitializeComponent();
           
       
           
   
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        
            try
            {
                Log.Instance.Info("*------------------------------ openEDA FileWatcher Application Loaded --------------- " + DateTime.Now.ToShortTimeString() + " --------------*");
                Log.Instance.Debug("frm_Main Loaded");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Log initialization Error " + ex.Message, "FileWatcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

          }


        private void btn_LoadXML_Click(object sender, EventArgs e)
        {
           Log.Instance.Debug("btn_LoadXML_Click: Go Button Clicked");
            ReadConfiguration();


        }

       
        private void btn_go_Click(object sender, EventArgs e)
        {

            Log.Instance.Debug("btn_go_Click: Go Button Clicked");
            ReadConfiguration();

            new Thread(CreateNewWatcher).Start();

            Log.Instance.Info("btn_go_Click: EventType Tread Started for root folder = " + _rootWatchPath);
            //MessageBox.Show("Watcher Thread Started.", "FileWatcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txb_Result.Text += "Watcher Thread Started " + "\r\n" + _rootWatchPath + "\r\n";
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Instance.Fatal("Form1_Closing: openFLE-FW Terminated By User");
        }


      

        


    }
}
