using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.model.dbKlant
{
    public class CustomerTransaction
    {
        private long _rijksregisterNummer;

        public long RijksregisterNummer
        {
            get { return _rijksregisterNummer; }
            set { _rijksregisterNummer = value; }
        }

        private double _verschilBedrag;

        public double VerschilBedrag
        {
            get { return _verschilBedrag; }
            set { _verschilBedrag = value; }
        }

    }
}
