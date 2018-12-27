using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using log4net;

namespace MAT
{
    class Dbj_GetSNAndIMEI
    {
        private string m_sn;
        private string m_imei;
        private string m_lastErroStr;

        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool InitDataByNetWork()
        {
            m_sn = "";
            m_imei = "";
            int tryTimes = 3;
            //check the network 
            //string hostIP = strTmps[0];
            

            //while(tryTimes>0)
            //{
            //    tryTimes--;
            //    IPStatus retIPStatus = PingToWeb(hostIP);
            //    if (retIPStatus != IPStatus.Success)
            //    {
            //        continue;
            //    }
            //    //通过网络获取到SN和IMEI
            //    HttpProc.WebClient webBrows = new HttpProc.WebClient();
            //    webBrows.Encoding = Encoding.UTF8;
            //    string urlString = string.Format("http://{0}{1}?version={2}", m_SNConfigObj.autoGetSN_host,
            //        m_SNConfigObj.autoGetSN_url, m_SNConfigObj.autoGetSN_Version);
            //    webBrows.OpenRead(urlString);//会出现阻塞，网络请求
            //    string getDataString = webBrows.RespHtml;
            //    log.Info(string.Format("获取SN和IMEI的返回数据：{0}", getDataString));
            //    TranJson_SNAndIMEI snAndImeiObj = new TranJson_SNAndIMEI(getDataString);
            //    bool ret = snAndImeiObj.ParseData();
            //    if (ret == true)
            //    {
            //        m_sn = snAndImeiObj.sn;
            //        m_imei = snAndImeiObj.imei;
            //        return true;
            //    }
            //}
            return false;
        }

        public string GetSN()
        {
            return m_sn;
        }

        public string GetIMEI()
        {
            return m_imei;
        }

        public string GetLastErroStr()
        {
            return this.m_lastErroStr;
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
                this.m_lastErroStr = string.Format("PingToWeb(string urlString) {0}", ex.Message);
                return IPStatus.Unknown;
            }
        }
    }
}
