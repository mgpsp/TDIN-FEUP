using System.Collections;
namespace Client
{
    partial class ChatRoom
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.msgToSend = new System.Windows.Forms.TextBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.onlineUsersGroup = new System.Windows.Forms.GroupBox();
            this.onlineUsers = new System.Windows.Forms.ListView();
            this.selectedUser = new System.Windows.Forms.GroupBox();
            this.conversation = new System.Windows.Forms.TextBox();
            this.onlineUsersGroup.SuspendLayout();
            this.selectedUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgToSend
            // 
            this.msgToSend.Location = new System.Drawing.Point(145, 290);
            this.msgToSend.Multiline = true;
            this.msgToSend.Name = "msgToSend";
            this.msgToSend.Size = new System.Drawing.Size(333, 37);
            this.msgToSend.TabIndex = 1;
            // 
            // sendBtn
            // 
            this.sendBtn.Location = new System.Drawing.Point(484, 290);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(75, 37);
            this.sendBtn.TabIndex = 3;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // onlineUsersGroup
            // 
            this.onlineUsersGroup.Controls.Add(this.onlineUsers);
            this.onlineUsersGroup.Location = new System.Drawing.Point(12, 12);
            this.onlineUsersGroup.Name = "onlineUsersGroup";
            this.onlineUsersGroup.Size = new System.Drawing.Size(119, 314);
            this.onlineUsersGroup.TabIndex = 4;
            this.onlineUsersGroup.TabStop = false;
            this.onlineUsersGroup.Text = "Online Users";
            // 
            // onlineUsers
            // 
            this.onlineUsers.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.onlineUsers.Location = new System.Drawing.Point(7, 20);
            this.onlineUsers.Name = "onlineUsers";
            this.onlineUsers.Size = new System.Drawing.Size(106, 288);
            this.onlineUsers.TabIndex = 0;
            this.onlineUsers.UseCompatibleStateImageBehavior = false;
            this.onlineUsers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.onlineUsers_ItemSelectionChanged);
            // 
            // selectedUser
            // 
            this.selectedUser.Controls.Add(this.conversation);
            this.selectedUser.Location = new System.Drawing.Point(137, 12);
            this.selectedUser.Name = "selectedUser";
            this.selectedUser.Size = new System.Drawing.Size(422, 270);
            this.selectedUser.TabIndex = 5;
            this.selectedUser.TabStop = false;
            this.selectedUser.Text = "selectedUser";
            // 
            // conversation
            // 
            this.conversation.Location = new System.Drawing.Point(8, 19);
            this.conversation.Multiline = true;
            this.conversation.Name = "conversation";
            this.conversation.Size = new System.Drawing.Size(408, 244);
            this.conversation.TabIndex = 0;
            // 
            // ChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(574, 338);
            this.Controls.Add(this.selectedUser);
            this.Controls.Add(this.onlineUsersGroup);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.msgToSend);
            this.Name = "ChatRoom";
            this.Text = "Chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatRoom_FormClosed);
            this.onlineUsersGroup.ResumeLayout(false);
            this.selectedUser.ResumeLayout(false);
            this.selectedUser.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.TextBox msgToSend;
        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.GroupBox onlineUsersGroup;
        private System.Windows.Forms.GroupBox selectedUser;
        private System.Windows.Forms.TextBox conversation;

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

        private System.Windows.Forms.ListView onlineUsers;
    }
}