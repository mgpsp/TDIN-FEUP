namespace Client
{
    partial class GroupChatInput
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
            this.groupChatNameLabel = new System.Windows.Forms.Label();
            this.groupChatName = new System.Windows.Forms.TextBox();
            this.confirmBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupChatNameLabel
            // 
            this.groupChatNameLabel.AutoSize = true;
            this.groupChatNameLabel.Location = new System.Drawing.Point(37, 16);
            this.groupChatNameLabel.Name = "groupChatNameLabel";
            this.groupChatNameLabel.Size = new System.Drawing.Size(35, 13);
            this.groupChatNameLabel.TabIndex = 0;
            this.groupChatNameLabel.Text = "Name";
            // 
            // groupChatName
            // 
            this.groupChatName.Location = new System.Drawing.Point(78, 13);
            this.groupChatName.Name = "groupChatName";
            this.groupChatName.Size = new System.Drawing.Size(100, 20);
            this.groupChatName.TabIndex = 1;
            // 
            // confirmBtn
            // 
            this.confirmBtn.Location = new System.Drawing.Point(29, 44);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmBtn.TabIndex = 2;
            this.confirmBtn.Text = "Create";
            this.confirmBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(115, 44);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // GroupChatInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 79);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.confirmBtn);
            this.Controls.Add(this.groupChatName);
            this.Controls.Add(this.groupChatNameLabel);
            this.Name = "GroupChatInput";
            this.Text = "New group chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label groupChatNameLabel;
        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.Button cancelBtn;
        public System.Windows.Forms.TextBox groupChatName;
    }
}