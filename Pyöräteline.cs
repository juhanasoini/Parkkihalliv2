using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace Parkkihalli
{
    public class Pyöräteline
    {
        public List<Polkupyora> polkupyorat = new List<Polkupyora>();
        private int maxLkm = 5;
        static string fillaritTiedosto = @"fillarit.json";

        public Pyöräteline()
        {
            if (!File.Exists(fillaritTiedosto))
                File.WriteAllText(fillaritTiedosto, "");
        }

        public bool parkkeeraa(Polkupyora fillari)
        {
            if (!onTilaa())
                return false;

            polkupyorat.Add(fillari);

            return true;
        }

        private bool onTilaa()
        {
            return polkupyorat.Count < maxLkm;
        }

        public void Listaa()
        {
            Console.WriteLine("Telineessä olevat fillarit:");

            for (int i = 0; i < polkupyorat.Count; i++)
            {
                Console.WriteLine("{0}: " + polkupyorat[i].kutsumanimi(), i);
            }
            Console.WriteLine();
        }

        public Polkupyora nouda(int paikka)
        {
            var rand = new Random();
            paikka = rand.Next(polkupyorat.Count - 1);

            Polkupyora fillari = polkupyorat.ElementAt(paikka);
            polkupyorat.RemoveAt(paikka);
            return fillari;
        }

        /// <summary>
        /// Tallentaa telineessä olevat polkupyörät JSON muodossa tiedostoon
        /// </summary>
        public void tallennaFillarit()
        {
            //https://stackoverflow.com/questions/46057081/json-newtonsoft-c-sharp-deserialize-list-of-objects-of-different-types
            KnownTypesBinder loKnownTypesBinder = new KnownTypesBinder()
            {
                KnownTypes = new List<Type> { typeof(Polkupyora) }
            };

            IEnumerable<Polkupyora> Data = polkupyorat.AsEnumerable();

            JsonSerializerSettings loJsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = loKnownTypesBinder,
                Formatting = Formatting.Indented
            };

            String json = JsonConvert.SerializeObject(Data, loJsonSerializerSettings);
            File.WriteAllText(fillaritTiedosto, json);
        }

        public void lataaFillarit()
        {
            if (new FileInfo(fillaritTiedosto).Length == 0)
                return;

            KnownTypesBinder loKnownTypesBinder = new KnownTypesBinder()
            {
                KnownTypes = new List<Type> { typeof(Polkupyora) }
            };

            JsonSerializerSettings loJsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = loKnownTypesBinder,
                Formatting = Formatting.Indented
            };

            polkupyorat = JsonConvert.DeserializeObject<List<Polkupyora>>(File.ReadAllText(fillaritTiedosto), loJsonSerializerSettings);

            if (polkupyorat.Count > maxLkm)
                polkupyorat.RemoveRange(maxLkm, polkupyorat.Count - maxLkm);
        }
    }
}