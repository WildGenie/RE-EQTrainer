namespace MapsV2
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.hideLabels = new System.Windows.Forms.CheckBox();
            this.TrackBar2 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(574, 462);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_onClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // hideLabels
            // 
            this.hideLabels.AutoSize = true;
            this.hideLabels.BackColor = System.Drawing.Color.Tan;
            this.hideLabels.Location = new System.Drawing.Point(13, 432);
            this.hideLabels.Name = "hideLabels";
            this.hideLabels.Size = new System.Drawing.Size(77, 17);
            this.hideLabels.TabIndex = 1;
            this.hideLabels.Text = "hideLabels";
            this.hideLabels.UseVisualStyleBackColor = false;
            // 
            // TrackBar2
            // 
            this.TrackBar2.BackColor = System.Drawing.Color.Tan;
            this.TrackBar2.LargeChange = 15;
            this.TrackBar2.Location = new System.Drawing.Point(0, 0);
            this.TrackBar2.Maximum = 500;
            this.TrackBar2.Minimum = 15;
            this.TrackBar2.Name = "TrackBar2";
            this.TrackBar2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TrackBar2.Size = new System.Drawing.Size(44, 45);
            this.TrackBar2.SmallChange = 15;
            this.TrackBar2.TabIndex = 5;
            this.TrackBar2.TickFrequency = 50;
            this.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar2.Value = 500;
            this.TrackBar2.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 461);
            this.Controls.Add(this.TrackBar2);
            this.Controls.Add(this.hideLabels);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.Text = "EQ Map System";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox hideLabels;
        private System.Windows.Forms.TrackBar TrackBar2;
    }
}

