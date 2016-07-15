using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace MessageQueueLog
{
    public class MessageQueueLogConfig
    {
        public static void RegisterExceptionLogWithRedis()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        if (MyExceptionAttribute.redisClient.GetListCount("ExceptionLog") > 0)
                        {
                            string errorMsg = MyExceptionAttribute.redisClient.DequeueItemFromList("ExceptionLog");
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                ILog log = log4net.LogManager.GetLogger("RollingLogFileAppender");
                                log.Error(errorMsg);
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception e)
                    {
                        MyExceptionAttribute.redisClient.EnqueueItemOnList("ExceptionLog", e.ToString());
                    }
                }
            });
        }
        public static void RegisterExceptionLog()
        {
            string logFilePath = HttpContext.Current.Server.MapPath("/App_Data/");

            //开启单独的线程处理
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {

                    try
                    {
                        if (MyExceptionAttribute.q.Count > 0)
                        {
                            Exception ex = MyExceptionAttribute.q.Dequeue();
                            if (null != ex)
                            {
                                //构建完整的日志文件名
                                string logFileName = logFilePath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                                //获得异常堆栈信息
                                string exceptionMsg = ex.ToString();

                                File.AppendAllText(logFileName, exceptionMsg, Encoding.UTF8);
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception e)
                    {
                        MyExceptionAttribute.q.Enqueue(e);
                    }
                }
            }, logFilePath);
        }
    }
}