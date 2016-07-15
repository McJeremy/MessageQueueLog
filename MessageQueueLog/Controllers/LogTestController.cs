using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageQueueLog.Controllers
{
    public class LogTestController : Controller
    {
        // GET: LogTest
        public ActionResult Index()
        {
            int a = 10;
            int b = 0;
            int c = a / b; //会抛一个DividedByZero的异常
            return View();
        }
    }
}