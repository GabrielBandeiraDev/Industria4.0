using System;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class FAIL : Form
    {
        Form1 instanciaDoFormMain;
        public FAIL(Form1 formMain)
        {
            instanciaDoFormMain = formMain;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void FAIL_Load(object sender, EventArgs e)
        {

        }
    }
}
