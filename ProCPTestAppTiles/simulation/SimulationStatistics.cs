using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace ProCPTestAppTiles.simulation
{
    public class SimulationStatistics
    {
        private SimulationMap simMap;
        private Stopwatch sw = new Stopwatch();
        private string totaltime;
        private List<Life> delayedCars = new List<Life>();
        private List<Stopwatch> stoppies = new List<Stopwatch>();
        private List<double> allAverageSpeedList = new List<double>();
        private List<Tile> allDocumentedTileAvg = new List<Tile>();
        private List<Tile> allDocumentedTileDelay = new List<Tile>();
        private List<int> AllDocumentedCarsPassedCrashedTile = new List<int>();
        private int countOfNrCarsThatPassed = 0;
        private int numbrOfCrashedCarsInTile;
        private double maxvelocity2;
        private double averagespeed;
        private double averagevelocity;
        public List<Life> allLifes { get; set; }

        /*
        SMART
        Average Road Usage
        Bottlenecks
        Inflow & Outflow
        */
        public SimulationStatistics(SimulationMap sim)
        {
            this.allLifes = new List<Life>();
            this.simMap = sim;
        }

        /// <summary>
        /// counts the total number of cars being drawn
        /// </summary>
        /// <returns>total number of drawables</returns>
        public double CalculateTotalCarsMoving()
        {
            return simMap.lifes.Count(l => l.IsMoving());
        }

        /// <summary>
        /// calculates average speed based on stopwatch in simulation class and total distance of cars travelled
        /// pixels / milliseconds
        /// </summary>
        /// <returns>a speed of pixels / milliseconds</returns>
        public double CalculateAverageSpeed()
        {
            if (simMap.lifes.Any())
            {
                averagespeed = simMap.lifes.Where(l => l != null).Select(l => l.velocity).Average();
                return averagespeed;
            }
            return averagespeed;
/*
            double averageofaverage = 0;
            double count = 0;
            if (simMap.lifes.Any())
            {
                double totalvelocity = 0;
                foreach (var life in simMap.lifes)
                {
                    totalvelocity += life.velocity;
                }

                double averagevelocity = totalvelocity / simMap.lifes.Count;
                allAverageSpeedList.Add(averagevelocity);
                return averagevelocity;
            }
            else
                foreach (var speed in allAverageSpeedList)
                {
                    averageofaverage += speed;
                    count = count + 1;
                }

            return averageofaverage / count;
*/
        }


        public int TotalCarsinSim()
        {
            return simMap.lifes.Count;
        }

        public int TotalCarsStopped()
        {
            return simMap.lifes.Count(l => !l.IsMoving());
        }

        // MARCELLO BELOW
        /// <summary>
        /// Returns total distance for all life
        /// </summary>
        /// <returns> total distance for all life </returns>
        /// 
        public double CalculateAllCarsDistanceTravelled()
        {
/*
            double totalCarDistance = 0;
            foreach (var life in simMap.finsihed)
            {
                if (life.objectId == 1)
                    totalCarDistance += life.distanceTraveled;
            }
*/

            return Math.Round(allLifes.Select(l => l.distanceTraveled).Sum() / 1000, 4);
        }


        /// <summary>
        /// returns an integer number with the total number of cars that completed the simulation
        /// </summary>
        /// <returns>int Simulation.FinishedmovingLife.Count</returns>
        public int TotalCarsCompletedRoute()
        {
            return simMap.finished.Count;
        }


        public Tile GetTileWithMostCrashes(Tile[,] tiles, Simulation simu)
        {
            if (simu == null)
            {
                return null;
            }

            int maxNumber = 0;
            Tile tileWithMaxCrashes = null;
            foreach (var tile in tiles)
            {
                var crasherPerTile = simu.crashReports.Count(cr => cr.GetTile().Equals(tile));

                if (crasherPerTile <= maxNumber)
                {
                    continue;
                    
                }
                
                tileWithMaxCrashes = tile;
                maxNumber = numbrOfCrashedCarsInTile;
                numbrOfCrashedCarsInTile = crasherPerTile;
            }

            return tileWithMaxCrashes;
        }

        public Tile GetTileWithMostStops(Tile[,] tiles, Simulation sim, List<Life> lives)
        {
            if (simMap.lifes.Any())
            {
                int maxNumber = 0;
                Tile tilewithMostDelay = null;
                foreach (var tile in tiles)
                {
                    int maxstoppedpertile = 0;
                    foreach (var life in lives)
                    {
                        if (life.currentPath.tile.Equals(tile) && !life.IsMoving())
                        {
                            maxstoppedpertile++;
                        }
                    }

                    if (maxstoppedpertile > maxNumber)
                    {
                        maxNumber = maxstoppedpertile;
                        tilewithMostDelay = tile;
                    }
                }

                allDocumentedTileDelay.Add(tilewithMostDelay);
                return tilewithMostDelay;
            }

            Tile most = allDocumentedTileDelay.GroupBy(i => i).OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key).First();
            Debug.WriteLine("Delay" + most);
            return most;

            /* var most = list.GroupBy(i=>i).OrderByDescending(grp=>grp.Count())
      .Select(grp=>grp.Key).First(); */
        }

        public Tile GetTileWithHighestAvgVelocity(Tile[,] tiles, Simulation sim, List<Life> lives)
        {
            if (simMap.lifes.Any())
            {
                int maxNumber = 0;
                double count = 0;
                Tile tilewithHighestAvgVelocity = null;
                foreach (var tile in tiles)
                {
                    double avgvelocitypertile = 0;
                    foreach (var life in lives)
                    {
                        if (life.currentPath.tile.Equals(tile))
                        {
                            avgvelocitypertile += life.velocity;
                            count = count + 1;
                        }
                    }

                    avgvelocitypertile = avgvelocitypertile / count;

                    if (avgvelocitypertile > maxNumber)
                    {
                        tilewithHighestAvgVelocity = tile;
                    }
                }

                allDocumentedTileAvg.Add(tilewithHighestAvgVelocity);
                return tilewithHighestAvgVelocity;
            }
            else
            {
                Tile most = allDocumentedTileAvg.GroupBy(i => i).OrderByDescending(grp => grp.Count())
                    .Select(grp => grp.Key).First();
                Debug.WriteLine("Avg" + most);
                return most;
            }
        }


        public int NumberOfCrashedCarsInTileWithMostCrashedCars(Tile tile, List<Life> lives)
        {
            int counter = 0;
            foreach(var life in lives)
            {
               if(life.currentPath.tile == tile && !life.enabled)
                {
                    counter++;
                }
            }
            return counter;
        }

        public double GetMaxVelocityInMostCrashedTile(Tile tileWithMostCrashedCars, Simulation simu)
        {
            if (simu.crashReports.Count == 0)
            {
                return 0;
            }
            return simu.crashReports
                .Where(cr => cr.GetTile().Equals(tileWithMostCrashedCars))
                .Select(cr => cr.GetMaxVelocity())
                .Max();
/*            
            List<CrashReport> reportsThatMatter = new List<CrashReport>();
            double maxVel = 0.0;

            foreach (var report in simu.crashReports)
            {
                if (report.GetTile() == tileWithMostCrashedCars)
                {
                    reportsThatMatter.Add(report);
                }
            }

            foreach (var report in reportsThatMatter)
            {
                double k = report.GetMaxVelocity();

                if (k > maxVel)
                {
                    maxVel = k;
                }
            }
            // Debug.Write(maxVel);

            return maxVel;
*/
        }

        public int NrOfCarsThatPassedThroughCrashedTile(Tile tilewithMostCrashedCars, List<Life> lives)
        {
            //   if (simMap.lifes.Any())
            // {
            countOfNrCarsThatPassed = 0;
            foreach (var life in lives)
            {
                if (life.currentPath.tile == tilewithMostCrashedCars)
                {
                    countOfNrCarsThatPassed++;
                }
            }

            AllDocumentedCarsPassedCrashedTile.Add(countOfNrCarsThatPassed);
            return countOfNrCarsThatPassed;
            //}
            /*  else
              {
                  int maxNrOfCars= AllDocumentedCarsPassedCrashedTile.Max();
                  return maxNrOfCars;

              }*/
        }


        public double GetAverageVelocityOfAllCrashedCars(Simulation simu)
        {
            if (simu.crashReports.Count == 0)
            {
                return 0;
            }
            
            return simu.crashReports.Select(cr => cr.GetAverageVelocity()).Average();
/*
            double totalAvg = 0;
            double nrOfCrashedCars = 0;
            double averageOfAllCrashedCars = 0;

            foreach (var report in simu.crashReports)
            {
                totalAvg += report.GetAverageVelocity();
                //Debug.Write(totalAvg + " --" );
                // Debug.Write(report.GetAverageVelocity() + " ---");
                nrOfCrashedCars++;
            }

            averageOfAllCrashedCars = totalAvg / nrOfCrashedCars;
            return averageOfAllCrashedCars;
*/
        }


        public string TotalAmountOfDelay(List<Life> lives, Simulation simu)
        {
            foreach (var life in lives)
            {
                if (!life.IsMoving() && life.GetNextRoadPosition().HasLife())
                {
                    sw.Start();
                    if (life.IsMoving())
                    {
                        sw.Stop();
                    }
                }
            }


            TimeSpan ts = sw.Elapsed;
            totaltime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            return totaltime;
        }
        
        public double GetAverageRoadUsagePerTile(Tile tile)
        {
            if (tile.GetRoadPositionGrid() == null || tile.GetRoadPositionGrid().Count == 0)
            {
                return 0;
            }
            return tile.GetRoadPositionGrid().Select(rp => rp.counter).Average();
        }

        public Tile GetMostUsedTile(Simulation sim)
        {
            Tile t = null;
            var max = 0d;
            foreach (var tile in sim.simulationMap.tiles)
            {
                var avgRoadUse = GetAverageRoadUsagePerTile(tile);
                if (avgRoadUse >= max)
                {
                    t = tile;
                    max = avgRoadUse;
                }
            }

            return t;
        }


        public int NrOfCrashedCars()
        {
            return simMap.lifes.Count(l => !l.enabled);
        }
    }
}