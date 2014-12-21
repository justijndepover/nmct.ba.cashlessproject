using nmct.project.model;
using nmct.project.web.itbedrijf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.project.web.itbedrijf.Controllers
{
    public class RegistersController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            List<Registers> list = RegistersDA.GetRegisters();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Registers r)
        {
            int validInsert = RegistersDA.InsertRegisters(r);

            if (validInsert != 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Registers r = RegistersDA.GetRegister(Convert.ToInt32(id));
                return View(r);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Registers r = RegistersDA.GetRegister(Convert.ToInt32(id));
                return View(r);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Registers r)
        {
            if (r.Device != null & r.RegisterName != null & r.PurchaseDate != null & r.ExpiresDate != null)
            {
                int validUpdate = RegistersDA.UpdateRegisters(r);
                if (validUpdate != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult RegisterToOrganisation()
        {
            return null;
        }
    }
}