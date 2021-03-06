﻿using System;
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
    public partial class scriptsForm : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public scriptsForm()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void scriptsForm_Load(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in this.RefToForm1.listViewScripts.Items)
            {
                listViewScripts.Items.Add((ListViewItem)listViewItem.Clone());
            }
        }

        private void buttonAllScriptsEnabled_Click(object sender, EventArgs e)
        {
            this.RefToForm1.MemLib.setFocus();
            this.RefToForm1.buttonAllScriptsEnabled.PerformClick();
            //timer1.Enabled = true;
            listViewScripts.Items.Clear();
            foreach (ListViewItem listViewItem in this.RefToForm1.listViewScripts.Items)
            {
                listViewScripts.Items.Add((ListViewItem)listViewItem.Clone());
            }
        }

        private void buttonAllScriptsDisabled_Click(object sender, EventArgs e)
        {
            this.RefToForm1.MemLib.setFocus();
            this.RefToForm1.buttonAllScriptsDisabled.PerformClick();
            //timer1.Enabled = true;
            listViewScripts.Items.Clear();
            foreach (ListViewItem listViewItem in this.RefToForm1.listViewScripts.Items)
            {
                listViewScripts.Items.Add((ListViewItem)listViewItem.Clone());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.RefToForm1.listViewScripts.Items.Clear();
            foreach (ListViewItem listViewItem in listViewScripts.Items)
            {
                //this.RefToForm1.checkBoxScripts.Checked = checkBoxScripts.Checked;
                this.RefToForm1.listViewScripts.Items.Add((ListViewItem)listViewItem.Clone());
            }
        }

        private void listViewScripts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // When ItemCheck is activated, it goes too fast.
            timer1.Enabled = true;
        }
    }
}
