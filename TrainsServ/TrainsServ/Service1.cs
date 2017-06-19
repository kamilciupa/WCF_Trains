using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web;

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
            List<string> outputList  = new List<string>();
            
            if (!IsCityIn(From, list).Equals("Good"))
                outputList.Add(IsCityIn(From, list));
            if (!IsCityIn(To, list).Equals("Good"))
                outputList.Add(IsCityIn(To, list));

            outputList.Add("Połączenia bezpośrednie");
            outputList.Add("");
            foreach (TrainData record in list)
            {
                if (record.TownA1.Equals(From) && record.TownB1.Equals(To) && DateTime.Compare(record.TimeFrom1, FromTime) >=0)
                {
                    outputList.Add(toStringTrainData(record));
                }
                
            }
            outputList.Add("");
            outputList.Add("Połączenia pośrednie");
            outputList.Add("");
            outputList.AddRange(findIndircetConnectionsWithTime(From, To, FromTime, list));

            return outputList;
        }

        public List<string> GetTripWithoutTime(string From, string To)
        {
            List<TrainData> list = ParseCVS();
            List<string> outputList = new List<string>();
            
            if (!IsCityIn(From, list).Equals("Good"))
                outputList.Add(IsCityIn(From, list));
            if (!IsCityIn(To, list).Equals("Good"))
                outputList.Add(IsCityIn(To, list));

            
            outputList.Add("Połączenia bezpośrednie");
            outputList.Add("");
            foreach (TrainData record in list)
            {
                if(record.TownA1.Equals(From))
                {
                    if (record.TownB1.Equals(To))
                    {
                        outputList.Add(toStringTrainData(record));
                    }
                   
                }
            }

            outputList.Add("");
            outputList.Add("Połączenia pośrednie");
            outputList.Add("");
            outputList.AddRange(findIndircetConnections(From, To, list));
            
            return outputList;
        }


        private List<string> findIndircetConnections(string start, string end, List<TrainData> trips)
        {
            List<TrainData> startTemp = new List<TrainData>();
            List<TrainData> temp = new List<TrainData>();
            foreach (TrainData trip in trips)
            {
                if (trip.TownB1 == end)
                {
                    temp.Add(trip);
                }
                else if (trip.TownA1 == start)
                {
                    startTemp.Add(trip);
                }
            }
            List<string> indirectRoutes = new List<string>();


            foreach (TrainData startT in startTemp)
            {
                foreach (TrainData endT in temp)
                {
                    if (startT.TownB1 == endT.TownA1 && DateTime.Compare(startT.TimeTo1, endT.TimeFrom1) <= 0 )
                    {
                        string res = "START:  " + toStringTrainData(startT);
                        string res2 = "PRZESIADKA: " + toStringTrainData(endT);
                        indirectRoutes.Add(res);
                        indirectRoutes.Add(res2);
                        indirectRoutes.Add("");
                    }

                }
            }
            return indirectRoutes;
        }


        private List<string> findIndircetConnectionsWithTime(string start, string end, DateTime startTime, List<TrainData> trips)
        {
            List<TrainData> startTemp = new List<TrainData>();
            List<TrainData> temp = new List<TrainData>();
            foreach (TrainData trip in trips)
            {
                if (trip.TownB1 == end)
                {
                    temp.Add(trip);
                }
                else if (trip.TownA1 == start)
                {
                    startTemp.Add(trip);
                }
            }
            List<string> indirectRoutes = new List<string>();


            foreach (TrainData startT in startTemp)
            {
                DateTime startTimestartT = startT.TimeFrom1;
                
                foreach (TrainData endT in temp)
                {
                    DateTime startTimeendT =  endT.TimeFrom1;
                    
                    if (startT.TownB1 == endT.TownA1 && DateTime.Compare(startT.TimeTo1, endT.TimeFrom1) <= 0 && DateTime.Compare(startTimestartT, startTime) >= 0)//startTimestartT >= startTime)
                    {
                        string res = "START: " + toStringTrainData(startT);
                        string res2 = "PRZESIADKA: " + toStringTrainData(endT);
                       
                            indirectRoutes.Add(res);
                            indirectRoutes.Add(res2);
                            indirectRoutes.Add("");
                    }

                }
            }
          
            return indirectRoutes;
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
            return "Brak w bazie miasta: " + from;
        }

        public List<TrainData> ParseCVS()
        {
          //path to bin/debug 
            string[] csvLines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trains.csv"));
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
