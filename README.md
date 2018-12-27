#文件夹说明：
- DotNET

	GPIB lib 库文件夹

- MAT  
	
	MAT代码文件夹	

- MAT.sln 

	MAT工程文件 
 
- MAT.v11.suo  

	MAT工程配置文件

- readme.md

	说明文档

#文件说明：

|文件 	 					| 说明		  				|
| :----------------------	| :----------------------	|
|BarcodeDesignConfig.cs	 	|条形码配置Mode 				|
|GPIBControl.cs				|GPIB控制对象				|
|PowerVoiceForm.Designer.cs	|							|
|CallBackMSgHandler.cs		|接收到终端串口数据响应对象		|
|IniFile.cs					|读取配置文件的对象接口		|
|PowerVoiceForm.cs			|是否测试过acc和熄火线对话框	|
|ComControl.cs				|串口通信控件对象				|
|JustinIO.cs				|串口通信接口封装对象			|
|PrintInterface.cs			|打印条码接口对象				|
|ComControlTmp.cs			|串口通信封装接口对象			|
|LEDTestForm.Designer.cs	|							|
|Program.cs					|							|
|Dbj_DBControl.cs			|数据库操作对象				|
|LEDTestForm.cs				|LED是否通过对话框			|
|TranJson.cs				|json数据解析对象				|
|Dbj_GetSNAndIMEI.cs		|记录SN和IMEI对象				|
|MainForm.Designer.cs		|							|
|TranJson_SNAndIMEI.cs		|通过json数据解析出来SN和IMEI	|
|Dbj_ListView.cs			|重写ListView控件			|
|MainForm.cs				|主窗口对象					|
|WebClient.cs				|访问http接口的对象			|
|Dbj_StationSign.cs			|站位信息记录对象				|
|POSPrinter.cs				|斑马打印机接口对象			|
|WriteSNAndICCIDToFile.cs	|							|
|DoTestControl.cs			|执行测试项目对象				|
|PTKPRN.cs					|打印机操作接口对象			|
|config.cs					|配置参数对象					|
|CDFPSK.dll					|打印机驱动库					|
|Newtonsoft.Json.dll		|json库						|
|log4net.dll				|log日志打印库				|
|log4net.xml				|lon日志配置文件				|
|BarcodeDesign.ini			|条形码配置文件				|
|FobIDDesign.ini			|挂件条形码配置文件			|
|SNDesign.ini				|SN条形码配置文件				|
|config.ini					|软件配置文件					|
|favicon.ico				|图标						|
|logo.ico					|图标						|
|logo2.ico					|图标						|
|logo3.ico					|图标						|
|logo4.ico					|图标						|
|logo5.ico					|图标						|
|logo6.ico					|图标						|
|fail2.jpg					|提示失败的图片				|
|success2.jpg				|提示成功的图片				|
|3c.bmp						|3c认证图标，打印的时候需要	|


#运行环境

- DoNet Framwork 3.0 - 运行环境
- MAT.accdb - 程序依赖的数据库
- config.ini - 程序依赖的配置文件
- BarcodeDesign.ini - 程序依赖的条码配置文件
- FobIDDesign.ini - 程序依赖的条码配置文件
- SNDesign.ini - 程序依赖的条码配置文件
- log4net.xml - log库需要的配置文件
- CDFPSK.dll - 程序依赖的库文件
- Newtonsoft.Json.dll - 程序依赖的库文件
- log4net.dll - 程序依赖的库文件
- 3c.bmp - 程序依赖的图片（打印条码时需要的资源）

#代码开发使用说明

##添加一项测试过程

首次有在终端软件的mat里面添加了对应的串口通信接口指令

1. 在config.ini 文件里面添加新增的配置信息
2. 在config.cs 文件里面添加对应配置结构体
3. 在config对象里面创建一个公有的对象（上面创建的结构体的对象实例）
4. 在config对象里面的`public void InitConfig()`函数里面添加读取配置信息的数据
5. 在DoTestControl对象里面添加新的开始测试函数eg：`public bool start_print_code()`

	1. 在这函数里面要添加执行的指令到数组m_CmdList，数组的下标在Station枚举里面添加
	2. 设置m_successSign对应枚举里面的值为true
	3. 在m_listView对应枚举的测试信息，读取到配置文件对应的detail值（为程序显示的测试项目名称）
	
6. 在DoTestControl对象里面的`public bool InitControl()`函数执行上面添加的函数eg：`private static bool WriteIMEICallback(string recv)`
7. 在里面判断接收到的值对应操作`self.m_parentListView`记得加代理引用
8. 在`RecvCallback[] recvHandlers`里添加一个新的（上面添加的函数）callback
