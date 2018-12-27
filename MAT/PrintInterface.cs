using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MAT
{
    class PrintInterface
    {
        private string m_configFileName;
        public string m_sn;
        public string m_sn_phone;
        public string m_ActiveCode;
        public string m_imei;
        public string m_fobID1;
        public string m_fobID2;
        public BarcodeDesignConfig m_barcodeDesign;
        public BarcodeDesignConfig m_sncodeDesign;
        public BarcodeDesignConfig m_FobIDDesign;
        private string m_lastErroString;
        private print_code m_printConfig;


        public PrintInterface(string sn,string sn_phone,string activeCode,string imei = null,string fobID1 = null,string fobID2 = null)
        {
            this.m_configFileName = "BarCodeDesign.ini";
            this.m_sn = sn;
            this.m_sn_phone = sn_phone;
            this.m_ActiveCode = activeCode;
            this.m_imei = imei;
            this.m_fobID1 = fobID1;
            this.m_fobID2 = fobID2;
        }

        public bool LoadData(print_code printConfig)
        {
            m_printConfig = printConfig;
            this.m_configFileName = m_printConfig.barcodeDesignName;
            m_barcodeDesign = new BarcodeDesignConfig(this.m_configFileName);
            m_barcodeDesign.i_Activecode = m_ActiveCode;
            m_barcodeDesign.i_SN = m_sn;
            m_barcodeDesign.i_Imei = m_imei;
            m_barcodeDesign.i_SN_PHONE = m_sn_phone;
            bool ret = m_barcodeDesign.InitConfigData();
            if (ret == false)
            {
                this.m_lastErroString = string.Format("InitData m_barcodeDesign.InitConfigData() erro:{0}",
                    m_barcodeDesign.getLastErro());
                return false;
            }

            if (m_printConfig.is_print_sn == true)
            {
                m_sncodeDesign = new BarcodeDesignConfig(m_printConfig.snDesignName);
                m_sncodeDesign.i_Activecode = m_ActiveCode;
                m_sncodeDesign.i_SN = m_sn;
                m_sncodeDesign.i_Imei = m_imei;
                m_sncodeDesign.i_SN_PHONE = m_sn_phone;
                bool ret2 = m_sncodeDesign.InitConfigData();
                if (ret2 == false)
                {
                    this.m_lastErroString = string.Format("InitData m_sncodeDesign.InitConfigData() erro:{0}",
                        m_sncodeDesign.getLastErro());
                    return false;
                }
            }
            else
            {
                m_sncodeDesign = null;
            }

            if (m_printConfig.is_print_FobID == true)
            {
                string fobIDDesignName = "FobIDDesign.ini";
                m_FobIDDesign = new BarcodeDesignConfig(fobIDDesignName);
                m_FobIDDesign.i_Activecode = m_fobID1;
                m_FobIDDesign.i_SN = m_sn;
                m_FobIDDesign.i_Imei = m_fobID2;
                bool ret3 = m_FobIDDesign.InitConfigData();
                if (ret3 == false)
                {
                    this.m_lastErroString = string.Format("InitData m_sncodeDesign.InitConfigData() erro:{0}",
                        m_FobIDDesign.getLastErro());
                    return false;
                }
            }
            else
            {
                m_FobIDDesign = null;
            }
            return true;
        }

        public bool CheckPrint()
        {
            erroCode = PTKPRN.OpenPort(m_printConfig.Port);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Printer OpenPort erro,code:{0}", erroCode);
                return false;
            }
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Printer PTK_ClearBuffer erro,code:{0}", erroCode);
                return false;
            }
            return true;
        }


        protected static int erroCode = 1;
        protected bool PrintDataBarcode()
        {

            float ratio = m_printConfig.Sticker_Scale / 100.0f;
            int offsetX = m_printConfig.Sticker_OffsetX;
            int offsetY = m_printConfig.Sticker_OffsetY;

            //:初始化打印机参数
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_ClearBuffer erroCode:{0}", erroCode);
                return false;
            }

            uint height = (uint)m_printConfig.Sticker_Height;
            uint gap = (uint)m_printConfig.Sticker_Gap;
            erroCode = PTKPRN.PTK_SetLabelHeight(height, gap);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelHeight erroCode:{0}", erroCode);
                return false;
            }

            uint width = (uint)m_printConfig.Sticker_Width;
            erroCode = PTKPRN.PTK_SetLabelWidth(width);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelWidth erroCode:{0}", erroCode);
                return false;
            }

            uint darkness = (uint)m_printConfig.Darkness;
            erroCode = PTKPRN.PTK_SetDarkness(darkness);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetDarkness erroCode:{0}", erroCode);
                return false;
            }

            uint printSpeed = (uint)m_printConfig.PrintSpeed;
            erroCode = PTKPRN.PTK_SetPrintSpeed(printSpeed);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetPrintSpeed erroCode:{0}", erroCode);
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

            uint px  ;
            uint py  ;
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

            foreach (KeyValuePair<int,Cell_Text> k in m_barcodeDesign.m_cells)
            {
                if (k.Value.DrawFun == 0)
                {
                    x = offsetX + (int)((k.Value.x)*ratio);
                    y = offsetY + (int)(k.Value.y*ratio);
                    FHeight = (int)(k.Value.fHeight*ratio);
                    FWidth =  (int)(k.Value.fWidth*ratio);
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
                     px = (uint)(offsetX + k.Value.x*ratio);
                     py = (uint)(offsetY + k.Value.y*ratio);
                     pdirec = (uint)k.Value.pdirec;
                     pCode = k.Value.pCode;
                     pHorizontal = (uint)k.Value.pHorizontal;
                     pVertical = (uint)k.Value.pVertical;
                     pbright = (uint)k.Value.pbright;
                     ptext = k.Value.ptext;
                     data = k.Value.data;
                    erroCode = PTKPRN.PTK_DrawBarcode(px,py ,
                        pdirec,pCode,pbright ,
                        pHorizontal,pVertical  ,ptext,
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
                    this.m_lastErroString = string.Format("erroCode:{0}\ndata:\n{1}",
                        erroCode, k.Value.DetailStr());
                    return false;
                }
            }
            
            //:打印标签
            erroCode = PTKPRN.PTK_PrintLabel(1, 1);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_PrintLabel erroCode:{0}", erroCode);
                return false;
            }
            return true;
        }

        protected bool PrintDataSN()
        {
            float ratio = m_printConfig.Sticker_Scale / 100.0f;
            int offsetX = m_printConfig.Sticker_OffsetX;
            int offsetY = m_printConfig.Sticker_OffsetY;

            //:初始化打印机参数
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_ClearBuffer erroCode:{0}", erroCode);
                return false;
            }

            uint height = (uint)m_printConfig.Sticker_Height;
            uint gap = (uint)m_printConfig.Sticker_Gap;
            erroCode = PTKPRN.PTK_SetLabelHeight(height, gap);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelHeight erroCode:{0}", erroCode);
                return false;
            }

            uint width = (uint)m_printConfig.Sticker_Width;
            erroCode = PTKPRN.PTK_SetLabelWidth(width);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelWidth erroCode:{0}", erroCode);
                return false;
            }

            uint darkness = (uint)m_printConfig.Darkness;
            erroCode = PTKPRN.PTK_SetDarkness(darkness);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetDarkness erroCode:{0}", erroCode);
                return false;
            }

            uint printSpeed = (uint)m_printConfig.PrintSpeed;
            erroCode = PTKPRN.PTK_SetPrintSpeed(printSpeed);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetPrintSpeed erroCode:{0}", erroCode);
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

            foreach (KeyValuePair<int, Cell_Text> k in m_sncodeDesign.m_cells)
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
                         o, r,  g,v, s,"Bin1", data);

                }
                else if (k.Value.DrawFun == 3)
                {
                    px = (uint)(offsetX + k.Value.x * ratio);
                    py = (uint)(offsetY + k.Value.y * ratio);
                    filename = k.Value.fileName;
                    string []temps = filename.Split('.');
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
                    this.m_lastErroString = string.Format("erroCode:{0}\ndata:\n{1}",
                        erroCode, k.Value.DetailStr());
                    return false;
                }
            }

            //:打印标签
            erroCode = PTKPRN.PTK_PrintLabel(1, 1);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_PrintLabel erroCode:{0}", erroCode);
                return false;
            }
            return true;
        }

        protected bool PrintFobID()
        {
            //TODO:打印挂件id标签
            float ratio = m_printConfig.Sticker_Scale / 100.0f;
            int offsetX = m_printConfig.Sticker_OffsetX;
            int offsetY = m_printConfig.Sticker_OffsetY;

            //:初始化打印机参数
            erroCode = PTKPRN.PTK_ClearBuffer();
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_ClearBuffer erroCode:{0}", erroCode);
                return false;
            }

            uint height = (uint)m_printConfig.Sticker_Height;
            uint gap = (uint)m_printConfig.Sticker_Gap;
            erroCode = PTKPRN.PTK_SetLabelHeight(height, gap);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelHeight erroCode:{0}", erroCode);
                return false;
            }

            uint width = (uint)m_printConfig.Sticker_Width;
            erroCode = PTKPRN.PTK_SetLabelWidth(width);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetLabelWidth erroCode:{0}", erroCode);
                return false;
            }

            uint darkness = (uint)m_printConfig.Darkness;
            erroCode = PTKPRN.PTK_SetDarkness(darkness);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetDarkness erroCode:{0}", erroCode);
                return false;
            }

            uint printSpeed = (uint)m_printConfig.PrintSpeed;
            erroCode = PTKPRN.PTK_SetPrintSpeed(printSpeed);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_SetPrintSpeed erroCode:{0}", erroCode);
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

            foreach (KeyValuePair<int, Cell_Text> k in m_FobIDDesign.m_cells)
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
                    this.m_lastErroString = string.Format("erroCode:{0}\ndata:\n{1}",
                        erroCode, k.Value.DetailStr());
                    return false;
                }
            }

            //:打印标签
            erroCode = PTKPRN.PTK_PrintLabel(1, 1);
            if (erroCode != 0)
            {
                this.m_lastErroString = string.Format("Print  PTK_PrintLabel erroCode:{0}", erroCode);
                return false;
            }
            return true;
        }

        public bool PrintData()
        {
            bool ret = false;
            for (int i = 0; i < m_printConfig.Sticker_Print_Number;i++ )
            {
                ret = PrintDataBarcode();
                if (ret == false)
                {
                    PTKPRN.ClosePort();
                    return false;
                }
            }
            if (m_printConfig.is_print_sn == true)
            {
                ret = PrintDataSN();
                if (ret == false)
                {
                    PTKPRN.ClosePort();
                    return false;
                }
            }
            if (m_printConfig.is_print_FobID == true)
            {
                ret = PrintFobID();
                if (ret == false)
                {
                    PTKPRN.ClosePort();
                    return false;
                }
            }
            PTKPRN.ClosePort();
            return true;
        }
        public string GetLastErroString()
        {
            return this.m_lastErroString;
        }
    }
}
