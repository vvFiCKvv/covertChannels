using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace covertFiles
{
    public partial class FormHelp : Form
    {
        public enum HelpId
        {
            FileLock,
            SharedFile,
            SharedCpuUsage,
            TcpPacketDelay,
            Upnp,
            ZeroBitDelay,
            OneBitDelay,
            MessageTimeOut,
            TimmingError,
            SilentInterval,
            HammingCorrection,
            IntervalAdjust,
            StartOfFrame,
            RunOneCore,
            Main
            
        };
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
        public FormHelp()
        {
            InitializeComponent();
        }

        public void showHelp(HelpId type)
        {
            if(Util.HelpForm.isVisible==false)
                Util.HelpForm.isVisible = true;
            if(type == HelpId.FileLock)
            {
                webHelp.DocumentText = covertFiles.Resources.FileLock;
            }else if(type == HelpId.HammingCorrection)
            {
                webHelp.DocumentText = covertFiles.Resources.HammingCorrection;
            }
            else if (type == HelpId.IntervalAdjust)
            {
                webHelp.DocumentText = covertFiles.Resources.IntervalAdjust;
            }
            else if (type == HelpId.MessageTimeOut)
            {
                webHelp.DocumentText = covertFiles.Resources.MessageTimeOut;
            }
            else if (type == HelpId.OneBitDelay)
            {
                webHelp.DocumentText = covertFiles.Resources.OneBitDelay;
            }
            else if (type == HelpId.RunOneCore)
            {
                webHelp.DocumentText = covertFiles.Resources.RunOneCore;
            }
            else if (type == HelpId.SharedCpuUsage)
            {
                webHelp.DocumentText = covertFiles.Resources.SharedCpuUsage;
            }
            else if (type == HelpId.SharedFile)
            {
                webHelp.DocumentText = covertFiles.Resources.SharedFile;
            }
            else if (type == HelpId.SilentInterval)
            {
                webHelp.DocumentText = covertFiles.Resources.SilentInterval;
            }
            else if (type == HelpId.StartOfFrame)
            {
                webHelp.DocumentText = covertFiles.Resources.StartOfFrame;
            }
            else if (type == HelpId.TcpPacketDelay)
            {
                webHelp.DocumentText = covertFiles.Resources.TcpPacketDelay;
            }
            else if (type == HelpId.TimmingError)
            {
                webHelp.DocumentText = covertFiles.Resources.TimmingError;
            }
            else if (type == HelpId.Upnp)
            {
                webHelp.DocumentText = covertFiles.Resources.Upnp;
            }
            else if (type == HelpId.ZeroBitDelay)
            {
                webHelp.DocumentText = covertFiles.Resources.ZeroBitDelay;
            }
            else
            {
                webHelp.DocumentText = covertFiles.Resources.helpMain;
            }
            
            this.Show();
            this.Focus();

        }
            
        private void FormHelp_Load(object sender, EventArgs e)
        {
            this.MdiParent = Util.MainForm;
            webHelp.DocumentText = covertFiles.Resources.helpMain;
            this.Resize += new EventHandler(FormHelp_Resize);
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
                    this.Resize -= FormHelp_Resize;
                    
                }
                else
                {
                    this.Resize += new EventHandler(FormHelp_Resize);
                }
            }

        }
        void FormHelp_Resize(object sender, EventArgs e)
        {
            Util.formsIgnoreResizeEvents = true;
            Util.MessageFormPercent = new SizeF(1 - this.Width / (float)(Util.MainForm.Width - 15), Util.MessageFormPercent.Height);
            Util.formsIgnoreResizeEvents = false;
        }

        private void FormHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Util.HelpForm.isVisible = false;
            this.Hide();
            e.Cancel = true;
        }

        private void webHelp_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

            if (e.Url.Scheme == "res")
            {
                e.Cancel = true;
                if(e.Url.Authority == "filelock")
                {
                    showHelp(HelpId.FileLock);
                }else if(e.Url.Authority == "hammingcorrection")
                {
                    showHelp(HelpId.HammingCorrection);
                }else if(e.Url.Authority == "intervaladjust")
                {
                    showHelp(HelpId.IntervalAdjust);
                }else if(e.Url.Authority == "main")
                {
                    showHelp(HelpId.Main);
                }else if(e.Url.Authority == "messagetimeout")
                {
                    showHelp(HelpId.MessageTimeOut);
                }else if(e.Url.Authority == "onebitdelay")
                {
                    showHelp(HelpId.OneBitDelay);
                }else if(e.Url.Authority == "runonecore")
                {
                    showHelp(HelpId.RunOneCore);
                }else if(e.Url.Authority == "sharedcpuusage")
                {
                    showHelp(HelpId.SharedCpuUsage);
                }else if(e.Url.Authority == "sharedfile")
                {
                    showHelp(HelpId.SharedFile);
                }else if(e.Url.Authority == "silentinterval")
                {
                    showHelp(HelpId.SilentInterval);
                }else if(e.Url.Authority == "startofframe")
                {
                    showHelp(HelpId.StartOfFrame);
                }else if(e.Url.Authority == "tcppacketdelay")
                {
                    showHelp(HelpId.TcpPacketDelay);
                }else if(e.Url.Authority == "timmingerror")
                {
                    showHelp(HelpId.TimmingError);
                }else if(e.Url.Authority == "upnp")
                {
                    showHelp(HelpId.Upnp);
                }else if(e.Url.Authority == "zerobitdelay")
                {
                    showHelp(HelpId.ZeroBitDelay);
                }
            }
            
        }

        private void webHelp_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
