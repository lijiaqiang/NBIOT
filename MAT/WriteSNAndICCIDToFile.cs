using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MAT
{
    class WriteSNAndICCIDToFile
    {
        private string m_fileName;

        public WriteSNAndICCIDToFile(string fileName)
        {
            m_fileName = fileName;
        }

        public bool wirteLine(string sn,string iccid)
        {
            FileStream fs = new FileStream(m_fileName, FileMode.Append,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(sn + "," + iccid + ",");
            sw.Close();
            fs.Close();
            return true;
        }

    }
}
