using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace covertFiles
{
    class CCSListView : System.Windows.Forms.ListView
    {
        private int sortedId;
        private enum direction { ASCENDING , DESCENDING };
        private direction dir;
        private ListBox listBox1;

        private List<ColumnFormat> ColumnFormatList = null;

        public enum ColumnFormat { String , Number };


        public CCSListView()
        {
            //  this.ColumnReordered;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint , true);
            this.SetStyle(ControlStyles.EnableNotifyMessage , true);
            ColumnFormatList = new List<ColumnFormat>();
            sortedId = 0;
            dir = direction.ASCENDING;

            this.SmallImageList = new ImageList();

            // this.SmallImageList.Images.Add("ASCENDING", AppStatus.Properties.Resources.sortAscending);
            //this.SmallImageList.Images.Add("DESCENDING", AppStatus.Properties.Resources.sortDescending);
            listBox1 = new ListBox();
            listBox1.Anchor = AnchorStyles.Bottom;
            listBox1.ResumeLayout(false);
           // this.Controls.Add(listBox1);
            System.ComponentModel.IContainer components;
            components = new System.ComponentModel.Container();
            components.Add(listBox1);
            

        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            this.Columns[sortedId].ImageKey = "";
            if (e.Column == sortedId)
            {
                if (dir == direction.ASCENDING)
                {
                    dir = direction.DESCENDING;
                }
                else
                {
                    dir = direction.ASCENDING;
                }
            }
            else
            {
                dir = direction.ASCENDING;
                sortedId = e.Column;
            }
            if (dir == direction.ASCENDING)
            {
                this.Columns[e.Column].ImageKey = "ASCENDING";
            }
            else
            {
                this.Columns[e.Column].ImageKey = "DESCENDING";
            }
            sortList();

            base.OnColumnClick(e);
        }

        public void sortList()
        {
            List<ListViewItem> tempList = new List<ListViewItem>();
            int i;

            for (i = 0; i < Items.Count; i++)
            {
                tempList.Add(Items[i]);
            }
            Items.Clear();

            for (i = 0; i < tempList.Count; i++)
            {
                this.addSorted(tempList[i]);
            }
        }


        public void addColumn(String _text , ColumnFormat _format)
        {
            this.Columns.Add(_text);
            this.ColumnFormatList.Add(_format);
        }


        public void addSorted(ListViewItem toAdd)
        {

            int i;

            for (i = 0; i < this.Items.Count; i++)
            {
                float dif = 0;

                if (ColumnFormatList[sortedId] == ColumnFormat.Number)
                {
                    float A , B;
                    A = float.Parse(Items[i].SubItems[sortedId].Text);
                    B = float.Parse(toAdd.SubItems[sortedId].Text);
                    dif = A - B;
                }
                else if (ColumnFormatList[sortedId] == ColumnFormat.String)
                {
                    String A , B;
                    A = Items[i].SubItems[sortedId].Text;
                    B = toAdd.SubItems[sortedId].Text;
                    dif = String.Compare(A , B);
                }


                if (dir == direction.DESCENDING)
                {
                    dif = -dif;
                }


                if (dif >= 0)
                {
                    Items.Insert(i , toAdd);
                    break;
                }
            }

            //if the item is not added in the for loop, add it in the end of the list.
            if (i == Items.Count)
            {
                Items.Add(toAdd);
            }
        }

        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 96);
            this.listBox1.TabIndex = 0;
            // 
            // CCSListView
            // 
            this.AllowColumnReorder = true;
            this.AllowDrop = true;
            this.ResumeLayout(false);

        }
    }
}
