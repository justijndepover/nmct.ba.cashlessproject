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
            List<RegisterPM> list = RegistersDA.GetRegisterPM();
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
            List<RegisterPM> AllRegisters = RegistersDA.GetRegisterPM();
            List<Organisations> AllOrganisations = OrganisationDA.GetOrganisations();
            ViewBag.Registers = AllRegisters;
            ViewBag.Organisations = AllOrganisations;

            return View();
        }

        [HttpPost]
        public ActionResult EditOrganisation(int kassa, int vereniging)
        {
            RegisterPM r = RegistersDA.GetRegisterPM(kassa);
            int OldOrganisation;
            if (r.Vereniging != null)
            {
                OldOrganisation = r.Vereniging.ID;
            }
            else
            {
                OldOrganisation = 0;
            }
            
            int validUpdate = RegistersDA.InsertRegisterPM(kassa, vereniging);
            int validUpdate2 = RegistersDA.UpdateOrganisationDatabase(kassa, vereniging, OldOrganisation);
            if (validUpdate > 0 & validUpdate2 > 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Search(int? id)
        {
            if (id != null)
            {
                Organisations o = OrganisationDA.GetOrganisation(Convert.ToInt32(id));
                ViewBag.Titel = "Kassas voor " + o.OrganisationName;
                List<RegisterPM> list = RegistersDA.GetRegistersPM(o.ID);
                return View(list);
            }
            else
            {
                return RedirectToAction("Index", "Organisations");
            } 
        }
    }
}