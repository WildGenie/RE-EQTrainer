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
    public partial class miniToolbar : Form
    {
        public miniToolbar()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void miniToolbar_Load(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            tt.SetToolTip(this.closeBtn, "Close EQTrainer");
            tt.SetToolTip(this.minimizeBtn, "Minimize Window");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.RefToForm1.Close();
            this.Close();
        }

        private void closeSubForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name.Equals("miniToolbar") == false && form.Name.Equals("TrainerForm") == false)
                    form.Visible = false;
            }
        }

        private void toggleSubForms(string btn)
        {
            //if (btn.Equals(btn))
            //{
                FormCollection fc = Application.OpenForms;
                bool closedFrm = false;

                if (fc != null)
                {
                    foreach (Form frm in fc)
                    {
                        if ((frm.Name.Equals("teleForm") && btn.Equals("teleport")) || (frm.Name.Equals("scriptsForm") && btn.Equals("scripts")))
                        {
                            frm.Close();
                            closedFrm = true;
                            break;
                        }
                    }
                }
                
                if (closedFrm == false)
                {
                    if (btn.Equals("teleport"))
                    {
                        teleForm obj3 = new teleForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(tpBtn.Location.X, this.Height);
                    }
                    if (btn.Equals("scripts"))
                    {
                        scriptsForm obj3 = new scriptsForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(scriptsBtn.Location.X, this.Height);
                    }
                }
            //}
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            closeSubForms();
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toggleSubForms("teleport");
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            closeSubForms();
            this.Close();
            this.RefToForm1.WindowState = FormWindowState.Normal;
        }

        private void scriptsBtn_Click(object sender, EventArgs e)
        {
            toggleSubForms("scripts");
        }
    }
}
