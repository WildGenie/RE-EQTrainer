using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EQTrainer
{
    public partial class settingsForm : Form
    {
        public TrainerForm RefToForm1 { get; set; }

        public settingsForm()
        {
            InitializeComponent();
            old_warp.Checked = Properties.Settings.Default.old_warp;
            no_coords.Checked = Properties.Settings.Default.no_coords;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.old_warp = old_warp.Checked;
            Properties.Settings.Default.Save();
            if (this.Visible)
            {
                ToolTip tt = new ToolTip();
                if (old_warp.Checked)
                {
                    this.RefToForm1.button1.Enabled = false;
                    tt.SetToolTip(this.RefToForm1.button1, "Old warp enabled. Inject happens when you teleport.");
                }
                else
                {
                    this.RefToForm1.button1.Enabled = true;
                    tt.SetToolTip(this.RefToForm1.button1, "Inject DLL to use teleporting.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Good for older computers that can't handle named pipes. C++ Redistributables (x86) is required to use this. If you had this option on before opening eqtrainer and want to turn it off, you will need to re-inject the dll or re-open eqtrainer.");
        }

        private void settingsForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Disables the popup when your teleport coordinates are blank.");
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.no_coords = no_coords.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
