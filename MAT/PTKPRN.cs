using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace MAT
{
    class PTKPRN
    {
        [DllImport("CDFPSK.dll")]
        public extern static int OpenPort(int PortFlag);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_SetDarkness(uint id);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_SetPrintSpeed(uint px);

        [DllImport("CDFPSK.dll")]
        public extern static int ClosePort();

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_PrintLabel(uint number, uint cpnumber);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawBarcode(uint px,
                             uint py,
                             uint pdirec,
                             string pCode,
                             uint pbright,
                             uint pHorizontal,
                             uint pVertical,
                             byte ptext,
                             string pstr);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_DrawTextTrueTypeW(int x, int y, int FHeight,
                                    int FWidth, String FType,
                                    int Fspin, int FWeight,
                                    bool FItalic, bool FUnline,
                                    bool FStrikeOut,
                                    String id_name,
                                    String data);


        [DllImport("CDFPSK.dll")]
        public extern static int PTK_SetLabelHeight(uint lheight, uint gapH);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_SetLabelWidth(uint lwidth);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_ClearBuffer();

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_DrawRectangle(uint px, uint py,
                                      uint thickness, uint pEx,
                                      uint pEy);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_PcxGraphicsDel(string pid);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_PcxGraphicsDownload(string pcxname, string pcxpath);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawPcxGraphics(uint px, uint py, string gname);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawBar2D_Pdf417(uint x, uint y,
                                      uint w, uint v,
                                      uint s, uint c,
                                      uint px, uint py,
                                      uint r, uint l,
                                      uint t, uint o,
                                      string pstr);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawBar2D_QR(uint x,
                                   uint y,
                                   uint w,
                                   uint v,
                                   uint o,
                                   uint r,
                                   uint m,
                                   uint g,
                                   uint s,
                                   string pstr);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawBar2D_QREx(uint x,
                                   uint y,
                                   uint o,
                                   uint r,
                                   uint g,
                                   uint v,
                                   uint s,
                                   string id_name,
                                   string pstr);

        [DllImport("CDFPSK.dll")]
        public extern static int PTK_DrawLineOr(uint px, uint py, uint plength, uint pH);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_DrawText(uint px, uint py,
                              uint pdirec, uint pFont,
                              uint pHorizontal,
                              uint pVertical,
                              byte ptext, string pstr);


        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_PrintPCX(uint px, uint py, string filename);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_BmpGraphicsDownload(string pcxname, string pcxpath, int iDire);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_BinGraphicsDel(string pid);

        [DllImport("CDFPSK.dll", CharSet = CharSet.Ansi)]
        public extern static int PTK_RecallBinGraphics(uint x,uint y,string pid);


        public static int PrintBMP(uint px, uint py, string filename, int iDire)
        {
            int errorcode;
            string pcxPath = System.Environment.CurrentDirectory + "\\";
            pcxPath += filename;
            errorcode = PTK_PcxGraphicsDel("PCXA");
            if (errorcode != 0) return errorcode;
            errorcode = PTK_BmpGraphicsDownload("PCXA", pcxPath, iDire);
            if (errorcode != 0) return errorcode;
            errorcode = PTK_DrawPcxGraphics(px, py, "PCXA");
            if (errorcode != 0) return errorcode;
            return 0;
        }

        public static int PrintQXEx(uint x,
                                   uint y,
                                   uint o,
                                   uint r,
                                   uint g,
                                   uint v,
                                   uint s,
                                   string id_name,
                                   string pstr)
        {
            int errorcode = -1;
            errorcode = PTK_DrawBar2D_QREx(x, y, o, r, g, v, s, id_name, pstr);
            if (errorcode != 0) return errorcode;
            return errorcode;
        }

    }
}
