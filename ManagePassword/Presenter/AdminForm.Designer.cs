﻿namespace ManagePassword
{
    partial class AdminForm
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
            this.tbAdmin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnterToAdminMode = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReg = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbAdmin
            // 
            this.tbAdmin.Location = new System.Drawing.Point(104, 12);
            this.tbAdmin.Name = "tbAdmin";
            this.tbAdmin.PasswordChar = '*';
            this.tbAdmin.Size = new System.Drawing.Size(318, 20);
            this.tbAdmin.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Master-Password";
            // 
            // btnEnterToAdminMode
            // 
            this.btnEnterToAdminMode.Location = new System.Drawing.Point(266, 38);
            this.btnEnterToAdminMode.Name = "btnEnterToAdminMode";
            this.btnEnterToAdminMode.Size = new System.Drawing.Size(75, 23);
            this.btnEnterToAdminMode.TabIndex = 2;
            this.btnEnterToAdminMode.Text = "Enter";
            this.btnEnterToAdminMode.UseVisualStyleBackColor = true;
            this.btnEnterToAdminMode.Click += new System.EventHandler(this.btnEnterToAdminMode_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(185, 38);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(104, 38);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReg
            // 
            this.btnReg.Location = new System.Drawing.Point(23, 38);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(75, 23);
            this.btnReg.TabIndex = 5;
            this.btnReg.Text = "Register";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(347, 38);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 71);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnReg);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnEnterToAdminMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAdmin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AdminForm";
            this.Text = "AdminMode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnterToAdminMode;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox tbAdmin;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.Button btnDel;
    }
}