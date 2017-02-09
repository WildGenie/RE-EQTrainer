namespace EQTrainer
{
    partial class mapForm
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
            try
            {
                base.Dispose(disposing);
            } catch
            {

            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.trackPlayer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.filterBox = new System.Windows.Forms.TextBox();
            this.TrackBar2 = new System.Windows.Forms.TrackBar();
            this.hideLabels = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(709, 560);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // trackPlayer
            // 
            this.trackPlayer.Location = new System.Drawing.Point(14, 507);
            this.trackPlayer.Name = "trackPlayer";
            this.trackPlayer.Size = new System.Drawing.Size(34, 21);
            this.trackPlayer.TabIndex = 13;
            this.trackPlayer.Text = "(X)";
            this.trackPlayer.UseVisualStyleBackColor = true;
            this.trackPlayer.Click += new System.EventHandler(this.trackPlayer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Tan;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(18, 535);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Filter";
            // 
            // filterBox
            // 
            this.filterBox.Location = new System.Drawing.Point(54, 532);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(142, 20);
            this.filterBox.TabIndex = 11;
            // 
            // TrackBar2
            // 
            this.TrackBar2.BackColor = System.Drawing.Color.Tan;
            this.TrackBar2.LargeChange = 2;
            this.TrackBar2.Location = new System.Drawing.Point(202, 507);
            this.TrackBar2.Maximum = 20;
            this.TrackBar2.Minimum = 1;
            this.TrackBar2.Name = "TrackBar2";
            this.TrackBar2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TrackBar2.Size = new System.Drawing.Size(495, 45);
            this.TrackBar2.TabIndex = 10;
            this.TrackBar2.TickFrequency = 50;
            this.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackBar2.Value = 5;
            this.TrackBar2.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // hideLabels
            // 
            this.hideLabels.AutoSize = true;
            this.hideLabels.BackColor = System.Drawing.Color.Tan;
            this.hideLabels.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.hideLabels.Location = new System.Drawing.Point(54, 510);
            this.hideLabels.Name = "hideLabels";
            this.hideLabels.Size = new System.Drawing.Size(82, 17);
            this.hideLabels.TabIndex = 9;
            this.hideLabels.Text = "Hide Labels";
            this.hideLabels.UseVisualStyleBackColor = false;
            // 
            // mapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(709, 560);
            this.Controls.Add(this.trackPlayer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filterBox);
            this.Controls.Add(this.TrackBar2);
            this.Controls.Add(this.hideLabels);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "mapForm";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map System";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mapForm_FormClosing);
            this.Load += new System.EventHandler(this.mapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button trackPlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filterBox;
        private System.Windows.Forms.TrackBar TrackBar2;
        private System.Windows.Forms.CheckBox hideLabels;
    }
}