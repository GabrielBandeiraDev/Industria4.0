using System;
using System.IO;
using System.Windows.Forms;

namespace FCT_FACTORY
{
    public partial class ranges : Form
    {

        public ranges()
        {
            InitializeComponent();

        }

        string cam = Path.Combine(Application.StartupPath, "script.txt");
        string cam2 = Path.Combine(Application.StartupPath, "script2.txt");

        private void ranges_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(cam))
                {
                    richTextBox1.Text = File.ReadAllText(cam);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao abrir o arquivo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            try
            {
                if (File.Exists(cam2))
                {
                    richTextBox2.Text = File.ReadAllText(cam2);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao abrir o arquivo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(cam, richTextBox1.Text);
                MessageBox.Show("Arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(cam2, richTextBox2.Text);
                MessageBox.Show("Arquivo salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
