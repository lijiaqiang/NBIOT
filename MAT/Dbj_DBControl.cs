using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace MAT
{
    /************************************************************************/
    /* Dbj_BMAT_table        BMAT测试结果记录对象                           */
    /************************************************************************/
    public class Dbj_BMAT_table
    {
        public Dictionary<string, string> table = new Dictionary<string,string>();
        private string undoStr = "UNDO";
        private string passStr = "PASS";
        private string failStr = "FAIL";
        public Dbj_BMAT_table()
        {
            table["TS"] = DateTime.Now.ToString();
            table["INTERNAL_SN"] = undoStr;
            table["CFT_STATUS"] = undoStr;
            table["BMAT_STATUS"] = undoStr;
            table["BMAT_CHARGE_CURRENT"] = undoStr;
            table["BMAT_WRITE_SN"] = undoStr;
            table["BMAT_GSENSOR"] = undoStr;
            table["BMAT_GPS"] = undoStr;
            table["BMAT_LED"] = undoStr;
            table["BMAT_SIM"] = undoStr;
            table["BMAT_TEMPERTURE"] = undoStr;
            table["BMAT_BLUETOOTH"] = undoStr;
            table["BMAT_PHASECHECK"] = undoStr;
            table["BMAT_USB_EXCEPTION"] = undoStr;
        }

        public void ClearAllData()
        {
            table["TS"] = DateTime.Now.ToString();
            table["INTERNAL_SN"] = undoStr;
            table["CFT_STATUS"] = undoStr;
            table["BMAT_STATUS"] = undoStr;
            table["BMAT_CHARGE_CURRENT"] = undoStr;
            table["BMAT_WRITE_SN"] = undoStr;
            table["BMAT_GSENSOR"] = undoStr;
            table["BMAT_GPS"] = undoStr;
            table["BMAT_LED"] = undoStr;
            table["BMAT_SIM"] = undoStr;
            table["BMAT_TEMPERTURE"] = undoStr;
            table["BMAT_BLUETOOTH"] = undoStr;
            table["BMAT_PHASECHECK"] = undoStr;
            table["BMAT_USB_EXCEPTION"] = undoStr;
        }

        public bool SetTestResult(string testName,
                                  int isPase/*0:false,1:pase,2 or other:undo*/)
        {
            string testNameTmp = string.Format("BMAT_{0}", testName);
            if (table.ContainsKey(testNameTmp) == false)
            {
                return false;
            }
            switch (isPase)
            {
                case 0:
                    table[testNameTmp] = failStr;
                    break;
                case 1:
                    table[testNameTmp] = passStr;
                    break;
                default:
                    table[testNameTmp] = undoStr;
                    break;
            }
            return true;
        }

        public string getInsertStr()
        {
            table["TS"] = DateTime.Now.ToString();
            string sqlStr = null;
            bool isBegin = true;
            sqlStr = sqlStr + "INSERT INTO BMAT_TABLE(";
            foreach (KeyValuePair<string, string> pair in table)
            {
                if (isBegin)
                {
                    sqlStr = sqlStr + pair.Key;
                    isBegin = false;
                }
                else
                {
                    sqlStr = sqlStr + ",";
                    sqlStr = sqlStr + pair.Key;
                }
            }
            sqlStr = sqlStr + ") VALUES (";
            isBegin = true;
            foreach (KeyValuePair<string, string> pair in table)
            {
                if (isBegin)
                {
                    sqlStr = sqlStr + string.Format("'{0}'", pair.Value);
                    isBegin = false;
                }
                else
                {
                    sqlStr = sqlStr + ",";
                    sqlStr = sqlStr + string.Format("'{0}'", pair.Value);
                }
            }
            sqlStr = sqlStr + ")";
            return sqlStr;
        }
    }

    /************************************************************************/
    /*Dbj_FMAT_table         FMAT测试结果记录对象                           */
    /************************************************************************/
    public class Dbj_FMAT_table
    {
        public Dictionary<string, string> table = new Dictionary<string, string>();
        private string undoStr = "UNDO";
        private string passStr = "PASS";
        private string failStr = "FAIL";
        public Dbj_FMAT_table()
        {
            table["TS"] = DateTime.Now.ToString();
            table["INTERNAL_SN"] = undoStr;
            table["EXTERNAL_SN"] = undoStr;
            table["IMEI"] = undoStr;
            table["CFT_STATUS"] = undoStr;
            table["BMAT_STATUS"] = undoStr;
            table["COUPLE_STATUS"] = undoStr;
            table["FMAT_STATUS"] = undoStr;
            table["FMAT_KEY"] = undoStr;
            table["FMAT_GSENSOR"] = undoStr;
            table["FMAT_GPS"] = undoStr;
            table["FMAT_LED"] = undoStr;
            table["FMAT_SCRIPT"] = undoStr;
            table["FMAT_BLUETOOTH"] = undoStr;
            table["FMAT_PHASECHECK"] = undoStr;
            table["FMAT_USB_EXCEPTION"] = undoStr;
        }

        public void ClearAllData()
        {
            table["TS"] = DateTime.Now.ToString();
            table["INTERNAL_SN"] = undoStr;
            table["EXTERNAL_SN"] = undoStr;
            table["IMEI"] = undoStr;
            table["CFT_STATUS"] = undoStr;
            table["BMAT_STATUS"] = undoStr;
            table["COUPLE_STATUS"] = undoStr;
            table["FMAT_STATUS"] = undoStr;
            table["FMAT_KEY"] = undoStr;
            table["FMAT_GSENSOR"] = undoStr;
            table["FMAT_GPS"] = undoStr;
            table["FMAT_LED"] = undoStr;
            table["FMAT_SCRIPT"] = undoStr;
            table["FMAT_BLUETOOTH"] = undoStr;
            table["FMAT_PHASECHECK"] = undoStr;
            table["FMAT_USB_EXCEPTION"] = undoStr;
        }

        public bool SetTestResult(string testName,
                                  int isPase/*0:false,1:pase,2 or other:undo*/)
        {
            string testNameTmp = string.Format("FMAT_{0}", testName);
            if (table.ContainsKey(testNameTmp) == false)
            {
                return false;
            }
            switch (isPase)
            {
                case 0:
                    table[testNameTmp] = failStr;
                    break;
                case 1:
                    table[testNameTmp] = passStr;
                    break;
                default:
                    table[testNameTmp] = undoStr;
                    break;
            }
            return true;
        }

        public string getInsertStr()
        {
            table["TS"] = DateTime.Now.ToString();
            string sqlStr = null;
            bool isBegin = true;
            sqlStr = sqlStr + "INSERT INTO FMAT_TABLE(";
            foreach (KeyValuePair<string,string> pair in table)
            {
                if (isBegin)
                {
                    sqlStr = sqlStr + pair.Key;
                    isBegin = false;
                }
                else
                {
                    sqlStr = sqlStr + ",";
                    sqlStr = sqlStr + pair.Key;
                }
            }
            sqlStr = sqlStr + ") VALUES (";
            isBegin = true;
            foreach (KeyValuePair<string, string> pair in table)
            {
                if (isBegin)
                {
                    sqlStr = sqlStr + string.Format("'{0}'", pair.Value);
                    isBegin = false;
                }
                else
                {
                    sqlStr = sqlStr + ",";
                    sqlStr = sqlStr + string.Format("'{0}'", pair.Value);
                }
            }
            sqlStr = sqlStr + ")";
            return sqlStr;
        }
    }

    /************************************************************************/
    /*Dbj_DBControl        数据库操作对象                                   */
    /************************************************************************/
    public class Dbj_DBControl
    {
        OleDbConnection conn = null;
        OleDbDataReader reader = null;
        OleDbCommand cmd = null;
        string exePathstr = null;

        string errStr = "Dbj_DBControl,没有异常";

        private string dataSource, connect_string;

        public Dbj_DBControl(string dataName, string connect_string)
        {
            exePathstr = System.Environment.CurrentDirectory;
            this.dataSource = exePathstr + "/";
            this.dataSource = this.dataSource + dataName;
            this.connect_string = connect_string;
        }

        public Dbj_DBControl()
        {
            exePathstr = System.Environment.CurrentDirectory;
            this.dataSource = exePathstr+"/MAT.accdb";
            this.connect_string = "Provider=Microsoft.ACE.OLEDB.12.0; ";
        }

        public string getErrStr()
        {
            return errStr;
        }

        private bool IsFileExist(string file)
        {
            return System.IO.File.Exists(file);
        }

        public bool initConnect()
        {
            if (IsFileExist(this.dataSource) == false)
            {
                this.errStr = this.dataSource + ",不存在!";
                return false;
            }
            try
            {
                conn = new OleDbConnection(this.connect_string + "Data Source=" + this.dataSource);
                conn.Open();
            }
            catch (System.Exception ex)
            {
                errStr = ex.Message;
                return false;
            }
            return true;
        }

        public bool closeConnect()
        {
            if (conn != null)
            {
                conn.Close();
            }
            return true;
        }


        public bool sqlCmd(string queryString)
        {//执行sql命令
            System.Console.WriteLine(queryString);
            bool ret = false;
            try
            {
                OleDbConnection m_oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=MAT.accdb");
                m_oleDbConnection.Open();
                OleDbCommand m_oleDbCommand = new OleDbCommand(queryString, m_oleDbConnection);
                OleDbDataReader m_oleDbDataReader = m_oleDbCommand.ExecuteReader();
                ret = (1 == m_oleDbDataReader.RecordsAffected);
                m_oleDbDataReader.Close();
                m_oleDbConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("sqlCmd() erro:{0}", e.Message);
            }

            return ret;
        }
        public bool RepeatCheckID(string id)
        {//??
            string queryString = String.Format("SELECT IDS FROM ID_TABLE WHERE IDS='{0}'", id);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
	            reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                //System.Console.WriteLine(reader.GetString(0));
                reader.Close();
                return false;
            }

            reader.Close();
            return true;
        }

        /*
         * 记录已经使用过的SN
         */
        public bool insertUsingSN(string SN, string IMEI)
        {
            string queryString;
            bool ret = false;
            queryString = String.Format("INSERT INTO SN_TABLE(SN,IMEI) VALUES (\"{0}\",\"{1}\");", SN,IMEI);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                this.m_ativeCodeValidStr = string.Format("ExecuteReader code:3 erro!info:{0}", ex.Message);
                reader.Close();
                return ret;
            }
            ret = (1 == reader.RecordsAffected);
            reader.Close();
            return ret;
        }

        public bool RepeatCheck(string SN)
        {//检查SN是否已经使用过
            string queryString = String.Format("SELECT IDS FROM SN_TABLE WHERE IDS='{0}'", SN);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
	            reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                reader.Close();
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                System.Console.WriteLine(reader.GetString(0));
                reader.Close();
                return false;
            }

            reader.Close();
            return true;
        }

        public bool IsSNValid(string SN,bool bFMAT)
        {//检查SN是否已经测试过
            string queryString;
            if (bFMAT)
                queryString = String.Format("SELECT * FROM FMAT_TABLE WHERE EXTERNAL_SN='{0}' AND FMAT_STATUS='PASS'", SN);
            else
                queryString = String.Format("SELECT * FROM BMAT_TABLE WHERE INTERNAL_SN='{0}' AND BMAT_STATUS='PASS'", SN);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;
            try
            {
	            reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                //System.Console.WriteLine(reader.GetString(0));
                reader.Close();
                return false;
            }
            reader.Close();
            return true;
        }

        public string GetIMEI(string SN)
        {//获取SN对应的IMEI号
            string imei_string = null;
            string queryString = String.Format("SELECT IMEI FROM IMEI_TABLE WHERE SN='{0}'",SN);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
	            reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                return "读取数据库出错！-bug";
            }
            if (!reader.HasRows)
            {
                imei_string = null;
                reader.Close();
                this.errStr = string.Format("没有找到IMEI，SN={0}",SN);
                return imei_string;
            }
            reader.Read();
            try
            {
	            imei_string = reader.GetString(0);
            }
            catch (System.Exception ex)
            {
                this.errStr = string.Format("获取IMEI的时候失败！{0}",ex.Message);
                imei_string = null;
            }
            reader.Close();
            return imei_string;
        }

        /************************************************************************/
        /*
         * AtiveCode
         */
        /************************************************************************/

        private string m_ativeCodeValidStr;
        public string getAtciveCodeValidFalseStr()
        {
            return this.m_ativeCodeValidStr;
        }
        public bool IsAticveCodeValid(string sn, string iccid)
        {
            m_ativeCodeValidStr = string.Empty;
            string queryString;
            queryString = String.Format("SELECT * FROM CODE_TABLE WHERE SN='{0}' and ICCID='{1}';",
                sn, iccid);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                this.m_ativeCodeValidStr = string.Format("ExecuteReader code:1 erro!info:{0}",ex.Message);
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                //System.Console.WriteLine(reader.GetString(0));
                reader.Close();
                queryString = string.Format("SELECT * FROM CODE_TABLE WHERE SN='{0}';", sn);
                cmd.CommandText = queryString;
                try
                {
                    reader = cmd.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    this.errStr = ex.Message;
                    this.m_ativeCodeValidStr = string.Format("ExecuteReader code:2 erro!info:{0}", ex.Message);
                    return false;
                }
                if (reader.HasRows && reader.Read())
                {
                    this.m_ativeCodeValidStr = string.Format("SN已经使用过！");
                    return false;
                }
                reader.Close();

                queryString = string.Format("SELECT * FROM CODE_TABLE WHERE ICCID='{0}';", iccid);
                cmd.CommandText = queryString;
                try
                {
                    reader = cmd.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    this.errStr = ex.Message;
                    this.m_ativeCodeValidStr = string.Format("ExecuteReader code:3 erro!info:{0}", ex.Message);
                    return false;
                }
                if (reader.HasRows && reader.Read())
                {
                    this.m_ativeCodeValidStr = string.Format("SIM已经使用过！");
                    return false;
                }
                reader.Close();

                return false;
            }
            reader.Close();
            return true;
        }

        public bool InsertRecondAtiveCode(string sn, string iccid, string code)
        {
            m_ativeCodeValidStr = string.Empty;
            string queryString;
            bool ret = false;
            queryString = String.Format("INSERT INTO CODE_TABLE(SN,ICCID,CODE) VALUES (\"{0}\",\"{1}\",\"{2}\");",
                sn, iccid, code);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                this.m_ativeCodeValidStr = string.Format("ExecuteReader code:3 erro!info:{0}", ex.Message);
                return ret;
            }
            ret = (1 == reader.RecordsAffected);
            return ret;
        }


        /*
         *	FOB_ID 
         */
        public bool checkFobIDIsUseing(string id)
        {
            string queryString = String.Format("SELECT IDS FROM ID_TABLE WHERE IDS='{0}'", id);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                //System.Console.WriteLine(reader.GetString(0));
                reader.Close();
                return false;
            }

            reader.Close();
            return true;
        }


        public bool CheckFobID(string fobID)
        {
            //读取数据
            string queryString = String.Format("SELECT user_time,fob_id,sn FROM FOBID_TABLE WHERE fob_id='{0}'", fobID);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                reader.Close();
                return false;
            }
            if (reader.HasRows && reader.Read())
            {
                //读取到了               
                reader.Close();
                return false;
            }
            else
            {
                //没有读取到
                reader.Close();
                return true;
            }
        }

        public FOBID_TABLE InsertFobID(string fobID,string sn)
        {//检查id是否已经被使用，否则添加数据记录
            FOBID_TABLE fobID_table = new FOBID_TABLE();

            //读取数据
            string queryString = String.Format("SELECT user_time,fob_id,sn FROM FOBID_TABLE WHERE fob_id='{0}'", fobID);
            if (null == cmd)
                cmd = new OleDbCommand(queryString, conn);
            else
                cmd.CommandText = queryString;

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                this.errStr = ex.Message;
                fobID_table.status = -1;
                reader.Close();
            }
            if (reader.HasRows && reader.Read())
            {
                //读取到了
                fobID_table.user_time = reader.GetString(0);
                fobID_table.fobID = reader.GetString(1);
                fobID_table.user_SN = reader.GetString(2);
                fobID_table.status = 1;
                reader.Close();
            }
            else
            {
                reader.Close();
                //没有读取到
                string user_timer = DateTime.Now.ToString(); ;
                string insertString = string.Format("INSERT INTO FOBID_TABLE(user_time,fob_id,sn) VALUES ('{0}','{1}','{2}');", 
                    user_timer, fobID, sn);

                if (sqlCmd(insertString)==false)
                {
                    fobID_table.status = -1;
                }
                else
                {
                    fobID_table.status = 0;
                }
            }
                        
            return fobID_table;
        }

        /*
         * 新的测试数据记录
         */
        public bool InsertNewTestData(String sn,String station,int testSign,int successSign,String detail="")
        {
            String insertString = String.Format("INSERT INTO MAT_TABLE(SN,Station,TestSign,SuccessSign,Detail,DataTime) VALUES ('{0}','{1}',{2},{3},'{4}','{5}');",
                sn, station, testSign, successSign, detail, DateTime.Now.ToLocalTime().ToString());
            if (sqlCmd(insertString) == false)
            {
                return false;
            }
             return true;
        }

     }

    public struct FOBID_TABLE
    {
        //当status=1的时候，有值，其他为空
        public string user_time;//使用的时间
        public string user_SN;//使用中的终端sn
        public string fobID;//挂件id

        public int status;/*查询反馈的状态码：0（保存成功）；1（已经使用）；other（其他原因） */
    }
}
