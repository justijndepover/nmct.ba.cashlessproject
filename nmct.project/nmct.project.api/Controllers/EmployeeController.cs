using nmct.project.api.Models;
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
    public class EmployeeController : ApiController
    {
        public List<Employee> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return EmployeeDA.GetEmployees(p.Claims);
        }

        public HttpStatusCode Delete(int Id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            EmployeeDA.DeleteEmployee(Id, p.Claims);
            return HttpStatusCode.OK;
        }

        public HttpResponseMessage Put(Employee e)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            if (e.ID == 0)
            {
                EmployeeDA.SaveEmployee(e, p.Claims);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                EmployeeDA.EditEmployee(e, p.Claims);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
