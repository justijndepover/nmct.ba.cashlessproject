using nmct.project.model;
using nmct.project.web.itbedrijf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.project.web.itbedrijf.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            List<Errorlog> list = new List<Errorlog>();
            list = ErrorDA.GetErrorLogs();
            return View(list);
        }
    }
}