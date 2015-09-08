using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace covertFiles
{
    public partial class FormMessageControl : Form
    {
        
        public FormMessageControl()
        {
            InitializeComponent();
        }
        private  bool _isVisible = true;
        public  bool isVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                Util.MainForm.Size = new System.Drawing.Size(Util.MainForm.Size.Width, Util.MainForm.Size.Height + 1);
                Util.MainForm.Size = new System.Drawing.Size(Util.MainForm.Size.Width, Util.MainForm.Size.Height - 1);
            }
        }
        private void FormMessageControl_Load(object sender , EventArgs e)
        {
            this.MdiParent = Util.MainForm;
            timer.Start();
            txtLog.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(txtLog_DocumentCompleted);
            this.Resize += new EventHandler(FormMessageControl_Resize);
           

        }
        private  bool _ignoreResizeEvents = false;
        public  bool ignoreResizeEvents
        {
            get
            {
                return _ignoreResizeEvents;
            }
            set
            {
                _ignoreResizeEvents = value;
                                
                    if (_ignoreResizeEvents == true)
                    {
                        this.Resize -= FormMessageControl_Resize;

                    }
                    else
                    {
                        this.Resize += new EventHandler(FormMessageControl_Resize);
                    }
                
                                
            }           

        }
        void FormMessageControl_Resize(object sender, EventArgs e)
        {
            Util.formsIgnoreResizeEvents = true;
            if (isVisible == true)
            {
                Util.MessageFormPercent = new SizeF((float)this.Width / (float)(Util.MainForm.Width - 15), (float)this.Height / (float)(Util.MainForm.Height - 80));
            }
            Util.formsIgnoreResizeEvents = false;
        }

        void txtLog_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            txtLog.Document.Body.ScrollTop = int.MaxValue;
        }

        private void btnSend_Click(object sender , EventArgs e)
        {


            sendMesage(this.txtMsg.Text);
            this.txtMsg.Text = "";
        }
        private void txtMsg_KeyUp(object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendMesage(this.txtMsg.Text);
                this.txtMsg.Text = "";
            }
        }
        public void recieveMesage(String msg)
        {
             txtLog.DocumentText += "<br> <font color='#FF0000'><b>" + DateTime.Now.ToString() + "</b> Recieved Message: </font> " + msg;
            
        }
        public void errorMessage(String msg)
        {
            txtLog.DocumentText += "<br> <font color='#0000FF'><b>" + DateTime.Now.ToString() + "</b> Error : </font> " + msg;
           // txtLog.Document.Body.ScrollIntoView(false);
        }
        public void sendMesage(String msg)
        {
            txtLog.DocumentText += "<br> <font color='#FF0000'><b>" + DateTime.Now.ToString() + "</b> Send Message: </font> " + msg + "";
            Util.covertChannel.sendMessage(msg);
            ETAProgressBar.Maximum = Util.covertChannel.sendQueu.Count;
           
        }

        private void FormMessageControl_FormClosing(object sender , FormClosingEventArgs e)
        {
            this.isVisible = false;
            this.Hide();
            
            e.Cancel = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ETAProgressBar.Value = ETAProgressBar.Maximum - Util.covertChannel.sendQueu.Count;
            
            TimeSpan ETA;
            ETA = new TimeSpan(0,0,0,0, Util.covertChannel.calculateETA());
            lblETA.Text = ETA.ToString();
            
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog frmDial;
            frmDial = new OpenFileDialog();
            frmDial.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            frmDial.FilterIndex = 2;
            frmDial.RestoreDirectory = true;

            if (frmDial.ShowDialog() == DialogResult.OK)
            {
                String fname = frmDial.FileName;
                String msg = System.IO.File.ReadAllText(fname);
                Util.covertChannel.sendMessage(msg);
            }
        }







    }
}
