using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using ChartDirector;

namespace covertFiles
{
    public partial class FormStatistics : Form
    {

       
        private Timer MainTimer;
        ChartControl chartLayer;
       
        public bool isRunning = true;
        private int LogPageData;



        public FormStatistics()
        {
            InitializeComponent();
            init();
        }
        #region init
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
        private void init()
        {

            chartLayer = new ChartControl();
            chartLayer.Interval = 1000;
            chartLayer.timeAtXAxis = true;
            lstLogInit();
            lstItemsInit();
            MainTimerInit();
            tabPage2.Controls.Add(chartLayer);

        }




        private void MainTimerInit()
        {

            MainTimer = new Timer();
            MainTimer.Interval = 1000;
            MainTimer.Tick += new EventHandler(logUpdater);
            MainTimer.Tick += new EventHandler(statisticUpdater);

            MainTimer.Start();
        }

        private void lstItemsInit()
        {
            chartLayer.add("Received Bits Per Second", new ChartDataLayer.UpdateFunction(delegate(ChartDataLayer me) { me.update(double.Parse(txtReceivedBitPSec.Text)); }));
            chartLayer.add("Sent Bits Per Second", new ChartDataLayer.UpdateFunction(delegate(ChartDataLayer me) { me.update( double.Parse(txtSendBitPSec.Text)); }));
            chartLayer.add("Total Received Bits",  new ChartDataLayer.UpdateFunction(delegate(ChartDataLayer me) { me.update( double.Parse(txtReceivedTotalBits.Text)); }));
            chartLayer.add("Total Sent Bits",  new ChartDataLayer.UpdateFunction(delegate(ChartDataLayer me) { me.update( double.Parse(txtSendTotalBits.Text)); }));
            chartLayer.Start();
        }
        private void lstLogInit()
        {
            lstLog.addColumn("  #", CCSListView.ColumnFormat.Number);
            lstLog.addColumn("Time", CCSListView.ColumnFormat.String);
            lstLog.addColumn("Source", CCSListView.ColumnFormat.String);
            lstLog.addColumn("Type", CCSListView.ColumnFormat.String);
            lstLog.addColumn("Value", CCSListView.ColumnFormat.String);
            lstLog.addColumn("Comment", CCSListView.ColumnFormat.String);
            lstLog.Columns[0].Width = 50;
            lstLog.Columns[1].Width = 100;
            lstLog.Columns[2].Width = 70;
            lstLog.Columns[3].Width = 70;
            lstLog.Columns[4].Width = 70;
            lstLog.Columns[5].Width = 200;
            LogPageData = 10;
        }
        #endregion

        private void logAddToList(Util.covertChannel.Statistics.LogEntry item, int index)
        {
            String[] args = new String[6];

            args[0] = index.ToString();
            args[1] = item.Time.ToString("HH:mm:ss:fff");
            args[2] = item.Source.ToString();
            args[3] = item.InstanceType.ToString();
            args[4] = item.Value.ToString();
            args[5] = item.Comment;
            ListViewItem tmpItem = new ListViewItem(args);
            if (txtFilter.Text == "")
            {
                lstLog.addSorted(tmpItem);
            }
            else
            {
                String fargs;
                fargs = String.Join("|", args);
                if (fargs.ToLower().Contains(txtFilter.Text.ToLower()))
                {
                    lstLog.addSorted(tmpItem);
                }
            }
        }
        void logUpdater(object sender, EventArgs e)
        {
            int i;
            i = cmpLogPage.Items.Count;
            if (i - 1 == cmpLogPage.SelectedIndex)
            {
                cmpLogPage_SelectedIndexChanged(sender, e);
            }
            while ((LogPageData * i) < Util.covertChannel.Statistics.LogFile.Count || (i - 1) > Util.covertChannel.Statistics.LogFile.Count / LogPageData)
            {
                i = cmpLogPage.Items.Count;
                if ((LogPageData * i) < Util.covertChannel.Statistics.LogFile.Count)
                {
                    cmpLogPage.Items.Add("Page #" + i);
                }
                if ((i - 1) > Util.covertChannel.Statistics.LogFile.Count / LogPageData)
                {
                    cmpLogPage.Items.RemoveAt(cmpLogPage.Items.Count - 1);
                }

            }
            if (chkFollowLogSream.Checked == true)
            {

                cmpLogPage.SelectedIndex = cmpLogPage.Items.Count - 1;
            }
        }
        void statisticUpdater(object sender, EventArgs e)
        {
            txtReceivedBitPSec.Text = (Util.covertChannel.Statistics.bitReceivedCount - long.Parse(txtReceivedTotalBits.Text)).ToString();
            txtSendBitPSec.Text = (Util.covertChannel.Statistics.bitSentCount - long.Parse(txtSendTotalBits.Text)).ToString();
            txtReceivedTotalBits.Text = Util.covertChannel.Statistics.bitReceivedCount.ToString();
            txtSendTotalBits.Text = Util.covertChannel.Statistics.bitSentCount.ToString();
            txtReceivedZeroBits.Text = Util.covertChannel.Statistics.zeroBitReceivedCount.ToString();
            txtSendZeroBit.Text = Util.covertChannel.Statistics.zeroBitSendCount.ToString();
            txtReceivedOneBits.Text = "" + (Util.covertChannel.Statistics.bitReceivedCount - Util.covertChannel.Statistics.zeroBitReceivedCount);
            txtSendOneBit.Text = "" + (Util.covertChannel.Statistics.bitSentCount - Util.covertChannel.Statistics.zeroBitSendCount);
            if (Util.covertChannel.Statistics.bitReceivedCount != 0)
            {
                txtReceivedZeroBitsPc.Text = (100 * Util.covertChannel.Statistics.zeroBitReceivedCount) / Util.covertChannel.Statistics.bitReceivedCount + "%";
                txtReceivedOneBitsPc.Text = 100 - (100 * Util.covertChannel.Statistics.zeroBitReceivedCount) / Util.covertChannel.Statistics.bitReceivedCount + "%";
                txtReceivedHamBitsPc.Text = (100 * Util.covertChannel.Statistics.ErrorBitReceivedCount) / Util.covertChannel.Statistics.bitReceivedCount + "%";
                
            }
            if (Util.covertChannel.Statistics.bitSentCount != 0)
            {
                txtSendZeroBitPc.Text = (100 * Util.covertChannel.Statistics.zeroBitSendCount) / Util.covertChannel.Statistics.bitSentCount + "%";
                txtSendOneBitPc.Text = 100 - (100 * Util.covertChannel.Statistics.zeroBitSendCount) / Util.covertChannel.Statistics.bitSentCount + "%";

            }
            txtReceivedHamBits.Text = "" + Util.covertChannel.Statistics.ErrorBitReceivedCount;

        }

        #region Events
        private void FormStatistics_Load(object sender, EventArgs e)
        {
            this.MdiParent = Util.MainForm;
            this.Resize += new EventHandler(FormStatistics_Resize);

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
                    this.Resize -= FormStatistics_Resize;
                    
                }
                else
                {
                    this.Resize += new EventHandler(FormStatistics_Resize);
                }
            }

        }
        void FormStatistics_Resize(object sender, EventArgs e)
        {
            Util.formsIgnoreResizeEvents = true;
            Util.MessageFormPercent = new SizeF((float)this.Width / (float)(Util.MainForm.Width - 15), 1 - (float)this.Height / (float)(Util.MainForm.Height - 80));
            Util.formsIgnoreResizeEvents = false;
        }

        private void FormStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.isVisible = false;
            e.Cancel = true;

        }


       
        #endregion

        private void cmpLogPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            int index = cmpLogPage.SelectedIndex;
            if (index < 0) return;
            Util.covertChannel.Statistics.LogEntry tmp;
            int szmin, szmax;
            lstLog.Items.Clear();


            szmin = (index) * LogPageData;
            szmax = (index + 1) * LogPageData;
            for (i = szmin; i < szmax && i < Util.covertChannel.Statistics.LogFile.Count; i++)
            {
                tmp = (Util.covertChannel.Statistics.LogEntry)Util.covertChannel.Statistics.LogFile[i];

                logAddToList(tmp, i);

            }
        }

        private void txtItemsPerPAge_ValueChanged(object sender, EventArgs e)
        {
            LogPageData = (int)txtItemsPerPage.Value;
            cmpLogPage_SelectedIndexChanged(sender, e);
        }

        private void chkFollowLogSream_CheckedChanged(object sender, EventArgs e)
        {
            cmpLogPage.Enabled = !chkFollowLogSream.Checked;
        }

     



    }
}
