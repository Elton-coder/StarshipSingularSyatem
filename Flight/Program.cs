using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GalaxyTravel
{
    class Program
    {
        //public abstract class ObjectInGalaxy
        //{

        //    public int X { get; set; }
        //    public int Y { get; set; }
        //    public int Z { get; set; }

        //    public double Distance { get; set; }
        //}
        //I thought of using public abstract class GalaxyEntity and Planet class as subclass.
        // So no need for abstract factory. 
        //As a galaxy entity can either be planet or monster.  Boolean would be fine
        public class Planet
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public double Distance { get; set; }
            public Planet(int _X, int _Y, int _Z, bool _isMonster, bool _isHabitable, int _surfaceArea, bool _isColonized)
            {
                this.X = _X;
                this.Y = _Y;
                this.Z = _Z;
                this.IsMonster = _isMonster;
                this.IsHabitable = _isHabitable;
                this.SurfaceArea = _surfaceArea;
                this.IsColonized = _isColonized;
            }
            public bool IsMonster { get; set; }
            public bool IsHabitable { get; set; }
            public int SurfaceArea { get; set; }
            public bool IsColonized { get; set; }
        }

        public class FlightPlan //Results of FlightPlot will be stored here and print to file
        {
            public List<Planet> Objects { get; set; }//Use List<T> because we want to modify and sort
            public double Time { get; set; }
            public int TotalPlanetsColonized { get; set; }
            public int TotalConquredSpace { get; set; }

            public double TimeTakenToColonizeLastPlanet { get; set; }
        }

        public class FlightPlot //To find the closest habitale planets
        {
            public List<Planet> Map { get; set; }
            public FlightPlan Route { get; set; }
            public bool EndIndicator { get; set; }
        }
        static List<Planet> GetGalaxyDetails()
        {
            List<Planet> planetList = new List<Planet>();

            Planet homePlanet = new Planet(123123991, 098098111, 456456999, false, true, 0, true); //Adding my home to act as vantage point

            planetList.Add(homePlanet);
            string fileName = @"c:\Galaxy\galaxy_data.txt";

            foreach (string line in File.ReadAllLines(fileName))
            {
                string[] variables = line.Split(',');
                planetList.Add(new Planet( // Adding all the other objects in the universe
                    int.Parse(variables[0]),
                    int.Parse(variables[1]),
                    int.Parse(variables[2]),
                    bool.Parse(variables[3]),
                    bool.Parse(variables[4]),
                    int.Parse(variables[5]),
                    false));
            }
            return planetList;

        }

        static void Main(string[] args)
        {
            List<FlightPlan> routes = new List<FlightPlan>();
            List<Planet> objectsInSpaceList = GetGalaxyDetails();

            objectsInSpaceList.ToList()
                .ForEach(x => x.Distance = CalculateDistance(objectsInSpaceList[0].X, objectsInSpaceList[0].Y, objectsInSpaceList[0].Z, x.X, x.Y, x.Z)); //Populate the distance to every other planet in the universe from the current one
            var nearestObjects = objectsInSpaceList.OrderBy(x => x.Distance).ToList();  //.Where(x => x.Habbitable == true && x.Planet == true && x.Controlled == false)

            routes.Add(new FlightPlan() { Objects = new List<Planet>() });
            routes[0].Objects.Add(nearestObjects[0]); //Add homeworld as my starting point for my route


            int loop = 0;
            try
            {
                using (var writer = new StreamWriter("C:\\Galaxy\\galaxy_flight_plan.txt"))
                {
                    FlightPlot map = new FlightPlot();
                    do
                    {
                        map = FindClosestHabitablePlanet(nearestObjects, routes[loop]); //
                        routes[loop] = map.Route;
                        nearestObjects = map.Map;
                    } while (map.EndIndicator == false);


                    writer.WriteLine("--------------------------THE 24 HOUR COLONIZATION FLIGHT OF STARSHIP--------------------------");
                    routes[0].Objects.Remove(routes[0].Objects[0]);//remove home planet
                    foreach (Planet item in routes[loop].Objects)
                    {
                        writer.WriteLine(
                        string.Format("- X - {0}  - Y - {1}  - Z - {2}   Area colonized : {3} Million square kilometers",
                        item.X.ToString("0,0").PadLeft(11),
                        item.Y.ToString("0,0").PadLeft(11),
                        item.Z.ToString("0,0").PadLeft(11),
                        item.SurfaceArea.ToString().PadLeft(3)
                        ));
                    }

                    writer.WriteLine(
                        string.Format(" Time : {0} hours, Colonized : {1} million square kilometers, Colonized a total of {2} planets",
                        (routes[loop].Time / 3600),//24h Hour period
                        routes[loop].TotalConquredSpace,
                        routes[loop].TotalPlanetsColonized
                        ));
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The directory was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }
        /* So, in this space a point is represented by an ordered triple(x,y,z)The distance formula states that the distance between two points in xyz-space is the square root of the sum of the 
        squares of the differences between corresponding coordinates.That is, given P1 = (x1, y1, z1) and P2 = (x2, y2, z2),
        the distance between P1 and P2 is given by d(P1-P2) = (x2-x1)2 + (y2-y1)2 + (z2 z1)2.Application of the Pythagorean Theorem.*/

        static double CalculateDistance(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            //Ensure we travel away from where we are in space.
            if ((x1 != x2) && (y1 != y2) && (z1 != z2))
            {
                double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
                return distance;
            }
            else
            {
                return 0;
            }
        }

        static FlightPlan ColonisePlanet(FlightPlan CurrentFlightPlan, Planet PlanetToColonise)
        {

            int moreThanHalfOfArea = (PlanetToColonise.SurfaceArea / 2) + 1; //In order to inhabit a planet, you have to colonize more than 50% of its surface.
            double timeTakenToColonize = moreThanHalfOfArea * 0.043;    //time = distance / speed. We assume 0.043 per million square km. Per square km has some challenges 
            if ((CurrentFlightPlan.Time + timeTakenToColonize) < 86400)
            {
                CurrentFlightPlan.Time += timeTakenToColonize;
                CurrentFlightPlan.TotalConquredSpace += moreThanHalfOfArea;
                CurrentFlightPlan.TotalPlanetsColonized++;
                CurrentFlightPlan.Objects.Add(PlanetToColonise);
            }
            else
                CurrentFlightPlan.TimeTakenToColonizeLastPlanet = timeTakenToColonize;

            return CurrentFlightPlan;
        }
        static FlightPlot FindClosestHabitablePlanet(List<Planet> OrderedPlanetList, FlightPlan route)
        {
            double speed = OrderedPlanetList[1].Distance / 600; //Speed = distance/time. 10 minutes to the immediate neighbour
           // FlightPlot map = new FlightPlot();
            for (int i = 1;; i++)
            {
                if ((OrderedPlanetList[i].IsMonster == false) && (OrderedPlanetList[i].IsHabitable == true) && (OrderedPlanetList[i].IsColonized == false))
                {
                    FlightPlot map = new FlightPlot();
                    map.Route = ColonisePlanet(route, OrderedPlanetList[i]);
                    map.Route.Time += (OrderedPlanetList[i].Distance / speed);
                    if (map.Route.Time < 86400)
                    {
                        OrderedPlanetList.ForEach(c => c.Distance =
                        CalculateDistance(OrderedPlanetList[i].X,
                        OrderedPlanetList[i].Y,
                        OrderedPlanetList[i].Z,
                        c.X,
                        c.Y,
                        c.Z));
                        map.Map = OrderedPlanetList;
                        map.Map[i].IsColonized = true;
                    }
                    else
                    {
                        map.Route.Time -= (OrderedPlanetList[i].Distance / speed);
                        map.Route.Time -= map.Route.TimeTakenToColonizeLastPlanet;
                        map.EndIndicator = true;
                    }
                     return map;
                }
                //return map;
            }
           // return map;
        }
    }
}
