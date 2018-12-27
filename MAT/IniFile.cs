using System;
using System.Collections.Generic;
// using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MAT
{
    class IniFile
    {
        public string path;				//INI文件名

        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,string key,
					string val,string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key,string def,
					StringBuilder retVal,int size,string filePath);

        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string segName, string keyName, string sDefault, byte[] buffer, int iLen, string fileName); // ANSI版本

        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSection(string segName, StringBuilder buffer, int nSize, string fileName);

        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileSection(string segName, string sValue, string fileName);

        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);

        public IniFile(string INIPath)
        {//类的构造函数，传递INI文件名

            path = INIPath;
        }

        public void IniWriteValue(string Section,string Key,string Value)
        {//写INI文件
            WritePrivateProfileString(Section,Key,Value,this.path);
        }
        

        public string IniReadValue(string Section,string Key)
        {//读取INI文件指定
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section,Key,"",temp,500,this.path);
            return temp.ToString();
        }

        public float IniReadValue(string Section, string Key, float defaultValue)
        {
            float value = defaultValue;
            try
            {
                value = float.Parse(IniReadValue(Section, Key));
            }
            catch (System.Exception ex)
            {
            	
            }
            return value;
        }

        public int IniReadValue(string Section, string Key, int defaultValue)
        {
            int value = defaultValue;
            try
            {
                value = Int32.Parse(IniReadValue(Section, Key));
            }
            catch (System.Exception ex)
            {

            }
            return value;
        }
    }
}
