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
    public partial class FormProperties : Form
    {
        static bool flgStartService = false;
        static bool flgConnect = false;
        System.IntPtr defaultProcessAffinity;
        public FormProperties()
        {
            InitializeComponent();
        }

        private void FormProperties_Load(object sender, EventArgs e)
        {
            this.MdiParent = Util.MainForm;
               
            checkFileOptions.Checked = true;
            checkFileLockOptions.Checked = true;
            txtRemoteHostName.Text = Util.covertChannel.useTCP.remoteHostName;
            txtRemotePort.Text = Util.covertChannel.useTCP.remotePort.ToString();
            txtLocalPort.Text = Util.covertChannel.useTCP.localPort.ToString();
            txtOneBitDelay.Value = Util.covertChannel.useTCP.timeOneBit;
            txtMessageTimeOut.Value = Util.covertChannel.useTCP.messageTimeOut;
            txtZeroBitDelay.Value = Util.covertChannel.useTCP.timeZeroBit;
            tabControl1.TabPages.Remove(tabPageCPUOptions);
            tabControl1.TabPages.Remove(tabPageFileLockOptions);
            tabControl1.TabPages.Remove(tabPageFileSystemOptions);
            tabControl1.TabPages.Remove(tabPageTCPOptions);

            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            defaultProcessAffinity = currentProcess.ProcessorAffinity;

        }

        private void checkFileOptions_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkFileOptions.Checked == true)
            {
                txt0bitFilePath.Enabled = false;
                txt1bitFilePath.Enabled = false;
                txtFilePAth.Enabled = false;
                txtSendFilePAth.Enabled = false;
                btnPath.Enabled = false;
                txtSendAckFilePAth.Enabled = false;
                txt0bitFilePath.Text = Util.covertChannel.useShareFile.DefaultBit0File;
                txt1bitFilePath.Text = Util.covertChannel.useShareFile.DefaultBit1File;
                txtFilePAth.Text = Util.covertChannel.useShareFile.DefaultFilePAth;
                txtSendFilePAth.Text = Util.covertChannel.useShareFile.DefaultSendFile;
                txtSendAckFilePAth.Text = Util.covertChannel.useShareFile.DefaultSendAckFile;

            }
            else
            {
                txt0bitFilePath.Enabled = true;
                txt1bitFilePath.Enabled = true;
                txtFilePAth.Enabled = true;
                txtSendFilePAth.Enabled = true;
                btnPath.Enabled = true;
                txtSendAckFilePAth.Enabled = true;
            }
        }

        private void FormProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!radioCPU.Checked && !radioFileLock.Checked && !radioTCP.Checked && !radioFileSystem.Checked)
            {
                MessageBox.Show("You must select a Covert Chanel Type to continue!!","Error");
                e.Cancel = true;
                return;
            }
            tabControl1.SelectTab(0);
            if (Util.HelpForm.isVisible == true)
                Util.HelpForm.showHelp(FormHelp.HelpId.Main);
            this.Hide();
            e.Cancel = true;
        }



        private void btnStartServer_Click(object sender, EventArgs e)
        {

            if (flgStartService == false)
            {
                Util.covertChannel.useTCP.localPort = Decimal.ToInt32(txtLocalPort.Value);
                Util.covertChannel.useTCP.localStart();
                flgStartService = true;
                btnStartServer.Text = "Stop";
            }
            else
            {
                Util.covertChannel.useTCP.localStop();
                flgStartService = false;
                btnStartServer.Text = "Start";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (flgConnect == false)
            {
                Util.covertChannel.useTCP.remotePort = Decimal.ToInt32(txtRemotePort.Value);
                Util.covertChannel.useTCP.remoteHostName = txtRemoteHostName.Text;
                Util.covertChannel.useTCP.remoteStart();
                btnConnect.Text = "Disconnect";
                flgConnect = true;
            }
            else
            {
                Util.covertChannel.useTCP.remoteStop();
                btnConnect.Text = "Connect";
                flgConnect = false;
            }



        }



        private void txtOneBitDelay_ValueChanged(object sender, EventArgs e)
        {
            txtTimingError.Value= (txtOneBitDelay.Value - txtZeroBitDelay.Value -1)/ 2;
            txtMessageTimeOut.Minimum = txtOneBitDelay.Value + 2*txtTimingError.Value;
        }

        private void txtZeroBitDelay_ValueChanged(object sender, EventArgs e)
        {
            txtTimingError.Value = (txtOneBitDelay.Value - txtZeroBitDelay.Value -1) / 2;
            txtMessageTimeOut.Minimum = txtOneBitDelay.Value + 2 * txtTimingError.Value;
        }

        private void radioTCP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTCP.Checked == true)
            {
                Util.covertChannel.useTCP.start();
                tabControl1.TabPages.Add(tabPageTCPOptions);
            }
            else
            {
                Util.covertChannel.useTCP.stop();
                tabControl1.TabPages.Remove(tabPageTCPOptions);
            }
        }

        private void radioFileLock_CheckedChanged(object sender, EventArgs e)
        {

            if (radioFileLock.Checked == true)
            {
                Util.covertChannel.useFileLock.start();
                tabControl1.TabPages.Add(tabPageFileLockOptions);
            }
            else
            {
                Util.covertChannel.useFileLock.stop();
                tabControl1.TabPages.Remove(tabPageFileLockOptions);
            }
        }

        private void radioCPU_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCPU.Checked == true)
            {
                Util.covertChannel.useSharedCpu.start();
                tabControl1.TabPages.Add(tabPageCPUOptions);
            }
            else
            {
                Util.covertChannel.useSharedCpu.stop();
                tabControl1.TabPages.Remove(tabPageCPUOptions);
            }
        }

        private void radioFileSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFileSystem.Checked == true)
            {
                Util.covertChannel.useShareFile.start();
                tabControl1.TabPages.Add(tabPageFileSystemOptions);
            }
            else
            {
                Util.covertChannel.useShareFile.stop();
                tabControl1.TabPages.Remove(tabPageFileSystemOptions);
            }

        }
        

        private void chkUpnp_CheckedChanged(object sender, EventArgs e)
        {
                
            NATUPNPLib.UPnPNATClass UPnPcfg;
            try
            {
                UPnPcfg = new NATUPNPLib.UPnPNATClass();
                int i = UPnPcfg.DynamicPortMappingCollection.Count;
                if (chkUpnp.Checked == true)
                {

                    UPnPcfg.StaticPortMappingCollection.Add(Util.covertChannel.useTCP.localPort, "TCP", Util.covertChannel.useTCP.localPort, Util.LocalIPAddress(), true, "Covert Chanel");
                }
                else
                {
                    UPnPcfg.StaticPortMappingCollection.Remove(Util.covertChannel.useTCP.localPort, "TCP");
                }

            }
            catch
            {
                chkUpnp_CheckedChanged(sender,e);
            }
           
        }

        private void chkSilentIntervals_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSilentIntervals.Checked == true)
            {
                Util.covertChannel.silentIntervals = (int)txtSilentIntervals.Value;
                txtSilentIntervals.Enabled = true;
            }
            else
            {
                Util.covertChannel.silentIntervals = 0;
                txtSilentIntervals.Enabled = false;
            }

        }

        private void chkHamming_CheckedChanged(object sender, EventArgs e)
        {

            Util.covertChannel.hammingErrorCorrection = chkHamming.Checked;

        }

        private void chkIntervalAdjust_CheckedChanged(object sender, EventArgs e)
        {
            
            if (chkIntervalAdjust.Checked == true)
            {
                Util.covertChannel.intervalAdjusting = (int)txtIntervalAdjust.Value;
                txtIntervalAdjust.Enabled = true;
            }
            else
            {
                Util.covertChannel.silentIntervals = 0;
                txtIntervalAdjust.Enabled = false;
            }
        }

        private void chkStartOfFrame_CheckedChanged(object sender, EventArgs e)
        {
            Util.covertChannel.startOfFrame = chkStartOfFrame.Checked;

        }


        private void txtTimingError_ValueChanged(object sender, EventArgs e)
        {
            txtMessageTimeOut.Minimum = txtOneBitDelay.Value + 2*txtTimingError.Value;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkFileLockOptions.Checked == true)
            {
                txtFileLockName.Enabled = false;
                txtFileLockPath.Enabled = false;
                txtFileLockName.Text = Util.covertChannel.useFileLock.FileName;
                txtFileLockPath.Text = Util.covertChannel.useFileLock.FilePath;

            }
            else
            {
                txtFileLockName.Enabled = true;
                txtFileLockPath.Enabled = true;
            }
        }

        private void chkHamming1_CheckedChanged(object sender, EventArgs e)
        {
            Util.covertChannel.hammingErrorCorrection = chkHamming1.Checked;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            submitChanges();
        }

        private void submitChanges()
        {
            Util.covertChannel.useShareFile.Bit0File = txt0bitFilePath.Text;
            Util.covertChannel.useShareFile.Bit1File = txt1bitFilePath.Text;
            Util.covertChannel.useShareFile.FilesPath = txtFilePAth.Text;
            Util.covertChannel.useShareFile.SendFile = txtSendFilePAth.Text;
            Util.covertChannel.useShareFile.SendAckFile = txtSendAckFilePAth.Text;
            Util.covertChannel.useTCP.timeZeroBit = Decimal.ToInt32(txtZeroBitDelay.Value);
            Util.covertChannel.useTCP.timeOneBit = Decimal.ToInt32(txtOneBitDelay.Value);
            Util.covertChannel.useTCP.messageTimeOut = Decimal.ToInt32(txtMessageTimeOut.Value);
            Util.covertChannel.useTCP.timeError = Decimal.ToInt32(txtTimingError.Value);
            Util.covertChannel.useFileLock.FileName = txtFileLockName.Text;
            Util.covertChannel.useFileLock.FilePath = txtFileLockPath.Text;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            submitChanges();
            this.Close();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.SharedFile);
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.FileLock);
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.SharedCpuUsage);
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.TcpPacketDelay);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.Upnp);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.ZeroBitDelay);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.OneBitDelay);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.MessageTimeOut);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.TimmingError);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.SilentInterval);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.HammingCorrection);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.IntervalAdjust);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.StartOfFrame);
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.RunOneCore);
        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.HammingCorrection);
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.HammingCorrection);
        }

        private void chkRunOneCore_CheckedChanged(object sender, EventArgs e)
        {

            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            
            if (chkRunOneCore.Checked == true)
            {
                currentProcess.ProcessorAffinity = (System.IntPtr)1;
            }
            else
            {
                currentProcess.ProcessorAffinity = defaultProcessAffinity;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Util.HelpForm.isVisible == false)
                return;
            if (tabControl1.SelectedTab.Name == "tabPageFileLockOptions")
            {
                Util.HelpForm.showHelp(FormHelp.HelpId.FileLock);

            }
            else if (tabControl1.SelectedTab.Name == "tabPageFileSystemOptions")
            {
                Util.HelpForm.showHelp(FormHelp.HelpId.SharedFile);

            }
            else if (tabControl1.SelectedTab.Name ==  "tabPageCPUOptions")
            {
                Util.HelpForm.showHelp(FormHelp.HelpId.SharedCpuUsage);

            }
            else if (tabControl1.SelectedTab.Name == "tabPageTCPOptions")
            {
                Util.HelpForm.showHelp(FormHelp.HelpId.TcpPacketDelay);

            }
            else
            {
                Util.HelpForm.showHelp(FormHelp.HelpId.Main);
            }
        }
    }
}
