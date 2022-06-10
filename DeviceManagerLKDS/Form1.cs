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
        int t = 1;

        StreamWriter logWriter = new StreamWriter("C:\\DeviceManagerLKDS\\DeviceManagerLKDS\\DeviceManagerLKDS\\Logs\\Log.txt"); // ПУТЬ
        List<byte[]> bytePackets = new List<byte[]>();
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
        byte[] CRC = new byte[2];
        byte[] clone = new byte[32];
        DataReader dr = null;

        //
        // MAIN
        //
     
        public enum TestStateType
        {
            [Description("Начало ЦБ")]
            Test1 = 0x01,
            [Description("После Аппаратов безопасности")]
            Test2 = 0x02,
            [Description("После ДК")]
            Test3 = 0x04,
            [Description("После ДШ")]
            Test4 = 0x08,
            [Description("Конец ЦБ")]
            Test5 = 0x10
        }
        TestStateType enumrdtrc = TestStateType.Test1;


        public Form1()
        {
            InitializeComponent();
            getCOMports();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*            try
                        {
                            if (port.IsOpen)
                            {
                                rtbLog.Text += "\n[" + DateTime.Now + "-" + DateTime.Now.Millisecond + "] >: " + inputBytes;
                                rtbLog.Text += "\n[" + DateTime.Now + "-" + DateTime.Now.Millisecond + "] <: " + outputBytes;
                                rtbLog.SelectionStart = rtbLog.Text.Length;
                                rtbLog.ScrollToCaret();
                                logWriter.AutoFlush = true;
                                logWriter.WriteLine(rtbLog.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

            */
            timer1.Enabled = !timer1.Enabled;
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
                        timer1.Enabled = true;
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
            /*            foreach (byte[] set in bytePackets)
                        {
                            string path2 = $@"C:\Users\Prometheus\Desktop\lift{count}.log.txt"; // ПУТЬ

                            File.WriteAllBytes(path2, set);
                            count++;
                            break;
                        }*/


        }

        private void button3_Click(object sender, EventArgs e)
        {
            rtbLog.Text += DataReader.log_input;
            rtbLog.Text += DataReader.log_output;
            rtbLog.Text += "BITS: " + DataReader.DeviceSeeker();
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
            logWriter.AutoFlush = true;
            logWriter.Write(rtbLog.Text);
        }
        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTimer.Text = i++.ToString();
            dr = new DataReader(cbConnectedPorts.SelectedItem.ToString(), query);
            rtbLog.Text += DataReader.log_input;
            rtbLog.Text += DataReader.log_output;
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
            logWriter.AutoFlush = true;
            logWriter.Write(rtbLog.Text);

            if (clone[0] != null && clone[clone.Length-1] != dr.setOfBytes[dr.setOfBytes.Length-1] && clone[clone.Length - 2] != dr.setOfBytes[dr.setOfBytes.Length - 2]) 
            {
                for (int i = 0; i < dr.setOfBytes.Length; i++)
                {
                    foreach (TestStateType value in Enum.GetValues(typeof(AdressTest)))
                    {
                        if (dr.setOfBytes[i] == (byte)value)
                        {
                            string title = "Page " + value;
                            TabPage newPage = new TabPage(title);
                            mainTabControl.TabPages.Add(newPage);
                            rtbLog.Text += $"\nСоздана страница {value}";
                            clone = dr.setOfBytes;
                        }
                    }
                }
            }
            


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

/*        protected override void WndProc(ref Message m)
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

        }*/
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

    }
}
