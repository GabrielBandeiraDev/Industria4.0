using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class UserId : Form
    {
        Form1 instanciaDoFormMain;
        public UserId(Form1 _form1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            instanciaDoFormMain = _form1;
        }

        string strcon = "";
        private TextBox focusedTextBox;

        private void label1_Click(object sender, EventArgs e)
        {

        }
        string tipeFile;
        private void UserId_Load(object sender, EventArgs e)
        {
            button1.Focus();
            panel4.Visible = false;
            IniFile _myIni = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
            strcon = _myIni.Read("strCon", "dataBase");
            tipeFile = _myIni.Read("mode", "dataBase");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pressEnterBtn();
        }

        private void pressEnterBtn()
        {
            if (tipeFile == "txt")
            {
                if (checkBox1.Checked)
                {
                    if (!Directory.Exists("C:\\MicrosoftUser\\FCT"))
                    {
                        Directory.CreateDirectory("C:\\MicrosoftUser\\FCT");
                    }

                    if (!File.Exists("C:\\MicrosoftUser\\FCT\\User.txt"))
                    {
                        using (StreamWriter writer = new StreamWriter("C:\\MicrosoftUser\\FCT\\User.txt"))
                        {
                            writer.Write("Name: " + textBox4.Text + "; User: " + textBox1.Text + "; PassWord: " + textBox2.Text);
                            MessageBox.Show("Salvo com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox3.Visible = false;
                            label3.Visible = false;
                            checkBox1.Checked = false;

                            label4.Visible = false;
                            textBox4.Visible = false;
                            button1.Text = "ENTER";
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter("C:\\MicrosoftUser\\FCT\\User.txt", true))
                        {
                            writer.WriteLine("Name: " + textBox4.Text + "; User: " + textBox1.Text + "; PassWord: " + textBox2.Text);
                            MessageBox.Show("Salvo com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox3.Visible = false;
                            label3.Visible = false;
                            checkBox1.Checked = false;

                            label4.Visible = false;
                            textBox4.Visible = false;
                            button1.Text = "ENTER";
                        }
                    }
                }
                else
                {
                    string line;
                    using (StreamReader reader = new StreamReader("C:\\MicrosoftUser\\FCT\\User.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("User: " + textBox1.Text) && line.Contains("PassWord: " + textBox2.Text))
                            {
                                string[] ms = line.Split(';');

                                //instanciaDoFormMain.login = true;
                                instanciaDoFormMain.UserName = ms[0].Substring(5, ms[0].Length - 5); ;
                                this.Close();
                            }
                        }
                    }
                }
            }
            else
            {
                OleDbConnection conexao = new OleDbConnection(strcon);
                OleDbCommand cmd;

                if (checkBox1.Checked)
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        cmd = new OleDbCommand("INSERT INTO tabelaLogin(nome, senha, usuario) VALUES(@nome, @senha, @usuario)", conexao);

                        cmd.Parameters.Add("@nome", OleDbType.VarChar).Value = textBox1.Text;
                        cmd.Parameters.Add("@senha", OleDbType.VarChar).Value = textBox3.Text;
                        cmd.Parameters.Add("@usuario", OleDbType.VarChar).Value = textBox4.Text;

                        try
                        {
                            conexao.Open();
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Salvo com sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox3.Visible = false;
                            label3.Visible = false;
                            checkBox1.Checked = false;

                            label4.Visible = false;
                            textBox4.Visible = false;
                            button1.Text = "ENTER";

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            conexao.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou Senha incorreta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    cmd = new OleDbCommand("SELECT usuario FROM tabelaLogin WHERE nome = '" + textBox1.Text + "' and senha = '" + textBox2.Text + "';", conexao);

                    try
                    {
                        conexao.Open();
                        OleDbDataReader dados = cmd.ExecuteReader();
                        bool result = dados.HasRows;

                        if (result)
                        {
                            dados.Read();
                            string nome = dados.GetString(0);

                            //instanciaDoFormMain.login = true;
                            instanciaDoFormMain.UserName = nome;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Autenticação falhou!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                textBox3.Visible = true;
                label3.Visible = true;

                label4.Visible = true;
                textBox4.Visible = true;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                button1.Text = "SAVE";
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox3.Visible = false;
                label3.Visible = false;

                label4.Visible = false;
                textBox4.Visible = false;
                button1.Text = "ENTER";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && !checkBox1.Checked)
            {
                pressEnterBtn();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (focusedTextBox != null)
            {
                focusedTextBox.Text += button.Text;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (focusedTextBox != null)
            {
                focusedTextBox.Text = "";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            focusedTextBox = (TextBox)sender;
            focusedTextBox.BackColor = Color.LightBlue;
            panel4.Visible = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            focusedTextBox.BackColor = Color.White;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }
    }
}
