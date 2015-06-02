namespace AutoBot
{
    partial class Startup
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.scriptBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EQGameIDs = new System.Windows.Forms.ComboBox();
            this.od1 = new System.Windows.Forms.OpenFileDialog();
            this.loopScript = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Green;
            this.button1.Location = new System.Drawing.Point(164, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start AutoBot";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(119, 39);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(177, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // scriptBox
            // 
            this.scriptBox.Location = new System.Drawing.Point(119, 67);
            this.scriptBox.Name = "scriptBox";
            this.scriptBox.Size = new System.Drawing.Size(120, 20);
            this.scriptBox.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(245, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "EQ Build";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "AutoBot Script";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "EQGame Proccess ID";
            // 
            // EQGameIDs
            // 
            this.EQGameIDs.FormattingEnabled = true;
            this.EQGameIDs.Location = new System.Drawing.Point(119, 12);
            this.EQGameIDs.Name = "EQGameIDs";
            this.EQGameIDs.Size = new System.Drawing.Size(177, 21);
            this.EQGameIDs.TabIndex = 6;
            this.EQGameIDs.SelectedIndexChanged += new System.EventHandler(this.EQGameIDs_SelectedIndexChanged);
            // 
            // od1
            // 
            this.od1.DefaultExt = "auto";
            this.od1.Filter = "AutoBot Script Files (*.auto)|*.auto|All files (*.*)|*.*";
            this.od1.FileOk += new System.ComponentModel.CancelEventHandler(this.od1_FileOk);
            // 
            // loopScript
            // 
            this.loopScript.AutoSize = true;
            this.loopScript.Checked = true;
            this.loopScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loopScript.Location = new System.Drawing.Point(54, 94);
            this.loopScript.Name = "loopScript";
            this.loopScript.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.loopScript.Size = new System.Drawing.Size(80, 17);
            this.loopScript.TabIndex = 8;
            this.loopScript.Text = "Loop Script";
            this.loopScript.UseVisualStyleBackColor = true;
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 148);
            this.Controls.Add(this.loopScript);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EQGameIDs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.scriptBox);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Startup";
            this.ShowIcon = false;
            this.Text = "AutoBot Startup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Startup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox scriptBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox EQGameIDs;
        private System.Windows.Forms.OpenFileDialog od1;
        private System.Windows.Forms.CheckBox loopScript;
    }
}