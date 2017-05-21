namespace Warehouse
{
    partial class Warehouse
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
            this.ordersList = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.filters = new System.Windows.Forms.ComboBox();
            this.shipBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ordersList
            // 
            this.ordersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader2,
            this.columnHeader3});
            this.ordersList.Location = new System.Drawing.Point(12, 38);
            this.ordersList.Name = "ordersList";
            this.ordersList.Size = new System.Drawing.Size(582, 311);
            this.ordersList.TabIndex = 0;
            this.ordersList.UseCompatibleStateImageBehavior = false;
            this.ordersList.View = System.Windows.Forms.View.Details;
            this.ordersList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ordersList_ItemSelectionChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 1;
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 292;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Text = "Quantity";
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 3;
            this.columnHeader3.Text = "Status";
            this.columnHeader3.Width = 168;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(438, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter";
            // 
            // filters
            // 
            this.filters.FormattingEnabled = true;
            this.filters.Items.AddRange(new object[] {
            "All",
            "Waiting expedition",
            "Dispatched"});
            this.filters.Location = new System.Drawing.Point(473, 11);
            this.filters.Name = "filters";
            this.filters.Size = new System.Drawing.Size(121, 21);
            this.filters.TabIndex = 2;
            this.filters.SelectedIndexChanged += new System.EventHandler(this.filters_SelectedIndexChanged);
            // 
            // shipBtn
            // 
            this.shipBtn.Enabled = false;
            this.shipBtn.Location = new System.Drawing.Point(519, 355);
            this.shipBtn.Name = "shipBtn";
            this.shipBtn.Size = new System.Drawing.Size(75, 23);
            this.shipBtn.TabIndex = 3;
            this.shipBtn.Text = "Ship order";
            this.shipBtn.UseVisualStyleBackColor = true;
            this.shipBtn.Click += new System.EventHandler(this.shipBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Orders";
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 0;
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 31;
            // 
            // Warehouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 384);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.shipBtn);
            this.Controls.Add(this.filters);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ordersList);
            this.Name = "Warehouse";
            this.Text = "Warehouse";
            this.Load += new System.EventHandler(this.Warehouse_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ordersList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox filters;
        private System.Windows.Forms.Button shipBtn;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}

