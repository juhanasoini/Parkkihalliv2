using System;
using Parkkihalli.Rajapinnat;

namespace Parkkihalli
{
    public class Moottoriajoneuvo : iAjoneuvo
    {
        private String _tyyppi;
        private String _merkki;
        private String _malli;
        protected int _rekNro;

        public Moottoriajoneuvo()
        {

        }
        public Moottoriajoneuvo(String merkki, String malli)
        {
            Merkki = merkki;
            Malli = malli;
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

        public virtual string kutsumanimi()
        {
            return Merkki + " " + Malli;
        }
    }
}