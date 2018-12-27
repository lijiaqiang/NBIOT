using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MAT
{
    class POSPrinter
    {
        const int OPEN_EXISTING = 3;
        string prnPort = "LPT1";
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName,
            int dwDesiredAccess, int dwShareMode, int lpSecurityAttributes,
            int dwCreationDisposition, int dwFlagsAndAttributes,
            int hTemplateFile);

        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(string barcodeText, string fontName,
            string fileName, int orient, int height, int width, int isBold, int isItalic,
            StringBuilder returnBarcodeCMD);

        public POSPrinter(string prnPort)
        {
            this.prnPort = prnPort;
        }

        public string PrintLine(string str)
        {
            try
            {
                IntPtr iHandle = CreateFile(prnPort, 0x40000000, 0, 0, OPEN_EXISTING, 0, 0);
                if (iHandle.ToInt32() == -1)
                {
                    return this.prnPort + "Port Open Failed";
                }
                else
                {
                    FileStream fs = new FileStream(iHandle, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(str);
                    sw.Close();
                    fs.Close();
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
