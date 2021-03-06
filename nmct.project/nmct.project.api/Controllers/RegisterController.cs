﻿using nmct.project.api.Models;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.project.api.Controllers
{
    public class RegisterController : ApiController
    {
        public List<Registers> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterDA.GetRegisters(p.Claims);
        }

        public List<RegisterEmployee> Get(int id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return RegisterDA.GetEmployeesOnRegister(id, p.Claims);
        }

        public HttpResponseMessage Post(RegisterEmployee re)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.LogEmployee(re, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
