namespace EQTrainer
{
    partial class spawnsForm
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
            this.buttonWarpToSpawn = new System.Windows.Forms.Button();
            this.textBoxSpawnListFilter = new System.Windows.Forms.TextBox();
            this.labelSpawnListFilter = new System.Windows.Forms.Label();
            this.buttonTargetSpawn = new System.Windows.Forms.Button();
            this.buttonRefreshSpawnList = new System.Windows.Forms.Button();
            this.listViewSpawnList = new System.Windows.Forms.ListView();
            this.columnHeaderSpawnListName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListZ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListHeading = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSpawnListType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // buttonWarpToSpawn
            // 
            this.buttonWarpToSpawn.Location = new System.Drawing.Point(291, 453);
            this.buttonWarpToSpawn.Name = "buttonWarpToSpawn";
            this.buttonWarpToSpawn.Size = new System.Drawing.Size(128, 24);
            this.buttonWarpToSpawn.TabIndex = 577;
            this.buttonWarpToSpawn.Text = "Warp to Spawn";
            this.buttonWarpToSpawn.UseVisualStyleBackColor = true;
            this.buttonWarpToSpawn.Click += new System.EventHandler(this.buttonWarpToSpawn_Click);
            // 
            // textBoxSpawnListFilter
            // 
            this.textBoxSpawnListFilter.AcceptsReturn = true;
            this.textBoxSpawnListFilter.Location = new System.Drawing.Point(50, 20);
            this.textBoxSpawnListFilter.Name = "textBoxSpawnListFilter";
            this.textBoxSpawnListFilter.Size = new System.Drawing.Size(433, 20);
            this.textBoxSpawnListFilter.TabIndex = 576;
            this.textBoxSpawnListFilter.TextChanged += new System.EventHandler(this.textBoxSpawnListFilter_TextChanged);
            this.textBoxSpawnListFilter.GotFocus += new System.EventHandler(this.textBoxSpawnListFilter_GotFocus);
            // 
            // labelSpawnListFilter
            // 
            this.labelSpawnListFilter.AutoSize = true;
            this.labelSpawnListFilter.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelSpawnListFilter.Location = new System.Drawing.Point(12, 23);
            this.labelSpawnListFilter.Name = "labelSpawnListFilter";
            this.labelSpawnListFilter.Size = new System.Drawing.Size(32, 13);
            this.labelSpawnListFilter.TabIndex = 575;
            this.labelSpawnListFilter.Text = "Filter:";
            // 
            // buttonTargetSpawn
            // 
            this.buttonTargetSpawn.Location = new System.Drawing.Point(426, 453);
            this.buttonTargetSpawn.Name = "buttonTargetSpawn";
            this.buttonTargetSpawn.Size = new System.Drawing.Size(128, 24);
            this.buttonTargetSpawn.TabIndex = 573;
            this.buttonTargetSpawn.Text = "Target Spawn";
            this.buttonTargetSpawn.UseVisualStyleBackColor = true;
            this.buttonTargetSpawn.Click += new System.EventHandler(this.buttonTargetSpawn_Click);
            // 
            // buttonRefreshSpawnList
            // 
            this.buttonRefreshSpawnList.Location = new System.Drawing.Point(489, 19);
            this.buttonRefreshSpawnList.Name = "buttonRefreshSpawnList";
            this.buttonRefreshSpawnList.Size = new System.Drawing.Size(65, 24);
            this.buttonRefreshSpawnList.TabIndex = 572;
            this.buttonRefreshSpawnList.Text = "SEARCH";
            this.buttonRefreshSpawnList.UseVisualStyleBackColor = true;
            this.buttonRefreshSpawnList.Click += new System.EventHandler(this.buttonRefreshSpawnList_Click);
            // 
            // listViewSpawnList
            // 
            this.listViewSpawnList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSpawnListName,
            this.columnHeaderSpawnListAddress,
            this.columnHeaderSpawnListX,
            this.columnHeaderSpawnListY,
            this.columnHeaderSpawnListZ,
            this.columnHeaderSpawnListHeading,
            this.columnHeaderSpawnListLevel,
            this.columnHeaderSpawnListClass,
            this.columnHeaderSpawnListType});
            this.listViewSpawnList.FullRowSelect = true;
            this.listViewSpawnList.GridLines = true;
            this.listViewSpawnList.HideSelection = false;
            this.listViewSpawnList.Location = new System.Drawing.Point(12, 47);
            this.listViewSpawnList.MultiSelect = false;
            this.listViewSpawnList.Name = "listViewSpawnList";
            this.listViewSpawnList.Size = new System.Drawing.Size(542, 400);
            this.listViewSpawnList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewSpawnList.TabIndex = 571;
            this.listViewSpawnList.UseCompatibleStateImageBehavior = false;
            this.listViewSpawnList.View = System.Windows.Forms.View.Details;
            this.listViewSpawnList.SelectedIndexChanged += new System.EventHandler(this.listViewSpawnList_SelectedIndexChanged);
            this.listViewSpawnList.DoubleClick += new System.EventHandler(this.listViewSpawnList_DoubleClick);
            // 
            // columnHeaderSpawnListName
            // 
            this.columnHeaderSpawnListName.Text = "Name";
            this.columnHeaderSpawnListName.Width = 172;
            // 
            // columnHeaderSpawnListAddress
            // 
            this.columnHeaderSpawnListAddress.Text = "Address";
            this.columnHeaderSpawnListAddress.Width = 52;
            // 
            // columnHeaderSpawnListX
            // 
            this.columnHeaderSpawnListX.Text = "X";
            this.columnHeaderSpawnListX.Width = 52;
            // 
            // columnHeaderSpawnListY
            // 
            this.columnHeaderSpawnListY.Text = "Y";
            this.columnHeaderSpawnListY.Width = 52;
            // 
            // columnHeaderSpawnListZ
            // 
            this.columnHeaderSpawnListZ.Text = "Z";
            this.columnHeaderSpawnListZ.Width = 50;
            // 
            // columnHeaderSpawnListHeading
            // 
            this.columnHeaderSpawnListHeading.Text = "Heading";
            this.columnHeaderSpawnListHeading.Width = 53;
            // 
            // columnHeaderSpawnListLevel
            // 
            this.columnHeaderSpawnListLevel.Text = "Lvl";
            this.columnHeaderSpawnListLevel.Width = 29;
            // 
            // columnHeaderSpawnListClass
            // 
            this.columnHeaderSpawnListClass.Text = "Class";
            this.columnHeaderSpawnListClass.Width = 39;
            // 
            // columnHeaderSpawnListType
            // 
            this.columnHeaderSpawnListType.Text = "Type";
            this.columnHeaderSpawnListType.Width = 39;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // spawnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(560, 485);
            this.Controls.Add(this.buttonWarpToSpawn);
            this.Controls.Add(this.textBoxSpawnListFilter);
            this.Controls.Add(this.labelSpawnListFilter);
            this.Controls.Add(this.buttonTargetSpawn);
            this.Controls.Add(this.buttonRefreshSpawnList);
            this.Controls.Add(this.listViewSpawnList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "spawnsForm";
            this.ShowInTaskbar = false;
            this.Text = "spawnsForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.spawnsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWarpToSpawn;
        private System.Windows.Forms.TextBox textBoxSpawnListFilter;
        private System.Windows.Forms.Label labelSpawnListFilter;
        private System.Windows.Forms.Button buttonTargetSpawn;
        private System.Windows.Forms.Button buttonRefreshSpawnList;
        private System.Windows.Forms.ListView listViewSpawnList;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListName;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListAddress;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListX;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListY;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListZ;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListHeading;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListLevel;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListClass;
        private System.Windows.Forms.ColumnHeader columnHeaderSpawnListType;
        private System.Windows.Forms.Timer timer1;
    }
}