using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAT
{
    class PrintImei
    {
        public struct Cell_Text
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
                detailStr = string.Format("x:{0}\n", x);
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
        struct print_code
        {
            public bool do_test;
            public int Darkness;
            public int PrintSpeed;
            public int Port;
            public string UPC_Code;
            public int Activation_Bar_Code_Style;
            public int Sticker_Print_Number;
            public int SN_Prefix_Number;
            public int Sticker_Height;
            public int Sticker_Width;
            public int Sticker_Gap;
            public int Sticker_Scale;
            public int Sticker_OffsetX;
            public int Sticker_OffsetY;
            public string barcodeDesignName;
            public string snDesignName;
            public string detail;
            public static string itemName = "PrintCode";
        };
        private Int32 cellCount = 0;
        public string lastError = string.Empty;
        static string imei = string.Empty;
        static string devImei = string.Empty;
        static string userData = string.Empty;
        public Dictionary<int, Cell_Text> m_cells;
        public int erroCode = 0;
        private print_code m_printCode;
        public PrintImei()
        {
            m_cells = new Dictionary<int, Cell_Text>();
        }
        public bool InitConfigData()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\BarcodeDesign.ini";
            IniFile ini = new IniFile(path);
            //标签需要打印的项目有几个
            string countStr = ini.IniReadValue("Row", "count");
            System.Console.WriteLine(string.Format("path:{0}, count:{1}", path, countStr));
            if (countStr.Length != 0)
            {
                if (Int32.TryParse(countStr, out cellCount) == true)
                {
                    for (int i = 1; i <= cellCount; i++)
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
                            //displayLog(string.Format("m_cells[{0}] = {1}", i,m_cells[i].DetailStr()));
                            string log = m_cells[i].DetailStr();
                        }
                        catch (System.Exception ex)
                        {
                            this.lastError = string.Format("InitConfigData erro:{0}", ex.Message);
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;

        }
        private string GetData(string dataStr)
        {
            string tDataStr = dataStr;

            if (dataStr.Equals("title") == true)
            {
                tDataStr = "和易停NB-IoT车检器";
            }
            else if (dataStr.Equals("line") == true)
            {
                tDataStr = "____________";
            }
            else if (dataStr.Equals("mode") == true)
            {
                tDataStr = "型号:";
            }
            else if (dataStr.Equals("devImei") == true)
            {
                tDataStr = "设备IMEI:";
            }
            else if (dataStr.Equals("i_devImei") == true)
            {
                tDataStr = devImei;
            }
            else if (dataStr.Equals("imei") == true)
            {
                tDataStr = "模组IMEI:";
            }
            else if (dataStr.Equals("m_imei") == true)
            {
                tDataStr = imei;
            }
            else if (dataStr.Equals("i_imei") == true)
            {
                tDataStr = imei;
            }
            else if (dataStr.Equals("DBJ") == true)
            {
                tDataStr = "中移物联网有限公司";
            }
            else if (dataStr.Equals("NP100") == true)
            {
                tDataStr = userData;
            }
            return tDataStr;
        }

        public string getLastErro()
        {
            return this.lastError;
        }
        public bool LoadData()
        {
            bool ret = InitConfigData();
            if (ret == false)
            {
                this.lastError = string.Format("InitData m_barcodeDesign.InitConfigData() erro");
                return false;
            }
            return true;
        }
        public bool checkPrint()
        {
            erroCode = PTKPRN.OpenPort(m_printCode.Port);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Printer OpenPort erro,code:{0}", erroCode);
                return false;
            }
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.lastError = string.Format("Printer OpenPort erro,code:{0}", erroCode);
                return false;
            }
            return true;
        }
        public void initPrintCode()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\config.ini";
            IniFile init = new IniFile(path);
            //print_code
            m_printCode.do_test = init.IniReadValue("print_code", "do_test").Equals("yes") == true ? true : false;
            m_printCode.Darkness = init.IniReadValue("print_code", "Darkness(0-20)", 20);//Int32.Parse(ini.IniReadValue("print_code", "Darkness(0-20)"));
            m_printCode.PrintSpeed = init.IniReadValue("print_code", "PrintSpeed(0-6)", 1);//Int32.Parse(ini.IniReadValue("print_code", "PrintSpeed(0-6)"));
            m_printCode.Port = init.IniReadValue("print_code", "Port", 255);//Int32.Parse(ini.IniReadValue("print_code", "Port"));
            m_printCode.UPC_Code = init.IniReadValue("print_code", "UPC_Code");
            m_printCode.Activation_Bar_Code_Style = init.IniReadValue("print_code", "Activation_Bar_Code_Style", 1);// Int32.Parse(ini.IniReadValue("print_code", "Activation_Bar_Code_Style"));
            m_printCode.Sticker_Print_Number = init.IniReadValue("print_code", "Sticker_Print_Number", 2);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Print_Number"));
            m_printCode.SN_Prefix_Number = init.IniReadValue("print_code", "SN_Prefix_Number", 0);//Int32.Parse(ini.IniReadValue("print_code", "SN_Prefix_Number"));
            m_printCode.Sticker_Height = init.IniReadValue("print_code", "Sticker_Height", 353);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Height"));
            m_printCode.Sticker_Width = init.IniReadValue("print_code", "Sticker_Width", 588);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Width"));
            m_printCode.Sticker_Gap = init.IniReadValue("print_code", "Sticker_Gap", 23);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Gap"));
            m_printCode.Sticker_Scale = init.IniReadValue("print_code", "Sticker_Scale", 100);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Scale"));
            m_printCode.Sticker_OffsetX = init.IniReadValue("print_code", "Sticker_OffsetX", 10);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_OffsetX"));
            m_printCode.Sticker_OffsetY = init.IniReadValue("print_code", "Sticker_OffsetY", 10);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_OffsetY"));
            m_printCode.barcodeDesignName = init.IniReadValue("print_code", "barcodeDesignName");
            m_printCode.snDesignName = init.IniReadValue("print_code", "snDesignName");
            m_printCode.detail = init.IniReadValue("print_code", "detail");
            System.Console.WriteLine(m_printCode.detail);
        }
        protected static int errCode = 1;
        protected bool PrintDataSerial()
        {
            float ratio = m_printCode.Sticker_Scale / 100.0f;
            int offsetX = m_printCode.Sticker_OffsetX;
            int offsetY = m_printCode.Sticker_OffsetY;

            //:初始化打印机参数
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_ClearBuffer erroCode:{0}", erroCode);
                return false;
            }

            uint height = (uint)m_printCode.Sticker_Height;
            uint gap = (uint)m_printCode.Sticker_Gap;
            erroCode = PTKPRN.PTK_SetLabelHeight(height, gap);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_SetLabelHeight erroCode:{0}", erroCode);
                return false;
            }

            uint width = (uint)m_printCode.Sticker_Width;
            erroCode = PTKPRN.PTK_SetLabelWidth(width);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_SetLabelWidth erroCode:{0}", erroCode);
                return false;
            }

            uint darkness = (uint)m_printCode.Darkness;
            erroCode = PTKPRN.PTK_SetDarkness(darkness);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_SetDarkness erroCode:{0}", erroCode);
                return false;
            }

            uint printSpeed = (uint)m_printCode.PrintSpeed;
            erroCode = PTKPRN.PTK_SetPrintSpeed(printSpeed);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_SetPrintSpeed erroCode:{0}", erroCode);
                return false;
            }

            //绘制标签内容

            int x;
            int y;
            int FHeight;
            int FWidth;
            String FType;
            int Fspin;
            int FWeight;
            bool FItalic;
            bool FUnline;
            bool FStrikeOut;
            String id_name;
            String data;

            uint px;
            uint py;
            uint pdirec;
            string pCode;
            uint pHorizontal;
            uint pVertical;
            uint pbright;
            byte ptext;

            uint w;
            uint v;
            uint o;
            uint r;
            uint m;
            uint g;
            uint s;

            String filename;

            foreach (KeyValuePair<int, Cell_Text> k in m_cells)
            {
                if (k.Value.DrawFun == 0)
                {
                    x = offsetX + (int)((k.Value.x) * ratio);
                    y = offsetY + (int)(k.Value.y * ratio);
                    FHeight = (int)(k.Value.fHeight * ratio);
                    FWidth = (int)(k.Value.fWidth * ratio);
                    FType = k.Value.fType;
                    Fspin = k.Value.fSpin;
                    FWeight = k.Value.fWeight;
                    FItalic = k.Value.fItalic;
                    FUnline = k.Value.fUnline;
                    FStrikeOut = k.Value.fStrickeOut;
                    id_name = k.Value.id_name;
                    data = k.Value.data;
                    erroCode = PTKPRN.PTK_DrawTextTrueTypeW(x, y,
                        FHeight, FWidth, FType, Fspin, FWeight, FItalic, FUnline,
                        FStrikeOut, id_name, data);
                }
                else if (k.Value.DrawFun == 1)
                {
                    px = (uint)(offsetX + k.Value.x * ratio);
                    py = (uint)(offsetY + k.Value.y * ratio);
                    pdirec = (uint)k.Value.pdirec;
                    pCode = k.Value.pCode;
                    pHorizontal = (uint)k.Value.pHorizontal;
                    pVertical = (uint)k.Value.pVertical;
                    pbright = (uint)k.Value.pbright;
                    ptext = k.Value.ptext;
                    data = k.Value.data;
                    erroCode = PTKPRN.PTK_DrawBarcode(px, py,
                        pdirec, pCode, pbright,
                        pHorizontal, pVertical, ptext,
                        data);
                }
                else if (k.Value.DrawFun == 2)
                {
                    px = (uint)(offsetX + k.Value.x * ratio);
                    py = (uint)(offsetY + k.Value.y * ratio);
                    w = k.Value.w;
                    v = k.Value.v;
                    o = k.Value.o;
                    r = k.Value.r;
                    m = k.Value.m;
                    g = k.Value.g;
                    s = k.Value.s;
                    data = k.Value.data;

                    //                     erroCode = PTKPRN.PTK_DrawBar2D_QR(px, py,
                    //                         w, v, o, r, m, g, s, data);
                    erroCode = PTKPRN.PrintQXEx(px, py,
                            o, r, g, v, s, "Bin1", data);

                }
                else if (k.Value.DrawFun == 3)
                {
                    px = (uint)(offsetX + k.Value.x * ratio);
                    py = (uint)(offsetY + k.Value.y * ratio);
                    filename = k.Value.fileName;
                    string[] temps = filename.Split('.');
                    if (temps[1].IndexOf("bmp") != -1)
                    {
                        PTKPRN.PrintBMP(px, py, filename, 0);
                    }
                    else
                    {
                        erroCode = PTKPRN.PTK_PrintPCX(px, py, filename);
                    }
                }

                if (erroCode != 0)
                {
                    this.lastError = string.Format("erroCode:{0}\ndata:\n{1}",
                        erroCode, k.Value.DetailStr());
                    return false;
                }
            }

            //:打印标签
            erroCode = PTKPRN.PTK_PrintLabel(1, 1);
            if (erroCode != 0)
            {
                this.lastError = string.Format("Print  PTK_PrintLabel erroCode:{0}", erroCode);
                return false;
            }
            return true;
        }
        public bool PrintData(string m_imei, string m_devImei, string m_mode)
        {
            bool ret = false;
            imei = m_imei;
            userData = m_mode;
            devImei = m_devImei;
            System.Console.WriteLine(string.Format("print serial>>>{0}", imei));
            ret = LoadData();
            initPrintCode();
            InitConfigData();
            if (ret == true)
            {
                ret = checkPrint();
                if (ret == false)
                {
                    return ret;
                }
                ret = PrintDataSerial();
                if (ret == false)
                {
                    //:打印出错
                    return ret;
                }
            }
            else
            {
                //导出数据出错
                return ret;
            }
            if (ret == false)
            {
                PTKPRN.ClosePort();
                return false;
            }
            PTKPRN.ClosePort();
            return true;
        }
    }
}


