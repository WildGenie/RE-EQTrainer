namespace EQTrainer
{
    partial class scriptsForm
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
            this.components = new System.ComponentModel.Container();
            this.buttonAllScriptsDisabled = new System.Windows.Forms.Button();
            this.buttonAllScriptsEnabled = new System.Windows.Forms.Button();
            this.checkBoxScripts = new System.Windows.Forms.CheckBox();
            this.listViewScripts = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEnable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDisable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // buttonAllScriptsDisabled
            // 
            this.buttonAllScriptsDisabled.Location = new System.Drawing.Point(427, 39);
            this.buttonAllScriptsDisabled.Name = "buttonAllScriptsDisabled";
            this.buttonAllScriptsDisabled.Size = new System.Drawing.Size(128, 24);
            this.buttonAllScriptsDisabled.TabIndex = 541;
            this.buttonAllScriptsDisabled.Text = "All Scripts Disabled";
            this.buttonAllScriptsDisabled.UseVisualStyleBackColor = true;
            this.buttonAllScriptsDisabled.Click += new System.EventHandler(this.buttonAllScriptsDisabled_Click);
            // 
            // buttonAllScriptsEnabled
            // 
            this.buttonAllScriptsEnabled.Location = new System.Drawing.Point(293, 39);
            this.buttonAllScriptsEnabled.Name = "buttonAllScriptsEnabled";
            this.buttonAllScriptsEnabled.Size = new System.Drawing.Size(128, 24);
            this.buttonAllScriptsEnabled.TabIndex = 540;
            this.buttonAllScriptsEnabled.Text = "All Scripts Enabled";
            this.buttonAllScriptsEnabled.UseVisualStyleBackColor = true;
            this.buttonAllScriptsEnabled.Click += new System.EventHandler(this.buttonAllScriptsEnabled_Click);
            // 
            // checkBoxScripts
            // 
            this.checkBoxScripts.AutoSize = true;
            this.checkBoxScripts.Checked = true;
            this.checkBoxScripts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScripts.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBoxScripts.Location = new System.Drawing.Point(219, 46);
            this.checkBoxScripts.Name = "checkBoxScripts";
            this.checkBoxScripts.Size = new System.Drawing.Size(58, 17);
            this.checkBoxScripts.TabIndex = 539;
            this.checkBoxScripts.Text = "Scripts";
            this.checkBoxScripts.UseVisualStyleBackColor = true;
            // 
            // listViewScripts
            // 
            this.listViewScripts.CheckBoxes = true;
            this.listViewScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderDescription,
            this.columnHeaderEnable,
            this.columnHeaderDisable});
            this.listViewScripts.FullRowSelect = true;
            this.listViewScripts.GridLines = true;
            this.listViewScripts.HideSelection = false;
            this.listViewScripts.Location = new System.Drawing.Point(12, 68);
            this.listViewScripts.Name = "listViewScripts";
            this.listViewScripts.Size = new System.Drawing.Size(543, 201);
            this.listViewScripts.TabIndex = 538;
            this.listViewScripts.UseCompatibleStateImageBehavior = false;
            this.listViewScripts.View = System.Windows.Forms.View.Details;
            this.listViewScripts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewScripts_ItemCheck);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 171;
            // 
            // columnHeaderDescription
            // 
            this.columnHeaderDescription.Text = "Description";
            this.columnHeaderDescription.Width = 306;
            // 
            // columnHeaderEnable
            // 
            this.columnHeaderEnable.Text = "Enable";
            this.columnHeaderEnable.Width = 20;
            // 
            // columnHeaderDisable
            // 
            this.columnHeaderDisable.Text = "Disable";
            this.columnHeaderDisable.Width = 21;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // scriptsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(566, 278);
            this.Controls.Add(this.buttonAllScriptsDisabled);
            this.Controls.Add(this.buttonAllScriptsEnabled);
            this.Controls.Add(this.checkBoxScripts);
            this.Controls.Add(this.listViewScripts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "scriptsForm";
            this.Opacity = 0.85D;
            this.ShowInTaskbar = false;
            this.Text = "scriptsForm";
            this.Load += new System.EventHandler(this.scriptsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAllScriptsDisabled;
        private System.Windows.Forms.Button buttonAllScriptsEnabled;
        private System.Windows.Forms.CheckBox checkBoxScripts;
        private System.Windows.Forms.ListView listViewScripts;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderDescription;
        private System.Windows.Forms.ColumnHeader columnHeaderEnable;
        private System.Windows.Forms.ColumnHeader columnHeaderDisable;
        private System.Windows.Forms.Timer timer1;
    }
}