using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;


namespace FCT_FACTORY
{
    public partial class dataTest : Form
    {
        public dataTest()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //string strcon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\PROJETOS\21 - FCT_FACTORY\FCT_FACTORY\tabelaHm.mdf;Integrated Security=True;Connect Timeout=30";
        string strcon = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("O campo está em branco!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (textBox1.Text == "")
                {
                    textBox1.Focus();
                }
                else
                {
                    comboBox1.Focus();
                }
            }
            else
            {
                richTextBox1.Text = "";
                if (tipeFile == "txt") { buscar(); }
                else { dbsearch(); }
            }
        }
        private void buscar()
        {
            string snum = "";
            string namePath = snum;
            string line;
            string msg = "";
            using (StreamReader reader = new StreamReader(directory + namePath + ".txt"))

            {
                if (comboBox1.Text == "SERIAL")
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Serial: " + textBox1.Text))
                        {
                            msg += line;
                        }
                    }

                    string[] partes = msg.Split(';');
                    for (int i = 0; i < partes.Length; i++)
                    {
                        richTextBox1.Text += partes[i] + "\r\n";
                    }
                }
                if (comboBox1.Text == "PASS" || comboBox1.Text == "FAIL")
                {
                    int cont = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Status: " + comboBox1.Text))
                        {
                            richTextBox1.Text += line + "\r\n";
                            cont++;
                        }
                    }
                    label6.Visible = true;
                    label6.Text = cont.ToString("000");
                }
            }
        }

        string directory = "", tipeFile;
        private void dataTest_Load(object sender, EventArgs e)
        {
            label6.Visible = false;
            IniFile _myIni = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
            strcon = _myIni.Read("strCon", "dataBase");
            //directory = _myIni.Read("dirExport", "dataBase");
            tipeFile = _myIni.Read("mode", "dataBase");

            if (tipeFile != "txt") { richTextBox1.Visible = false; }
        }


        int count = 7;
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void liberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Ocorreu um erro durante a liberação do objeto " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void dbsearch()
        {
            //serial, data, operador, duracao, status, test_short_curt, test_100vac_4a, test_efficiency, test_ripple, test_overload_75a, test_poweroff, teste_high_vac4a, test_high_vac75a, test_high_vac0a, test_discharger
            OleDbConnection conexao = new OleDbConnection(strcon);
            OleDbCommand cmd;
            string dataInicio = (dateTimePicker1.Value.Day - 1) + "/" + dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Year + " 00:00:00";
            string dataFim = dateTimePicker2.Value.Day + "/" + dateTimePicker2.Value.Month + "/" + dateTimePicker2.Value.Year + " 00:00:00";



            if (comboBox1.Text == "PASS" || comboBox1.Text == "FAIL")
            {
                label5.Visible = true;

                cmd = new OleDbCommand("SELECT * FROM tabelaTestes WHERE (data >= '" + dataInicio + "') AND (data < '" + dataFim + "') and status = '" + comboBox1.Text + "'", conexao);
                //cmd = new OleDbCommand("SELECT * FROM tabelaTestes WHERE (data >= '" + dataInicio + "') AND (data < '" + dataFim + "') and status = '" + comboBox1.Text + "'", conexao);
            }

            else if (comboBox1.Text == "SERIAL")
            {
                cmd = new OleDbCommand("SELECT * FROM tabelaTestes WHERE (data >= '" + dataInicio + "') AND (data < '" + dataFim + "') and serial = '" + textBox1.Text + "'", conexao);
                //cmd = new OleDbCommand("SELECT * FROM tabelaTestes WHERE (data >= '" + dataInicio + "') AND (data < '" + dataFim + "') and status = '" + comboBox1.Text + "'", conexao);
            }
            else
            {
                label5.Visible = false;
                cmd = new OleDbCommand("SELECT * FROM tabelaTestes WHERE (data >= '" + dataInicio + "') AND (data <= '" + dataFim + "') ", conexao);
                Console.WriteLine("ALL FILES");
            }


            try
            {
                conexao.Open();
                cmd.ExecuteNonQuery();

                OleDbDataAdapter da = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;

                da.Fill(ds);
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = ds.Tables[0].TableName;

                label5.Text = "Total: " + (dataGridView1.RowCount - 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro " + ex.Message);

            }

            finally
            {
                conexao.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dbsearch();
            }
        }
    }
}
