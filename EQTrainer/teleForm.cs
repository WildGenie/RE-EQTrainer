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
    public partial class teleForm : Form
    {
        public teleForm()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void teleport_Load(object sender, EventArgs e)
        {
            tele_label1.Text = this.RefToForm1.tele_label1.Text;
            tele_label2.Text = this.RefToForm1.tele_label2.Text;
            tele_label3.Text = this.RefToForm1.tele_label3.Text;
            tele_label4.Text = this.RefToForm1.tele_label4.Text;
            x_tele.Text = this.RefToForm1.x_tele.Text;
            x_tele2.Text = this.RefToForm1.x_tele2.Text;
            x_tele3.Text = this.RefToForm1.x_tele3.Text;
            x_tele4.Text = this.RefToForm1.x_tele4.Text;
            y_tele.Text = this.RefToForm1.y_tele.Text;
            y_tele2.Text = this.RefToForm1.y_tele2.Text;
            y_tele3.Text = this.RefToForm1.y_tele3.Text;
            y_tele4.Text = this.RefToForm1.y_tele4.Text;
            z_tele.Text = this.RefToForm1.z_tele.Text;
            z_tele2.Text = this.RefToForm1.z_tele2.Text;
            z_tele3.Text = this.RefToForm1.z_tele3.Text;
            z_tele4.Text = this.RefToForm1.z_tele4.Text;
            h_tele1.Text = this.RefToForm1.h_tele1.Text;
            h_tele2.Text = this.RefToForm1.h_tele2.Text;
            h_tele3.Text = this.RefToForm1.h_tele3.Text;
            h_tele4.Text = this.RefToForm1.h_tele4.Text;
        }

        private void openTP1_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button17.PerformClick();
            timer1.Enabled = true;
        }

        private void openTP2_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button18.PerformClick();
            timer1.Enabled = true;
        }

        private void openTP3_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button19.PerformClick();
            timer1.Enabled = true;
        }

        private void openTP4_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button20.PerformClick();
            timer1.Enabled = true;
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

        private void tele_label1_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.tele_label1.Text = tele_label1.Text;
        }

        private void tele_label2_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.tele_label2.Text = tele_label2.Text;
        }

        private void tele_label3_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.tele_label3.Text = tele_label3.Text;
        }

        private void tele_label4_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.tele_label4.Text = tele_label4.Text;
        }

        private void x_tele_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.x_tele.Text = x_tele.Text;
        }

        private void x_tele2_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.x_tele2.Text = x_tele2.Text;
        }

        private void x_tele3_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.x_tele3.Text = x_tele3.Text;
        }

        private void x_tele4_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.x_tele4.Text = x_tele4.Text;
        }

        private void y_tele_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.y_tele.Text = y_tele.Text;
        }

        private void y_tele2_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.y_tele2.Text = y_tele2.Text;
        }

        private void y_tele3_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.y_tele3.Text = y_tele3.Text;
        }

        private void y_tele4_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.y_tele4.Text = y_tele4.Text;
        }

        private void z_tele_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.z_tele.Text = z_tele.Text;
        }

        private void z_tele2_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.z_tele2.Text = z_tele2.Text;
        }

        private void z_tele3_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.z_tele3.Text = z_tele3.Text;
        }

        private void z_tele4_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.z_tele4.Text = z_tele4.Text;
        }

        private void h_tele1_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.h_tele1.Text = h_tele1.Text;
            timer1.Enabled = false;
        }

        private void h_tele2_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.h_tele2.Text = h_tele2.Text;
            timer1.Enabled = false;
        }

        private void h_tele3_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.h_tele3.Text = h_tele3.Text;
            timer1.Enabled = false;
        }

        private void h_tele4_TextChanged(object sender, EventArgs e)
        {
            this.RefToForm1.h_tele4.Text = h_tele4.Text;
            timer1.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button5.PerformClick();
            timer1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button6.PerformClick();
            timer1.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button7.PerformClick();
            timer1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button8.PerformClick();
            timer1.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button9.PerformClick();
            timer1.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button10.PerformClick();
            timer1.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button11.PerformClick();
            timer1.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.RefToForm1.button12.PerformClick();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tele_label1.Text = this.RefToForm1.tele_label1.Text;
            tele_label2.Text = this.RefToForm1.tele_label2.Text;
            tele_label3.Text = this.RefToForm1.tele_label3.Text;
            tele_label4.Text = this.RefToForm1.tele_label4.Text;
            x_tele.Text = this.RefToForm1.x_tele.Text;
            x_tele2.Text = this.RefToForm1.x_tele2.Text;
            x_tele3.Text = this.RefToForm1.x_tele3.Text;
            x_tele4.Text = this.RefToForm1.x_tele4.Text;
            y_tele.Text = this.RefToForm1.y_tele.Text;
            y_tele2.Text = this.RefToForm1.y_tele2.Text;
            y_tele3.Text = this.RefToForm1.y_tele3.Text;
            y_tele4.Text = this.RefToForm1.y_tele4.Text;
            z_tele.Text = this.RefToForm1.z_tele.Text;
            z_tele2.Text = this.RefToForm1.z_tele2.Text;
            z_tele3.Text = this.RefToForm1.z_tele3.Text;
            z_tele4.Text = this.RefToForm1.z_tele4.Text;
            h_tele1.Text = this.RefToForm1.h_tele1.Text;
            h_tele2.Text = this.RefToForm1.h_tele2.Text;
            h_tele3.Text = this.RefToForm1.h_tele3.Text;
            h_tele4.Text = this.RefToForm1.h_tele4.Text;
        }
    }
}
