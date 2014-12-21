using nmct.project.model;
using nmct.project.web.itbedrijf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.project.web.itbedrijf.Controllers
{
    public class OrganisationsController : Controller
    {
        public ActionResult Index()
        {
            List<Organisations> list = OrganisationDA.GetOrganisations();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Organisations o)
        {
            int validInsert = OrganisationDA.InsertOrganisations(o);
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
                Organisations o = OrganisationDA.GetOrganisation(Convert.ToInt32(id));
                return View(o);
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
                Organisations o = OrganisationDA.GetOrganisation(Convert.ToInt32(id));
                return View(o);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Organisations o)
        {
            if (o.Login != null & o.Password != null & o.DbName != null & o.DbLogin != null & o.DbPassword != null & o.OrganisationName != null & o.Address != null & o.Email != null)
            {
                int validUpdate = OrganisationDA.UpdateOrganisations(o);
                if (validUpdate != 0)
                {
                    return View("Details", o);
                }
            }
            return View();
        }
    }
}