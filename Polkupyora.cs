using System;
using Parkkihalli.Rajapinnat;

namespace Parkkihalli
{
    public class Polkupyora : iAjoneuvo
    {
        private String _tyyppi;
        private String _merkki;
        private String _malli;
        public Polkupyora()
        {
            Tyyppi = "Fillari";
        }

        public string Merkki
        {
            get
            {
                return _merkki;
            }
            set
            {
                _merkki = value;
            }
        }

        public string Malli
        {
            get
            {
                return _malli;
            }
            set
            {
                _malli = value;
            }
        }

        public string Tyyppi
        {
            get
            {
                return _tyyppi;
            }
            set
            {
                _tyyppi = value;
            }
        }

        public string kutsumanimi()
        {
            return Tyyppi + ": " + Merkki + " " + Malli;
        }
    }


}