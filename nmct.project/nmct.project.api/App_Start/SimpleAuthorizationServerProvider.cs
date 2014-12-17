﻿using Microsoft.Owin.Security.OAuth;
using nmct.project.api.Models;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace nmct.project.api.App_Start
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Organisations o = OrganisationsDA.CheckCredentials(context.UserName, context.Password);
            if (o == null)
            {
                context.Rejected();
                return Task.FromResult(0);
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("dbname", o.DbName));
            id.AddClaim(new Claim("dblogin", o.DbLogin));
            id.AddClaim(new Claim("dbpass", o.DbPassword));

            context.Validated(id);
            return Task.FromResult(0);
        }
    }
}