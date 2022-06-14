using DeviceManagerLKDS.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Timers;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static DeviceManagerLKDS.Classes.Enums;

namespace DeviceManagerLKDS
{
    public partial class Form1 : Form
    {
        //
        // SOME VARIABLES
        //

        StreamWriter logWriter = new StreamWriter("C:\\DeviceManagerLKDS\\DeviceManagerLKDS\\DeviceManagerLKDS\\Logs\\Log.txt"); // ПУТЬ
        int[] connectedDevices = new int[64];
        byte[] query = new byte[]
                                   {
                                        0x01,
                                        0x04,
                                        0x1F,
                                        0xF0,
                                        0x00,
                                        0x10,
                                        0,
                                        0
                                   };
        byte[] query2 = new byte[]
                                   {
                                        0x01,
                                        0x04,
                                        0x00,
                                        0x00,
                                        0x00,
                                        0x73,
                                        0,
                                        0
                                   };
        byte[] query3 = new byte[]
                                   {
                                        0x01,
                                        0x04,
                                        0x12,
                                        0x00, // НУЖНО 0x00
                                        0x00,
                                        0x01,
                                        0,
                                        0
                                   };
        byte[] CRC = new byte[2];
        byte[] clone = new byte[34];
        DataReader dr = null;

        //
        // MAIN
        //
     


        public Form1()
        {
            InitializeComponent();
            getCOMports();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            labelTimer.Text = i++.ToString();
            dr.Send(query);
            do
            {
                System.Threading.Thread.Sleep(10);
            } while (dr.setOfBytes == null);
       
            try
            {
                if (clone[clone.Length - 1] != dr.setOfBytes[dr.setOfBytes.Length - 1] || clone[clone.Length - 2] != dr.setOfBytes[dr.setOfBytes.Length - 2])
                {
                    for (int i = 0; i < 256; i++)
                    {
                        int b = dr.setOfBytes[(int)(i / 8)];
                        if ((b & (1 << (i % 8))) != 0)
                            rtbLog.Text += $"Address {i + 7} set\n";
                        /*                      string[] binary = dr.setOfBytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
                                                for (int j = 0; j < binary.Length; j++)
                                                {
                                                    if (binary[j].Contains("1"))
                                                    {
                                                        binary[j]
                                                        char[] arr = binary[j].ToCharArray();
                                                    }
                                                }*/
                        //byte val = dr.setOfBytes[i / 8]; //НЕВЕРНО ОТОБРАЖАЕТ 1-БИТЫ
                        //int a1 = dr.setOfBytes[val];
                        //int a2 = (1 << (i % 8));
                        //int b = a1 & a2;

                        //if (b != 0)
                        //{
                        //    bits.Add(i);
                        //}

                        /* if (clone[i] != dr.setOfBytes[i])
                         {

                             *//*foreach (AdressTest value in Enum.GetValues(typeof(AdressTest)))
                             {
                                 if (dr.setOfBytes[i] == (byte)value)
                                 {
                                     //split()
                                     *//*string title = "Page " + value.GetNameOfEnum();
                                     TabPage newPage = new TabPage(title);
                                     mainTabControl.TabPages.Add(newPage);
                                     rtbLog.Text += $"\nСоздана страница {value.GetNameOfEnum()}";
                                     clone = dr.setOfBytes;*//*
                                 }
                             }*//*
                         }*/

                    }
                    /*int adress = 0x1200 + 0x0010 * i;
                    byte[] array = new byte[]
                    {
                            (byte)adress,
                            Convert.ToByte(adress>>8)
                    };
                    Union16 var = new Union16();
                    var.Byte0 = array[0];
                    var.Byte1 = array[1];
                    query3[2] = var.Byte0;
                    query3[3] = var.Byte1;

                    dr = new DataReader(cbConnectedPorts.SelectedItem.ToString(), query3);
                    lock (locker)
                    {
                        string title = "";
                        foreach (byte[] set in DataReader.bytePackets)
                        {

                            title = dr.ByteToString(set);
                            char[] titl = title.Replace(" ", "").ToCharArray();
                            CAN_Devices type = (CAN_Devices)titl[3];

                            title = type.GetNameOfEnum();

                        }
                        TabPage newPage = new TabPage(title);
                        mainTabControl.TabPages.Add(newPage);
                        rtbLog.Text += $"\nСоздана страница {title}";
                    }*/
                }
            }
            catch
            {
            }
            //lock (locker)
            //{
            //    rtbLog.Text += DataReader.log_input;
            //    rtbLog.Text += DataReader.log_output;
            //    DataReader.log_output = "";
            //    DataReader.log_input = "";
            //    //rtbLog.Text += "\nBITS: " + DataReader.DeviceSeeker();
            //    rtbLog.SelectionStart = rtbLog.Text.Length;
            //    rtbLog.ScrollToCaret();
            //    logWriter.AutoFlush = true;
            //    logWriter.Write(rtbLog.Text);
            //}

            //if (mainTabControl.SelectedIndex == 0)
            //{
            //    dr = new DataReader(cbConnectedPorts.SelectedItem.ToString());
            //    lock (locker)
            //    {
            //        rtbLog.Text += DataReader.log_input;
            //        rtbLog.Text += DataReader.log_output;
            //        DataReader.log_output = "";
            //        DataReader.log_input = "";
            //        //rtbLog.Text += "\nBITS: " + DataReader.DeviceSeeker();
            //        rtbLog.SelectionStart = rtbLog.Text.Length;
            //        rtbLog.ScrollToCaret();
            //        logWriter.AutoFlush = true;
            //        logWriter.Write(rtbLog.Text);
            //    }

            //}
            //else
            //{
            //    foreach (int bit in bits)
            //    {
            //        if (mainTabControl.SelectedIndex == bit)
            //        {


            //            rtbLog.Text += DataReader.log_input;
            //            rtbLog.Text += DataReader.log_output;
            //            DataReader.log_output = "";
            //            DataReader.log_input = "";
            //            //rtbLog.Text += "\nBITS: " + DataReader.DeviceSeeker();
            //            rtbLog.SelectionStart = rtbLog.Text.Length;
            //            rtbLog.ScrollToCaret();
            //            logWriter.AutoFlush = true;
            //            logWriter.Write(rtbLog.Text);

            //        }
            //    }
            //}




            /*  byte[] output = dr.PortWrite(query);
              if (clone != output)
              {
                  rtbLog.Text += DataReader.log_input;
                  rtbLog.Text += DataReader.log_output;
                  foreach (var i in output)
                  {
                      if (output[i] != 00 && i != 0)
                      {
                          string title = "Page " + output[i];
                          TabPage newPage = new TabPage(title);
                          mainTabControl.TabPages.Add(newPage);
                          rtbLog.Text += $"\nСоздана страница {title}";
                      }
                  }
              }
              clone = output;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = "Page " + (mainTabControl.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            mainTabControl.TabPages.Add(myTabPage);
        }

        //
        // CAN
        //


        private void bConnectPort_Click(object sender, EventArgs e)
        {
            bDisconnectPort.Enabled = true;
            try
            {
                for (int i = 0; i < cbConnectedPorts.Items.Count; i++)
                {
                    if (i == cbConnectedPorts.SelectedIndex)
                    {
                        dr = new DataReader(cbConnectedPorts.SelectedItem.ToString());

                        timer1.Start();
                    }
                }
                if (cbConnectedPorts.Text == "")
                {

                }
                rtbLog.Text += DataReader.log_conStatus;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //bytePackets = DataReader.setData();
        }

        private void bDisconnectPort_Click(object sender, EventArgs e)
        {
            dr.Disconnect();
            bDisconnectPort.Enabled = false;
            rtbLog.Text += $"\nСоединение с портом {cbConnectedPorts.SelectedItem} разорвано";
            timer1.Enabled = false;
        }

        // FUNCTIONS

        public void PrintLog()
        {
            rtbLog.Text += DataReader.log_input;
            rtbLog.Text += DataReader.log_output;
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
            logWriter.AutoFlush = true;
            logWriter.Write(rtbLog.Text);

        }
        int i = 0;

        void SendQuery(byte[] query)
        {
            dr.Send(query);
            while (dr.setOfBytes == null)
            {

            }
            rtbLog.Text += DataReader.log_input + DataReader.log_output;
            dr.outputBytes = "";
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
            logWriter.AutoFlush = true;
            logWriter.Write(rtbLog.Text);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            labelTimer.Text = i++.ToString();
            /*if (i % 3 == 0)
            {
                if (mainTabControl.SelectedIndex != 0)
                {
                    int adress = 0x1200 + 0x0010 * Math.Abs((Convert.ToInt32(mainTabControl.SelectedTab.Name) - 32));
                    byte[] array = new byte[]
                    {
                           (byte)adress,
                           Convert.ToByte(adress>>8)
                    };
                    Union16 var = new Union16();
                    var.Byte0 = array[0];
                    var.Byte1 = array[1];
                    query3[2] = var.Byte1;
                    query3[3] = var.Byte0;
                    query3[query.Length - 3] = 16;
                    SendQuery(query3);
                }
            }*/
            /*if (i % 15 == 0)
            {
                if (mainTabControl.SelectedIndex == 0)
                {
                    SendQuery(query2);
                }
            }*/
            SendQuery(query2);
/*            try
            {
                while (dr.setOfBytes.Length != 4)
                {
                    dr.setOfBytes = null;
                }
            }
            catch { }*/



/*            dr.setOfBytes = null;
            dr.rawData = new List<byte>();
            dr.bytePackets = new List<byte[]>();*/

            rtbLog.Text += $"\nВыбранная вкладка{mainTabControl.TabPages[mainTabControl.SelectedIndex].Text}";

/*            SendQuery(query);*/

            while (dr.setOfBytes == null)
            {

            }

            /*try 
            {
                for (int c = 0; c < dr.setOfBytes.Length; c++) {}
                if (clone[clone.Length - 1] != dr.setOfBytes[dr.setOfBytes.Length - 1] || clone[clone.Length - 2] != dr.setOfBytes[dr.setOfBytes.Length - 2])
                {

                    Array.Copy(dr.setOfBytes, clone, 34);
                    List<int> bits = new List<int>();
                    for (int i = 0; i < 256; i++)
                    {
                        int b = dr.setOfBytes[(int)(i / 8)];
                        if (((b & (1 << (i % 8))) != 0) && i >= 32)
                        {
                            bits.Add(i);
                            
                        }
                    }

                    for (int p = 0; connectedDevices[p] != 0; p++)
                    {
                        connectedDevices[p] = 0; //обнуление списка подключённых устройств и вкладок
                    }
                    mainTabControl.TabPages.Clear();
                    TabPage lbPage = new TabPage("ЛБ Концентратор");
                    mainTabControl.TabPages.Add(lbPage);

                    for (int i = 0, j = 0; i < bits.Count; i++)
                    {
                        dr.setOfBytes = null;
                        dr.rawData = new List<byte>();
                        dr.bytePackets = new List<byte[]>();
                        int adress = 0x1200 + 0x0010 * Math.Abs((bits[i] - 32));
                        byte[] array = new byte[]
                        {
                           (byte)adress,
                           Convert.ToByte(adress>>8)
                        };
                        Union16 var = new Union16();
                        var.Byte0 = array[0];
                        var.Byte1 = array[1];
                        query3[2] = var.Byte1;
                        query3[3] = var.Byte0;
                        query3[query.Length - 3] = 0;


                        SendQuery(query3);



                        if (dr.setOfBytes[1] != 255)
                        {
                            connectedDevices[j] = dr.setOfBytes[1];
                            connectedDevices[j + 1] = bits[i];
                            j = j + 2;
                        }
                    }
                    rtbLog.Text += "\n\n----------------\nПодключенные устройства:\n";
                    for (int o = 0; o < connectedDevices.Length; o++)
                    {
                        if (connectedDevices[o] != 0)
                        {

                            foreach (CAN_Devices value in Enum.GetValues(typeof(CAN_Devices)))
                            {
                                if (connectedDevices[o] == (byte)value)
                                {
                                    TabPage newPage = new TabPage(value.GetNameOfEnum());
                                    newPage.Name = $"{connectedDevices[o+1]}"; 
                                    mainTabControl.TabPages.Add(newPage);
                                    rtbLog.Text += $"\n{connectedDevices[o]} - {value.GetNameOfEnum()}. Адрес CAN: {newPage.Name}";
                                }
                            }
                        }
                    }
                    rtbLog.Text += "\n----------------\n";
                }

            }
            catch { }*/

            timer1.Start();

        }

        private struct DEV_BROADCAST_HDR
        {
            //отключаем предупреждения компилятора для ошибки 0649
        #pragma warning disable 0649
            internal UInt32 dbch_size;
            internal UInt32 dbch_devicetype;
            internal UInt32 dbch_reserved;
            //включаем предупреждения компилятора для ошибки 0649
        #pragma warning restore 0649
        };

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0219)
            {
                DEV_BROADCAST_HDR dbh;
                switch ((int)m.WParam)
                {
                    case 0x8000:
                        dbh = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbh.dbch_devicetype == 0x00000003)
                        {
                            getCOMports();
                        }
                        break;
                    case 0x8004:
                        dbh = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbh.dbch_devicetype == 0x00000003)
                        {

                            cbConnectedPorts.Text = "";
                            getCOMports();
                        }
                        break;
                }
            }
        }
        public void getCOMports()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                cbConnectedPorts.Items.Clear();
                cbConnectedPorts.Items.AddRange(ports);
                bDisconnectPort.Enabled = false;
                cbConnectedPorts.SelectedIndex = 0;
            }
            catch (Exception)
            {
                //MessageBox.Show("Нет доступных соединений");
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelTimer.Text = mainTabControl.SelectedIndex.ToString();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            dr?.Disconnect();
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            rtbLog.Text = "";
        }
    }
}
