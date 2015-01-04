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
    public class SalesController : ApiController
    {
        public HttpResponseMessage Post(ProductList pl)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            CustomerDA.LowerBalance(pl.CustomerTransaction.VerschilBedrag, pl.CustomerTransaction.RijksregisterNummer, p.Claims);
            SalesDA.SaveSales(pl, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
