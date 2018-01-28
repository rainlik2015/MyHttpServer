using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyHttpServer;
using MyHttpServer.ActionResults;
using MyHttpServer.Mvc;
using MyWebSite.Models;
using System.Net;

namespace MyWebSite.Controllers
{
    public class HomeController : Controller
    {
        public HttpStatusCodeResult Code()
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound,"CodePage");
        }
        public ActionResult Index()
        {
            //ViewPage p = new Index();
            //return View(p);
            return View("Index");
        }
        public ActionResult GetUser(int id)
        {
            return Content(id.ToString());
        }
        public ActionResult GetUser(User u)
        {
            return Content(u.UserName);
        }
    }
}
