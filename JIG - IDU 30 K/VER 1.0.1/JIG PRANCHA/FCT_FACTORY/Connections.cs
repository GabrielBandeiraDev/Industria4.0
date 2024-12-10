using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text;

namespace FCT_FACTORY
{
    class Connections
    {
        int portListen;

        IniFile iniFileSetup = new IniFile(Application.StartupPath.ToString() + "\\Setup.ini");
        //IniFile _fileSystem = new IniFile(Application.StartupPath.ToString() + "\\System.ini");

        public string dataListen;
        public string status;
        public int IdJig;
        private Thread readthread;

        public Boolean clientConnected = false;
        string mode;

        //varivael do ID do jig
        public int _idFile;
        string[] _msg = new string[12];

        //controla o fluxo da resposta de start
        public bool[] startJig = new bool[12];


        //função que atualiza status dos Jigs
        public void sendMessage(string[] msg)
        {
            _msg = msg;
        }


        public void initRunServer()
        {
            portListen = int.Parse(iniFileSetup.Read("PortListen", "Connection"));
            //recebe o ID do arquivo
            _idFile = int.Parse(iniFileSetup.Read("ID", "dataBase"));
            //mode = _fileSystem.Read("SWVersion", "Parameters");

            readthread = new Thread(new ThreadStart(RunServer));
            readthread.Start();
        }

        public string retorno = "";
        public bool starting = false;
        public bool testModeOnOff = false;
        public int timerSimulation = 0;
        public void RunServer()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = portListen;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests.
                server.Start();



                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    dataListen = " (Desconectado)";

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    dataListen = " Connectado!";

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //Console.WriteLine("Received: {0}", data);
                        dataListen = data;

                        if (dataListen.Length > 28)
                        {
                            retorno = "Waiting,serial";

                            if (_msg[1] == "On")
                            {
                                _msg[1] = "000";
                            }
                        }


                        #region DONE_JIG

                        else if (dataListen.Contains("doneJig"))
                        {
                            if (_msg[1].Contains("Pass") || _msg[1].Contains("Fail"))
                            {
                                if (starting)
                                {
                                    startJig[1] = false;
                                    starting = false;
                                    Thread.Sleep(100);
                                    retorno = _msg[1];
                                    _msg[1] = "000";
                                }
                                else
                                {
                                    retorno = "000";
                                }
                            }

                            else
                            {
                                retorno = _msg[1];
                            }
                        }

                        #endregion

                        #region DESLIGAR
                        else if (dataListen == "TurnOff")
                        {
                            retorno = "Ok";
                        }
                        #endregion

                        #region START POSITION
                        else if (dataListen.Contains("start"))
                        {
                            starting = true;
                            startJig[1] = true;
                            retorno = "Ok";
                        }

                        #endregion

                        #region SIMULATION

                        else if (dataListen.Contains("simulation"))
                        {
                            try
                            {
                                string[] tm = dataListen.Split(',');
                                timerSimulation = int.Parse(tm[1]);
                                testModeOnOff = !testModeOnOff;
                                if (testModeOnOff)
                                {
                                    retorno = "Simulation Mode [On]\r\nTimer: " + timerSimulation.ToString();
                                }
                                else
                                {
                                    retorno = "Simulation Mode [Off]";
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                        #endregion

                        #region MODEL
                        else if (dataListen.Contains("Status"))
                        {
                            retorno = _msg[1];
                        }

                        //MODELO R12 R14
                        else if (dataListen == "mode,R14")
                        {
                            retorno = "R14,ok";
                        }

                        else if (dataListen == "mode,R12")
                        {
                            retorno = "R12,ok";
                        }

                        else if (dataListen == "mode=")
                        {
                            if (mode.Contains("R14")) { mode = "R14"; }
                            else { mode = "R12"; }
                            retorno = mode;
                        }
                        #endregion


                        else
                        {
                            retorno = "xxx";
                        }

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(retorno);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        data = null;
                        //dataListen = "";
                        //Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.ToString());
                //Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }


        //class cuty message read of the chip
        public void cutyMsg(string msg, TextBox sn, TextBox imei)
        {
            try
            {
                sn.Text = "";
                imei.Text = "";

                string[] etiquetas;
                etiquetas = msg.Split(',');
                sn.Text = etiquetas[0];
                imei.Text = etiquetas[1];
                IdJig = int.Parse(etiquetas[2]);

                //sn.Text = msg.Substring(0, 10);
                //imei.Text = msg.Substring(11, 15);
                //IdJig = int.Parse(msg.Substring(27, 2));

                if (IdJig > 4)
                {
                    IdJig -= 4;
                }
            }
            catch
            {

            }

        }

        public void history(RichTextBox hs)
        {
            if (dataListen.Length > 2)
            {
                hs.Text += dataListen + "--" + retorno + "\r\n";
            }
        }
        //sn1234567890imei123456789012345


        
    }
}
