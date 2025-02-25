namespace ManagePassword
{
    partial class ManagePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagePassword));
            this.dgvDB = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tbAddopen = new System.Windows.Forms.TextBox();
            this.tbAddsecret = new System.Windows.Forms.TextBox();
            this.lblOS = new System.Windows.Forms.Label();
            this.lblSS = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDelid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tbsecretFind = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbopenFind = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblSelectedId = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.tbChangeSecret = new System.Windows.Forms.TextBox();
            this.tbChangeOpen = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnAdmMode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDB)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDB
            // 
            this.dgvDB.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDB.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvDB.Location = new System.Drawing.Point(12, 12);
            this.dgvDB.Name = "dgvDB";
            this.dgvDB.Size = new System.Drawing.Size(338, 509);
            this.dgvDB.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(67, 89);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(288, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(21, 46);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(55, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tbAddopen
            // 
            this.tbAddopen.Location = new System.Drawing.Point(67, 40);
            this.tbAddopen.Name = "tbAddopen";
            this.tbAddopen.Size = new System.Drawing.Size(288, 20);
            this.tbAddopen.TabIndex = 3;
            // 
            // tbAddsecret
            // 
            this.tbAddsecret.Location = new System.Drawing.Point(67, 63);
            this.tbAddsecret.Name = "tbAddsecret";
            this.tbAddsecret.Size = new System.Drawing.Size(288, 20);
            this.tbAddsecret.TabIndex = 4;
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Location = new System.Drawing.Point(23, 43);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(43, 13);
            this.lblOS.TabIndex = 5;
            this.lblOS.Text = "Service";
            // 
            // lblSS
            // 
            this.lblSS.AutoSize = true;
            this.lblSS.Location = new System.Drawing.Point(13, 66);
            this.lblSS.Name = "lblSS";
            this.lblSS.Size = new System.Drawing.Size(53, 13);
            this.lblSS.TabIndex = 6;
            this.lblSS.Text = "Password";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleGreen;
            this.panel1.Controls.Add(this.tbAddsecret);
            this.panel1.Controls.Add(this.lblSS);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblOS);
            this.panel1.Controls.Add(this.tbAddopen);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(357, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 121);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(163, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Add";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.IndianRed;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.tbDelid);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Location = new System.Drawing.Point(357, 419);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(87, 73);
            this.panel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "id";
            // 
            // tbDelid
            // 
            this.tbDelid.Location = new System.Drawing.Point(21, 20);
            this.tbDelid.Name = "tbDelid";
            this.tbDelid.Size = new System.Drawing.Size(55, 20);
            this.tbDelid.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(21, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Delete";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel3.Controls.Add(this.tbsecretFind);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.tbopenFind);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(357, 140);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(358, 117);
            this.panel3.TabIndex = 10;
            // 
            // tbsecretFind
            // 
            this.tbsecretFind.Location = new System.Drawing.Point(66, 60);
            this.tbsecretFind.Name = "tbsecretFind";
            this.tbsecretFind.Size = new System.Drawing.Size(288, 20);
            this.tbsecretFind.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(162, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Find";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Password";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(65, 86);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(289, 23);
            this.btnFind.TabIndex = 11;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbopenFind
            // 
            this.tbopenFind.Location = new System.Drawing.Point(66, 37);
            this.tbopenFind.Name = "tbopenFind";
            this.tbopenFind.Size = new System.Drawing.Size(288, 20);
            this.tbopenFind.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Service";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(357, 498);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(358, 23);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gold;
            this.panel4.Controls.Add(this.lblSelectedId);
            this.panel4.Controls.Add(this.btnChange);
            this.panel4.Controls.Add(this.tbChangeSecret);
            this.panel4.Controls.Add(this.tbChangeOpen);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(357, 263);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(358, 148);
            this.panel4.TabIndex = 13;
            // 
            // lblSelectedId
            // 
            this.lblSelectedId.AutoSize = true;
            this.lblSelectedId.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectedId.Location = new System.Drawing.Point(11, 35);
            this.lblSelectedId.Name = "lblSelectedId";
            this.lblSelectedId.Size = new System.Drawing.Size(132, 26);
            this.lblSelectedId.TabIndex = 14;
            this.lblSelectedId.Text = "Selected id: ";
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(66, 114);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(287, 26);
            this.btnChange.TabIndex = 14;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // tbChangeSecret
            // 
            this.tbChangeSecret.Location = new System.Drawing.Point(66, 88);
            this.tbChangeSecret.Name = "tbChangeSecret";
            this.tbChangeSecret.Size = new System.Drawing.Size(288, 20);
            this.tbChangeSecret.TabIndex = 13;
            // 
            // tbChangeOpen
            // 
            this.tbChangeOpen.Location = new System.Drawing.Point(65, 64);
            this.tbChangeOpen.Name = "tbChangeOpen";
            this.tbChangeOpen.Size = new System.Drawing.Size(288, 20);
            this.tbChangeOpen.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(90, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(214, 25);
            this.label7.TabIndex = 0;
            this.label7.Text = "Select And Change";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Password";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Service";
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(450, 419);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(127, 73);
            this.btnInfo.TabIndex = 15;
            this.btnInfo.Text = "Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnAdmMode
            // 
            this.btnAdmMode.Location = new System.Drawing.Point(580, 419);
            this.btnAdmMode.Name = "btnAdmMode";
            this.btnAdmMode.Size = new System.Drawing.Size(132, 73);
            this.btnAdmMode.TabIndex = 16;
            this.btnAdmMode.Text = "Admin Mode";
            this.btnAdmMode.UseVisualStyleBackColor = true;
            this.btnAdmMode.Click += new System.EventHandler(this.btnAdmMode_Click);
            // 
            // ManagePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 529);
            this.Controls.Add(this.btnAdmMode);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvDB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManagePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ManagePassword";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDB)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDB;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox tbAddopen;
        private System.Windows.Forms.TextBox tbAddsecret;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.Label lblSS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDelid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbsecretFind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbopenFind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblSelectedId;
        private System.Windows.Forms.TextBox tbChangeSecret;
        private System.Windows.Forms.TextBox tbChangeOpen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnAdmMode;
    }
}

