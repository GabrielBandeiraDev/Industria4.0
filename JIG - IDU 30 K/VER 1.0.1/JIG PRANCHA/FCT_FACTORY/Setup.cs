using System;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FCT_FACTORY
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.InitialDirectory = "c:\\";
            opf.Filter = "All files (*.txt)|*txt";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = opf.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string caminho = Application.StartupPath.ToString() + "/config.txt";
            if (Directory.Exists("/home"))
            {
                string[] lines = System.IO.File.ReadAllLines(caminho);
                string newStr = "";
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    string[] str = line.Split('=');
                    if (str[0] == "port") { newStr += str[0] + "=" + cbxPort.Text + "\r\n"; }
                   
                    if (str[0] == "speed") { newStr += str[0] + "=" + comboBox1.Text + "\r\n"; }
                    if (str[0] == "baudRate") { newStr += str[0] + "=" + cbxRate.Text + "\r\n"; }
                 
                    if (str[0] == "script") { newStr += str[0] + "=" + textBox1.Text + "\r\n"; }
                }

                StreamWriter sw = new StreamWriter(caminho);
                sw.WriteLine(newStr);
                sw.Close();

                MessageBox.Show("Salvo com sucesso!", "Savlo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                IniFile file = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
                if (textBox1.Text != "") { file.Write("SCRIPT", textBox1.Text, "dataBase"); }
                if (cbxPort.Text != "") { file.Write("com", cbxPort.Text, "Port"); }
           
                if (cbxRate.Text != "") { file.Write("baudRate", cbxRate.Text, "Port"); }

                if (!string.IsNullOrEmpty(textBox3.Text)) { file.Write("IP", textBox3.Text, "Servidor"); }

                if (comboBox1.Text != "") { file.Write("Speed", comboBox1.Text, "setSpeed"); }
                if (radioButton1.Checked) { file.Write("track", "true", "dataBase"); }
                if (radioButton2.Checked) { file.Write("track", "false", "dataBase"); }
                if (radioButton4.Checked) { file.Write("mode", "txt", "dataBase"); }
                if (radioButton3.Checked) { file.Write("mode", "accdb", "dataBase"); }
                if (comboBox2.Text != "") { file.Write("model", comboBox2.Text, "Modelo"); }
                

                MessageBox.Show("Dados salvos com sucesso!", "Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        string[] ports = SerialPort.GetPortNames();
        public void portas()
        {
            foreach (string portName in ports)
            {
                cbxPort.Items.Add(portName);
            

            }
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            portas();

            button3.Enabled = false;
           

            string caminho = Application.StartupPath.ToString() + "/config.txt";
            if (Directory.Exists("/home"))
            {
                string[] lines = System.IO.File.ReadAllLines(caminho);

                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    string[] str = line.Split('=');
                    if (str[0] == "port") { cbxPort.Text = str[1]; }
                
                    if (str[0] == "speed") { comboBox1.Text = str[1]; }
                    if (str[0] == "baudRate") { cbxRate.Text = str[1]; }
                
                    if (str[0] == "script") { textBox1.Text = str[1]; }

                    if (str[0] == "IP") { textBox3.Text = str[1]; }
                }

                this.Text += " Lin";
            }
            else
            {

                IniFile _myIni = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
                cbxPort.Text = _myIni.Read("com", "Port");
          
                cbxRate.Text = _myIni.Read("baudRate", "Port");
               
                comboBox1.Text = _myIni.Read("Speed", "setSpeed");

                comboBox2 .Text = _myIni.Read("model", "Modelo");

                textBox3.Text = _myIni.Read("IP", "Servidor");

                string track = _myIni.Read("track", "dataBase");
                string file = _myIni.Read("mode", "dataBase");

               

                if (track == "true") { radioButton1.Checked = true; }
                else { radioButton2.Checked = true; }

                if (file == "txt") { radioButton4.Checked = true; }
                if (file == "accdb") { radioButton3.Checked = true; }


                this.Text += " Win";
            }
        }
    }
}
