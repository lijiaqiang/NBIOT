using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
// using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;
using System.Text.RegularExpressions;

namespace MAT
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        private Point LocationOnClient(Control c)
        {
            Point retval = new Point(0, 0);
            for (; c.Parent != null; c = c.Parent)
            {
                retval.Offset(c.Location);
            }
            return retval;
        }
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
            self = this;
            return;
        }

        ComControl comTest = new ComControl();
        config m_config;
        DoTestControl testControl;
        public static MainForm self;
        CallBackMSgHandler m_doCallBackHander;
        bool m_needInsertDB = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.version_label.Text = "v1512.008";
            testControl = new DoTestControl(listView1, comTest);
            testControl.m_owner = this;
            bool initRet = testControl.InitControl();
            if (initRet == false)
            {
                MessageBox.Show(testControl.getListErroMsg());
                this.Close();
            }
            testControl.PrintAllCMD();

            InitTimer();

            m_config = testControl.getConfig();

            m_doCallBackHander = new CallBackMSgHandler(listView1,versionLabel,m_config);
            m_doCallBackHander.m_ower = this;
            m_doCallBackHander.m_doTestControl = testControl;
            m_doCallBackHander.m_staionSignSuccess = testControl.m_successSign;
            m_doCallBackHander.m_stationSignTemp = testControl.m_resultSign;

            comTest.m_connectlistInitOK = new ComControl.OnConnectListIsOK(ComListInitOK);
            comTest.m_recvDataDelegate = new ComControl.OnRecvMsgHandler(m_doCallBackHander.OnRecvMsgHandler/*OnRecvCallBack*/);
            comTest.m_DoCallBackHandler = m_doCallBackHander;
            comTest.m_delay_time = m_config.m_base_info.delay_time;

            if (m_config.m_base_info.fix_old_version == true)
            {
                this.BackColor = Color.Yellow;
            }

            m_doCallBackHander.m_dbcontrol = testControl.m_DataBaseControl;



            //设置对话框的标题
            string formTest = string.Format("[{2}] {0}-{1}  [build:{3}]", m_config.m_base_info.product_version,
                              m_config.m_base_info.hardware_version,
                              m_config.m_base_info.station,
                              System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location));
            this.Text = formTest;
            if (m_config.m_nbSn.do_test == true)
            {
                ShowPairingEdit(true);
            }
            comTest.StartConnect();
            self.startTestBtn.Enabled = false;
        }

        public void StartTest()
        {
            this.SafeCall(delegate()
            {
                m_isStart = true;
                StopTimer();
                comTest.reStartSerial();
                listView1.ResetListView();
                testResult.BackColor = Color.White;
                versionLabel.BackColor = Color.White;
                m_doCallBackHander.ResetAllData();
                this.listView1.BackgroundImage = null;
                self.m_needInsertDB = true;
                self.usbConnectLabel.Text = "USB已连接到终端";
                self.usbConnectLabel.BackColor = Color.Green;
                self.startTestBtn.Text = "停止";
                self.StartTimer();
                snEdit.Enabled = true;
            });
        }

        protected void StopTest()
        {
            this.SafeCall(delegate()
            {
                m_isStart = false;
                this.startTestBtn.Text = string.Format("开始");
                usbConnectLabel.Text = "测试等待开始...";
                //comTest.disConnectSerial();
                usbConnectLabel.BackColor = Color.White;
                stationLabel.BackColor = Color.White;
                testResult.Text = string.Format("测试结果:\n{0}", this.listView1.getErroStr());
                try
                {
                    this.comTest.StopConnect();
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

                if (m_needInsertDB == true)
                {
                    if(m_config.m_access_sql_info.do_test == true)
                    {
                        //m_doCallBackHander.InsertDataToDB(); //记录测试数据到数据库                       
                    }
                    m_needInsertDB = false;
                }
            });
            
        }
        public void testFailToDo()
        {
            this.SafeCall(delegate ()
            {
                StopTimer();
                StopTest();
                this.testResult.BackColor = Color.Red;
                this.listView1.BackgroundImage = MAT.Properties.Resources.fail2;
                this.timeLabel.BackColor = Color.Red;
            });
        }

        public void testSuccessToDo()
        {
            StopTimer();
            StopTest();
            this.listView1.BackgroundImage = MAT.Properties.Resources.success2;
        }


        protected void ShowPairingEdit(bool isShow = false)
        {//显示配对输入框
            snEdit.Location = new System.Drawing.Point(snEdit.Location.X, 
                snEdit.Location.Y - 10);
            snEditLabel.Location = new System.Drawing.Point(snEditLabel.Location.X,
                snEditLabel.Location.Y - 10);
            //Show Pair Edit
            snEdit.Show();
            snEdit.Focus();
            snEditLabel.Show();
            //sn2EditLable.Show();
            //sn2Edit.Show();

            if (isShow == true)
            {
                //sn2Edit.Show();
                //sn2EditLable.Show();

                snEdit.Location = new System.Drawing.Point(634,6);
                snEditLabel.Location = new System.Drawing.Point(566,11);

                //sn2Edit.Location = new System.Drawing.Point(634,53);
                //sn2EditLable.Location = new System.Drawing.Point(551, 58);
            }
            else
            {
                //sn2Edit.Hide();
                //sn2EditLable.Hide();

                snEdit.Location = new System.Drawing.Point(634, 35);
                snEditLabel.Location = new System.Drawing.Point(566, 40);
            }
        }

        /************************************************************************/
        /* 定时器                                                               */
        /************************************************************************/
        protected System.Timers.Timer m_timer;
        protected System.Timers.Timer m_listenComConnectTimer;
        protected System.Timers.Timer m_checkComByHWTDtimer;
        protected int tickCount = 0;
        protected void InitTimer()
        {
            m_timer = new System.Timers.Timer(1000);
            m_timer.Elapsed += tmr_Elapsed;
            m_timer.AutoReset = true;

            m_listenComConnectTimer = new System.Timers.Timer(50);
            m_listenComConnectTimer.Elapsed += tmr_Elapsed_listenTimer;
            m_timer.AutoReset = true;
            
        }
        protected void StartTimer()
        {
            m_timer.Enabled = true;
            m_listenComConnectTimer.Enabled = true;
            this.timeLabel.Text = string.Format("00:00");
            this.timeLabel.BackColor = Color.White;
        }
        protected void StopTimer()
        {
            m_timer.Enabled = false;
            m_listenComConnectTimer.Enabled = false;
            //this.timeLabel.Text = string.Format("00:00");
            tickCount = 0;
        }
        protected  void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tickCount++;
            int minuteNumber = tickCount / 60;
            int secondNumber = tickCount % 60;
            if (minuteNumber / 60 >0)
            {
                tickCount = 0;
            }
            String timeTips = String.Format("{0,2:D}:{1,2:D}", minuteNumber, secondNumber);
            timeLabel.SafeCall(delegate()
            {
                timeLabel.Text = timeTips;
            });

            PeriodSendCMD();

            this.SafeCall(delegate ()
            {
                if (this.listView1.isTestAll())
                {
                    testSuccessToDo();
                }
            });

            if (tickCount >= testControl.getConfigTimerout())
           {
               testFailToDo();
               return;
           }


           //TODO:check the test have the fail test,then stop the test.
           this.listView1.SafeCall(delegate()
           {
               if (this.listView1.isHaveFail() == true)
               {
                    self.m_doCallBackHander.reportTestResult(0);
                   testFailToDo();
               }
           });

        }

        private int m_nOutComCount = 0;
        protected void tmr_Elapsed_listenTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (comTest.isConnectOK() == false)
            {
                System.Console.WriteLine("端口已断开！");
            }
        }

        protected void PeriodSendCMD()
        {//需要在测试中，周期发送的指令，或者执行的内容,1s的定时器
            self.m_doCallBackHander.DoExternSomething();
        }

        /************************************************************************/
        /* 回调函数                                                             */
        /************************************************************************/
        Dbj_GetSNAndIMEI m_getSnAndImeiObj;

        public static void ComListInitOK()
        {
            self.SafeCall(delegate ()
            {
                self.m_needInsertDB = true;
                self.usbConnectLabel.Text = string.Format("USB已连接到{0}", "终端");
                self.usbConnectLabel.BackColor = Color.Green;
                self.startTestBtn.Text = "停止";
                self.StartTest();

            });

            self.log.Info(string.Format("------[{0}]------", DateTime.Now.ToString()));
        }

        public string getSNText()
        {
            return self.snEdit.Text;
        }

        public static bool OnRecvCallBack(string recv, string comID)
        {
            System.Console.WriteLine("[MainForm]{0}>{1}", comID, recv);
            return true;
        }


        private int m_iTestSign;
        private int m_iItemSign;

        public void SetStationInfo(string stationStr, bool isPass,int iTestSign,int iItemSign)
        {
            string temp = this.stationLabel.Text;
            m_iTestSign = iTestSign;
            m_iItemSign = iItemSign;
            this.stationLabel.SafeCall(delegate()
            {
                this.stationLabel.Text = string.Format("{0}\n{1}", temp, stationStr);
            });
            if (isPass == false)
            {
                this.stationLabel.BackColor = Color.Red;
                if(m_config.m_base_info.is_check_station == true)
                {
                    StopTimer();
                    StopTest();
                    listView1.SafeCall(delegate()
                    {
                        listView1.ResetListView();
                    });
                }
            }
            else
            {
                this.stationLabel.BackColor = Color.Green;
            }
        }

        public void SetTestOverDetail(bool isPass,string resultStr)
        {
            this.SafeCall(delegate()
            {
                if (isPass == false)
                {
                    this.testResult.BackColor = Color.Red;
                }
                else
                {
                    this.testResult.BackColor = Color.Green;
                }
                this.testResult.Text = string.Format("测试结果：[PASS]\n{0}", resultStr);
            });


        }


        public int FobRecvCallBack(string sn)
        {
            string sn1 = snEdit.Text/*.Replace("0","")*/;
            if (sn1.Equals(sn)== true)
            {
                return 1;
            }
            return -1;
        }

        private int getNums(string text)
        {
            string str = text;
            char[] ch = str.ToCharArray();
            string strNum = "";
            int nums = -1; 
            for (int i = 0; i < ch.Length; i++)
            {
                if (char.IsNumber(ch[i]))
                {
                    strNum += ch[i];
                }              
            }
            try
            {
                nums = Int32.Parse(strNum);
            }
            catch (System.Exception ex)
            {
                return -1;
            }
            return nums;
        }

        private string getString(string text)
        {
            string str = text;
            char[] ch = str.ToCharArray();
            string strReturn = "";
            for (int i = 0; i < ch.Length; i++)
            {
                if (!char.IsNumber(ch[i]))
                {
                    strReturn += ch[i];
                }
            }

            return strReturn;
        }

        public void CheckVersion(string versionStr)
        {
            string[] ver = versionStr.Split(' ');
            bool is_check_version = m_config.m_base_info.is_check_version;
            if (is_check_version == true)
            {
                int minVersion = -1;
                int maxVersion = -1;
                int nowVersion = -1;
                string minSeries = "";
                string maxSeries = "";
                string nowSeries = "";
                minVersion = getNums(m_config.m_base_info.minimal_version);
                maxVersion = getNums(m_config.m_base_info.maximal_version);
                nowVersion = getNums(ver[0]);
                minSeries = getString(m_config.m_base_info.minimal_version);
                maxSeries = getString(m_config.m_base_info.maximal_version);
                nowSeries = getString(ver[0]);
                if (nowVersion != -1 && maxVersion !=-1 && minVersion != -1 )
                {
                    if (minVersion > nowVersion || maxVersion < nowVersion)
                    {
                        versionLabel.BackColor = Color.Red;
                        startTestBtn_Click(null, null);
                        MessageBox.Show("终端版本不在测试的范围内！");
                        return;
                    }
                    else if (nowSeries != minSeries && nowSeries != maxSeries)
                    {
                        versionLabel.BackColor = Color.Red;
                        startTestBtn_Click(null, null);
                        MessageBox.Show("终端版本不在测试的范围内！");
                        return;
                    }
                }
                else
                {
                    versionLabel.BackColor = Color.Red;
                    startTestBtn_Click(null, null);
                    MessageBox.Show("终端版本配置错误！");
                    return;
                }

                if (m_config.m_base_info.mcu_version!=string.Empty)
                {
                    if (m_config.m_base_info.mcu_version.ToUpper() != ver[1].ToUpper())
                    {
                        versionLabel.BackColor = Color.Red;
                        startTestBtn_Click(null, null);
                        MessageBox.Show("MCU版本不在测试的范围内！");
                        return;
                    }
                }
            }
        }

        private bool m_HWTDTest_begin = false;
        public void start_connect_com_timer_by_hwtd( string comID)
        {
            m_HWTDTest_begin = true;
            m_nOutComCount = 0;
            //启动串口检测定时器，检测到端口连接就成功
            m_checkComByHWTDtimer.Enabled = true;
        }

        public void CheckComConnectFoyHWTD(bool isOK)
        {//停止检测端口的定时器，端口已经连接上,call form CallBackMSgHandler.HWTD_Test_Connect_OK_CallBack
            m_checkComByHWTDtimer.Enabled = false;
//             listView1.SetDetail(HWTD.itemName, true);
//            UpdateStaionFlg();
        }

        /************************************************************************/
        /* 消息函数                                                             */
        /************************************************************************/
        private bool m_isStart = false;
        private void startTestBtn_Click(object sender, EventArgs e)
        {
            //self.m_doCallBackHander.PostImeiCode("869976033517526", 1);

            //if (m_isStart == false)
            //{
            //    //StartTest();
            //}
            //else
            //{
            //    StopTest();
            //    StopTimer();

            //}
        }

        private string lastImeiStr = string.Empty;
        public string getLastImeiStr()
        {
            return lastImeiStr;
        }
        private void snEdit_TextChanged(object sender, EventArgs e)
        {
            string msg = snEdit.ToString();
            if (snEdit.TextLength == m_config.m_nbSn.lenght)
            {
                lastImeiStr = snEdit.Text.Substring(0, 15);

                msg = string.Format("IMEI: {0}", lastImeiStr);
                snEdit.Text = "";
                listView1.SafeCall(delegate ()
                {
                    listView1.SetDetail(NBSN.itemName, true, lastImeiStr);
                });
                if ((lastImeiStr.Length == 15) && m_config.m_nbSn.do_test == true)
                {
                    if (m_config.m_base_info.station.CompareTo("PrintCode") == 0)
                    {
                        m_doCallBackHander.GetPrinterCode(lastImeiStr);
                    }
                    else if (m_config.m_base_info.station.CompareTo("Packing") == 0)
                    {
                        m_doCallBackHander.GetDeviveStatus(lastImeiStr);
                    }
                    else if (m_config.m_base_info.station.CompareTo("UnPaired") == 0)
                    {
                        m_doCallBackHander.GetUnpairDevice(lastImeiStr, m_config.m_nbIMEI.NPModeNum);
                    }
                    else
                    { }
                }
            }
            listView1.SetDetail(NBSN.itemName, msg);
        }

        private void testResult_Click(object sender, EventArgs e)
        {
            MessageBox.Show(testResult.Text,"测试结果:");
        }

        private void stationLabel_Click(object sender, EventArgs e)
        {
//             System.Console.WriteLine("OpenComAgine");//test
//             comTest.OpenComAgine(m_doCallBackHander.m_nowCOMID);
//             testControl.send_HWTD_test();
        }

        private void usbConnectLabel_Click(object sender, EventArgs e)
        {
            //comTest.CloseComAgine();//test
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            //comTest.SendCMD("DBJ_Test SOSBTN 1\0");//test
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(testControl.gpibControl != null)
            {
                testControl.gpibControl.Close();
            }
            try
            {
                comTest.StopConnect();
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
        }
    }
}
