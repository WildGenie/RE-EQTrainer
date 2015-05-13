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
            nameLabel.Text = this.RefToForm1.name_label.Text;
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
                if (!form.Name.Equals("miniToolbar"))
                {
                    form.Close();
                }
            }
        }

        private void toggleSubForms(string btn)
        {
            if (btn.Equals("teleport"))
            {
                FormCollection fc = Application.OpenForms;
                bool closedFrm = false;

                if (fc != null)
                {
                    foreach (Form frm in fc)
                    {
                        if (frm.Name.Equals("teleport"))
                        {
                            frm.Close();
                            closedFrm = true;
                            break;
                        }
                    }
                }
                
                if (closedFrm == false)
                {
                    teleport obj3 = new teleport();
                    obj3.RefToForm1 = this.RefToForm1;
                    obj3.Show();
                    obj3.Location = new Point(tpBtn.Location.X, this.Height);
                }
            }
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
    }
}
