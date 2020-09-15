using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ScheduledService
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer(86400000.0);
            timer.Elapsed += new ElapsedEventHandler(Program.timer1);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            backup();
            Console.ReadKey();
        }
        private static void timer1(object source, ElapsedEventArgs e)
        {
            backup();
        }
        private static void backup()
        {
            //调用mysqldump备份mysql数据库的语句
            var name = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string backupsql = $"\"C:\\Program Files\\MySQL\\MySQL Server 8.0\\bin\\mysqldump\" -hlocalhost -P3306 -uroot -pabc@1234 --default-character-set=utf8 tradeforce > D:\\backup\\tradeforce{name}.sql";
            RunCMDCommand(backupsql);
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:sss") + ":Backup complete");
        }
        public static string RunCMDCommand(string cmd)
        {
            string cmdPath = "C:\\Windows\\System32\\cmd.exe";   //cmd.exe执行文件目录
            cmd = cmd.Trim().TrimEnd('&') + "&exit";  //不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态

            string result = string.Empty;
            Process process = new Process();
            try
            {
                //设置要启动的执行程序
                process.StartInfo.FileName = cmdPath;

                //是否使用操作系统shell启动进程
                process.StartInfo.UseShellExecute = false;
                //应用程序的输入是否从Process.StandardInput流中读取/是否接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardInput = true;

                //是否将应用程序的输出写入Process.StandardOutput流中/是否调用程序获取输出信息
                //置为false时StandardOutput.ReadToEnd获取异常
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                //向cmd窗口写入命令
                process.StandardInput.WriteLine(cmd);
                process.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();//等待程序执行完退出进程
                process.Close();
            }
            catch (Exception ex)
            {
                //记录错误日志信息
                //log4net
                result = string.Empty;
            }
            finally
            {
                //释放
                process.Dispose();
            }
            return result;
        }
    }
}
