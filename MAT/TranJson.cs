using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MAT
{

    /************************************************************************/
    /* 
        {
        “status”: 0,
        “message”: “操作成功”, 
        “sn”:”123refgfdfdg”,
        “iccid”:’’89302720396911973415”,
        “activation_code”: “123456789d” 
        } 
     * 
        {
        "status": 35,
        "message": "终端录入成功!",
        "activation_code": "JQ4WDGEKR6",
        "sn": "38A64015E5"
        }
     */
    /************************************************************************/
    class ResultObject
    {
        public struct responseBody
        {
            public int status;
            public string message;
            public int m_result;
            public int time_used;
            public int m_deviceType;
            public int m_lastStatus;
            public int m_cardType;
            public Int32 m_lastTimestamp;
            public string activation_code;
            public string m_jsonStr;
            public string m_imei;
            public string m_pairImei;
            public string m_version;
        }

        private string m_lastErro;
        public responseBody m_responseBody;

        public ResultObject(string jsonStr)
        {
            m_responseBody = new responseBody();
            m_responseBody.m_jsonStr = jsonStr;
        }

        public responseBody getResponseBody()
        {
            return m_responseBody;
        }

        public string getLastErro()
        {
            return m_lastErro;
        }
        public bool ParseResultData(string jsonStr)
        {
            JObject ja;
            bool ret = false;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonStr);
            }
            catch (System.Exception ex)
            {
                this.m_lastErro = ex.Message;
                return ret;
            }

            try
            {
                if (ja["status"] != null) m_responseBody.status = (int)ja["status"];
                if (ja["message"] != null) m_responseBody.message = ja["message"].ToString();
                if ((m_responseBody.status == 200) && (m_responseBody.message.CompareTo("success") == 0))
                {
                    if (ja["data"] != null)
                    {
                        if (ja["data"]["card_type"] != null) m_responseBody.m_cardType = (int)ja["data"]["card_type"];
                        if (ja["data"]["last_status"] != null) m_responseBody.m_lastStatus = (int)ja["data"]["last_status"];
                        if (ja["data"]["version"] != null) m_responseBody.m_version = (string)ja["data"]["version"];
                        if (ja["data"]["pair_imei"] != null) m_responseBody.m_pairImei = (string)ja["data"]["pair_imei"];
                        if (ja["data"]["device_type"] != null) m_responseBody.m_deviceType = (int)ja["data"]["device_type"];
                        if (ja["data"]["result"] != null) m_responseBody.m_result = (int)ja["data"]["result"];
                        if (ja["data"]["last_timestamp"] != null) m_responseBody.m_lastTimestamp = (Int32)ja["data"]["last_timestamp"];

                        System.Console.WriteLine(string.Format("m_result:{0}, version:{1}", m_responseBody.m_result, m_responseBody.m_version));
                    }
                }
                
                return m_responseBody.status == 200;

            }
            catch (System.Exception ex)
            {
                this.m_lastErro = ex.Message;
            }
            return ret;
        }
    }

    /*
    {
    “sn”: “330A0000CB”,
    “iccid”: “460023249296387”,
    “sign”:”0c4f7f7b57b91e7520ab66fb18a7a517”
    }
     * 
     * #define HMAC_MD5_KEY "5ZOl5Lus77yM5L2g5piv5YaF6YOo5pyN5Yqh5Yi35paw55qE5ZCX77yf"
     */
    class SendObject
    {
        private string imei;
        private int type;
        private int result;
        private int action;
        private Int32 power;
        private Int32 geomagnetism;
        private Int32 csq;
        private Int32 radar;
        private string version;
        private static string HMAC_MD5_KEY = "5ZOl5Lus77yM5L2g5piv5YaF6YOo5pyN5Yqh5Yi35paw55qE5ZCX77yf";
        public string sign;
        public SendObject(string m_imei, string m_version, Int32 m_power, Int32 m_geomagnetism, Int32 m_radar, Int32 m_csq, int m_result)
        {
            this.imei = m_imei;
            this.version = m_version;
            this.power = m_power;
            this.geomagnetism = m_geomagnetism;
            this.radar = m_radar;
            this.csq = m_csq;
            this.result = m_result;
        }

        public string packJsonStr()
        {
            string jsonStr = string.Empty;
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("IMEI");
            writer.WriteValue(imei);
            writer.WritePropertyName("version");
            writer.WriteValue(version);
            writer.WritePropertyName("power");
            writer.WriteValue(power);
            writer.WritePropertyName("geomagnetism");
            writer.WriteValue(geomagnetism);
            writer.WritePropertyName("radar");
            writer.WriteValue(radar);
            writer.WritePropertyName("csq");
            writer.WriteValue(csq);
            writer.WritePropertyName("result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            writer.Flush();

            jsonStr = sw.GetStringBuilder().ToString();
            Console.WriteLine(jsonStr);
            return jsonStr;
        }

        /**
         *
         *  hmac_md5口令加密算法
         * 
         */
        public byte[] hmac_md5(string timespan, string password)
        {
            byte[] b_tmp;
            byte[] b_tmp1;
            if (password == null)
            {
                return null;
            }
            byte[] digest = new byte[512];
            byte[] k_ipad = new byte[64];
            byte[] k_opad = new byte[64];
            byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
            System.Security.Cryptography.MD5 shainner = new MD5CryptoServiceProvider();
            for (int i = 0; i < 64; i++)
            {
                k_ipad[i] = 0 ^ 0x36;
                k_opad[i] = 0 ^ 0x5c;
            }

            try
            {
                if (source.Length > 64)
                {
                    shainner = new MD5CryptoServiceProvider();
                    source = shainner.ComputeHash(source);
                }
                for (int i = 0; i < source.Length; i++)
                {
                    k_ipad[i] = (byte)(source[i] ^ 0x36);
                    k_opad[i] = (byte)(source[i] ^ 0x5c);
                }
                b_tmp1 = System.Text.ASCIIEncoding.ASCII.GetBytes(timespan);
                b_tmp = adding(k_ipad, b_tmp1);
                shainner = new MD5CryptoServiceProvider();
                digest = shainner.ComputeHash(b_tmp);
                b_tmp = adding(k_opad, digest);
                shainner = new MD5CryptoServiceProvider();
                digest = shainner.ComputeHash(b_tmp);
                return digest;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /**
        *
        *  填充byte
        * 
        */
        public byte[] adding(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }

        /**
         * 转换为16进制的字符串
         */
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }


    }
}
