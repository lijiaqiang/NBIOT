using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
//using NationalInstruments.NI4882;
using NationalInstruments.NI4882;
using System.Windows.Forms;
using log4net;


namespace MAT
{
    public class GPIBControl
    {
        protected Device device = null;
        public int protectedVoltage = 6;    //m_nChannel=1,protectedVoltage=6,m_nCurrentLimit=2000,m_strOutpuType="LOW"
        private static readonly ILog LOG = LogManager.GetLogger(typeof(MainForm));

        public void Open(int boarID,byte address,byte secondAddr)
        {
            try
            {
                device = new Device(boarID, address, secondAddr);
            }
            catch (System.Exception e)
            {
                throw new Exception("无法打开Agilent 6631X! " + e.Message);
            } 
        }

        public void Close()
        {
            if(null != device)
            {
                ResetDC();
                device.Dispose();
                device = null;
            }
        }

        protected void Write(string str)
        {
            try
            {
                device.Write(ReplaceCommonEscapeSequences(str));
            }
            catch (System.Exception e)
            {
                throw new Exception("Fail to Write GPIB Command " + e.Message);
            }
        }

        protected string Read()
        {
            string ret = "";
            try
            {
                //ret = InsertCommonEscapeSequences(device.ReadString());
                ret = device.ReadString();
            }
            catch (System.Exception e)
            {
                throw new Exception("Fail to Read GPIB " + e.Message);
            }
            return ret;
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        public void SetVoltage(double voltage)
        {
            String protectCmd = String.Format("VOLT:PROT {0};:VOLT:PROT:STAT ON",protectedVoltage);
            String voltageCmd = String.Format("OUTP1 ON;:VOLT1 {0}", voltage);  //OUTP1 maybe OUTP2
            Write(protectCmd);
            Write(voltageCmd);
        }

        public double GetCurrent()
        {
            double retCurrent = 0;
            string recv = null;
            //Write("MEAS:CURR? FETC:CURR:DC?");
            Write("MEAS:CURR?");
            recv = Read();
            LOG.Info(recv);
            if (-1 != recv.IndexOf("9.91000E+37"))
            {
                Write("SENS:CURR:RANG MAX");
                Write("MEAS:CURR?");
                recv = Read();
                recv.TrimEnd('\n');
                LOG.InfoFormat("too big,{0}",recv);
            }
            else
            {
                Write("SENS:CURR:RANG MAX");
            }
            retCurrent = Convert.ToDouble(recv);
            retCurrent *= 1000;
            return retCurrent;
        }

        public double GetAverageCurrent(int span)
        {
            int lastTick = System.DateTime.Now.Millisecond;
            DateTime lastTime = System.DateTime.Now;
            TimeSpan spanTime = lastTime - lastTime;
            int now = lastTick;
            int count = 0;
            double total = 0;
            double curCurrent = 0;

            while(spanTime.TotalMilliseconds <= span)
            {
                curCurrent = GetCurrent();
                total += curCurrent;
                count++;
                spanTime = System.DateTime.Now - lastTime;
            }
            if (0 < count)
                return total / count;
            return 0;
        }

        public void SetCurrentRange(double range)
        {
            range = (double)(range > 2.0 ? 2.0 : range);
            range = (double)(range < 0.2 ? 0.2 : range);
            string setRangeCmd = String.Format("SENS:CURR:RANG {0:F2}", range);
            Write(setRangeCmd);
        }

        public void InitDCSource()
        {
            Write("*RST;*CLS");
            Write("DISP:MODE NORM;:DISP ON;:DISP:CHAN 1");
            Write("SENS:CURR:DET ACDC");
            Write("SENS:CURR:RANG 0.5;:SENS:FUNC \"CURR\"");
            Write("OUTP:STAT ON");
            Write("OUTP:TYPE LOW");
            Write("VOLT:PROT 6;:VOLT:PROT:STAT ON");
            Write("CURR 2");
            Write("INST:COUP:OUTP:STAT NONE");
        }

        public void InitDCSourceExtendNB()
        {
            Write("*RST;*CLS");
            Write("DISP:MODE NORM;:DISP ON;:DISP:CHAN 1");
            Write("SENS:CURR:DET ACDC");
            Write("SENS:CURR:RANG 0.02;:SENS:FUNC \"CURR\"");
            Write("OUTP:STAT ON");
            Write("OUTP:TYPE LOW");
            Write("VOLT:PROT 6;:VOLT:PROT:STAT ON");
            Write("CURR 2");
            Write("INST:COUP:OUTP:STAT NONE");
        }

        public void ResetDC()
        {
            Write("*RST;*CLS");
        }
    }
}
