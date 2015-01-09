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
    public class ErrorController : ApiController
    {
        public HttpResponseMessage Post(Errorlog errorlog)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ErrorDA.CreateErrorlog(errorlog, p.Claims);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
