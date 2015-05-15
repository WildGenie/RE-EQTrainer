namespace EQTrainer
{
    partial class miniToolbar
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tpBtn = new System.Windows.Forms.Button();
            this.scriptsBtn = new System.Windows.Forms.Button();
            this.spawnBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.mapBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.tpBtn);
            this.flowLayoutPanel1.Controls.Add(this.scriptsBtn);
            this.flowLayoutPanel1.Controls.Add(this.spawnBtn);
            this.flowLayoutPanel1.Controls.Add(this.mapBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, -1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1535, 32);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // tpBtn
            // 
            this.tpBtn.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.tpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.tpBtn.Location = new System.Drawing.Point(3, 3);
            this.tpBtn.Name = "tpBtn";
            this.tpBtn.Size = new System.Drawing.Size(95, 25);
            this.tpBtn.TabIndex = 0;
            this.tpBtn.Text = "TELEPORT";
            this.tpBtn.UseVisualStyleBackColor = false;
            this.tpBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // scriptsBtn
            // 
            this.scriptsBtn.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.scriptsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptsBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.scriptsBtn.Location = new System.Drawing.Point(104, 3);
            this.scriptsBtn.Name = "scriptsBtn";
            this.scriptsBtn.Size = new System.Drawing.Size(95, 25);
            this.scriptsBtn.TabIndex = 1;
            this.scriptsBtn.Text = "SCRIPTS";
            this.scriptsBtn.UseVisualStyleBackColor = false;
            this.scriptsBtn.Click += new System.EventHandler(this.scriptsBtn_Click);
            // 
            // spawnBtn
            // 
            this.spawnBtn.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.spawnBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spawnBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.spawnBtn.Location = new System.Drawing.Point(205, 3);
            this.spawnBtn.Name = "spawnBtn";
            this.spawnBtn.Size = new System.Drawing.Size(95, 25);
            this.spawnBtn.TabIndex = 2;
            this.spawnBtn.Text = "SPAWN LIST";
            this.spawnBtn.UseVisualStyleBackColor = false;
            this.spawnBtn.Click += new System.EventHandler(this.spawnBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.Maroon;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.closeBtn.Location = new System.Drawing.Point(1498, 1);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(35, 27);
            this.closeBtn.TabIndex = 4;
            this.closeBtn.Text = "X";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // mapBtn
            // 
            this.mapBtn.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.mapBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.mapBtn.Location = new System.Drawing.Point(306, 3);
            this.mapBtn.Name = "mapBtn";
            this.mapBtn.Size = new System.Drawing.Size(95, 25);
            this.mapBtn.TabIndex = 3;
            this.mapBtn.Text = "MAP SYSTEM";
            this.mapBtn.UseVisualStyleBackColor = false;
            this.mapBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // miniToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1535, 29);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "miniToolbar";
            this.Opacity = 0.85D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EQTrainer Toolbar";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.miniToolbar_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public System.Windows.Forms.Button tpBtn;
        private System.Windows.Forms.Button closeBtn;
        public System.Windows.Forms.Button scriptsBtn;
        public System.Windows.Forms.Button spawnBtn;
        public System.Windows.Forms.Button mapBtn;

    }
}