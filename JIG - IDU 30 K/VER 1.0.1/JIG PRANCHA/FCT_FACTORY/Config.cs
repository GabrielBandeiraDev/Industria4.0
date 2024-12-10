using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;



namespace FCT_FACTORY
{
    public partial class Config : Form
    {
        SerialPort serial = new SerialPort();
        Boolean serialRead = false;

        public Config()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Config_Load(object sender, EventArgs e)
        {
            portas();
            timer1.Start();
        }

        string[] ports = SerialPort.GetPortNames();
        public void portas()
        {
            foreach (string portName in ports)
            {
                comboBox1.Items.Add(portName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.BaudRate = 9600;
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                groupBox2.Enabled = true;
                groupBox4.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                groupBox4.Enabled = false;
                groupBox2.Enabled= false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen)
            {
                label4.Text = "Port Open";
                textBox2.BackColor = Color.GreenYellow;
            }

            else
            {
                label4.Text = "Port Closed";
                textBox2.BackColor = Color.Black;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialRead = !serialRead;
        }

        string rxData = "";
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            rxData = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(dataReceived));
        }

        private void dataReceived(object sender, EventArgs e)
        {
            textBox1.AppendText(rxData);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                textBox1.Text = "";
                serialPort1.WriteLine(textBox3.Text);
                textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("Comando não enviado, verifique a conexão serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Write("P0");
            }
            else
            {
                MessageBox.Show("Verifique a conexão serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("P1");
            }
            else
            {
                MessageBox.Show("Verifique a conexão serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("P2");
            }
            else
            {
                MessageBox.Show("Verifique a conexão serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("CS");
            }
            else
            {
                MessageBox.Show("Verifique a conexão serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
