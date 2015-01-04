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
    public class CustomerController : ApiController
    {
        public Customer Get(string nummer)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomerDA.GetCustomer(nummer, p.Claims);
        }

        public List<Customer> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return CustomerDA.GetCustomers(p.Claims);
        }

        public HttpResponseMessage Put(Customer c)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.SaveCustomer(c, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Post(CustomerTransaction ct)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.LowerBalance(ct.VerschilBedrag, ct.RijksregisterNummer, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
