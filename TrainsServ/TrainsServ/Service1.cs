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
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<string> GetTripWithTime(string From, string To, DateTime FromTime)
        {
            List<TrainData> list = ParseCVS();
            List<string> helpList = new List<string>();
            //int code;
            //if((code = IsCityOnData(From, To, list)) != 0)
            //{
            //    helpList.Add( NoCityMsg(code, From, To));
            //    return helpList;
            //}
            foreach(TrainData record in list)
            {
                if (record.TownA1.Equals(From) && record.TownB1.Equals(To) && FromTime <= record.TimeFrom1)
                {
                    helpList.Add(toStringTrainData(record));
                }
                
            }
            return helpList;
        }

        public List<string> GetTripWithoutTime(string From, string To)
        {
            List<TrainData> list = ParseCVS();
            List<string> outputList = new List<string>();
            //int code;
            //if ((code = IsCityOnData(From, To, list)) != 0)
            //{
            //    outputList.Add(NoCityMsg(code, From, To));
            //    return outputList;
            //}
            foreach (TrainData record in list)
            {
                if(record.TownA1.Equals(From) && record.TownB1.Equals(To))
                {
                    outputList.Add(toStringTrainData(record));
                }
            }

            return outputList;
        }

        //public string NoCityMsg(int code, string one, string two)
        //{
        //    switch (code)
        //    {
        //        case 1:
        //            return "There is no city like: " + one;
        //            break;
        //        case 2:
        //            return "There is no city like: " + two;
        //            break;
        //        case 3:
        //            return "There is no cities like: " + one + " and " + two;
        //            break;
        //    }
        //    return "";
        //}

        //public int IsCityOnData(string From, string To, List<TrainData> list)
        //{
        //    int i = 0, j = 0, wynik = 0;
        //    foreach (TrainData record in list)
        //    {
        //        if (record.TownA1.Equals(From))
        //        {
        //            i++;
        //        }
        //        if (record.TownB1.Equals(To))
        //        {
        //            j++;
        //        }
        //    }
        //        if(i != 0)  wynik++;
        //        if (j != 0) wynik += 2;
        //    return wynik;
        //}

        public List<TrainData> ParseCVS()
        {
            string[] csvLines = File.ReadAllLines(@"C:\Desktop\WCF\WCF_Trains\TrainsServ\TrainsServ\trains.csv");
            List<TrainData> list = new List<TrainData>();

            foreach (string line in csvLines.Skip(1))
            {
                var splitedLine = line.Split(',');
                list.Add(
                    new TrainData(
                        splitedLine[0],
                        splitedLine[2],
                        Convert.ToDateTime(splitedLine[1]),
                        Convert.ToDateTime(splitedLine[3])
                        ));
             }
            return list;
        }
        
        public string toStringTrainData(TrainData record)
        {
            return record.TownA1.ToString() + " " + record.TimeFrom1.ToString() + " " +
                            record.TownB1.ToString() + " " + record.TimeTo1.ToString();
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
