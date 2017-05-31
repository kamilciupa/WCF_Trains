using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Globalization;

namespace TrainsServ
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string GetTripWithTime(string From, string To, DateTime FromTime)
        {
            return "";
        }

        public string GetTripWithoutTime(string From, string To)
        {
            return "";
        }

        public List<TrainData> ParseCVS()
        {
            string[] csvLines = File.ReadAllLines(@"C:\Desktop\WCF\WCF_Trains\TrainsServ\TrainsServ\trains.csv");
            List<TrainData> list = new List<TrainData>();

            foreach (string line in csvLines)
            {
                var splitedLine = line.Split(',');
                list.Add(
                    new TrainData(
                        splitedLine[0],
                        splitedLine[1],
                        DateTime.ParseExact(splitedLine[2], "yyyy-MM-dd HH:mm", new CultureInfo("pl-PL")),
                        DateTime.ParseExact(splitedLine[3], "yyyy-MM-dd HH:mm", new CultureInfo("pl-PL"))));
             }
            return list;
        }

        public class TrainData
        {
            string TownA;
            string TownB;
            DateTime TimeFrom;
            DateTime TimeTo;

            public TrainData(string TownA, string TownB, DateTime TimeFrom, DateTime TimeTo)
            {
                this.TownA = TownA;
                this.TownB = TownB;
                this.TimeFrom = TimeFrom;
                this.TimeTo = TimeTo;
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
