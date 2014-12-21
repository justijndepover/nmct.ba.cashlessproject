using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.project.web.itbedrijf.Presentation_Models
{
    public class RegisterPM
    {
        public Registers Kassa { get; set; }
        public Organisations Vereniging { get; set; }

        public override string ToString()
        {
            if (Vereniging == null)
            {
                return Kassa.RegisterName + " - Geen vereniging";
            }
            else
            {
                return Kassa.RegisterName + " - " + Vereniging.OrganisationName;
            }
        }
    }
}