using System;
using System.Collections.Generic;
using System.IO;

namespace Galaxy
{
    public class Galaxy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool isMonster { get; set; }
        public bool isHabitable { get; set; }
        public int surfaceArea { get; set; }
    }


    class Program
    {
        private const int totalEntities = 15000; //Maximum entities allowed

        static void Main(string[] args)
        {
            List<Galaxy> galaxyEntityList = new List<Galaxy>();
            Random random = new Random();
            int cntEntity = 0;
            string path = @"c:\Galaxy";
            Directory.CreateDirectory(path);
            string fileName = Path.Combine(path, "galaxy_data.txt");
            try
            {
                using (var writer = new StreamWriter(fileName))
                {
                    for (int i = 0; i < totalEntities; i++)
                    {
                        galaxyEntityList.Add(new Galaxy()
                        {

                            X = random.Next(0, 999999999),
                            Y = random.Next(0, 999999999),
                            Z = random.Next(0, 999999999),
                            isMonster = random.Next(2) == 0,//generate random boolean
                            isHabitable = random.Next(2) == 0,//will be changed in the writeline
                            surfaceArea = random.Next(1, 100)
                        });
                        writer.WriteLine(
                            string.Format("{0},{1},{2},{3},{4},{5}",
                            //galaxyEntityList[i].X.ToString(@"000\.000\.00\.0"),
                            //galaxyEntityList[i].Y.ToString(@"000\.000\.00\.0"),
                            //galaxyEntityList[i].Z.ToString(@"000\.000\.00\.0"),
                            galaxyEntityList[i].X,
                            galaxyEntityList[i].Y,
                            galaxyEntityList[i].Z,
                            galaxyEntityList[i].isMonster,
                          //  galaxyEntityList[i].isHabitable,
                            // If entity is a monster, then its not habitable. Reverse is true.
                            galaxyEntityList[i].isHabitable = !galaxyEntityList[i].isMonster ? true : false,
                            galaxyEntityList[i].surfaceArea
                            ));
                        cntEntity++;
                    }
                }
                Console.WriteLine("Galaxy data file created with " + cntEntity + " entities!");
                Console.ReadLine();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to create/overwite this file");
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
                Console.WriteLine($"An exception occurred:\nError code: " + $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
        }
    }
}
