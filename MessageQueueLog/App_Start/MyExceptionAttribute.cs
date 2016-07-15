using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageQueueLog
{
    
    public class MyExceptionAttribute:System.Web.Mvc.HandleErrorAttribute
    {
        //版本1：使用内置队列
        public static Queue<Exception> q = new Queue<Exception>();

        //版本2：使用redis做队列
        public static IRedisClientsManager redisClientManager = new PooledRedisClientManager(new string[] {
            "192.168.0.70:6379"
        });

        public static IRedisClient redisClient = redisClientManager.GetClient();

        public override void OnException(ExceptionContext filterContext)
        {
            //版本一
            q.Enqueue(filterContext.Exception);

            //版本二
            
            redisClient.EnqueueItemOnList("ExceptionLog", filterContext.Exception.ToString());

            filterContext.HttpContext.Response.Redirect("~/Common/CustomError");
            base.OnException(filterContext);
        }
    }
}