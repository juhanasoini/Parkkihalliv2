using System;
using System.Collections.Generic;

namespace Parkkihalli
{
    static class Ohjeet
    {

        public static List<string> TulostaOhjeet()
        {
            Console.WriteLine("|-------------------------------|");
            Console.WriteLine("| [p]  = parkkeeraa auto        |");
            Console.WriteLine("| [f]  = jätä fillari           |");
            Console.WriteLine("| [n]  = nouda auto             |");
            Console.WriteLine("| [nf] = nouda fillari          |");
            Console.WriteLine("| [l]  = listaa autot           |");
            Console.WriteLine("| [lf] = listaa fillarit        |");
            Console.WriteLine("| [t]  = listaa autot tarkemmin |");
            Console.WriteLine("| [o]  = näytä ohjeet           |");
            Console.WriteLine("|-------------------------------|\n");

            List<string> vaihtoehdot = new List<string>();
            vaihtoehdot.Add("p");
            vaihtoehdot.Add("f");
            vaihtoehdot.Add("n");
            vaihtoehdot.Add("nf");
            vaihtoehdot.Add("l");
            vaihtoehdot.Add("lf");
            vaihtoehdot.Add("t");
            vaihtoehdot.Add("o");
            vaihtoehdot.Add("c");

            return vaihtoehdot;
        }
    }
}
