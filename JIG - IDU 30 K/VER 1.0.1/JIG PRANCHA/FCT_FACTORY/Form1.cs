using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FCT_FACTORY
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public string recebeFormSerial;
        public bool instanciaEmerg = false;
        string diretoriorange1;
        public string UserName = "";
        string arquivotxt;
        string diretorio;
        string ip;
        string nomeTeste;
        string range;
        int index;

        List<int> cmd = new List<int>();

        DateTime start;

        bool inicio = false;
        bool sobe_falha1 = false;
        bool aprovou1 = false;
        bool semPlaca = false;


        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        //CARREGA RANGE BERÇO 1
        private void LoadScript()
        {
            string[] lines = File.ReadAllLines(diretoriorange1);
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (int.TryParse(parts[0], out int num))
                {
                    cmd.Add(num);
                }
                else if (parts[0].Trim() == "cmd_fim")
                {
                    cmd.Add(-1);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;

            IniFile _myIni = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
            serialPort1.PortName = _myIni.Read("com", "Port");
            serialPort1.BaudRate = int.Parse(_myIni.Read("baudRate", "Port"));
            arquivotxt = "script.txt";
            diretorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arquivotxt);

            diretoriorange1 = _myIni.Read("SCRIPT", "dataBase");

            ip = _myIni.Read("IP", "Servidor");

            if (!File.Exists(diretorio))
            {
                MessageBox.Show($"Arquivo '{arquivotxt}' não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //EXTRAI OS VALORES DO RANGE 1
            string[] lines = File.ReadAllLines(diretoriorange1);
            foreach (var line in lines)
            {
                string[] partes = line.Split('=');
                if (partes.Length == 2)
                {
                    nomeTeste = partes[1].Split('{')[0].Trim();
                    range = partes[1].Split('{')[1].TrimEnd('}');
                    range = range.Replace(";", " a ");
                    dataGridView1.Rows.Add(nomeTeste, "", range);
                }
            }


            LoadScript();


            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.Open();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro na Porta Serial.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            dataGridView1.Rows[index].Selected = false;


            timer4.Start();
            timer3.Start();
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            if (textBox7.Text.Contains("EMERG ON"))
            {
                textBox2.Text = "";
                textBox7.Text = "";
                timer2.Stop();
                timer1.Stop();
                frmEmergencia emergencia = new frmEmergencia(this);
                instanciaEmerg = true;
                emergencia.ShowDialog();

                serialPort1.Write("14");


            }
            if (textBox7.Text.Contains("EMERG OFF"))
            {
                instanciaEmerg = false;
                textBox2.Text = "";
                textBox7.Text = "";
            }

            if (textBox7.Text.Contains("NO_PCB1"))
            {
                timer1.Stop();
                timer2.Stop();

                semPlaca = true;
                textBox7.Text = "";
                textBox9.Text = "BERÇO SEM PLACA";
                textBox9.BackColor = Color.Orange;
                inicio = false;
                serialPort1.Write("14");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) { serialPort1.Close(); }
            Config cfg = new Config();
            cfg.ShowDialog();
        }

        private void simulatorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Config cf = new Config();
            cf.ShowDialog();
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setup st = new Setup();
            st.ShowDialog();
        }

        private void dataTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataTest dt = new dataTest();
            dt.ShowDialog();
        }

        //TIMER DO BERÇO 1
        private  async void timer1_Tick(object sender, EventArgs e)
        {
            if (inicio == true)
            {
                if (textBox7.Text.Contains("X"))
                {
                    string[] dados = textBox7.Text.Split(';');
                    textBox7.Text = "";

                    dataGridView1.Rows[index].Cells[1].Value = dados[1];
                    dataGridView1.Rows[index].Cells[3].Value = dados[2];

                    if (double.TryParse(dados[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double valor))
                    {
                        string rangeString = dataGridView1.Rows[index].Cells[2].Value.ToString();
                        string[] rangeParts = rangeString.Split(new string[] { " a " }, StringSplitOptions.None);

                        if (rangeParts.Length == 2 &&
                            double.TryParse(rangeParts[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double min) &&
                            double.TryParse(rangeParts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double max))
                        {
                            if (valor >= min && valor <= max)
                            {
                                dataGridView1.Rows[index].Cells[4].Value = "Aprovado";
                            }
                            else
                            {

                                dataGridView1.ClearSelection();
                                dataGridView1.Rows[index].Selected = false;
                                dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                dataGridView1.Rows[index].Cells[4].Value = "Reprovado";
                                dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Red;

                                textBox9.Text = $"{tempo} - FALHA";
                                textBox9.BackColor = Color.Red;

                                inicio = false;
                                // sobe_falha1 = true;


                                timer2.Stop();
                                timer1.Stop();

                                serialPort1.Write("14");

                                if (semPlaca == false)
                                {
                                    GerarLog();
                                }

                                return;
                            }
                        }
                    }
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[index].Selected = true;
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#014e7f");
                    dataGridView1.FirstDisplayedScrollingRowIndex = index;

                    if (index <= cmd.Count)
                    {
                        serialPort1.Write(cmd[index].ToString());
                        index++;
                    }

                    if (index == cmd.Count)
                    {

                        timer1.Stop();
                        textBox7.Text = "";
                       // aprovou1 = true;
                        timer2.Stop();

                        textBox9.Text = $"{tempo} - APROVADO";
                        textBox9.BackColor = Color.GreenYellow;

                        if (semPlaca == false)
                        {
                            GerarLog();
                        }

                    }
                }
            }
        }

        private void limpaGrid()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells[1].Value = "";
                    row.Cells[3].Value = "";
                    row.Cells[4].Value = "";
                    dataGridView1.ClearSelection();

                }
            }
        }

        string tempo;

        //CONTAGEM DO BERÇO 1
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - start;
            tempo = $"{elapsed:hh\\:mm\\:ss}";
            textBox9.Text = $"{tempo} - TESTANDO...";
        }

        private void rangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ranges r = new ranges();
            r.ShowDialog();
        }

        bool inicia_teste = false;

        private async void timer4_Tick(object sender, EventArgs e)
        {
            if (textBox7.Text.Contains("start"))
            {
                textBox2.Text = "";
                textBox7.Text = "";

                semPlaca = false;

                //COMANDO DA DESCIDA DO PISTÃO
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("P_ON");
                }

                textBox2.Text = "";
                textBox7.Text = "";
                inicia_teste = true;

                //FLAG DOS TIMERS DO BERÇO 1 E BERÇO 2
                inicio = true;

                textBox9.Text = "";
                textBox10.Text = "";
                limpaGrid();

                index = 0;

                //INICIA CONTAGEM
                timer2.Start();
                timer6.Start();

                textBox9.Visible = true;
                textBox9.BackColor = Color.Yellow;
                start = DateTime.Now;
                textBox9.Text = $"{tempo} - TESTANDO...";

                //DELAY PARA AGUARDAR DESCIDA DO PISTÃO
                await Task.Delay(2000);

                if (inicia_teste == true)
                {

                    serialPort1.Write("0");
                    timer1.Start();
                    timer5.Start();
                    inicia_teste = false;
                }
            }
        }


        string rxData;
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            rxData = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(dataReceived));
        }

        private void dataReceived(object sender, EventArgs e)
        {
            textBox7.AppendText(rxData);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ENVIA O COMANDO 9 PARA SUBIDA DO PISTÃO
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("P_OFF");
            }

            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private async void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private async Task GerarLog()
        {
            try
            {
                // Definir o caminho para a pasta de logs
                string logDirectory = Path.Combine(Application.StartupPath, "logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Lê o arquivo INI
                IniFile _myIni = new IniFile(Path.Combine(Application.StartupPath, "Setup.ini"));

                // Ler o valor de "Modelo" do arquivo INI
                string modelo = _myIni.Read("model", "Modelo");
                string ip = _myIni.Read("IP", "dataBase");
                string IDJIG = _myIni.Read("IDJIG", "dataBase");

                // Gerar um nome de arquivo único usando a data e hora atual com mais precisão
                string logFileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".txt";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Verificar se há dados no DataGridView antes de tentar escrever o log
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("O DataGridView está vazio, não há dados para gerar o log.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Criar estrutura para o JSON
                var logData = new List<Dictionary<string, object>>();

                // Escrever o log
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    // Escrever a data e hora do teste apenas uma vez no início do arquivo
                    string data = $"DataTeste: {DateTime.Now:yyyy_MM_dd_HH:mm:ss}";
                    writer.WriteLine(data);  // Escreve a data no início

                    // Loop para cada linha relevante no DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            // Obter os valores de cada célula das colunas
                            string nome = row.Cells[0].Value?.ToString() ?? "N/A";
                            string resultado = row.Cells[4].Value?.ToString() ?? "N/A";

                            // Gerar ID (pode ser um contador ou algum outro valor, aqui estou usando o índice da linha + 1)
                            int id = row.Index + 1;

                            // Formatar a data e hora no formato ISO 8601 com fuso horário
                            string dataHora = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz"); // Formato desejado

                            // Criar o dicionário com os dados do log
                            var logEntry = new Dictionary<string, object>
                    {
                         {"IP", ip},
                        {"IDJIG", IDJIG},
                        { "Nome", nome },
                        { "Resultado", resultado },
                        { "DataHora", dataHora },
                        { "Modelo", modelo } // Aqui você insere o valor do modelo
                    };

                            // Adicionar o dicionário ao logData
                            logData.Add(logEntry);

                            // Escrever a linha no arquivo de log
                            string logLine = $"{{\"IDJIG\": {IDJIG}, \"IP\": \"{ip}\", \"Nome\": \"{nome}\", \"Resultado\": \"{resultado}\", \"DataHora\": \"{dataHora}\", \"Modelo\": \"{modelo}\"}}";
                            writer.WriteLine(logLine); // Adiciona a linha ao arquivo de log
                        }
                    }
                }

                // Enviar o JSON para o servidor
                string json = JsonConvert.SerializeObject(logData, Newtonsoft.Json.Formatting.Indented);
                await EnviarJsonParaServidor(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar o log: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task EnviarJsonParaServidor(string json)
        {
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://26.193.50.118:3000/api/logs", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao enviar JSON para o servidor: " + response.ReasonPhrase);
            }
        }
    }
}
