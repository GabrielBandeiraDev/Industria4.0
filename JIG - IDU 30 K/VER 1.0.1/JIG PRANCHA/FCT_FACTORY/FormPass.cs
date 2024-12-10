using System;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class FormPass : Form
    {
        Form1 form1;
        public FormPass(Form1 fm1)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            form1 = fm1;
        }


        private void FormPass_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("shutdown.bat");
            this.Close();
        }
    }
}
