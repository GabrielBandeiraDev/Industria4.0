using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace FCT_FACTORY
{
    public partial class Serial : Form
    {
        Form1 instanciaDoForm1;
        public Serial(Form1 fm1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            instanciaDoForm1 = fm1;
        }

        string strcon = "";
        string statusTest = "PASS";
        bool autoriza = true;
        private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtSerial.Text.Length <= 9)
                {
                    autoriza = true;
                    MessageBox.Show("Serial Inválida!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSerial.Text = "";
                }
                else
                {
                    if (autoriza)
                    {
                        instanciaDoForm1.recebeFormSerial = txtSerial.Text;
                        this.Close();
                    }
                }
            }
        }

        string tipeFile;
        private void Serial_Load(object sender, EventArgs e)
        {
            IniFile _myIni = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
            strcon = _myIni.Read("strCon", "dataBase");
            tipeFile = _myIni.Read("mode", "dataBase");

           // Location = new Point(740, 703);
            Location = new Point(200, 470);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
