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
    public partial class miniToolbar : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public miniToolbar()
        {
            InitializeComponent();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void miniToolbar_Load(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            tt.SetToolTip(this.closeBtn, "Close Toolbar");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.RefToForm1.MemLib.setFocus();
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
                        if (frm.Name.Equals(btn))
                        {
                            frm.Close();
                            closedFrm = true;
                            break;
                        }
                    }
                }
                
                if (closedFrm == false)
                {
                    if (btn.Equals("teleForm"))
                    {
                        teleForm obj3 = new teleForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(tpBtn.Location.X, this.Height);
                    }
                    if (btn.Equals("scriptsForm"))
                    {
                        scriptsForm obj3 = new scriptsForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(scriptsBtn.Location.X, this.Height);
                    }
                    if (btn.Equals("spawnsForm"))
                    {
                        spawnsForm obj3 = new spawnsForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(spawnBtn.Location.X, this.Height);
                    }
                    if (btn.Equals("mapForm"))
                    {
                        mapForm obj3 = new mapForm();
                        obj3.RefToForm1 = this.RefToForm1;
                        obj3.Show();
                        obj3.Location = new Point(mapBtn.Location.X, this.Height);
                    }
                }
                try
                {
                    this.RefToForm1.MemLib.setFocus();
                }
                catch
                {
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
            toggleSubForms("teleForm");
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            closeSubForms();
            this.Close();
            this.RefToForm1.WindowState = FormWindowState.Normal;
        }

        private void scriptsBtn_Click(object sender, EventArgs e)
        {
            toggleSubForms("scriptsForm");
        }

        private void spawnBtn_Click(object sender, EventArgs e)
        {
            toggleSubForms("spawnsForm");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            toggleSubForms("mapForm");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                hpProgressBar.Invoke(new MethodInvoker(delegate { hpProgressBar.Value = this.RefToForm1.progressBarHP.Value; }));
                SendMessage(hpProgressBar.Handle, 1040, 2, 0);
                hpProgressBar.CreateGraphics().DrawString(this.RefToForm1.progressBarHP.Value + "%", new Font("Arial", (float)8), Brushes.Black, new PointF(hpProgressBar.Width / 2 - 10, hpProgressBar.Height / 2 - 7));

                mpProgressBar.Invoke(new MethodInvoker(delegate { mpProgressBar.Value = this.RefToForm1.progressBarMP.Value; }));
                SendMessage(mpProgressBar.Handle, 1040, 0, 0);
                mpProgressBar.CreateGraphics().DrawString(this.RefToForm1.progressBarMP.Value + "%", new Font("Arial", (float)8), Brushes.Black, new PointF(mpProgressBar.Width / 2 - 10, mpProgressBar.Height / 2 - 7));

                xpProgressBar.Invoke(new MethodInvoker(delegate { xpProgressBar.Value = this.RefToForm1.progressBarXP.Value; }));
                SendMessage(xpProgressBar.Handle, 1040, 3, 0);
                xpProgressBar.CreateGraphics().DrawString(this.RefToForm1.progressBarXP.Value + "%", new Font("Arial", (float)8), Brushes.Black, new PointF(xpProgressBar.Width / 2 - 10, xpProgressBar.Height / 2 - 7));
            }
        }
    }
}
