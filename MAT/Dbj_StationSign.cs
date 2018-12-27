using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;

namespace MAT
{
    public enum Station
    {
        NBWATCHDOG_SUCCESS_SIGN = 0,
        NBVERSION_SUCCESS_SIGN,
        NBBATADC_SUCCESS_SIGN,
        NBRADARADC_SUCCESS_SIGN,
        NBPOWER_SUCCESS_SIGN,
        NBFlASH_SUCCESS_SIGN,
        NBNETWORK_SUCCESS_SIGN,
        NBMAGSENSOR_SUCCESS_SIGN,
        NBIMEI_SUCCESS_SIGN,
        NBMODE_SUCCESS_SIGN,
        NBSN_SUCCEDD_SIGN,
        NBPACKING_SUCCEDD_SIGN,
        REPORT_RESULT_SUCCEDD_SIGN,
        PRINTER_SUCCEDD_SIGN,
        StationEnd,//max 30
    }
    public enum nbStation
    {
        NBFlASH_TEST_SIGN = 1,
        NBMAGSENSOR_TEST_SIGN,
        NBNETWORK_TEST_SIGN,
        NBBATADC_TEST_SIGN,
        NBWATCHDOG_TEST_SIGN,
        NBPOWER_TEST_SIGN,
        NBVERSION_TEST_SIGN,
        NBGETIMEI_TEST_SIGN,
        NBSETMODE_TEST_SIGN,
        NBSTART_TEST_SIGN,
    }

    public class Bitwise
    {
        //index从0开始 
        //获取取第index位 
        protected int GetBit(int b, int index)
        {
            return ((b & (1 << index)) > 0) ? 1 : 0;
        }
        //将第index位设为1 
        protected int SetBit(int b, int index)
        {
            return (int)(b | (1 << index));
        }
        //将第index位设为0 
        protected int ClearBit(int b, int index)
        {
            return (int)(b & (int.MaxValue - (1 << index)));
        }
        //将第index位取反 
        protected int ReverseBit(int b, int index)
        {
            return (int)(b ^ (int)(1 << index));
        }
    }

    /*
     * 测试项目的计算
     */
    public class Dbj_ItemSign:Bitwise
    {
        /*
        protected const int SI4330_SUCCESS_SIGN = 1<<1; 2
        protected const int GSENSOR_SUCCESS_SIGN = 1<<2;
        protected const int KEYBOARD_SUCCESS_SIGN = 1<<3;
        protected const int LED_SUCCESS_SIGN = 1<<4;
        protected const int SIM_SUCCESS_SIGN = 1<<5;
        protected const int SCRIPT_SUCCESS_SIGN = 1<<6;
        protected const int TEMPERATURE_SUCCESS_SIGN = 1<<7;
        protected const int BLUETOOTH_SUCCESS_SIGN = 1<<8;
        protected const int KEYFOB_SUCCESS_SIGN = 1 << 9;
        protected const int ACC_SUCCESS_SIGN = 1 << 10;
        protected const int VOICE_SUCCESS_SIGN = 1 << 11;
        protected const int POWER_SUCCESS_SIGN = 1 << 12;
        protected const int PAIR_CHECK_SUCCESS_SIGN = 1 << 13;
        protected const int PAIR_SAVE_SUCCESS_SIGN = 1 << 14;
         */
        public int sign = 0;
        public Dbj_ItemSign(int initSign = 0)
        {
            this.sign = initSign;
            /*test{
            int a = 0;
            int b = SetBit(a, (int)Station.StationEnd);
            System.Console.WriteLine("{0}", b);
            System.Console.WriteLine("{0}", 1 << 1);
            resetSign(true);
            System.Console.WriteLine("{0}",sign);
            //}end*/
        }

        public bool checkTestSign(int testSign,int successSign)
        {
            return false;
        }

        public bool checkResultSign(int resultSign,int successSign)
        {
            return false;
        }

        public void resetSign(bool isHight = false)
        {//重置站位信息，isHight选择是否用1高位来填充
            if (isHight)
            {
                for (int index = 1; index < (int)Station.StationEnd; index++)
                {
                    sign = SetBit(sign, index);
                }
            }
            else
            {
                sign = 0;
            }
        }

        public int setSign(Station station, bool isPass)
        {//设置站位信息是否通过,返回设置后的站位信息
            if (isPass)
            {
                sign = SetBit(sign, (int)station);
            }
            else
            {
                sign = ClearBit(sign, (int)station);
            }
            return sign;
        }

        public int getSign()
        {//获取到站位信息
            return sign;
        }

        public bool getItemSign(Station station)
        {//获取当前站位的是否通过
            bool isPass = false;
            if (GetBit(this.sign, (int)station) == 1)
            {
                isPass = true;
            }
            else
            {
                isPass = false;
            }
            return isPass;
        }

    }

    public class DBJ_StationSign:Bitwise
    {
        public string m_allStaion;
        public Queue<string> stations;
        public DBJ_StationSign(string all_staion)
        {
            m_allStaion = all_staion;
            stations = new Queue<string>();
            string[]staionStrs = m_allStaion.Split(' ');
            for (int i = 0; i < staionStrs.Length;i++ )
            {
                stations.Enqueue(staionStrs[i]);
            }
        }

        public int getStaionInt(string stationStr)
        {//获取测试站位的对应的下标
            int nStation = -1;
            int i = 0;
            foreach (string str in stations)
            {
                if (str.Equals(stationStr)==true)
                {
                    nStation = i;
                    break;
                }
                i++;
            }
            return nStation;
        }


        public bool getStationSign(int station,int iTestSign)
        {//获取当前站位的是否通过
            if (iTestSign == -1)
            {
                return false;
            }
            bool isPass = false;
            if (GetBit(iTestSign, station) == 0)
            {
                isPass = true;
            }
            else
            {
                isPass = false;
            }
            return isPass;
        }

        public bool getStationSign(string station, int iTestSign)
        {//获取当前站位的是否通过
            if (iTestSign == -1)
            {
                return false;
            }
            bool isPass = false;
            int nStation = getStaionInt(station);
            if (nStation == -1)
            {
                return false;
            }
            if (GetBit(iTestSign,nStation) == 0)
            {
                isPass = true;
            }
            else
            {
                isPass = false;
            }
            return isPass;
        }

        public string getIsNotTestStation(string stationStr, int iTestSign,int iItemSign)
        {//检测测试站位是否已经测试过
            if (iTestSign == -1)
            {
                return "iTestSign erro";
            }
            string passStation = "";
            foreach (string str in stations)
            {
                if (str.Equals(stationStr)==true)
                {
                    break;
                }
                else
                {

                    if (getStationSign(str, iTestSign) == false)
                    {
                        string temp = string.Format("{0}-没有测试\n", str);
                        passStation += temp;
                    }
                    else
                    {
                        if (getStationSign(str,iItemSign) == false)
                        {
                            string temp = string.Format("{0}-没有通过\n", str);
                            passStation += temp;
                        }
                    }

                }
            }
            return passStation;
        }

        public string getIsNotPassStation(string stationStr, int iTestSign)
        {//检测测试站位是否已经测通过
            if (iTestSign == -1)
            {
                return "iTestSign erro";
            }
            string passStation = "";
            foreach (string str in stations)
            {
                if (str.Equals(stationStr) == true)
                {
                    break;
                }
                else
                {

                    if (getStationSign(str, iTestSign) == false)
                    {
                        string temp = string.Format("{0}-没有测试\n", str);
                        passStation += temp;
                    }                   
                }
            }
            return passStation;
        }
         
        public int ResetStationFlag(int count,bool isHight = false)
        {//清空前面count站位信息
            int flag = 0;
            for (int i = 0; i < count; i++)
            {
                if (isHight == true)
                {
                    flag = SetBit(flag, i);
                }
                else
                {
                    flag = ClearBit(flag, i);
                }
            }
            return flag;
        }

        public int SetStaionPass(string stationStr, int iTestSign, bool isPass = true)
        {//设置站位的测试结果
            if (getIsNotPassStation(stationStr, iTestSign).Length != 0)
            {
                return iTestSign;
            }
            int iTestSignTemp = iTestSign;
            int stationSign = getStaionInt(stationStr);
            if (isPass == false)
            {
                iTestSignTemp = SetBit(iTestSign, stationSign);
            }
            else
            {
                iTestSignTemp = ClearBit(iTestSign, stationSign);
            }
            return iTestSignTemp;
        }

        public int SetStaionTestSign(int iItemSign, string stationStr,bool isTest = true)
        {//设置测试站位，已经测试过
            int stationSign = getStaionInt(stationStr);
            int iItemSignTemp = iItemSign;
            if (isTest == true)
            {
                iItemSignTemp = ClearBit(iItemSign, stationSign);
            }
            else
            {
                iItemSignTemp = SetBit(iItemSign, stationSign);
            }
            return iItemSignTemp;
        }

        public int isLastStaion(string stationStr)
        {
            int stationSign = getStaionInt(stationStr);
            if (stationSign == (stations.Count -1))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
