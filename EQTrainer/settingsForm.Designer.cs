namespace EQTrainer
{
    partial class settingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.old_warp = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.no_coords = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // old_warp
            // 
            this.old_warp.AutoSize = true;
            this.old_warp.Location = new System.Drawing.Point(13, 13);
            this.old_warp.Name = "old_warp";
            this.old_warp.Size = new System.Drawing.Size(104, 17);
            this.old_warp.TabIndex = 0;
            this.old_warp.Text = "old warp method";
            this.old_warp.UseVisualStyleBackColor = true;
            this.old_warp.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(19, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // no_coords
            // 
            this.no_coords.AutoSize = true;
            this.no_coords.Location = new System.Drawing.Point(13, 37);
            this.no_coords.Name = "no_coords";
            this.no_coords.Size = new System.Drawing.Size(174, 17);
            this.no_coords.TabIndex = 2;
            this.no_coords.Text = "no popup w/ coordinates blank";
            this.no_coords.UseVisualStyleBackColor = true;
            this.no_coords.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(193, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(19, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "?";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // settingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 76);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.no_coords);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.old_warp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settingsForm";
            this.ShowIcon = false;
            this.Text = "EQTrainer Settings";
            this.Load += new System.EventHandler(this.settingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox old_warp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox no_coords;
        private System.Windows.Forms.Button button2;
    }
}