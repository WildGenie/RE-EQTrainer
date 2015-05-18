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

        public miniToolbar()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        void setFocus()
        {
            IntPtr hWnd = IntPtr.Zero;
            //if (this.RefToForm1.listView2.Items.Count > 0 && this.RefToForm1.listView2.SelectedItems.Count == 0)
            //{
                string procID = this.RefToForm1.listView2.SelectedItems[0].SubItems[1].Text;
                Process EQProc = Process.GetProcessById(Convert.ToInt32(procID));
                SetForegroundWindow(EQProc.MainWindowHandle);
            //}
        }

        private void miniToolbar_Load(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            tt.SetToolTip(this.closeBtn, "Close Toolbar");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setFocus();
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
                    try
                    {
                        setFocus();
                    }
                    catch
                    {
                    }
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
    }
}
