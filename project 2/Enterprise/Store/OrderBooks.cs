using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store
{
    public partial class OrderBooks : Form
    {
        public int quantity;
        public OrderBooks()
        {
            InitializeComponent();
            numericUpDown1.Value = 1;
            quantity = 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            quantity = (int)numericUpDown1.Value;
        }

        private void orderBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
