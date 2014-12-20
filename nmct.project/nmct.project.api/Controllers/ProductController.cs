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
    public class ProductController : ApiController
    {
        public List<Products> Get()
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            return ProductDA.GetProducts(p.Claims);
        }

        public HttpStatusCode Delete(int Id)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            ProductDA.DeleteProduct(Id, p.Claims);
            return HttpStatusCode.OK;
        }

        public HttpResponseMessage Put(Products product)
        {
            ClaimsPrincipal p = RequestContext.Principal as ClaimsPrincipal;
            if (product.ID == 0)
            {
                ProductDA.SaveProduct(product, p.Claims);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                ProductDA.EditProduct(product, p.Claims);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
