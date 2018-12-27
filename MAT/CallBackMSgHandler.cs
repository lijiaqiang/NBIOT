using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
/*using System.Linq;*/
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using log4net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MAT
{
    class CallBackMSgHandler
    {
        protected Dbj_ListView m_parentListView;
        protected System.Windows.Forms.Label m_parentVersionLabel;
        protected static CallBackMSgHandler self;
        public Dbj_ItemSign m_stationSignTemp;
        public Dbj_ItemSign m_staionSignSuccess;
        public Dbj_DBControl m_dbcontrol;
        protected config m_config;
        public MainForm m_ower;
        public DoTestControl m_doTestControl;
        private string m_imei = string.Empty;
        private static Int32 m_geomagnetism = 0;
        private static Int32 m_csq = 0;
        private static Int32 m_power = 0;
        private static Int32 m_radar = 0;
        private static string m_version = string.Empty;
        private static int resultStatus = 0;
        private static int testResultFail = 0;
        private static int testResultSuccess = 1;

        public CallBackMSgHandler(Dbj_ListView listView, System.Windows.Forms.Label versionLabel, config configData)
        {
            m_parentListView = listView;
            m_parentVersionLabel = versionLabel;
            m_config = configData;
            self = this;
        }

        public void ResetAllData()
        {//clear all data which be used again
            m_stationSignTemp.resetSign(false);
            m_imei = string.Empty;
            m_version = string.Empty;
            m_geomagnetism = 0;
            m_csq = 0;
            m_power = 0;
            resultStatus = 0;
            m_radar = 0;
        }
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected delegate bool RecvCallback(string recv);
        RecvCallback[] recvHandlers = {
                new RecvCallback(GetNBFlashCallback),
                new RecvCallback(GetNBBatADCCallback),
                new RecvCallback(GetNBMaGsensorCallback),
                new RecvCallback(GetNBVersionCallback),
                new RecvCallback(GetNBNetworkCallback),
                new RecvCallback(GetNBWatchDogCallback),
                new RecvCallback(GetNBPowerCallback),
                new RecvCallback(GetNBImeiCallback),
                new RecvCallback(GetNBRadarADCCallback),
                new RecvCallback(GetNBMode),

            };

        public bool OnRecvMsgHandler(string recv, string comID)
        {
            if (this == null)
            {
                return false;
            }
            log.Info(string.Format("{0}->{1}", comID, recv));
            string[] recvs = recv.Split('|');
            foreach (string recvStr in recvs)
            {
                foreach (RecvCallback handler in recvHandlers)
                {
                    if (handler.Invoke(recvStr))
                        break;
                }
            }
            return true;
        }

        public void DoExternSomething()
        {
            //:判读是否已经测试成功
            string detailStr = string.Format("temp:{0},success:{1}", m_stationSignTemp.getSign(),
                    m_staionSignSuccess.getSign());
            log.Info(detailStr);
            this.m_parentListView.SafeCall(delegate ()
            {
                if (m_stationSignTemp.getSign() == m_staionSignSuccess.getSign())
                {
                    // 上报测试结果
                    resultStatus = 1;
                    self.reportTestResult(testResultSuccess);
                    if (m_parentListView.isTestAll())
                    {
                        System.Console.WriteLine("test all");
                        this.m_ower.SafeCall(delegate ()
                        {
                            m_ower.testSuccessToDo();
                        });
                    }
                }
            });
        }


        private static bool GetNBFlashCallback(string recv)
        {
            if (!self.m_config.m_nbFlash.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("flash:"))
            {
                string[] result = recv.Split(':');
                int flash_count = 1;

                if (Int32.TryParse(result[1], out flash_count) == true)
                {
                    if (flash_count == self.m_config.m_nbFlash.count)
                    {
                        ret = true;
                    }
                }

                self.m_stationSignTemp.setSign(Station.NBFlASH_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBFlash.itemName, ret, recv);
                });
            }
            return ret;
        }

        private static bool GetNBMaGsensorCallback(string recv)
        {
            if (!self.m_config.m_nbMagsensor.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("mag:"))
            {
                string[] result = recv.Split(':');
                string[] xyzVaule = result[1].Split(',');
                double x = double.Parse(xyzVaule[0]);
                double y = double.Parse(xyzVaule[1]);
                double z = double.Parse(xyzVaule[2]);
                double vector;
                string logInfo = recv;
                if (x == 0 && y == 0 && z == 0)
                {
                }
                else if ((x > self.m_config.m_nbMagsensor.x_min) && (y > self.m_config.m_nbMagsensor.y_min) && (z > self.m_config.m_nbMagsensor.z_min) && (x < self.m_config.m_nbMagsensor.x_max) && (y < self.m_config.m_nbMagsensor.y_max) && (z < self.m_config.m_nbMagsensor.z_max))
                {
                    vector = Math.Sqrt(x * x + y * y + z * z);
                    m_geomagnetism = Convert.ToInt32(vector);
                    logInfo = string.Format("recv:{0}, vector:{1}", recv, vector);
                    System.Console.WriteLine(logInfo);
                    if (vector >= self.m_config.m_nbMagsensor.vector_min)
                    {
                        ret = true;
                    }
                }

                self.log.Info(logInfo);
                self.m_stationSignTemp.setSign(Station.NBMAGSENSOR_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBMagsensor.itemName, ret, logInfo);
                });
            }
            return ret;
        }

        private static bool GetNBNetworkCallback(string recv)
        {
            if (!self.m_config.m_nbNetwork.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("nbnet:"))
            {
                string[] result = recv.Split(':');
                Int32 nb_csq = 0;

                if (Int32.TryParse(result[1], out nb_csq) == true)
                {
                    m_csq = nb_csq;
                    self.log.Info(string.Format("nb_csq:{0},nbCsq_min{1}", nb_csq, self.m_config.m_nbNetwork.nbCsq_min));
                    if (nb_csq >= self.m_config.m_nbNetwork.nbCsq_min)
                    {
                        ret = true;
                    }
                }

                self.m_stationSignTemp.setSign(Station.NBNETWORK_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBNetwork.itemName, ret, recv);
                });
            }
            return ret;
        }

        private static bool GetNBBatADCCallback(string recv)
        {
            if (!self.m_config.m_nbBatAdc.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("adc:"))
            {
                string[] result = recv.Split(':');
                Int32 adc_min = 0;

                if (Int32.TryParse(result[1], out adc_min) == true)
                {
                    if (adc_min >= self.m_config.m_nbBatAdc.adc_min)
                    {
                        ret = true;
                    }
                }

                self.m_stationSignTemp.setSign(Station.NBBATADC_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBBatAdc.itemName, ret, recv);
                });
            }
            return ret;
        }

        private static bool GetNBRadarADCCallback(string recv)
        {
            if (!self.m_config.m_nbRadarAdc.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("adc1:"))
            {
                string[] recvs = recv.Split(':');
                string[] adcs = recvs[1].Split(',');

                if (adcs.Length == 2)
                {
                    Int32 adc_min = Int32.Parse(adcs[0]);
                    Int32 adc_max = Int32.Parse(adcs[1]);
                    if (adc_min >= self.m_config.m_nbRadarAdc.adc_min && adc_max <= self.m_config.m_nbRadarAdc.adc_max)
                    {
                        m_radar = adc_min;
                        ret = true;
                    }
                }

                self.m_stationSignTemp.setSign(Station.NBRADARADC_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBRadarAdc.itemName, ret, recv);
                });
            }
            return ret;
        }
        public static bool isRightVersion(string mVersion)
        {
            return (0 == mVersion.CompareTo(self.m_config.m_nbVersion.version));
        }
        private static bool GetNBVersionCallback(string recv)
        {
            if (!self.m_config.m_nbVersion.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("imei_v:"))
            {
                string[] result = recv.Split(':');
                string[] version = result[1].Split(',');
                //System.Console.WriteLine(string.Format("softVer:{0}, hardVer:{1}, sVer:{2}, hVer:{3}", self.m_config.m_nbVersion.softVer, self.m_config.m_nbVersion.hardVer, version[0], version[1]));
                if (version[0].Length > 0 && isRightVersion(version[0]))
                {
                    m_version = version[0];
                    ret = true;
                }

                self.m_stationSignTemp.setSign(Station.NBVERSION_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBVersion.itemName, ret, recv);
                });
            }
            return ret;
        }
        private static bool GetNBWatchDogCallback(string recv)
        {
            if (!self.m_config.m_nbWatchDog.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("wdg:"))
            {
                string[] result = recv.Split(':');
                int watchdog = 0;
                if (Int32.TryParse(result[1], out watchdog) == true)
                {
                    if (watchdog == 1)
                    {
                        ret = true;
                    }
                }

                self.m_stationSignTemp.setSign(Station.NBWATCHDOG_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBWatchDog.itemName, ret, recv);
                });
            }
            return ret;
        }
        private static int m_isTestPowerTimes = 1;

        private static bool GetNBPowerCallback(string recv)
        {
            if (!self.m_config.m_nbPower.do_test) return false;
            if (-1 != recv.IndexOf("lowpower testing"))
            {
                try
                {
                    Int32 testTime = self.m_config.m_nbPower.timeout;
                    double nbLowRange = self.m_config.m_nbPower.lowRange;
                    double nbHighRange = self.m_config.m_nbPower.highRange;
                    double absCurrent = 0;
                    double avgCurrent = 0;

                    for (int i = 0; i < m_isTestPowerTimes; i++)
                    {
                        Thread.Sleep(testTime);
                        avgCurrent = self.m_doTestControl.gpibControl.GetAverageCurrent(1000);
                        absCurrent = Math.Abs(avgCurrent);
                        m_power = (Int32)Math.Round(absCurrent * 1000, 4);
                        self.log.Info(string.Format("get current {0}", avgCurrent));
                        if ((absCurrent) >= nbLowRange && (absCurrent) <= nbHighRange)
                        {
                            self.m_parentListView.SafeCall(delegate ()
                            {
                                string err = String.Format("当前电流为{0},其预期的最小{1}、最大值为{2}", absCurrent, nbLowRange, nbHighRange);
                                self.m_parentListView.SetDetail(NBPower.itemName, true, err);
                            });
                            System.Console.WriteLine(string.Format("test pass;"));
                            self.m_stationSignTemp.setSign(Station.NBPOWER_SUCCESS_SIGN, true);
                            return true;
                        }
                    }
                    self.m_parentListView.SafeCall(delegate ()
                    {
                        string err = String.Format("充电电流测试项-未通过，当前电流为{0},其预期的最小{1}、最大值为{2}", absCurrent, nbLowRange, nbHighRange);
                        self.m_parentListView.SetDetail(NBPower.itemName, false, err);
                    });
                    self.m_stationSignTemp.setSign(Station.NBPOWER_SUCCESS_SIGN, false);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message.ToString());
                }
            }
            else if (-1 != recv.IndexOf("lowpower end"))
            {
                //下发指令设置模式
                if (self.m_config.m_nbMode.do_test)
                {
                    self.m_parentListView.SafeCall(delegate ()
                    {
                        if (resultStatus == 1 && !self.m_parentListView.isHaveFail())
                        {
                            // 发指令进入正常模式
                            resultStatus = 0;
                            self.log.Info("sendCmdSetNbMode");
                            self.m_doTestControl.sendCmdSetNbMode(1);
                            Thread.Sleep(500);
                            self.m_doTestControl.sendCmdSetNbMode(2);
                        }
                    });
                }
            }
            return false;
        }

        private static bool GetNBImeiCallback(string recv)
        {
            if (!self.m_config.m_nbIMEI.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("imei:"))
            {
                string[] result = recv.Split(':');
                if (result.Length != 2) return false;

                string[] imeiAndImsi = result[1].Split(',');
                string imei = imeiAndImsi[0];

                if (imei.Length == 15)
                {
                    self.m_imei = imei;
                    //if (self.m_config.m_nbIMEI.readIMEI == true)
                    //{
                    //    self.GetPrinterCode(imei);
                    //}
                    ret = true;
                }
                self.m_stationSignTemp.setSign(Station.NBIMEI_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBIMEI.itemName, ret, imei);
                });
            }
            return ret;
        }

        private static bool GetNBMode(string recv)
        {
            if (!self.m_config.m_nbMode.do_test) return false;
            bool ret = false;
            if (-1 != recv.IndexOf("mode:"))
            {
                string[] result = recv.Split(':');
                Int32 mode = Int32.Parse(result[1]);
                ret = (mode == self.m_config.m_nbMode.mode) ? true : false;

                self.m_stationSignTemp.setSign(Station.NBMODE_SUCCESS_SIGN, ret);
                self.m_parentListView.SafeCall(delegate ()
                {
                    self.m_parentListView.SetDetail(NBMODE.itemName, ret, mode.ToString());
                });
            }
            return ret;
        }
        private IPStatus PingToWeb(string urlString)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(urlString, timeout, buffer, options);
                string info = "";
                info = info + "Status:" + reply.Status.ToString() + "\n";
                System.Console.WriteLine(info);
                return reply.Status;
            }
            catch (System.Exception ex)
            {
                return IPStatus.Unknown;
            }
        }

        private int PingWeb4Actitioncode(string address, string url)
        {
            HttpProc.WebClient webBrows = new HttpProc.WebClient();
            string urlString = string.Format("http://{0}{1}", address, url);
            webBrows.OpenRead(urlString);
            return webBrows.StatusCode;
        }
        public bool reportTestResult(int m_state)
        {
            //self.m_doTestControl.insertTestResult(self.m_imei, self.m_config.m_nbIMEI.NPModeNum, m_state, self.m_power, m_geomagnetism, m_csq);
            if (self.m_config.m_base_info.station.CompareTo("BMAT") == 0)
                PostBmatCode(self.m_imei, m_state);
            return true;
        }
        public bool PostBmatCode(string imei, int result)
        {
            bool ret = false;
            string msg = string.Empty;
            Console.WriteLine("imei:{0}, status:{1}", imei, result);
            try
            {
                if (imei.Length == 15)
                {
                    SendObject sendJsonObj = new SendObject(imei, m_version, m_power, m_geomagnetism, m_radar, m_csq, result);
                    HttpProc.WebClient webBrows = new HttpProc.WebClient();
                    string postData = sendJsonObj.packJsonStr();
                    string urlString = string.Format("http://{0}{1}", m_config.m_nbServer.host, m_config.m_nbServer.bmatUrl);
                    System.Console.WriteLine(urlString);
                    webBrows.Encoding = Encoding.UTF8;
                    webBrows.OpenRead(urlString, postData, "application/json");//会出现阻塞，网络请求
                    string getDataString = webBrows.RespHtml;

                    System.Console.WriteLine(getDataString);
                    ResultObject resultJsonObj = new ResultObject(getDataString);
                    ret = resultJsonObj.ParseResultData(getDataString);
                    msg = ret ? "succeed" : "上报测试结果失败";
                    if (self.m_config.m_reportResult.do_test)
                    {
                        self.m_stationSignTemp.setSign(Station.REPORT_RESULT_SUCCEDD_SIGN, true);
                        self.m_parentListView.SafeCall(delegate ()
                        {
                            self.m_parentListView.SetDetail(ReportResult.itemName, ret, msg);
                        });
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("PostBmatCode:{0}", err.Message);
            }

            return ret;
        }

        public bool GetPrinterCode(string imei)
        {
            bool ret = false;
            try
            {
                if (imei.Length == 15)
                {
                    string msg = string.Empty;
                    string url = string.Format("http://{0}{1}?imei={2}&device_type={3}", m_config.m_nbServer.host, m_config.m_nbServer.printerUrl, imei, self.m_config.m_nbIMEI.NPModeNum);
                    System.Console.WriteLine("url:{0}", url);
                    HttpProc.WebClient webBrows = new HttpProc.WebClient();
                    webBrows.Encoding = Encoding.GetEncoding("UTF-8");
                    string parseData = webBrows.OpenRead(url);
                    msg = parseData;
                    if (parseData.Length > 0)
                    {
                        ResultObject resultJsonObj = new ResultObject(parseData);
                        System.Console.WriteLine(parseData);
                        if (resultJsonObj.ParseResultData(parseData))
                        {
                            ResultObject.responseBody responseBody = new ResultObject.responseBody();
                            responseBody = resultJsonObj.getResponseBody();
                            if (responseBody.m_result == testResultSuccess && isRightVersion(responseBody.m_version))
                            {
                                if (responseBody.m_deviceType == m_config.m_nbIMEI.NPModeNum && responseBody.m_pairImei.Length == 15)
                                {
                                    string imeiStr = self.m_ower.getLastImeiStr();
                                    if (imeiStr.Length == 15)
                                    {
                                        if (m_config.m_printCode.do_test)
                                        {
                                            PrintImei myPrinter = new PrintImei();
                                            if (myPrinter.PrintData(imeiStr, responseBody.m_pairImei, self.m_config.m_nbIMEI.NPMode))
                                            {
                                                msg = "success";
                                                ret = true;
                                            }
                                            else
                                            {
                                                msg = myPrinter.getLastErro();
                                            }
                                        }
                                        System.Console.WriteLine(string.Format("printer imei:{0}, pairImei: {0}", imeiStr, responseBody.m_pairImei));
                                    }
                                    else
                                    {
                                        msg = string.Format("获取IMEI长度错误：{0}", imeiStr);
                                    }
                                }
                                else
                                {
                                    msg = string.Format("参数匹配错误:m_deviceType[{0}],m_pairImei[{1}]", responseBody.m_deviceType, responseBody.m_pairImei);
                                }
                            }
                            else
                            {
                                msg = string.Format("参数匹配错误:m_result[{0}],m_version[{1}]", responseBody.m_result, responseBody.m_version);
                            }
                        }
                        else
                        {
                            msg = parseData;
                        }
                        self.m_stationSignTemp.setSign(Station.PRINTER_SUCCEDD_SIGN, true);
                        self.m_parentListView.SafeCall(delegate ()
                        {
                            self.m_parentListView.SetDetail(print_code.itemName, ret, msg);
                        });
                        log.Info(parseData + msg);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("GetPrinterCode:{0}", err.Message);
            }

            return ret;
        }

        public bool GetDeviveStatus(string imei)
        {
            bool ret = false;
            try
            {
                if (imei.Length == 15)
                {
                    string msg = string.Empty;
                    string url = string.Format("http://{0}{1}?imei={2}&type={3}", m_config.m_nbServer.host, m_config.m_nbServer.packingUrl, imei, m_config.m_nbIMEI.NPModeNum);
                    System.Console.WriteLine("url:{0}", url);
                    HttpProc.WebClient webBrows = new HttpProc.WebClient();
                    webBrows.Encoding = Encoding.GetEncoding("UTF-8");
                    string parseData = webBrows.OpenRead(url);
                    //msg = parseData;
                    if (parseData.Length > 0)
                    {
                        ResultObject resultJsonObj = new ResultObject(parseData);
                        System.Console.WriteLine(parseData);
                        if (resultJsonObj.ParseResultData(parseData))
                        {
                            ResultObject.responseBody responseBody = new ResultObject.responseBody();
                            responseBody = resultJsonObj.getResponseBody();
                            if (responseBody.m_result == testResultSuccess && isRightVersion(responseBody.m_version))
                            {
                                if (responseBody.m_deviceType == m_config.m_nbIMEI.NPModeNum && responseBody.m_lastStatus == m_config.m_nbPackingStatus.lastOnlineStatus)
                                {
                                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                                    DateTime dt = startTime.AddSeconds(responseBody.m_lastTimestamp);
                                    log.Info(string.Format("hour:{0}", dt.Hour));
                                    if (dt.Hour >= m_config.m_nbPackingStatus.earliestTime && dt.Hour <= m_config.m_nbPackingStatus.lastestTime && dt.Day == m_config.m_nbPackingStatus.lastCommunicationDay)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        msg = string.Format("参数匹配错误：Hour[{0}]", dt.Hour);
                                    }
                                }
                                else
                                {
                                    msg = string.Format("参数匹配错误:m_deviceType[{0}],m_lastStatus[{1}]", responseBody.m_deviceType, responseBody.m_lastStatus);
                                }
                            }
                            else
                            {
                                msg = string.Format("参数匹配错误:m_result[{0}],m_version[{1}]", responseBody.m_result, responseBody.m_version);
                            }
                        }
                        else
                        {
                            msg = parseData;
                        }
                        self.m_stationSignTemp.setSign(Station.NBPACKING_SUCCEDD_SIGN, ret);
                        self.m_parentListView.SafeCall(delegate ()
                        {
                            self.m_parentListView.SetDetail(NBPackingStatus.itemName, ret, msg);
                        });
                        log.Info(parseData + msg);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("GetDeviveStatus:{0}", err.Message);
            }

            return ret;
        }
        public bool GetUnpairDevice(string imei, int type)
        {
            bool ret = false;
            try
            {
                if (imei.Length == 15)
                {
                    string msg = string.Empty;
                    string url = string.Format("http://{0}{1}?imei={2}&device_type={3}", m_config.m_nbServer.host, m_config.m_nbServer.unPairUrl, imei, type);
                    System.Console.WriteLine("url:{0}", url);
                    HttpProc.WebClient webBrows = new HttpProc.WebClient();
                    webBrows.Encoding = Encoding.GetEncoding("UTF-8");
                    string parseData = webBrows.OpenRead(url);
                    msg = parseData;
                    if (parseData.Length > 0)
                    {
                        ResultObject resultJsonObj = new ResultObject(parseData);
                        System.Console.WriteLine(parseData);
                        if (resultJsonObj.ParseResultData(parseData))
                        {
                            ret = true;
                            msg = "解绑成功";
                            self.m_stationSignTemp.setSign(Station.NBPACKING_SUCCEDD_SIGN, true);
                            self.m_parentListView.SafeCall(delegate ()
                            {
                                self.m_parentListView.SetDetail(NBPackingStatus.itemName, ret, msg);
                            });
                        }
                        self.m_stationSignTemp.setSign(Station.NBPACKING_SUCCEDD_SIGN, ret);
                        self.m_parentListView.SafeCall(delegate ()
                        {
                            self.m_parentListView.SetDetail(NBPackingStatus.itemName, ret, msg);
                        });
                        log.Info(parseData + msg);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("GetUnpairDevice:{0}", err.Message);
            }

            return ret;
        }
    }
}
