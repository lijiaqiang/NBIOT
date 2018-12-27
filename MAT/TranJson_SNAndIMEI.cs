using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MAT
{
    /*
{
    "data": {
        "sn": "3A28200012",
        "timestamp": 1416292358426,
        "allocate": true,
        "_id": "546ae8066ec46dd44dc509a3",
        "alloc_timestamp": 1416292476732
    },
    "status_code": 200,
    "time_used": 5
}
{"status_code":404,"time_used":2}
     * */
    class TranJson_SNAndIMEI
    {
        public string sn;
        public string imei;
        private int status;
        private string m_jsonData;
        private string m_lastErro;
        public TranJson_SNAndIMEI(string jsonData)
        {
            m_jsonData = jsonData;
            sn = "";
            imei = "";
        }

        public string getLastErro()
        {
            return this.m_lastErro;
        }

        public bool ParseData()
        {
            sn = "";
            imei = "";

            JObject ja;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(m_jsonData);
            }
            catch (System.Exception ex)
            {
                this.m_lastErro = ex.Message;
                return false;
            }

            try
            {
                status = (int)ja["status_code"];
                if (status != 200)
                {
                    this.m_lastErro = string.Format("获取到的数据为空！");
                    return false;
                }
                sn = ja["data"]["sn"].ToString();
                imei = ja["data"]["imei"].ToString();
            }
            catch (System.Exception ex)
            {
                this.m_lastErro = ex.Message;
                return false;
            }
            return true;
        }
         
    }
}
