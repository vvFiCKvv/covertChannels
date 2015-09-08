namespace covertFiles
{
    partial class FormAbout
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
            this.webAbout = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webAbout
            // 
            this.webAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webAbout.Location = new System.Drawing.Point(0, 0);
            this.webAbout.MinimumSize = new System.Drawing.Size(20, 20);
            this.webAbout.Name = "webAbout";
            this.webAbout.ScrollBarsEnabled = false;
            this.webAbout.Size = new System.Drawing.Size(655, 218);
            this.webAbout.TabIndex = 0;
            this.webAbout.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webAbout_Navigating);
            this.webAbout.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webAbout_DocumentCompleted);
            // 
            // FormAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(655, 218);
            this.Controls.Add(this.webAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.Leave += new System.EventHandler(this.FormAbout_Leave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAbout_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webAbout;
    }
}