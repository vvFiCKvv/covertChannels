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

    public partial class FormMain : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public extern static int SetParent(int child, int parent);
        private int childFormNumber = 0;

        public FormMain()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender , EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender , EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender , EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender , EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender , EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender , EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender , EventArgs e)
        {
        }

       

        private void CascadeToolStripMenuItem_Click(object sender , EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender , EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender , EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender , EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void FormMain_Load(object sender , EventArgs e)
        {
            Util.MainForm = this;
            Util.MessageForm = new FormMessageControl();
            Util.MessageForm.Show();
            Util.StatForm = new FormStatistics();
            Util.PropertiesForm = new FormProperties();
            Util.AboutForm = null;
            Util.HelpForm = new FormHelp();
            Util.covertChannel.init();
            Util.PropertiesForm.Show();
            Util.StatForm.Show();
            SetParent((int)Util.PropertiesForm.Handle, (int)this.Handle);
            Util.HelpForm.Show();

            this.Resize += new EventHandler(FormMain_Resize);
            
        }

        void FormMain_Resize(object sender, EventArgs e)
        {
            Util.formsIgnoreResizeEvents = true;
            this.Resize -= FormMain_Resize;
            int width = this.Size.Width -15;
            int height = this.Size.Height - 80;
            if (Util.HelpForm.isVisible == false)
            {
                if (Util.StatForm.isVisible == true)
                {
                    if (Util.MessageForm.isVisible == true)
                    {
                        Util.MessageForm.Size = new Size(width, (int)(height * Util.MessageFormPercent.Height));
                    }
                }
                else
                {
                    if (Util.MessageForm.isVisible == true)
                    {
                        Util.MessageForm.Size = new Size(width, height);
                    }
                }
            }
            else
            {
                if (Util.StatForm.isVisible == true)
                {
                    if (Util.MessageForm.isVisible == true)
                    {
                        Util.MessageForm.Size = new Size((int)(width * Util.MessageFormPercent.Width), (int)(height * Util.MessageFormPercent.Height));
                    }
                }
                else
                {
                    if (Util.MessageForm.isVisible == true)
                    {
                        Util.MessageForm.Size = new Size((int)(width * Util.MessageFormPercent.Width), height);
                    }
                }
                if (Util.StatForm.isVisible == false && Util.MessageForm.isVisible == false)
                {
                    if (Util.HelpForm.isVisible == true)
                    {
                        Util.HelpForm.Size = new Size(width, height);
                    }
                }
                else
                {
                    if (Util.HelpForm.isVisible == true)
                    {
                        Util.HelpForm.Size = new Size((int)(width * (1 - Util.MessageFormPercent.Width)), height);
                    }
                }
            }
            
            Util.MessageForm.Location = new Point(0, 0);
            if (Util.MessageForm.isVisible == true)
            {
                if (Util.StatForm.isVisible == true)
                {
                    Util.StatForm.Location = new Point(0, Util.MessageForm.Height);
                    Util.StatForm.Size = new Size(Util.MessageForm.Size.Width, height - Util.MessageForm.Size.Height);
                }
            }
            else
            {
                if (Util.StatForm.isVisible == true)
                {
                    Util.StatForm.Location = new Point(0, 0);
                    Util.StatForm.Size = new Size(Util.MessageForm.Size.Width, height);
                }
            }
            if (Util.StatForm.isVisible == false && Util.MessageForm.isVisible == false)
            {
                if (Util.HelpForm.isVisible == true)
                {
                    Util.HelpForm.Location = new Point(0, 0);
                }
            }
            else
            {
                if (Util.HelpForm.isVisible == true)
                {
                    Util.HelpForm.Location = new Point(Util.MessageForm.Size.Width, 0);
                }
            }
            
            int nx, ny;
            nx = (Util.MessageForm.Size.Width - Util.PropertiesForm.Size.Width) / 2;
            ny=(Height - Util.PropertiesForm.Size.Height)/2;
            if (nx < 2)
                nx = 2;
            if (ny < 24)
                ny = 24;

            Util.PropertiesForm.Location = new Point(nx,ny);
            lblStatus.Text = "" + this.Width + ":" + this.Height;
            this.Resize += new EventHandler(FormMain_Resize);
            Util.formsIgnoreResizeEvents = false;

        }

        private void FormMain_FormClosing(object sender , FormClosingEventArgs e)
        {
            Util.covertChannel.finalize();
            Application.ExitThread();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void toolStripButton1_Click(object sender , EventArgs e)
        {
            Util.PropertiesForm.Show();
            Util.PropertiesForm.Focus();
            SetParent((int)Util.PropertiesForm.Handle, (int)this.Handle);
            
        }

        private void toolStripButton2_Click(object sender , EventArgs e)
        {
            Util.StatForm.isVisible = true;
            Util.StatForm.Show();
            Util.StatForm.Focus();

        }

        private void toolStripButton3_Click(object sender , EventArgs e)
        {
            Util.MessageForm.isVisible = true;
            Util.MessageForm.Show();
            Util.MessageForm.Focus();
        }

        private void covertChannelHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Util.HelpForm.showHelp(FormHelp.HelpId.Main);
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.AboutForm = new FormAbout();
            Util.AboutForm.Show();
            Util.AboutForm.Focus();
            SetParent((int)Util.AboutForm.Handle, (int)this.Handle);
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            Util.HelpForm.showHelp(FormHelp.HelpId.Main);
        }

        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.csd.uoc.gr/~milios/CC/");
        }

        private void checkForUbdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
