using System.Runtime.InteropServices;

namespace TodoApi.Common
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void LogErr(string info)
        {
            logerror.Error(info);
        }

        public static void LogTrace(string info)
        {
            loginfo.Debug("trace: " + info);

        }

        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void SetConfig()
        {
            var path = new FileInfo("log4net.config");
            log4net.Config.XmlConfigurator.Configure(path);
            logerror.Info("Enter application");
            loginfo.Info("Enter application");
        }

        /// 删除N天之前的日志
        public static void DeleteLogBeforeN(int n)
        {
            DateTime nowTime = DateTime.Now;
            string[] pcfiles = Directory.GetFiles(System.Environment.CurrentDirectory + @".\\Log\\LogInfo");


            string[] robotfiles = Directory.GetFiles(System.Environment.CurrentDirectory + @".\\IPCLog");

            foreach (string file in pcfiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                string pcdata = fileInfo.Name.Substring(0, 8);
                DateTime filedate = DateTime.ParseExact(pcdata, "yyyyMMdd", null);
                TimeSpan t = DateTime.Now - filedate;  //当前时间  减去 文件创建时间
                int day = t.Days;
                if (day > n)   //保存的时间，单位：天
                {
                    if (IsOccupy(fileInfo.FullName)) //判断文件是否被占用
                    {
                        System.IO.File.Delete(fileInfo.FullName); //删除文件
                    }
                    else
                    {
                        LogHelper.loginfo.Error("当前日志文件被占用，无法操作!");
                    }
                }
            }

            foreach (string file in robotfiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                string robotdata = fileInfo.Name.Substring(4, 10);
                DateTime filedate = DateTime.ParseExact(robotdata, "yyyy-MM-dd", null);
                TimeSpan t = DateTime.Now - filedate;  //当前时间  减去 文件创建时间
                int day = t.Days;
                if (day > n)   //保存的时间，单位：天
                {
                    if (IsOccupy(fileInfo.FullName)) //判断文件是否被占用
                    {
                        System.IO.File.Delete(fileInfo.FullName); //删除文件
                    }
                    else
                    {
                        LogHelper.loginfo.Error("文件被占用，无法操作!");
                    }
                }
            }

        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);



        private static bool IsOccupy(string file)
        {
            bool result = true; //默认状态此文件未被占用
            try
            {
                //string vFileName = @"c:\temp\temp.bmp";
                string vFileName = file;
                if (!System.IO.File.Exists(vFileName))
                {
                    //Logger.Info("文件都不存在!");
                    result = false;
                }
                IntPtr vHandle = _lopen(vFileName, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    LogHelper.loginfo.Error("文件被占用！");
                    result = false;
                }
                CloseHandle(vHandle);
            }
            catch (Exception err)
            {
                result = false;
                LogHelper.loginfo.Error(err);
            }
            return result;
        }


        public static void WriteLog(string info, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }
    }

}
