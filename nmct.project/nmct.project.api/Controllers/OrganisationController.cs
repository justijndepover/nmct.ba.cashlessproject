﻿using nmct.project.api.Models;
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
        [HttpGet]
        public List<Organisations> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return AdminDA.GetOrganisations(p.Claims);
        }

        [HttpGet]
        public Organisations Get(string username)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return AdminDA.GetOrganisation(username, p.Claims);
        }

        public HttpResponseMessage Post(Organisations o)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            OrganisationsDA.ChangePassword(o.Password, o.Login);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
