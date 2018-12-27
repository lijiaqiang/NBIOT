using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Reflection;
using log4net;
using System.Windows.Forms;

namespace MAT
{
    class ComControl
    {
        System.IO.Ports.SerialPort mySerialPort = new SerialPort();
        private string dbjRightResp = string.Empty;
        private string dbjDetect = string.Empty;

        public ComControl()
        {
            self = this;
            m_connectComDictionary = new Dictionary<string, SerialPort>();
            m_readTimeout = 10;
            m_delay_time = 0;
        }

        public ComControl(int readTimeout)
        {
            self = this;
            m_connectComDictionary = new Dictionary<string, SerialPort>();
            m_readTimeout = readTimeout;
        }

        public void setReadTimeout(int readTimeout)
        {
            m_readTimeout = readTimeout;
        }

        protected Thread m_ConnectThread;
        protected bool m_isRuningConnectThread = false;

        public CallBackMSgHandler m_DoCallBackHandler;
        public config m_config = new config();

        private int m_readTimeout;
        public int m_delay_time;

        public bool StartConnect(string rightResp = "DBJ_Detect_COM OK", string detect = "DBJ_Detect_COM")
        {
            if (m_isRuningConnectThread == true)
            {
                return false;
            }
            dbjDetect = detect;
            dbjRightResp = rightResp;
            m_connectComDictionary.Clear();
            m_ConnectThread = new Thread(CheckConnectThreadFunc);
            m_ConnectThread.Start();
            return true;
        }

        public bool StopConnect()
        {
            return true;
        }

        ILog log = log4net.LogManager.GetLogger(typeof(CallBackMSgHandler));

        public bool SendCMD(string cmdStr,string cmdID=null)
        {
            if (mySerialPort != null)
            {
                if (mySerialPort.IsOpen)
                {
                    try
                    {
                        mySerialPort.WriteLine(cmdStr);
                        log.Info(string.Format("{0}<-{1}", mySerialPort.PortName, cmdStr));
                    }
                    catch (System.Exception ex)
                    {
                        this.m_listErroMsg = string.Format("serialPort_agine.WriteLine erro:{0}", ex.Message);
                        log.Info(this.m_listErroMsg);
                        return false;
                    }
                    return true;
                }
            }
            if (m_connectComDictionary.Count == 0)
            {
                this.m_listErroMsg = "没有连接的串口";
                return false;
            }

            if (cmdID == null)
            {
                foreach (KeyValuePair<string, SerialPort> k in m_connectComDictionary)
                {
                    if (isConnectOK(k.Key) == true)
                    {
                        log.InfoFormat("{0}<-{1}",k.Value.PortName,cmdStr);
                        try
                        {
                            if(k.Value != null)
                            {
                                k.Value.WriteLine(cmdStr);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                            this.m_listErroMsg = ex.Message;
                            log.Info(this.m_listErroMsg);
                            return false;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if (isConnectOK(cmdID) == true)
                    {
                        m_connectComDictionary[cmdID].WriteLine(cmdStr);
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    this.m_listErroMsg = ex.Message;
                    log.Info(this.m_listErroMsg);
                    return false;
                }
            }

            return true;
        }

        protected string m_listErroMsg;
        public string getListErro()
        {
            return this.m_listErroMsg;
        }

        public delegate bool OnRecvMsgHandler(string recv,string comID);
        public OnRecvMsgHandler m_recvDataDelegate = new OnRecvMsgHandler(OnRecvMsgFunc);
        public static bool OnRecvMsgFunc(string recv,string comID)
        {
            System.Console.WriteLine("{0}>{1}",comID,recv);
            return false;
        }

        public bool m_isMutilCom = false;
        protected static ComControl self = null;

        protected Dictionary<string,SerialPort > m_connectComDictionary;

        public delegate void OnComIsOkHandler(SerialPort serialPort,string comID);
        public OnComIsOkHandler m_comOkDelegate = new OnComIsOkHandler(OnComOKHandler);
        private static void OnComOKHandler(SerialPort serialPort, string comID)
        {
            self.mySerialPort.DataReceived += MySerialPort_DataReceived;
            self.mySerialPort.ErrorReceived += MySerialPort_ErrorReceived;
            self.m_connectComDictionary[comID] = serialPort;
            System.Console.WriteLine(comID);

        }

        private static void MySerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            self.log.Info(e.ToString());
        }

        private static void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string recv = self.mySerialPort.ReadLine();
                System.Console.WriteLine("[MySerialPort_DataReceived:{0}]\n", recv);
                self.OnReceive(sender, recv);
            }
            catch
            {
                System.Console.WriteLine("MySerialPort_DataReceived error");
            }
        }

        public delegate void OnConnectListIsOK();
        public OnConnectListIsOK m_connectlistInitOK = null;

        protected void OnErroReceive(object sender, Exception e)
        {
            JustinIO.CommPort serialPort = (JustinIO.CommPort)sender;
            System.Console.WriteLine(e.ToString());
            log.Info(e.ToString());
        }

        public bool isConnectOK(string comID = null)
        {
            bool isOk = false;
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            if (comID != null)
            {
                foreach (string portStr in ports)
                {
                    string newPort = portStr.Replace("\0", "");
                    if (comID == newPort)
                    {
                        isOk = true;
                    }
                }
            }
            else
            {
                foreach (string comStr in m_connectComDictionary.Keys)
                {
                    foreach (string portStr in ports)
                    {
                        string newPort = portStr.Replace("\0", "");
                        if (comStr == newPort)
                        {
                            isOk = true;
                        }
                    }
                }
            }
            if (isOk)
            {
                return true;
            }
            return false;
        }
        private static bool m_disableSerial = false;
         protected void OnReceive(object sender, string strRead)
         {
             SerialPort mySerial = (SerialPort)sender;
             try
             {
                if (-1 != strRead.IndexOf("enter user prog"))
                {
                    if (m_connectlistInitOK != null)
                    {
                        m_connectlistInitOK();
                    }
                }
                
                if (!m_disableSerial)
                {
                    if (0 != strRead.Length)
                        m_recvDataDelegate(strRead, mySerial.PortName);
                }
             }
             catch (System.Exception e1)
             {
                 System.Console.WriteLine("[{0}]>OnRecevie:{1}" , mySerial.PortName, e1.Message);
                 log.Info(string.Format("[{0}]>OnRecevie erro:{1}", mySerial.PortName, e1.Message));
             }
         }
        public void disConnectSerial()
        {
            m_disableSerial = true;
        }
        public void reStartSerial()
        {
            m_disableSerial = false;
        }
        public string m_versionStr = "-----";
        protected void CheckConnectThreadFunc()
        {
            m_isRuningConnectThread = true;
            string portResp = null;
            m_versionStr = "-----";
            string newPort = m_config.m_base_info.comPort.ToString();
            System.Console.WriteLine(newPort);
            mySerialPort.BaudRate = 9600;
            mySerialPort.PortName = newPort;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.ReadTimeout = m_readTimeout;
            mySerialPort.DtrEnable = true;
            mySerialPort.Parity = Parity.None;

            try
            {
                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.Open();
                }
                m_comOkDelegate(mySerialPort, newPort);
            }
            catch (System.Exception e1)
            {
            }
            if (m_connectlistInitOK != null)
            {
                //m_connectlistInitOK();
            }
        }
    }
}
