using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAT
{
    struct Cell_Text
    {
        public int x;
        public int y;
        public int fHeight;
        public int fWidth;
        public string fType;
        public int fSpin;
        public int fWeight;
        public bool fItalic;
        public bool fUnline;
        public bool fStrickeOut;
        public string id_name;
        public string data;

        public int DrawFun;
        //Cell_CodeBar
        public int pdirec;
        public string pCode;
        public int pHorizontal;
        public int pVertical;
        public int pbright;
        public byte ptext;

        //Cell_Bar2D
        public uint w;
        public uint v;
        public uint o;
        public uint r;
        public uint m;
        public uint g;
        public uint s;

        //Cell_PrintPCX
        public string fileName;

        public string DetailStr()
        {
            string detailStr = string.Empty;
            detailStr = string.Format("x:{0}\n",x);
            detailStr += string.Format("y:{0}\n", y);
            detailStr += string.Format("fHeight:{0}\n", fHeight);
            detailStr += string.Format("fWidth:{0}\n", fWidth);
            detailStr += string.Format("fType:{0}\n", fType);
            detailStr += string.Format("fWeight:{0}\n", fWeight);
            detailStr += string.Format("fSpin:{0}\n", fSpin);
            detailStr += string.Format("fItalic:{0}\n", fItalic);
            detailStr += string.Format("fUnline:{0}\n", fUnline);
            detailStr += string.Format("fStrickeOut:{0}\n", fStrickeOut);
            detailStr += string.Format("id_name:{0}\n", id_name);
            detailStr += string.Format("data:{0}\n", data);
            detailStr += string.Format("isBarCode:{0}\n", DrawFun);
            detailStr += string.Format("pdirec:{0}\n", pdirec);
            detailStr += string.Format("pCode:{0}\n", pCode);
            detailStr += string.Format("pHorizontal:{0}\n", pHorizontal);
            detailStr += string.Format("pVertical:{0}\n", pVertical);
            detailStr += string.Format("pbright:{0}\n", pbright);
            detailStr += string.Format("ptext:{0}\n", ptext);
            detailStr += string.Format("w:{0}\n", w);
            detailStr += string.Format("v:{0}\n", v);
            detailStr += string.Format("o:{0}\n", o);
            detailStr += string.Format("r:{0}\n", r);
            detailStr += string.Format("m:{0}\n", m);
            detailStr += string.Format("g:{0}\n", g);
            detailStr += string.Format("s:{0}\n", s);
            return detailStr;
        }
    };

    class BarcodeDesignConfig
    {
        private int m_cellCount;
        private bool m_isHaveActiveCodeTwo;
        private string m_fileName;
        public Dictionary<int, Cell_Text> m_cells;

        public string i_Activecode;
        public string i_SN;
        public string i_SN_PHONE;
        public string i_Imei;


        private string m_lastErroStr;

        public BarcodeDesignConfig(string fileName)
        {
            this.m_fileName = fileName;
            m_cells = new Dictionary<int, Cell_Text>();
        }

        public bool InitConfigData()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\";
            path += m_fileName;
            IniFile ini = new IniFile(path);

            string countStr = ini.IniReadValue("Row", "count");
            int isHaveActiveCodeTwo = 0;
            try
            {
                isHaveActiveCodeTwo = Int32.Parse(ini.IniReadValue("Row", "isHaveActiveCodeTwo"));
            }catch(System.Exception e){
                isHaveActiveCodeTwo = 0;
            }
            m_isHaveActiveCodeTwo = isHaveActiveCodeTwo==0?false:true;
            if(countStr.Length != 0)
            {
                try
                {
                    this.m_cellCount = Int32.Parse(countStr);
                }
                catch (System.Exception ex)
                {
                    this.m_lastErroStr = string.Format("Int32.Parse(count) erro:{0}",ex.Message);
                    return false;
                }
            }
            else
            {
                this.m_cellCount = 0;
            }

            for (int i = 1; i <= m_cellCount; i++)
            {
                string sectionStr = string.Format("Cell_{0}", i);
                Cell_Text cellTemp = new Cell_Text();
                try
                {
                    cellTemp.DrawFun = Int32.Parse(ini.IniReadValue(sectionStr, "DrawFun"));
                    cellTemp.x = Int32.Parse(ini.IniReadValue(sectionStr, "x"));
                    cellTemp.y = Int32.Parse(ini.IniReadValue(sectionStr, "y"));
                    if (cellTemp.DrawFun == 0)
                    {
                        cellTemp.fType = ini.IniReadValue(sectionStr, "fType");
                        cellTemp.fHeight = Int32.Parse(ini.IniReadValue(sectionStr, "fHeight"));
                        cellTemp.fWidth = Int32.Parse(ini.IniReadValue(sectionStr, "fWidth"));
                        cellTemp.fWeight = Int32.Parse(ini.IniReadValue(sectionStr, "fWeight"));
                        cellTemp.fSpin = Int32.Parse(ini.IniReadValue(sectionStr, "fSpin"));
                        cellTemp.fItalic = (ini.IniReadValue(sectionStr, "fItalic").Equals("yes") == true ? true : false);
                        cellTemp.fUnline = (ini.IniReadValue(sectionStr, "fUnline").Equals("yes") == true ? true : false);
                        cellTemp.fStrickeOut = (ini.IniReadValue(sectionStr, "fStrickeOut").Equals("yes") == true ? true : false);
                        cellTemp.id_name = ini.IniReadValue(sectionStr, "id_name");
                    }
                    else if (cellTemp.DrawFun == 1)
                    {
                        cellTemp.pdirec = Int32.Parse(ini.IniReadValue(sectionStr, "pdirec"));
                        cellTemp.pCode = (ini.IniReadValue(sectionStr, "pCode"));
                        cellTemp.pHorizontal = Int32.Parse(ini.IniReadValue(sectionStr, "pHorizontal"));
                        cellTemp.pVertical = Int32.Parse(ini.IniReadValue(sectionStr, "pVertical"));
                        cellTemp.pbright = Int32.Parse(ini.IniReadValue(sectionStr, "pbright"));
                        string tempText = (ini.IniReadValue(sectionStr, "ptext"));
                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        Byte[] bytes = encoding.GetBytes(tempText);
                        cellTemp.ptext = bytes[0];
                    }
                    else if (cellTemp.DrawFun == 2)
                    {
                        cellTemp.w = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "w"));
                        cellTemp.v = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "v"));
                        cellTemp.o = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "o"));
                        cellTemp.r = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "r"));
                        cellTemp.m = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "m"));
                        cellTemp.g = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "g"));
                        cellTemp.s = (uint)Int32.Parse(ini.IniReadValue(sectionStr, "s"));
                    }
                    else if (cellTemp.DrawFun == 3)
                    {
                        cellTemp.fileName = (ini.IniReadValue(sectionStr, "fileName"));
                    }
                    cellTemp.data = ini.IniReadValue(sectionStr, "data");
                    cellTemp.data = GetData(cellTemp.data);

                    m_cells[i] = cellTemp;
                }
                catch (System.Exception ex)
                {
                    this.m_lastErroStr = string.Format("InitConfigData erro:{0}", ex.Message);
                    return false;
                }

                System.Console.WriteLine(cellTemp.DetailStr());
            }

            return true;
        }

        private string GetData(string dataStr)
        {
            string tDataStr = dataStr;

 
            if (dataStr.Equals("i_Activecode") == true)
            {
                    tDataStr = i_Activecode;
            }
            if (dataStr.Equals("i_Activecode1") == true)
            {
                if (m_isHaveActiveCodeTwo)
                {
                    tDataStr = i_Activecode.Substring(0, i_Activecode.Length / 2);
                }
            }
            if (dataStr.Equals("i_Activecode2") == true)
            {
                if (m_isHaveActiveCodeTwo)
                {
                    tDataStr = i_Activecode.Substring(i_Activecode.Length / 2);
                }
            } 
            if (dataStr.Equals("i_SN") == true)
            {
                tDataStr = i_SN;
            }
            if (dataStr.Equals("i_SN1") == true)
            {
                tDataStr = i_SN.Substring(0, i_SN.Length / 2);
            }
            if (dataStr.Equals("i_SN2") == true)
            {
                tDataStr = i_SN.Substring(i_SN.Length / 2);
            }

            if (dataStr.Equals("i_SN_PHONE") == true)
            {
                tDataStr = i_SN_PHONE;
            }
            if (dataStr.Equals("i_SN_PHONE1") == true)
            {
                tDataStr = i_SN_PHONE.Substring(0, i_SN_PHONE.Length / 2);
            }
            if (dataStr.Equals("i_SN_PHONE2") == true)
            {
                tDataStr = i_SN_PHONE.Substring(i_SN_PHONE.Length / 2);
            }

            if (dataStr.Equals("i_IMEI") == true)
            {
                tDataStr = i_Imei;
            }
            return tDataStr;
        }

        public string getLastErro()
        {
            return this.m_lastErroStr;
        }
    }
}
