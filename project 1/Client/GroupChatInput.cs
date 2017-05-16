using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class GroupChatInput : Form
    {
        public GroupChatInput()
        {
            InitializeComponent();
            confirmBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
