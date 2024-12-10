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
    public partial class frmEmergencia : Form
    {

        Form1 instanciaDoForm1;
        public frmEmergencia(Form1 form_1)
        {
            InitializeComponent();
            instanciaDoForm1 = form_1;
            timer1.Start();
        }

        private void frmEmergencia_Load(object sender, EventArgs e)
        {
            //
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (instanciaDoForm1.instanciaEmerg == false)
            {
                this.Close();
            }
        }
    }
}
