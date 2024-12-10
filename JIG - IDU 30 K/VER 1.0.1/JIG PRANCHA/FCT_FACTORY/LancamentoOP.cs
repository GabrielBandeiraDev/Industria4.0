using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class LancamentoOP : Form
    {
        public LancamentoOP()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboDefeito.Enabled = true; 
        }

        private void LancamentoOP_Load(object sender, EventArgs e)
        {
            comboDefeito.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
    }
}
