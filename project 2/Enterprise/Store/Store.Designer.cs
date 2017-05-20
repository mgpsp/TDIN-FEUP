namespace Store
{
    partial class Store
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
            this.label1 = new System.Windows.Forms.Label();
            this.booksList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectedBookPanel = new System.Windows.Forms.Panel();
            this.bookPrice = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.bookCover = new System.Windows.Forms.PictureBox();
            this.bookStock = new System.Windows.Forms.Label();
            this.bookYear = new System.Windows.Forms.Label();
            this.authorName = new System.Windows.Forms.Label();
            this.bookName = new System.Windows.Forms.Label();
            this.orderBtn = new System.Windows.Forms.Button();
            this.sellBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ordersList = new System.Windows.Forms.ListView();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.selectedBookPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookCover)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Books";
            // 
            // booksList
            // 
            this.booksList.Location = new System.Drawing.Point(13, 30);
            this.booksList.Name = "booksList";
            this.booksList.Size = new System.Drawing.Size(176, 289);
            this.booksList.TabIndex = 1;
            this.booksList.UseCompatibleStateImageBehavior = false;
            this.booksList.View = System.Windows.Forms.View.List;
            this.booksList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.booksList_ItemSelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.selectedBookPanel);
            this.groupBox1.Location = new System.Drawing.Point(195, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 306);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected book";
            // 
            // selectedBookPanel
            // 
            this.selectedBookPanel.Controls.Add(this.bookPrice);
            this.selectedBookPanel.Controls.Add(this.label8);
            this.selectedBookPanel.Controls.Add(this.bookCover);
            this.selectedBookPanel.Controls.Add(this.bookStock);
            this.selectedBookPanel.Controls.Add(this.bookYear);
            this.selectedBookPanel.Controls.Add(this.authorName);
            this.selectedBookPanel.Controls.Add(this.bookName);
            this.selectedBookPanel.Controls.Add(this.orderBtn);
            this.selectedBookPanel.Controls.Add(this.sellBtn);
            this.selectedBookPanel.Controls.Add(this.label5);
            this.selectedBookPanel.Controls.Add(this.label4);
            this.selectedBookPanel.Controls.Add(this.label3);
            this.selectedBookPanel.Controls.Add(this.label2);
            this.selectedBookPanel.Location = new System.Drawing.Point(7, 20);
            this.selectedBookPanel.Name = "selectedBookPanel";
            this.selectedBookPanel.Size = new System.Drawing.Size(200, 280);
            this.selectedBookPanel.TabIndex = 0;
            // 
            // bookPrice
            // 
            this.bookPrice.AutoSize = true;
            this.bookPrice.Location = new System.Drawing.Point(55, 206);
            this.bookPrice.Name = "bookPrice";
            this.bookPrice.Size = new System.Drawing.Size(0, 13);
            this.bookPrice.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(18, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Price:";
            // 
            // bookCover
            // 
            this.bookCover.Location = new System.Drawing.Point(13, 1);
            this.bookCover.Name = "bookCover";
            this.bookCover.Size = new System.Drawing.Size(174, 125);
            this.bookCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bookCover.TabIndex = 10;
            this.bookCover.TabStop = false;
            // 
            // bookStock
            // 
            this.bookStock.AutoSize = true;
            this.bookStock.Location = new System.Drawing.Point(55, 190);
            this.bookStock.Name = "bookStock";
            this.bookStock.Size = new System.Drawing.Size(0, 13);
            this.bookStock.TabIndex = 9;
            // 
            // bookYear
            // 
            this.bookYear.AutoSize = true;
            this.bookYear.Location = new System.Drawing.Point(55, 173);
            this.bookYear.Name = "bookYear";
            this.bookYear.Size = new System.Drawing.Size(0, 13);
            this.bookYear.TabIndex = 8;
            // 
            // authorName
            // 
            this.authorName.AutoSize = true;
            this.authorName.Location = new System.Drawing.Point(55, 155);
            this.authorName.Name = "authorName";
            this.authorName.Size = new System.Drawing.Size(0, 13);
            this.authorName.TabIndex = 7;
            // 
            // bookName
            // 
            this.bookName.AutoSize = true;
            this.bookName.Location = new System.Drawing.Point(55, 137);
            this.bookName.Name = "bookName";
            this.bookName.Size = new System.Drawing.Size(0, 13);
            this.bookName.TabIndex = 6;
            // 
            // orderBtn
            // 
            this.orderBtn.Enabled = false;
            this.orderBtn.Location = new System.Drawing.Point(35, 253);
            this.orderBtn.Name = "orderBtn";
            this.orderBtn.Size = new System.Drawing.Size(130, 23);
            this.orderBtn.TabIndex = 5;
            this.orderBtn.Text = "Order from warehouse";
            this.orderBtn.UseVisualStyleBackColor = true;
            // 
            // sellBtn
            // 
            this.sellBtn.Enabled = false;
            this.sellBtn.Location = new System.Drawing.Point(35, 227);
            this.sellBtn.Name = "sellBtn";
            this.sellBtn.Size = new System.Drawing.Size(130, 23);
            this.sellBtn.TabIndex = 4;
            this.sellBtn.Text = "Sell";
            this.sellBtn.UseVisualStyleBackColor = true;
            this.sellBtn.Click += new System.EventHandler(this.sellBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Stock:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Year:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Author:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(434, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Pending orders";
            // 
            // ordersList
            // 
            this.ordersList.Location = new System.Drawing.Point(437, 30);
            this.ordersList.Name = "ordersList";
            this.ordersList.Size = new System.Drawing.Size(171, 260);
            this.ordersList.TabIndex = 4;
            this.ordersList.UseCompatibleStateImageBehavior = false;
            this.ordersList.View = System.Windows.Forms.View.List;
            // 
            // acceptBtn
            // 
            this.acceptBtn.Location = new System.Drawing.Point(437, 296);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(171, 23);
            this.acceptBtn.TabIndex = 5;
            this.acceptBtn.Text = "Accept order";
            this.acceptBtn.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(423, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1, 306);
            this.label7.TabIndex = 6;
            // 
            // Store
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 334);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.ordersList);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.booksList);
            this.Controls.Add(this.label1);
            this.Name = "Store";
            this.Text = "Store";
            this.Load += new System.EventHandler(this.Store_Load);
            this.groupBox1.ResumeLayout(false);
            this.selectedBookPanel.ResumeLayout(false);
            this.selectedBookPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookCover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView booksList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel selectedBookPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button orderBtn;
        private System.Windows.Forms.Button sellBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView ordersList;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label bookStock;
        private System.Windows.Forms.Label bookYear;
        private System.Windows.Forms.Label authorName;
        private System.Windows.Forms.Label bookName;
        private System.Windows.Forms.PictureBox bookCover;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label bookPrice;
    }
}

