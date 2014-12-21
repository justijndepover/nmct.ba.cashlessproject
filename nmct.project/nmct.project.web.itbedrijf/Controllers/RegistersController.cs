using nmct.project.model;
using nmct.project.web.itbedrijf.Models;
using nmct.project.web.itbedrijf.Presentation_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.project.web.itbedrijf.Controllers
{
    public class RegistersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            List<RegisterPM> list = RegistersDA.GetAllRegisters();
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

        [HttpGet]
        public ActionResult EditOrganisation()
        {
            List<RegisterPM> AllRegisters = RegistersDA.GetAllRegisters();
            List<Organisations> AllOrganisations = OrganisationDA.GetOrganisations();
            ViewBag.Registers = AllRegisters;
            ViewBag.Organisations = AllOrganisations;

            return View();
        }

        [HttpPost]
        public ActionResult EditOrganisation(int? s, int? s2)
        {
            return View();
        }
    }
}