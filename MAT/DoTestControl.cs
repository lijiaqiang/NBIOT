using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
// using System.Linq;
using System.Text;
using System.Threading;
using log4net;

namespace MAT
{
    class DoTestControl
    {
        public DoTestControl(Dbj_ListView listView,ComControl comControl)
        {
            this.m_listView = listView;
            this.m_comControl = comControl;
            m_CmdList = new Dictionary<Station, string>();
            m_config = new config();
            comControl.setReadTimeout(m_config.m_base_info.readTimeout);
        }


        config m_config;
        ComControl m_comControl;
        Dbj_ListView m_listView;
        public Dbj_DBControl m_DataBaseControl = null;
        public Dbj_ItemSign m_successSign = new Dbj_ItemSign();
        public Dbj_ItemSign m_resultSign = new Dbj_ItemSign();
        Dictionary<Station, string> m_CmdList;
        protected string m_listErroMsg;
        protected Int32 lastCmd = 0;
        protected int m_testTimeoutTimes = 0;
        public MainForm m_owner;

        public string getListErroMsg()
        {
            return this.m_listErroMsg;
        }

        public int getConfigTimerout()
        {
            return m_config.m_base_info.timeout;
        }

        public config getConfig()
        {
            return m_config;
        }

        public bool InitControl()
        {//初始化显示测试的列表，和需要测试的指令列表
            bool ret = start_access_sql_connect();
            if (ret == false)
            {
                return false;
            }

            start_nbFlash();
            start_nbMagsensor();
            start_nbNetwork();
            start_nbBatAdc();
            start_nbRadarAdc();
            start_nbWatchDog();
            start_nbPower();
            start_nbVersion();
            start_nbGetIMEI();
            start_nbMode();
            start_nbCCT();
            start_nbSn();
            start_nbPackingStatus();
            start_reportResult();
            start_nbPrinterCode();
            return true;
        }

        public bool DoAllCMD()
        {//发送所以的测试指令，通过串口通信
            //string cmdStr = string.Format("DBJ_Get SN");
            //m_CmdList[Station.GET_SN_FLAG] = cmdStr;
            //m_successSign.setSign(Station.GET_SN_FLAG, true);

            foreach (KeyValuePair<Station,string> k in m_CmdList)
            {
                System.Console.WriteLine(k.Value);

                bool sendRet = m_comControl.SendCMD(k.Value);
                if (sendRet == false)
                {
                    this.m_listErroMsg = string.Format("DoAllCMD() send cmd erro:{0}", k.Value);
                    return false;
                }
                Thread.Sleep(50);
            }

            return true;
        }

        private void delayToDo(Station Key,int delayTime)
        {//延时发送指令
            System.Timers.Timer timer = new System.Timers.Timer(delayTime);
            //timer.Elapsed += DelayTodo_Elapsed;
            timer.Elapsed += (sender,e)=>{
                bool sendRet = m_comControl.SendCMD(m_CmdList[Key]);
                
                if (sendRet == false)
                {
                    this.m_listErroMsg = string.Format("DoAllCMD() send cmd erro:{0}", m_CmdList[Key]);
                    System.Windows.Forms.MessageBox.Show(this.m_listErroMsg);
                }
            };
            timer.AutoReset = false;
            timer.Enabled = true;
        }


        public bool PrintAllCMD()
        {
            foreach (KeyValuePair<Station, string> k in m_CmdList)
            {
                System.Console.WriteLine("station:{0},cmd:{1}", k.Key, k.Value);
            }
            return true;
        }

        public bool start_access_sql_connect()
        {
            if (m_DataBaseControl != null)
            {
                m_DataBaseControl.closeConnect();
            }
            m_DataBaseControl = new Dbj_DBControl(m_config.m_access_sql_info.access_file
                                                , m_config.m_access_sql_info.connect_string);
            bool dbRet = m_DataBaseControl.initConnect();
            if (dbRet == false)
            {
                this.m_listErroMsg = m_DataBaseControl.getErrStr();
                return false;
            }
            return true;
        }

        public bool insertTestResult(string m_imei, int m_type, int m_status, double m_power, Int32 m_geomagnetism, Int32 m_csq)
        {
            if (m_imei.Length == 15)
            {
                bool ret = false;
                try
                {
                    string cmd = string.Format("INSERT INTO NP100_TABLE(TS, IMEI, Type, Status, Power, Geomagnetism, csq) VALUES(\"{0}\", \"{1}\",{2},{3},{4},{5},{6})", DateTime.Now.ToString() , m_imei, m_type, m_status, m_power, m_geomagnetism, m_csq);
                    //Console.WriteLine("insertTestResult:{0}", cmd);
                    ret = m_DataBaseControl.sqlCmd(cmd);
                    if (ret == false)
                    {
                        Console.WriteLine("InsertTestResult() erro sql:{0}", cmd);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("insertTestResult:{0}", err.Message);
                }

                if(!ret)
                    start_access_sql_connect();
                return ret;
            }
            return false;
        }

        public void sendCmdSetNbMode(int mode)
        {
            string cmd = string.Format("nb:{0}", mode);
            System.Console.WriteLine(string.Format("write cmd:{0}", cmd));

            if (!m_comControl.SendCMD(cmd))
            {
                this.m_listErroMsg = string.Format("start_nbTest() send cmd erro: n{0}", cmd);
            }
        }

        public void start_nbCCT()
        {
            double nbVoltage = 0.0;
            if (m_config.m_nbPower.do_test == true)
            {
                if (double.TryParse(m_config.m_nbPower.cct_3v6[0], out nbVoltage) == true)
                {
                    InitGPIBControlExtend(m_config.m_nbPower.PowerAddress, nbVoltage);
                }
            }
        }

        public bool start_nbGetIMEI()
        {
            if (m_config.m_nbIMEI.do_test == true)
            {
                m_listView.AddItem(NBIMEI.itemName, m_config.m_nbIMEI.detail);
                m_successSign.setSign(Station.NBIMEI_SUCCESS_SIGN, true);
                return true;
            }

            return false;
        }

        public bool start_nbMode()
        {
            if (m_config.m_nbMode.do_test == true)
            {
                m_listView.AddItem(NBMODE.itemName, m_config.m_nbMode.detail);
                //m_successSign.setSign(Station.NBMODE_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbSn()
        {
            if (m_config.m_nbSn.do_test == true)
            {
                m_listView.AddItem(NBSN.itemName, m_config.m_nbSn.detail);
                m_successSign.setSign(Station.NBSN_SUCCEDD_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_reportResult()
        {
            if (m_config.m_reportResult.do_test == true)
            {
                m_listView.AddItem(ReportResult.itemName, m_config.m_reportResult.detail);
                //m_successSign.setSign(Station.REPORT_RESULT_SUCCEDD_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbVersion()
        {
            if (m_config.m_nbVersion.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBVersion.itemName, m_config.m_nbVersion.detail);
                m_successSign.setSign(Station.NBVERSION_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }
        public bool start_nbWatchDog()
        {
            if (m_config.m_nbWatchDog.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBWatchDog.itemName, m_config.m_nbWatchDog.detail);
                m_successSign.setSign(Station.NBWATCHDOG_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbBatAdc()
        {
            if (m_config.m_nbBatAdc.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBBatAdc.itemName, m_config.m_nbBatAdc.detail);
                m_successSign.setSign(Station.NBBATADC_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }
        public bool start_nbRadarAdc()
        {
            if (m_config.m_nbRadarAdc.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBRadarAdc.itemName, m_config.m_nbRadarAdc.detail);
                m_successSign.setSign(Station.NBRADARADC_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbPower()
        {
            if (m_config.m_nbPower.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBPower.itemName, m_config.m_nbPower.detail);
                m_successSign.setSign(Station.NBPOWER_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbFlash()
        {
            if (m_config.m_nbFlash.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBFlash.itemName, m_config.m_nbFlash.detail);
                m_successSign.setSign(Station.NBFlASH_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbNetwork()
        {
            if (m_config.m_nbNetwork.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBNetwork.itemName, m_config.m_nbNetwork.detail);
                m_successSign.setSign(Station.NBNETWORK_SUCCESS_SIGN, true);
                return true;
            }
            return false;
        }

        public bool start_nbMagsensor()
        {
            if (m_config.m_nbMagsensor.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBMagsensor.itemName, m_config.m_nbMagsensor.detail);
                m_successSign.setSign(Station.NBMAGSENSOR_SUCCESS_SIGN, true);
   
                return true;
            }
            return false;
        }

        public bool start_nbPackingStatus()
        {
            if (m_config.m_nbPackingStatus.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(NBPackingStatus.itemName, m_config.m_nbPackingStatus.detail);
                m_successSign.setSign(Station.NBPACKING_SUCCEDD_SIGN, true);

                return true;
            }
            return false;
        }

        public bool start_nbPrinterCode()
        {
            if (m_config.m_printCode.do_test == true)
            {
                //添加测试项目到列表框
                m_listView.AddItem(print_code.itemName, m_config.m_printCode.detail);
                m_successSign.setSign(Station.PRINTER_SUCCEDD_SIGN, true);

                return true;
            }
            return false;
        }

        public GPIBControl gpibControl = null;
        protected void InitGPIBControl(int powerAddress)
        {
                 try
                {
                    gpibControl = new GPIBControl();
                    gpibControl.Open(0, (byte)powerAddress, 0);
                    gpibControl.InitDCSource();
                    gpibControl.SetVoltage(4.0);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
        }

        protected void InitGPIBControlExtend(int powerAddress, double vol)
        {
            try
            {
                gpibControl = new GPIBControl();
                gpibControl.Open(0, (byte)powerAddress, 0);
                gpibControl.InitDCSourceExtendNB();
                gpibControl.SetVoltage(vol);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }  
    }
}
