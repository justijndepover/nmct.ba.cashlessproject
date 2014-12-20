using nmct.project.api.Models;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.project.api.Controllers
{
    public class OrganisationController : ApiController
    {
        public List<Organisations> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return AdminDA.GetOrganisations(p.Claims);
        }

        public String OrganisationName(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return AdminDA.GetOrganisation(id, p.Claims).OrganisationName;
        }
    }
}
