using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform;
using ProCPTestAppTiles.forms.mapcreatorform.board;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.utils;
using ProCPTestAppTiles.utils.tile;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.simulation.entities.life
{
    public abstract class Life : IDrawable, ISaveable
    {
        private static LifeDao _lifeDao = (LifeDao) DaoFactory.GetByType<Life>();
        
        public static int MAX_VELOCITY = 1000;

        public int objectId { get; set; }
        public RoadPosition curRoadPosition { get; set; }
        public Path currentPath { get; set; }
        public Path endingPath { get; set; }
        public double rotation { get; set; }
        public double velocity = 100;
        public List<Position> vPositions = new List<Position>();
        public bool enabled = true;
        

        //variables for stats
        public double distanceTraveled { get; set; }
        private Stopwatch stopTrafficLight = new Stopwatch();
        public double timeStoppedAtTrafficLight = 0;
        private Stopwatch stopNonMoving = new Stopwatch();
        public double timeStopped = 0;

        /// <summary>
        /// Event that is invoked when Life has finished it's path.
        /// </summary>
        /// <param name="life">Life that has finished it's path.</param>
        public delegate void finishedPathDelegate(Life life);

        public event finishedPathDelegate finishedPath;

        protected Life()
        {
        }

        protected Life(Path startPath, Path endingPath)
        {
            currentPath = startPath;
            this.endingPath = endingPath;
            SetCurRoadPosition(startPath.Start());
            velocity = Utils.GetRandom(60, 100);
        }

        // Methods

        /// <summary>
        /// Returns Position of 'CurRoadPosition'
        /// </summary>
        /// <returns>CurRoadPosition (current position x y of road)</returns>
        protected Position GetPosition()
        {
            return curRoadPosition?.position;
        }

        /// <summary>
        /// Returns Position of 'Life' in the Simulation
        /// </summary>
        /// <returns> CurRoadPosition </returns>
        protected Position GetVPosition()
        {
            if (vPositions != null && vPositions.Count > 0)
            {
                return vPositions[0];
            }

            return curRoadPosition.position;
        }

        /// <summary>
        /// Remove position of 'Life' in the simulation and makes the position of 'life' available for next road position.
        /// </summary>
        protected void RemoveFromVPositions()
        {
            if (vPositions != null && vPositions.Count > 0)
            {
                vPositions.RemoveAt(0); // delete first entry
                if (vPositions.Count == 0)
                {
                    EndOfMoveTo();
                }
            }
        }

        /// <summary>
        /// Prepares the drawing by removing the position of life to keep it free for the next one.
        /// </summary>
        /// <returns></returns>
        protected Position PrepareDrawing()
        {
            var position = GetVPosition();
            if (vPositions != null && vPositions.Count > 0)
            {
                position = vPositions[0];
                RemoveFromVPositions();
            }

            // Null Check
            if (position == null)
            {
                Debug.WriteLine("[Err] Null Position");
                return null;
            }

            return position;
        }

        public RoadPosition GetNextRoadPosition()
        {
            return currentPath.GetNext(curRoadPosition);
        }

        /// <summary>
        /// Checks if VPosition list exists and if it's more than 0, meaning if 'Life' is moving.
        /// </summary>
        /// <returns>True/False</returns>
        public bool IsMoving()
        {
            return vPositions != null && vPositions.Count > 0;
        }


        /// <summary>
        /// Moves 'Life' to a new road position.
        /// If velocity of 'Life' is not 0, the new given road position is not null, the new road position is not the same
        /// as the current road position and the new road position isn't already taken by another 'life' THEN
        /// then 'Life' position will be the new roads position and the current position of 'Life' will be free'd (null) 
        /// </summary>
        /// <param name="newRoadPosition"></param>
        public virtual void MoveTo(RoadPosition newRoadPosition)
        {
            // Sanity Checks
            if (newRoadPosition == null || velocity <= 0 || newRoadPosition.Equals(curRoadPosition))
            {
                return;
            }

            if (SimulationConstants.CAR_DRIVES_AROUND_CRASH)
            {
                if (!newRoadPosition.isFree() && newRoadPosition.oldLife != null)
                {
                    var best = TileUtils.GetNearestRoadPosition(curRoadPosition, currentPath.tile, currentPath,
                        GetTiles());
                    if (best != null)
                    {
                        var bestRoadPosition = best.Item1;
                        var bestPath = best.Item2;

                        currentPath = bestPath;
                        if (bestRoadPosition == null)
                        {
                            return;
                        }

                        MoveTo(bestRoadPosition);
                    }
                }
            }

            if (!newRoadPosition.isFree() && newRoadPosition.oldLife != null && velocity >= SimulationConstants.CRASH_VELOCITY)
            {
                DoCrash(newRoadPosition.oldLife);
            }

            if (!newRoadPosition.isFree())
            {
                return;
            }

            vPositions = CalculateVirtualPositions(newRoadPosition.position, curRoadPosition.position);

            AddDistanceTraveled(Utils.CalculateDistanceBetweenPoints(curRoadPosition.position, newRoadPosition.position));
            StartMoveTo();
            newRoadPosition.counter++;
            //Debug.Write(this.velocity);

        }

        private void StartMoveTo()
        {
            var nextRoadPosition = GetNextRoadPosition();
            var curRoadPosition = this.curRoadPosition;

            SetNextRoadPosition(nextRoadPosition);
            SetCurRoadPosition(curRoadPosition);
        }

        private void EndOfMoveTo()
        {
            var nextRoadPosition = GetNextRoadPosition();
            var curRoadPosition = this.curRoadPosition;

            RemoveCurrentRoadPosition(curRoadPosition); // Remove curRoadPosition
            SetCurRoadPosition(nextRoadPosition); // Set the curRoadPosition & curRoadPosition.oldLife
            RemoveNextRoadPosition(nextRoadPosition); // Remove nextRoadPosition.newLife
        }

        private void SetCurRoadPosition(RoadPosition roadPosition)
        {
            curRoadPosition = roadPosition;
            curRoadPosition.oldLife = this;
        }

        private void SetNextRoadPosition(RoadPosition roadPosition)
        {
            roadPosition.newLife = this;
        }

        private void RemoveNextRoadPosition(RoadPosition roadPosition)
        {
            roadPosition.newLife = null;
        }

        private void RemoveCurrentRoadPosition(RoadPosition roadPosition)
        {
            roadPosition.oldLife = null;
            this.curRoadPosition = null;
        }

        private List<Position> CalculateVirtualPositions(Position newPos, Position curPos)
        {
            var dX = newPos.X - curPos.X;
            var dY = newPos.Y - curPos.Y;

            CalculateAcceleration();
            var count = MAX_VELOCITY / velocity;
            double uX = dX / count;
            double uY = dY / count;

            List<Position> positions = new List<Position>();

            for (int i = 1; i <= count; i++)
            {
                var pos = curPos.Add(new Position(uX * i, uY * i));

                positions.Add(pos);
            }

            return positions;
        }

        /// <summary>
        /// Draw 'life'
        /// </summary>
        /// <param name="e"></param>
        public abstract void Draw(PaintEventArgs e);

        /// <summary>
        /// Adds the 'new' distance to the total amount of Distance travelled.
        /// </summary>
        /// <param name="distance"></param>
        public void AddDistanceTraveled(double distance)
        {
            distanceTraveled += distance;
        }

        public void SetNewPath(Path path)
        {
            // TODO -> new Path should NEVER be null, for used Tiles.
            if (path != null)
            {
                currentPath = path;
                MoveTo(currentPath.Start());
            }
        }

        /// <summary>
        /// Checks if the timer needs to keep ticking or stop.
        /// IF the next road position is null, then the timer has to stop ticking and 'life' has finished it's path
        /// If life is not moving but th timer is ticking, 'life' should move to next position. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Move()
        {
            var nextNextRoadPos = currentPath.GetNext(GetNextRoadPosition());
            var nextRoadPos = GetNextRoadPosition();
            if (nextNextRoadPos == null || nextRoadPos == null)
            {
                // Path has ended
                finishedPath?.Invoke(this);
                return;
            }

            if (!IsMoving())
            {

                MoveTo(nextRoadPos);
            }
        }


        public void CalculateAcceleration()
        {
            if (CanAccelerate() && MAX_VELOCITY > velocity)
            {
                velocity += 3;
            }
            else
            {
                velocity = 100;
            }
        }
        
        private bool CanAccelerate()
        {
            var canAcc = true;
            var howManyRoadPositionsToCheck = 2;
            var roadPos = curRoadPosition;
            for(int i = 0; i < howManyRoadPositionsToCheck; i++)
            {
                var nextRoadPosition = currentPath.GetNext(roadPos);
                if(nextRoadPosition == null )
                {
                    continue;
                }
                roadPos = nextRoadPosition;
                if (!nextRoadPosition.isFree())
                {
                    canAcc = false;
                    break;
                }
               
            }
            return canAcc;
        }

        public void DoCrash(Life lifeInfront)
        {
            // Crash both Cars
            ((Car) lifeInfront).crash();
            ((Car) this).crash();
            
            // Create Crash Report
            var crashReport = new CrashReport(this, lifeInfront);
            GetSimulation().crashReports.Add(crashReport);
            
            //add to list
           
        }

        private Simulation GetSimulation()
        {
            var t = endingPath?.tile;
            var b = ((BoardControl) t?.mommyControl)?.GetLogic();
            var m = ((MapCreatorControl) b?.mommyControl)?.GetLogic();
            return m?.simulation;
        }
        
        public Tile[,] GetTiles()
        {
            return GetSimulation().simulationMap.tiles;
        }

        public abstract void crash();

        /// <summary>
        /// ToString of variables from 'Life'
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        { 
            return  $"[{GetType().Name}] " +
                    $"Id= {objectId} " +
                    $"Rotation= {rotation} " +
                    $"Velocity= {velocity} " +
                    $"Distance Travelled= {distanceTraveled}";
        }

        public void Save(BinaryWriter writer)
        {
            _lifeDao.Save(this, writer);
        }
    }
}