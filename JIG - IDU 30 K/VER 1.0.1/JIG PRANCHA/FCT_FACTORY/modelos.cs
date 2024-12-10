using System;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class modelos : Form
    {
        Form1 instanciaDoFormMain;
        public modelos(Form1 _form1)
        {
            InitializeComponent();
            instanciaDoFormMain = _form1;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public string modelo;
        private void modelos_Load(object sender, EventArgs e)
        {
            //
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //modelo = "PRIMO II 127V";
            //instanciaDoFormMain.modeloSelecionado = modelo;
            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //modelo = "PRIMO II 220V";
            //instanciaDoFormMain.modeloSelecionado = modelo;
            //this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //modelo = "PRIMO II BIVOLT";
            //instanciaDoFormMain.modeloSelecionado = modelo;
            //this.Close();
        }
    }
}
