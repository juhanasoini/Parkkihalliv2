using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Parkkihalli.Rajapinnat;
using Newtonsoft.Json.Serialization;

namespace Parkkihalli
{
    public class Parkkihalli
    {
        // Parkkihallin ruutujen määrä
        static int ruutuja = 10;
        static string autotTiedosto = @"autot.json";

        public List<Parkkipaikka> parkkipaikat = new List<Parkkipaikka>();
        public Pyöräteline pyorateline = new Pyöräteline();

        public Parkkihalli()
        {
            //Luodaan tyhjä tiedosto jos tiedostoa ei löydy. Vähennetään virhetilanteiden todennäköisyyttä
            if (!File.Exists(autotTiedosto))
                File.WriteAllText(autotTiedosto, "");

            for (int i = 0; i < ruutuja; ++i)
                this.parkkipaikat.Add(new Parkkipaikka());
        }

        // Lisää auton vapaaseen ruutuun
        public int parkkeeraa(Moottoriajoneuvo auto)
        {
            int vapaaRuutu = this.etsiVapaaRuutu();
            if (vapaaRuutu >= 0)
                this.parkkipaikat[vapaaRuutu].parkkeeraa(auto);
            else
                Console.WriteLine("Parkkihalli on täynnä");

            return vapaaRuutu;
        }

        // Noutaa auton ruudusta jos ruutu löytyy
        // Laskee parkkeerauksen hinnan ja pyytää maksua
        // Jos maksu hyväksytään, auto poistetaan parkkihallista
        public Moottoriajoneuvo nouda(int ruutu)
        {
            if (this.parkkipaikat.ElementAtOrDefault(ruutu) == null)
            {
                Console.WriteLine("Ruutua {0} ei ole", ruutu);
                return null;
            }
            if (this.parkkipaikat[ruutu].onVarattu() == false)
            {
                Console.WriteLine("Ruudussa {0} ei ole ajoneuvoa", ruutu);
                return null;
            }

            Parkkipaikka parkkipaikka = this.parkkipaikat[ruutu];
            Moottoriajoneuvo ajoneuvo = parkkipaikka.ajoneuvo;
            bool maksettu = false;
            string komento = "-1";
            while (!maksettu)
            {
                Console.WriteLine();
                Console.WriteLine("Kulunut aika {0} tuntia", parkkipaikka.kulunutAika());
                Console.WriteLine("Parkkeerauksen hinta on {0} €", parkkipaikka.laskeHinta());
                Console.Write("Kaiva kuvetta[y/n]: ");

                // *** Start changes
                // impeding that a real string is entered
                // read a char with ReadLine() 
                // and use ToString() to convert a char in string
                
                // old code komento = Console.ReadLine();
                
                komento = Console.ReadKey().ToString();
                // added to 
                komento = komento.ToUpperInvariant();
                // *** End changes

                if (komento == "n")
                {
                    Console.WriteLine("Parkkihalli pitää ajoneuvon!\n");
                    maksettu = true;
                    return null;
                }
                else if (komento == "y")
                    maksettu = true;
            }

            this.parkkipaikat[ruutu].ajoneuvo = null;

            return ajoneuvo;
        }

        // Tallentaa ruudut tiedostoon
        public void tallennaParkkipaikat()
        {
            KnownTypesBinder loKnownTypesBinder = new KnownTypesBinder()
            {
                KnownTypes = new List<Type> { typeof(Auto), typeof(Moottoripyora) }
            };

            IEnumerable<Parkkipaikka> Data = parkkipaikat.AsEnumerable();

            JsonSerializerSettings loJsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = loKnownTypesBinder,
                Formatting = Formatting.Indented
            };

            String json = JsonConvert.SerializeObject(Data, loJsonSerializerSettings);
            File.WriteAllText(autotTiedosto, json);
        }

        // Lataa ruudut tiedostosta
        public void lataaParkkipaikat()
        {
            if (new FileInfo(autotTiedosto).Length == 0)
                return;

            KnownTypesBinder loKnownTypesBinder = new KnownTypesBinder()
            {
                KnownTypes = new List<Type> { typeof(Auto), typeof(Moottoripyora), typeof(Parkkipaikka) }
            };


            JsonSerializerSettings loJsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = loKnownTypesBinder,
                Formatting = Formatting.Indented
            };

            this.parkkipaikat = JsonConvert.DeserializeObject<List<Parkkipaikka>>(File.ReadAllText(autotTiedosto), loJsonSerializerSettings);
        }

        // Etsii olemassa olevista ruuduista vapaan
        // Palauttaa vapaan ruudun numeron tai -1
        public int etsiVapaaRuutu()
        {
            int vapaaRuutu = -1;
            for (int i = 0; i < this.parkkipaikat.Count; i++)
            {
                if (!this.parkkipaikat[i].onVarattu())
                {
                    vapaaRuutu = i;
                    break;
                }
            }

            return vapaaRuutu;
        }

    }
    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}