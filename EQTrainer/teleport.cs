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
    public partial class teleport : Form
    {
        public teleport()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void teleport_Load(object sender, EventArgs e)
        {
            tele_label1.Text = this.RefToForm1.tele_label1.Text;
        }

        private void openTP1_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button17.PerformClick();
        }

        private void openTP2_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button18.PerformClick();
        }

        private void openTP3_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button19.PerformClick();
        }

        private void openTP4_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button20.PerformClick();
        }

        private void teleportBtn1_Click(object sender, EventArgs e)
        {
            this.RefToForm1.teleportBtn1.PerformClick();
        }

        private void teleportBtn2_Click(object sender, EventArgs e)
        {
            this.RefToForm1.teleportBtn2.PerformClick();
        }

        private void teleportBtn3_Click(object sender, EventArgs e)
        {
            this.RefToForm1.teleportBtn3.PerformClick();
        }

        private void teleportBtn4_Click(object sender, EventArgs e)
        {
            this.RefToForm1.teleportBtn4.PerformClick();
        }
    }
}
