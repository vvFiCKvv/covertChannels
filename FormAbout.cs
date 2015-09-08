using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace covertFiles
{
    public partial class FormAbout : Form    {
        System.Windows.Forms.Timer timer;
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
           
            //this.MdiParent = Util.MainForm;
            this.Opacity = 0.85;
            webAbout.DocumentText = covertFiles.Resources.About.Replace("$version$",Util.Version);
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 4000;
            timer.Start();
            int nx, ny;
            nx = (Util.MainForm.Size.Width   - this.Width) / 2;
            ny = (Util.MainForm.Size.Height  - this.Height) / 2;
            if (nx < 2)
                nx = 2;
            if (ny < 24)
                ny = 24;

            this.Location = new Point(nx, ny);

        }

        void timer_Tick(object sender, EventArgs e)
        {
           
            timer.Stop();
            this.Close();
            this.Dispose();

        }

        private void FormAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }



        private void webAbout_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Scheme == "res")
            {
                e.Cancel = true;
                if (e.Url.Authority == "close")
                {
                    this.Hide();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MdiParent = Util.MainForm;
        }

        private void FormAbout_Leave(object sender, EventArgs e)
        {
            this.Show();
            this.Focus();
        }

        private void webAbout_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
