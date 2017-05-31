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
            List<TrainData> list = ParseCVS();
            foreach(TrainData record in list)
            {
                if (record.TownA1.Equals(From) && record.TownB1.Equals(To) && record.TimeFrom1.Equals(FromTime))
                {
                    return record.ToString();
                }
                
            }
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

            public string TownA1 { get => TownA; set => TownA = value; }
            public string TownB1 { get => TownB; set => TownB = value; }
            public DateTime TimeFrom1 { get => TimeFrom; set => TimeFrom = value; }
            public DateTime TimeTo1 { get => TimeTo; set => TimeTo = value; }
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
