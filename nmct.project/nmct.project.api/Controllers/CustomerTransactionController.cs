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
    public class CustomerTransactionController : ApiController
    {
        public HttpResponseMessage Post(CustomerTransaction ct)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.LowerBalance(ct.VerschilBedrag, ct.RijksregisterNummer, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
