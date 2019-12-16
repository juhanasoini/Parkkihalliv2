using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parkkihalli
{
    static public class MainProgram
    {
        public static Parkkihalli parkkihalli = new Parkkihalli();
        static public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            //Ladataan tiedosto, jossa parkissa olevat autot ja tyhjät ruudut.
            parkkihalli.lataaParkkipaikat();
            //Ladataan fillarit
            parkkihalli.pyorateline.lataaFillarit();
            List<string> vaihtoehdot = Ohjeet.TulostaOhjeet();

            Polkupyora fillari = null;
            int paikka = -1;

            bool jatka = true;
            while (jatka)
            {
                Console.WriteLine("");
                string komento = "-1";
                while (vaihtoehdot.IndexOf(komento) < 0)
                {
                    Console.Write("Anna komento (o näyttää ohjeen): ");
                    komento = Console.ReadLine();
                }

                Parkkeeraaja parkkeeraaja = new Parkkeeraaja();
                //Kuunnellaan käyttäjän inputtia
                switch (komento)
                {
                    case "o":
                        Ohjeet.TulostaOhjeet();
                        break;
                    case "p":
                        Console.Clear();
                        // Etsitään alkuun vapaa ruutu. Jos löytyy, kysytään käyttäjältä ajoneuvon tiedot ja merkataan se vapaaseen ruutuun
                        // Lopuksi tallennetaan parkkipaikat tiedostoon
                        Console.Clear();
                        if (parkkihalli.etsiVapaaRuutu() < 0)
                        {
                            Console.WriteLine("Parkkihalli on täynnä!");
                            break;
                        }

                        Console.WriteLine("");
                        var ajoneuvo = parkkeeraaja.parkkeeraa();

                        if (ajoneuvo is Auto || ajoneuvo is Moottoripyora)
                        {
                            Moottoriajoneuvo auto = ajoneuvo as Moottoriajoneuvo;
                            paikka = parkkihalli.parkkeeraa(auto);
                            Console.WriteLine("Parkkeerattu paikalle {0}. Tarvitset tätä poistuessasi!", paikka);
                        }

                        parkkihalli.tallennaParkkipaikat();
                        break;
                    case "f":
                        Console.Clear();
                        Console.WriteLine("Jätä polkupyörä. HUOM: Ei takeita, että saat omasi takaisin!");

                        fillari = parkkeeraaja.varastoiFillari();
                        var tallessa = parkkihalli.pyorateline.parkkeeraa(fillari);

                        parkkihalli.pyorateline.tallennaFillarit();

                        if (tallessa)
                            Console.WriteLine("Homma hyvä");

                        break;
                    case "n":
                        // Kysytään käyttäjältä ruutu ja poistetaan auto parkkihallista. Lopuksi tallennetaan parkkipaikat tiedostoon
                        Console.Clear();
                        Moottoriajoneuvo noudettuAjoneuvo = null;
                        int ruutu = parkkeeraaja.kysyRuutu();
                        noudettuAjoneuvo = parkkihalli.nouda(ruutu);

                        if (noudettuAjoneuvo != null)
                        {
                            Console.WriteLine("Autonne: {0}. Pesty ja puunattu.", noudettuAjoneuvo.kutsumanimi());
                            parkkihalli.tallennaParkkipaikat();
                        }

                        //Ohjeet.TulostaOhjeet();

                        break;
                    case "nf":
                        // Kysytään käyttäjältä ruutu ja poistetaan auto parkkihallista. Lopuksi tallennetaan parkkipaikat tiedostoon
                        Console.Clear();
                        parkkihalli.pyorateline.Listaa();
                        paikka = parkkeeraaja.kysyFillariPaikka();
                        fillari = parkkihalli.pyorateline.nouda(paikka);

                        if (fillari != null)
                        {
                            Console.Write("Fillari: {0}. Pesty ja puunattu. Onko oma [K/E]: ", fillari.kutsumanimi());
                            string vastaus = "";
                            while (vastaus != "K" && vastaus != "E")
                            {
                                vastaus = Console.ReadLine();
                                // I crashed the program adding a " to the input - 
                                // the bug can be removed using vastaus = vastaus.Trim();

                                // Making it comparable also iuf imput on "k" and "e"
                                vastaus = vastaus.ToUpperInvariant();
                            }
                            if (vastaus == "K")
                                Console.WriteLine("Sepäs sattui");
                            else
                                Console.WriteLine("Harmi. Kiitos hei!");
                            Console.WriteLine();
                            parkkihalli.pyorateline.tallennaFillarit();
                        }

                        break;
                    case "l":
                    case "t":
                        // Listaa parkkihallista löytyvät ajoneuvot
                        Console.Clear();
                        Parkkipaikka parkkipaikka;
                        for (int i = 0; i < parkkihalli.parkkipaikat.Count; i++)
                        {
                            if (parkkihalli.parkkipaikat[i].onVarattu())
                            {
                                parkkipaikka = parkkihalli.parkkipaikat[i];
                                var ajoneuvo1 = parkkipaikka.getOccupant();

                                Console.WriteLine("Ruutu {0}: {1}.", i, ajoneuvo1.kutsumanimi());
                                if (komento == "t")
                                {
                                    Console.WriteLine(" - Parkissa {0} lähtien.", parkkipaikka.parkkeerattuAika.ToString("dd.MM.yyyy klo HH:mm:ss"));
                                    Console.WriteLine(" - Hinta tähän asti: {0} €, tunteja {1}", parkkipaikka.laskeHinta(), parkkipaikka.kulunutAika());
                                }

                                Console.WriteLine();
                            }
                            else
                                Console.WriteLine("Ruutu {0}: VAPAA", i);
                        }
                        Console.WriteLine();
                        //Ohjeet.TulostaOhjeet();

                        break;
                    case "lf":
                        // Listaa parkkihallista löytyvät fillarit
                        Console.Clear();
                        parkkihalli.pyorateline.Listaa();

                        break;

                    case "c":
                        // Lopettaa ohjelman
                        Console.WriteLine();
                        Console.WriteLine("Hei hei!");
                        System.Threading.Thread.Sleep(2000);

                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
