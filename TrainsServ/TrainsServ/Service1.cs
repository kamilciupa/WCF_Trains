﻿using System;
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

            if (!IsCityIn(From, list).Equals("Good"))
                helpList.Add(IsCityIn(From, list));
            if (!IsCityIn(To, list).Equals("Good"))
                helpList.Add(IsCityIn(To, list));


            foreach (TrainData record in list)
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

            if (!IsCityIn(From, list).Equals("Good"))
                outputList.Add(IsCityIn(From, list));
            if (!IsCityIn(To, list).Equals("Good"))
                outputList.Add(IsCityIn(To, list));

            foreach (TrainData record in list)
            {
                if(record.TownA1.Equals(From) && record.TownB1.Equals(To))
                {
                    outputList.Add(toStringTrainData(record));
                }
            }

            return outputList;
        }


        public string IsCityIn(string from, List<TrainData> list)
        {
            foreach(TrainData record in list)
            {
                if(record.TownA1.Equals(from) || record.TownB1.Equals(from))
                {
                    return "Good";
                }
            }
            return "There is no city like " + from;
        }

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
