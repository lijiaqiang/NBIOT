using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Threading;

namespace MAT
{
    struct base_info
    {
        public string product_version;
        public string hardware_version;
        public int hardware_num;
        public string minimal_version;
        public string maximal_version;
        public string mcu_version;
        public bool is_check_version;
        public bool is_check_station;
        public string station;
        public string all_station;
        public bool enable_log;
        public bool fix_old_version;
        public int timeout;
        public int readTimeout;
        public string product_date;
        public string detail;
        public int delay_time;
        public string comPort;
        public static string itemName = "base_info";
    };
    struct pairing
    {
        public static string detail = "配对测试";
        public static string itemName = "pairing";
        public static string itemName_sn1 = "pairing_sn1";
        public static string detail_sn1 = "测试SN1";
        public static string itemName_sn2 = "pairing_sn2";
        public static string detail_sn2 = "测试SN2";
    };
    struct access_sql_info
    {
        public bool do_test;
        public string connect_string;
        public string access_file;
        public string detail;
        public static string itemName = "access_sql_info";
    };
    struct update_phasecheck
    {
        public bool do_test;
        public string detail;
        public static string itemName = "update_phasecheck";
    };
    struct print_code
    {
        public bool do_test;
        public bool is_shouqi;
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
        public bool is_print_sn;
        public bool is_print_FobID;
        public bool is_need_getFobID;
        public string snDesignName;
        public string detail;
        public static string itemName = "PrintCode";
    };

    struct NBVersion
    {
        public bool do_test;
        public string version;
        public string detail;
        public static string itemName = "NBVersion";
    }

    struct NBFlash
    {
        public bool do_test;
        public Int32 count;
        public string detail;
        public static string itemName = "NBFlash";
    }

    struct ReportResult
    {
        public bool do_test;
        public string detail;
        public static string itemName = "ReportResult";
    }

    struct NBMagsensor
    {
        public bool do_test;
        public string[] msg_min;
        public string[] msg_max;
        public Int32 x_min;
        public Int32 y_min;
        public Int32 z_min;
        public Int32 x_max;
        public Int32 y_max;
        public Int32 z_max;
        public Int32 vector_min;
        public string detail;
        public static string itemName = "NBMagsensor";
    }

    struct NBNetwork
    {
        public bool do_test;
        public Int32 nbCsq_min;
        public Int32 timeout;
        public string detail;
        public static string itemName = "NBNetwork";
    }

    struct NBBatAdc
    {
        public bool do_test;
        public Int32 adc_min;
        public string detail;
        public static string itemName = "NBBatAdc";
    }

    struct NBRadarAdc
    {
        public bool do_test;
        public Int32 adc_min;
        public Int32 adc_max;
        public string detail;
        public static string itemName = "NBRadarAdc";
    }

    struct NBPower
    {
        public bool do_test;
        public Int32 PowerAddress;
        public string[] cct_3v6;
        public double lowRange;
        public double highRange;
        public Int32 timeout;
        public string detail;
        public static string itemName = "NBPower";
    }

    struct NBWatchDog
    {
        public bool do_test;
        public string detail;
        public static string itemName = "NBWatchDog";
    }

    struct NBIMEI
    {
        public bool do_test;
        public string NPMode;
        public int NPModeNum;
        public bool readIMEI;
        public string detail;
        public static string itemName = "NBIMEI";
    }

    struct NBServer
    {
        public string packingUrl;
        public string unPairUrl;
        public string printerUrl;
        public string bmatUrl;
        public string host;
    }

    struct NBMODE
    {
        public bool do_test;
        public int mode;
        public string detail;
        public static string itemName = "NBMODE";
    }

    struct NBSN
    {
        public bool do_test;
        public Int32 lenght;
        public string detail;
        public static string itemName = "NBSN";
    }

    struct NBPackingStatus
    {
        public bool do_test;
        public int cardMode;
        public int lastOnlineStatus;
        public int lastCommunicationDay;
        public int earliestTime;
        public int lastestTime;
        public string detail;
        public static string itemName = "NBPackingStatus";
    }

    class config
    {
        public base_info m_base_info;
        public access_sql_info m_access_sql_info;
        public update_phasecheck m_update_phasecheck;
        public print_code m_printCode;
        public NBVersion m_nbVersion;
        public NBFlash m_nbFlash;
        public NBMagsensor m_nbMagsensor;
        public NBNetwork m_nbNetwork;
        public NBBatAdc m_nbBatAdc;
        public NBPower m_nbPower;
        public NBWatchDog m_nbWatchDog;
        public NBIMEI m_nbIMEI;
        public NBMODE m_nbMode;
        public NBSN m_nbSn;
        public ReportResult m_reportResult;
        public NBRadarAdc m_nbRadarAdc;
        public NBServer m_nbServer;
        public NBPackingStatus m_nbPackingStatus;

        public config()
        {
            try
            {
                InitConfig();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        public void InitConfig()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\config.ini";
            IniFile ini = new IniFile(path);
            //base_info
            m_base_info.product_version = ini.IniReadValue("base_info", "product_version");
            m_base_info.hardware_version = ini.IniReadValue("base_info", "hardware_version");
            m_base_info.hardware_num = ini.IniReadValue("base_info", "hardware_num", 1);
            m_base_info.minimal_version = ini.IniReadValue("base_info", "minimal_version");
            m_base_info.maximal_version = ini.IniReadValue("base_info", "maximal_version");
            m_base_info.mcu_version = ini.IniReadValue("base_info", "mcu_version");
            m_base_info.is_check_version = (ini.IniReadValue("base_info", "is_check_version").Equals("no") == true ? false : true);
            m_base_info.is_check_station = (ini.IniReadValue("base_info", "is_check_station").Equals("no") == true ? false : true);
            m_base_info.station = ini.IniReadValue("base_info", "station");
            m_base_info.all_station = ini.IniReadValue("base_info", "all_station");
            m_base_info.enable_log = (ini.IniReadValue("base_info", "enable_log").Equals("yes") == true ? true : false);
            m_base_info.fix_old_version = (ini.IniReadValue("base_info", "fix_old_version").Equals("yes") == true ? true : false);
            m_base_info.timeout = ini.IniReadValue("base_info", "timeout", 30); //Int32.Parse(ini.IniReadValue("base_info", "timeout"));
            m_base_info.readTimeout = ini.IniReadValue("base_info", "readTimeout", 100);//Int32.Parse(ini.IniReadValue("base_info", "readTimeout"));
            m_base_info.product_date = ini.IniReadValue("base_info", "product_date");
            m_base_info.detail = ini.IniReadValue("base_info", "detail");
            m_base_info.comPort = ini.IniReadValue("base_info", "comPort");
            m_base_info.delay_time = ini.IniReadValue("base_info", "delay_time", 0);
            System.Console.WriteLine(m_base_info.detail);

            //access_sql_info
            m_access_sql_info.do_test = ini.IniReadValue("access_sql_info", "do_test").Equals("yes") == true ? true : false;
            m_access_sql_info.connect_string = ini.IniReadValue("access_sql_info", "connect_string");
            m_access_sql_info.access_file = ini.IniReadValue("access_sql_info", "access_file");
            m_access_sql_info.detail = ini.IniReadValue("access_sql_info", "detail");
            System.Console.WriteLine(m_access_sql_info.detail);

            //update_phasecheck
            m_update_phasecheck.do_test = ini.IniReadValue("update_phasecheck", "do_test").Equals("yes") == true ? true : false;
            m_update_phasecheck.detail = ini.IniReadValue("update_phasecheck", "detail");
            System.Console.WriteLine(m_update_phasecheck.detail);

            //m_nbVersion
            m_nbVersion.do_test = ini.IniReadValue("NBVersion", "do_test").Equals("yes") == true ? true : false;
            m_nbVersion.version = ini.IniReadValue("NBVersion", "version");
            m_nbVersion.detail = ini.IniReadValue("NBVersion", "detail");

            //m_nbflash
            m_nbFlash.do_test = ini.IniReadValue("NBFlash", "do_test").Equals("yes") == true ? true : false;
            m_nbFlash.count = ini.IniReadValue("NBFlash", "count", 1);
            m_nbFlash.detail = ini.IniReadValue("NBFlash", "detail");
            System.Console.WriteLine(m_nbFlash.detail);

            //m_reportResult
            m_reportResult.do_test = ini.IniReadValue("ReportResult", "do_test").Equals("yes") == true ? true : false;
            m_reportResult.detail = ini.IniReadValue("ReportResult", "detail");
            System.Console.WriteLine(m_reportResult.detail);

            //m_nb_magsensor
            m_nbMagsensor.do_test = ini.IniReadValue("NBMagsensor", "do_test").Equals("yes") == true ? true : false;
            m_nbMagsensor.msg_min = ini.IniReadValue("NBMagsensor", "msg_min").Split(':');
            m_nbMagsensor.msg_max = ini.IniReadValue("NBMagsensor", "msg_max").Split(':');
            m_nbMagsensor.x_min = Int32.Parse(m_nbMagsensor.msg_min[0]);
            m_nbMagsensor.y_min = Int32.Parse(m_nbMagsensor.msg_min[1]);
            m_nbMagsensor.z_min = Int32.Parse(m_nbMagsensor.msg_min[2]);
            m_nbMagsensor.x_max = Int32.Parse(m_nbMagsensor.msg_max[0]);
            m_nbMagsensor.y_max = Int32.Parse(m_nbMagsensor.msg_max[1]);
            m_nbMagsensor.z_max = Int32.Parse(m_nbMagsensor.msg_max[2]);
            m_nbMagsensor.vector_min = ini.IniReadValue("NBMagsensor", "vector_min", 1);
            m_nbMagsensor.detail = ini.IniReadValue("NBMagsensor", "detail");
            System.Console.WriteLine(m_nbMagsensor.detail);

            //m_nbNetwork
            m_nbNetwork.do_test = ini.IniReadValue("NBNetwork", "do_test").Equals("yes") == true ? true : false;
            m_nbNetwork.nbCsq_min = ini.IniReadValue("NBNetwork", "nbCsq_min", 1);
            m_nbNetwork.timeout = ini.IniReadValue("NBNetwork", "timeout", 10);
            m_nbNetwork.detail = ini.IniReadValue("NBNetwork", "detail");
            System.Console.WriteLine(m_nbNetwork.detail);

            //m_nbRadarAdc
            m_nbRadarAdc.do_test = ini.IniReadValue("NBRadarAdc", "do_test").Equals("yes") == true ? true : false;
            m_nbRadarAdc.adc_min = ini.IniReadValue("NBRadarAdc", "adc_min", 1);
            m_nbRadarAdc.adc_max = ini.IniReadValue("NBRadarAdc", "adc_max", 10000);
            m_nbRadarAdc.detail = ini.IniReadValue("NBRadarAdc", "detail");
            System.Console.WriteLine(m_nbRadarAdc.detail);

            //m_nbBatAdc
            m_nbBatAdc.do_test = ini.IniReadValue("NBBatAdc", "do_test").Equals("yes") == true ? true : false;
            m_nbBatAdc.adc_min = ini.IniReadValue("NBBatAdc", "adc_min", 1);
            m_nbBatAdc.detail = ini.IniReadValue("NBBatAdc", "detail");
            System.Console.WriteLine(m_nbBatAdc.detail);

            //m_nbPower
            m_nbPower.do_test = ini.IniReadValue("NBPower", "do_test").Equals("yes") == true ? true : false;
            m_nbPower.PowerAddress = ini.IniReadValue("NBPower", "PowerAddress", 5);
            m_nbPower.cct_3v6 = ini.IniReadValue("NBPower", "cct_3v6").Split(' ');
            if (m_nbPower.cct_3v6.Length == 4)
            {
                m_nbPower.timeout = Int32.Parse(m_nbPower.cct_3v6[1]);
                m_nbPower.lowRange = double.Parse(m_nbPower.cct_3v6[2]);
                m_nbPower.highRange = double.Parse(m_nbPower.cct_3v6[3]);
            }
            m_nbPower.detail = ini.IniReadValue("NBPower", "detail");
            System.Console.WriteLine(m_nbPower.detail);

            //m_nbWatchDog
            m_nbWatchDog.do_test = ini.IniReadValue("NBWatchDog", "do_test").Equals("yes") == true ? true : false;
            m_nbWatchDog.detail = ini.IniReadValue("NBWatchDog", "detail");
            System.Console.WriteLine(m_nbPower.detail);

            //m_nbIMEI
            m_nbIMEI.do_test = ini.IniReadValue("NBIMEI", "do_test").Equals("yes") == true ? true : false;
            m_nbIMEI.NPMode = ini.IniReadValue("NBIMEI", "NPMode");
            m_nbIMEI.NPModeNum = ini.IniReadValue("NBIMEI", "NPModeNum", 1);
            m_nbIMEI.readIMEI = ini.IniReadValue("NBIMEI", "readIMEI").EndsWith("yes") == true ? true : false;
            m_nbIMEI.detail = ini.IniReadValue("NBIMEI", "detail");
            System.Console.WriteLine(m_nbIMEI.detail);

            //m_nbServer
            m_nbServer.bmatUrl = ini.IniReadValue("NBServer", "bmatUrl");
            m_nbServer.printerUrl = ini.IniReadValue("NBServer", "printerUrl");
            m_nbServer.unPairUrl = ini.IniReadValue("NBServer", "unPairUrl");
            m_nbServer.packingUrl = ini.IniReadValue("NBServer", "packingUrl");
            m_nbServer.host = ini.IniReadValue("NBServer", "host");

            //m_nbMode
            m_nbMode.do_test = ini.IniReadValue("NBMODE", "do_test").Equals("yes") == true ? true : false;
            m_nbMode.mode = ini.IniReadValue("NBMODE", "mode", 1);
            m_nbMode.detail = ini.IniReadValue("NBMODE", "detail");
            System.Console.WriteLine(m_nbMode.detail);

            //m_nbSn
            m_nbSn.do_test = ini.IniReadValue("NBSN", "do_test").Equals("yes") == true ? true : false;
            m_nbSn.lenght = ini.IniReadValue("NBSN", "lenght", 36);
            m_nbSn.detail = ini.IniReadValue("NBSN", "detail");
            System.Console.WriteLine(m_nbSn.detail);

            //m_nbCardMode
            m_nbPackingStatus.do_test = ini.IniReadValue("NBPackingStatus", "do_test").Equals("yes") == true ? true : false;
            m_nbPackingStatus.cardMode = ini.IniReadValue("NBPackingStatus", "cardMode", 1);
            m_nbPackingStatus.lastOnlineStatus = ini.IniReadValue("NBPackingStatus", "lastOnlineStatus", 0);
            m_nbPackingStatus.lastCommunicationDay = ini.IniReadValue("NBPackingStatus", "lastCommunicationDay", 26);
            string[] lastCommunicationTime = ini.IniReadValue("NBPackingStatus", "lastCommunicationHour").Split(':');
            if (lastCommunicationTime.Length == 2)
            {
                m_nbPackingStatus.earliestTime = Int32.Parse(lastCommunicationTime[0]);
                m_nbPackingStatus.lastestTime = Int32.Parse(lastCommunicationTime[1]);
            }
            m_nbPackingStatus.detail = ini.IniReadValue("NBPackingStatus", "detail");
            System.Console.WriteLine(m_nbPackingStatus.detail);

            //print_code
            m_printCode.do_test = ini.IniReadValue("print_code", "do_test").Equals("yes") == true ? true : false;
            m_printCode.Darkness = ini.IniReadValue("print_code", "Darkness(0-20)", 20);//Int32.Parse(ini.IniReadValue("print_code", "Darkness(0-20)"));
            m_printCode.PrintSpeed = ini.IniReadValue("print_code", "PrintSpeed(0-6)", 1);//Int32.Parse(ini.IniReadValue("print_code", "PrintSpeed(0-6)"));
            m_printCode.Port = ini.IniReadValue("print_code", "Port", 255);//Int32.Parse(ini.IniReadValue("print_code", "Port"));
            m_printCode.UPC_Code = ini.IniReadValue("print_code", "UPC_Code");
            m_printCode.Activation_Bar_Code_Style = ini.IniReadValue("print_code", "Activation_Bar_Code_Style", 1);// Int32.Parse(ini.IniReadValue("print_code", "Activation_Bar_Code_Style"));
            m_printCode.Sticker_Print_Number = ini.IniReadValue("print_code", "Sticker_Print_Number", 2);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Print_Number"));
            m_printCode.SN_Prefix_Number = ini.IniReadValue("print_code", "SN_Prefix_Number", 0);//Int32.Parse(ini.IniReadValue("print_code", "SN_Prefix_Number"));
            m_printCode.Sticker_Height = ini.IniReadValue("print_code", "Sticker_Height", 353);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Height"));
            m_printCode.Sticker_Width = ini.IniReadValue("print_code", "Sticker_Width", 588);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Width"));
            m_printCode.Sticker_Gap = ini.IniReadValue("print_code", "Sticker_Gap", 23);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Gap"));
            m_printCode.Sticker_Scale = ini.IniReadValue("print_code", "Sticker_Scale", 100);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_Scale"));
            m_printCode.Sticker_OffsetX = ini.IniReadValue("print_code", "Sticker_OffsetX", 10);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_OffsetX"));
            m_printCode.Sticker_OffsetY = ini.IniReadValue("print_code", "Sticker_OffsetY", 10);//Int32.Parse(ini.IniReadValue("print_code", "Sticker_OffsetY"));
            m_printCode.barcodeDesignName = ini.IniReadValue("print_code", "barcodeDesignName");
            m_printCode.is_print_sn = ini.IniReadValue("print_code", "is_print_sn").Equals("yes") == true ? true : false;
            m_printCode.is_print_FobID = ini.IniReadValue("print_code", "is_print_FobID").Equals("yes") == true ? true : false;
            m_printCode.is_need_getFobID = ini.IniReadValue("print_code", "is_need_getFobID").Equals("yes") == true ? true : false;
            m_printCode.snDesignName = ini.IniReadValue("print_code", "snDesignName");
            m_printCode.detail = ini.IniReadValue("print_code", "detail");
            System.Console.WriteLine(m_printCode.detail);
        }
    }
}
