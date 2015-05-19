using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace EQTrainer
{
    public partial class spawnsForm : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public spawnsForm()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        void setFocus()
        {
            IntPtr hWnd = IntPtr.Zero;
            //if (this.RefToForm1.listView2.Items.Count > 0 && this.RefToForm1.listView2.SelectedItems.Count == 0)
            //{
                string procID = this.RefToForm1.listView2.SelectedItems[0].SubItems[0].Text;
                Process EQProc = Process.GetProcessById(Convert.ToInt32(procID));
                SetForegroundWindow(EQProc.MainWindowHandle);
            //}
        }

        private void spawnsForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonWarpToSpawn_Click(object sender, EventArgs e)
        {
            setFocus();
            this.RefToForm1.buttonWarpToSpawn.PerformClick();
        }

        private void buttonTargetSpawn_Click(object sender, EventArgs e)
        {
            setFocus();
            this.RefToForm1.buttonTargetSpawn.PerformClick();
        }

        private void buttonRefreshSpawnList_Click(object sender, EventArgs e)
        {
            setFocus();
            this.RefToForm1.buttonRefreshSpawnList.PerformClick();
            timer1.Enabled = true;
        }

        private void listViewSpawnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSpawnList.SelectedItems.Count > 0)
            {
                int i = 0;
                foreach (ListViewItem listViewItem in listViewSpawnList.Items)
                {
                    this.RefToForm1.listViewSpawnList.Items[i].Selected = true;
                    i++;
                }
            }
        }

        private void listViewSpawnList_DoubleClick(object sender, EventArgs e)
        {
            if (listViewSpawnList.SelectedItems.Count == 1)
                this.RefToForm1.buttonWarpToSpawn.PerformClick();
        }

        private void textBoxSpawnListFilter_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.textBoxSpawnListFilter.Text = textBoxSpawnListFilter.Text;
        }

        private void textBoxSpawnListFilter_GotFocus(object sender, EventArgs e)
        {
            this.AcceptButton = buttonRefreshSpawnList;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            listViewSpawnList.Items.Clear();
            foreach (ListViewItem listViewItem in this.RefToForm1.listViewSpawnList.Items)
            {
                listViewSpawnList.Items.Add((ListViewItem)listViewItem.Clone());
            }
        }
    }
}
