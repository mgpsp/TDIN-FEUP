namespace Client
{
    partial class chatRoom
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.onlineUsers = new System.Windows.Forms.ListBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.onlineUsersGroup = new System.Windows.Forms.GroupBox();
            this.selectedUser = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.onlineUsersGroup.SuspendLayout();
            this.selectedUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 290);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(333, 37);
            this.textBox1.TabIndex = 1;
            // 
            // onlineUsers
            // 
            this.onlineUsers.FormattingEnabled = true;
            this.onlineUsers.Location = new System.Drawing.Point(6, 19);
            this.onlineUsers.Name = "onlineUsers";
            this.onlineUsers.Size = new System.Drawing.Size(71, 290);
            this.onlineUsers.TabIndex = 2;
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(448, 290);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(75, 37);
            this.sendBtn.TabIndex = 3;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            // 
            // onlineUsersGroup
            // 
            this.onlineUsersGroup.Controls.Add(this.onlineUsers);
            this.onlineUsersGroup.Location = new System.Drawing.Point(12, 12);
            this.onlineUsersGroup.Name = "onlineUsersGroup";
            this.onlineUsersGroup.Size = new System.Drawing.Size(83, 314);
            this.onlineUsersGroup.TabIndex = 4;
            this.onlineUsersGroup.TabStop = false;
            this.onlineUsersGroup.Text = "Online Users";
            // 
            // selectedUser
            // 
            this.selectedUser.Controls.Add(this.textBox2);
            this.selectedUser.Location = new System.Drawing.Point(101, 12);
            this.selectedUser.Name = "selectedUser";
            this.selectedUser.Size = new System.Drawing.Size(422, 270);
            this.selectedUser.TabIndex = 5;
            this.selectedUser.TabStop = false;
            this.selectedUser.Text = "selectedUser";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(8, 19);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(408, 244);
            this.textBox2.TabIndex = 0;
            // 
            // chatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(536, 338);
            this.Controls.Add(this.selectedUser);
            this.Controls.Add(this.onlineUsersGroup);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.textBox1);
            this.Name = "chatRoom";
            this.Text = "chatRoom";
            this.onlineUsersGroup.ResumeLayout(false);
            this.selectedUser.ResumeLayout(false);
            this.selectedUser.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

      
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox onlineUsers;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.GroupBox onlineUsersGroup;
        private System.Windows.Forms.GroupBox selectedUser;
        private System.Windows.Forms.TextBox textBox2;
    }
}