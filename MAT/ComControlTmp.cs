using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Reflection;
using log4net;

namespace MAT
{
    /************************************************************************/
    /*给硬件看门使用的，触发了看门狗的Test指令，终端会重启
     * 检测是否已经连接上，发送更新站位信息
     */
    /************************************************************************/
    class ComControlTmp
    {
        private string m_comID;
        public ComControlTmp(string comID)
        {
            this.m_comID = comID;
        }

        private bool m_connectIsOK = false;

        protected Thread m_ConnectThread;
        protected bool m_isRuningConnectThread = false;
        SerialPort serialPort = null;
        public delegate void ConnectOKDelegate(string recv);
        private ConnectOKDelegate m_connectOK;
        public void SetConnectOKCallback(ConnectOKDelegate connectOkFun)
        {
            m_connectOK = connectOkFun;
        }

        protected string m_lastErro;
        public string getLastErro()
        {
            return m_lastErro;
        }

        public bool start_connect()
        {
            m_isRuningConnectThread = true;
            m_ConnectThread = new Thread(CheckConnectThreadFunc);
            return true;
        }

        public bool stop_connect()
        {
            m_isRuningConnectThread = false;
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                }
                catch (System.Exception ex)
                {
                    m_lastErro = string.Format(" stop_connect() erro:{0}", ex.Message);
                    return false;
                }
            }
            return true;
        }

        protected void CheckConnectThreadFunc()
        {
            m_isRuningConnectThread = true;
            string portResp = null;
            const string rightPortResp = "DBJ_Detect_COM OK";
            const string portDetect = "DBJ_Detect_COM";
            m_connectIsOK = false;

            while (m_isRuningConnectThread == true)
            {
                System.Console.WriteLine(m_comID);
                serialPort = new SerialPort();
                serialPort.BaudRate = 115200;
                serialPort.PortName = m_comID;
                serialPort.DataBits = 8;
                try
                {
                    if (!serialPort.IsOpen)
                    {
                        serialPort.Open();
                    }
                    serialPort.WriteLine(portDetect);
                    Thread.Sleep(500);
                    portResp = serialPort.ReadExisting();
                }
                catch (System.Exception e1)
                {
                    m_lastErro = string.Format("CheckConnectThreadFunc() erro:{0}", e1.Message);
                    log.Info(m_lastErro);
                }
                //System.Console.WriteLine("[{0}]:{1}\n",newPort,portResp);
                int index = portResp.IndexOf(rightPortResp);
                if (-1 != index)
                {
                    if (m_connectOK != null)
                    {
                        char[] cSplit = { '\0' };
                        char[] arHead = { '~' };
                        string[] arRead = portResp.Split(cSplit);
                        string strSingle = null;
                        int i = 0;
                        for (; i < arRead.Length; i++)
                        {
                            strSingle = arRead[i];
                            strSingle = strSingle.TrimStart(arHead);
                            if (0 != strSingle.Length)
                            {
                                m_connectIsOK = true;
                                m_connectOK(strSingle);
                            }
                        }
                    }
                    break;
                }
            }
        }
        ILog log = log4net.LogManager.GetLogger(typeof(CallBackMSgHandler));
        public bool SendCmd(string cmdStr)
        {
            if (m_connectIsOK == false)
            {
                string erro = "串口没有连接";
                this.m_lastErro = string.Format("SendCmd erro:{0}",erro);
                log.Info(string.Format("SendCmd erro:{0}", erro));
                return false;
            }
            else
            {
                try
                {
                    log.Info(string.Format("{0}<-{1}",m_comID,cmdStr));
                    serialPort.WriteLine(cmdStr);
                }
                catch (System.Exception ex)
                {
                    this.m_lastErro = string.Format("SendCmd erro:{0}", ex.Message);
                    log.Info(string.Format("SendCmd erro:{0}", ex.Message));
                    return false;
                }
                return true;
            }
        }

//         public bool write_update_phasecheck(int test_flag, int status_flag, int app_flag)
//         {//更新站位信息
//             string cmdStr = string.Format("DBJ_Update_PhaseCheck {0} {1} {2}\0", test_flag,
//                                            status_flag, app_flag);
//             System.Console.WriteLine(cmdStr);
//             bool sendRet = SendCmd(cmdStr);
//             if (sendRet == false)
//             {
//                 return false;
//             }
//             log.Info(string.Format("{0}<-{1}", m_comID, cmdStr));
//             return true;
//         }
    }
}
