using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.simulationform;
using ProCPTestAppTiles.forms.simulationform.simulationmap;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;
using ProCPTestAppTiles.utils.astar;
using ProCPTestAppTiles.utils.tile;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.simulation.entities.simulation.simulationmap
{
    public class SimulationMap : Attachable<SimulationMapControl>, ISaveable
    {
        private static SimulationMapDao _simulationMapDao = (SimulationMapDao) DaoFactory.GetByType<SimulationMap>();
        
        public static int OBJECT_ID_COUNTER = 1;
        
        public Tile[,] tiles { get; set; }
        public PictureBox pictureBox { get; set; }
        public List<Life> lifes { get; set; }
        public List<IDrawable> drawables { get; set; }
        public List<Life> finished { get; set; }
        public ListBox lbStats { get; set; }
        public SimulationStatistics simstats { get; set; }
        public Timer timer1 { get; set; }
        public Graphs graphs { get; set; }


        public SimulationMap()
        {
        }

        public SimulationMap(Tile[,] tiles, Control mommyControl, Point location) : base (mommyControl, location)
        {
            this.tiles = tiles;

            lifes = new List<Life>();
            drawables = new List<IDrawable>();
            pictureBox = new PictureBox();
            
            simstats = new SimulationStatistics(this); 
            finished = new List<Life>();
            timer1 = new Timer();
            Init();
        }

        protected override void InitTControl()
        {
            control = new SimulationMapControl(this);
        }

        public override void Init()
        {
            lifes = new List<Life>();
            drawables = new List<IDrawable>();
            pictureBox = new PictureBox();
            
            simstats = new SimulationStatistics(this); 
            finished = new List<Life>();
            timer1 = new Timer();
            
            
            // Add PathInfo to Tiles
            AddInfoToTiles();
            
            // InitSimulationMap
            pictureBox.Paint += RefreshGraphics;
            timer1.Interval = 350;
            timer1.Start();


            var x = 0;
            var y = 0;

            // PictureBox
            pictureBox.Image = CreateMapImageFromTiles();
            pictureBox.Size = pictureBox.Image.Size;
            AddControl(pictureBox);
            x += pictureBox.Right;

/*
            //Listbox Stats
            lbStats = new ListBox { Size = new Size(250, 250), Location = new Point(x, 90) };
            AddControl(lbStats);
            timer1.Tick += Timer1_Tick;
*/

            // Determine Size of SimulationMapControl
            UpdateSize();
           
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            lbStats.ItemHeight = 20;
            lbStats.Items.Clear();
            lbStats.Items.Add("Total amount of cars in Simulation:" + simstats.TotalCarsinSim()); lbStats.Items.Add("");
            lbStats.Items.Add("Total amount of cars Moving : " + simstats.CalculateTotalCarsMoving()); lbStats.Items.Add("");
            lbStats.Items.Add("Total amount of cars Stopped: " + simstats.TotalCarsStopped()); lbStats.Items.Add("");
            lbStats.Items.Add("Total amount of cars Finished route : " + simstats.TotalCarsCompletedRoute()); lbStats.Items.Add("");
            lbStats.Items.Add("Total distance : " + simstats.CalculateAllCarsDistanceTravelled() + " km"); lbStats.Items.Add("");
        }

        /// <summary>
        /// Draws every drawable onto the PaintEventArgs sender.
        /// Is hooked onto the 'Paint' event. Causing it to be caused when Invalidate() is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshGraphics(object sender, PaintEventArgs e)
        {
            lifes.Where(l => l.enabled).ToList().ForEach(l => l.Move());
            foreach (var drawable in drawables)
            {
                drawable.Draw(e);
            }
        }

        /// <summary>
        /// Safely adds instances of the Life class to the Simulation.
        /// </summary>
        /// <param name="life"></param>
        public void AddLife(Life life)
        {
            if (life.objectId == 0)
            {
                life.objectId = GetNextObjectId();
            }
            
            lifes.Add(life);
            drawables.Add(life);
            life.finishedPath += CalculateNextPath;
            simstats.allLifes.Add(life);
        }

        /// <summary>
        /// Safely removes instances of the Life class from the Simulation.
        /// </summary>
        /// <param name="life"></param>
        public void RemoveLife(Life life)
        {
            life.curRoadPosition.oldLife = null;
            if (life.GetNextRoadPosition() != null && life.GetNextRoadPosition().newLife == life)
            {
                life.GetNextRoadPosition().newLife = null;
            }
            lifes.Remove(life);
            finished.Add(life);
            drawables.Remove(life);
        }

        public static int GetNextObjectId()
        {
            return OBJECT_ID_COUNTER++;
        }

        public void CalculateNextPath(Life life)
        {
            var tile = life.currentPath.tile;
            if (tile == null)
            {
                return;
            }

            var simulation = (mommyControl as SimulationControl)?.GetLogic();
            if (simulation == null)
            {
                return;
            }

            var endTile = life.endingPath.tile;
            var completePath = new AStarPathFinder(simulation).FindPath(tile, endTile);

            if (completePath == null || completePath.Count <= 1)
            {
                RemoveLife(life);
                return;
            }
            
            var nextPaths = PathFinderUtils.GetNextPathByAStar(life, completePath, tiles);

            // TODO  -> Get Closest Path, Not Random!
            var randPath = Utils.GetRandomFromCollection(nextPaths);
            
            life.SetNewPath(randPath);
        }
        
        public void Refresh()
        {
            pictureBox.Invalidate();
        }

        public void Start()
        {
            foreach (var tile in tiles)
            {
                tile.Start();
            }

            graphs?.Start();
        }

        public void Stop()
        {
            foreach (var tile in tiles)
            {
                tile.Stop();
            }
            graphs.Stop();
        }

        /// <summary>
        /// Creates 1 Image from the 2D array of Tiles.
        /// </summary>
        /// <returns></returns>
        public Image CreateMapImageFromTiles()
        {
            var image = new Bitmap(TileConstants.TILE_WIDTH * tiles.GetLength(1), TileConstants.TILE_HEIGHT * tiles.GetLength(0));
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var tile in tiles)
                {
                    if (tile?.Image == null)
                    {
                        continue;
                    }
                    
                    graphics.DrawImage(tile.Image, new Rectangle(tile.Location, tile.Size));
                }
            }

            return image;
        }

        private void AddInfoToTiles()
        {
            foreach (var tile in tiles)
            {
                if (tile?.roadType == null)
                {
                    continue;
                }
                tile.AddInfo();
                if (tile.ittl != null)
                {
                    foreach (var roadPosition in tile.GetRoadPositionGrid())
                    {
                        if (roadPosition.TrafficLight != null)
                        {
                            drawables.Add(roadPosition.TrafficLight);
                        }
                    }
                }
            }
        }

        private void RenewTileGrid(Tile[,] tiles)
        {
            for (var i = 0; i < tiles.GetLength(0); i++)
            {
                for (var j = 0; j < tiles.GetLength(1); j++)
                {
                    var tile = tiles[i, j];
                    var newTile = new Tile(tile.mommyControl, tile.Location)
                    {
                        Size = tile.Size,
                        lanes = tile.lanes,
                        rotation = tile.rotation,
                        configData = tile.configData,
                    };
                    if (tile.roadType != null)
                    {
                        newTile.SetRoadType(tile.roadType);
                        newTile.rotation = tile.rotation;
                    }
                    
                    newTile.Refresh();
                    tiles[i, j] = newTile;
                }
            }
        }

        public List<Tile> GetUsedTiles()
        {
            var tiles = new List<Tile>();
            foreach (var tile in tiles)
            {
                if (tile.roadType == null)
                {
                    continue;
                }
                
                tiles.Add(tile);
            }

            return tiles;
        }

        public List<Tile> GetOuterTiles()
        {
            if (tiles == null)
            {
                return null;
            }

            var list = new List<Tile>();
            for (int h = 0; h < tiles.GetLength(0); h++)
            {
                for (int w = 0; w < tiles.GetLength(1); w++)
                {
                    if (h == 0 || w == 0 || h == (tiles.GetLength(0) - 1) || w == (tiles.GetLength(1) - 1))
                    {
                        var tile = tiles[h, w];
                        if (tile.roadType == null)
                        {
                            continue;
                        }
                        list.Add(tile);
                    }
                }
            }

            return list;
        }

        public List<Path> GetStartingPaths()
        {
            var startingTiles = GetOuterTiles();
            var startingPaths = new List<Path>();
            if (startingTiles == null)
            {
                return startingPaths;
            }

            foreach (var tile in startingTiles)
            {
                var paths = tile.GetPaths();
                if (paths == null)
                {
                    continue;
                }
                
                foreach (var path in paths)
                {
                    var startPos = path.Start().position;
                    if (Math.Abs(startPos.X) < 1 || Math.Abs(startPos.Y) < 1 || Math.Abs(startPos.X - pictureBox.Width) < 1 || Math.Abs(startPos.Y - pictureBox.Height) < 1)
                    {
                        startingPaths.Add(path);
                    }
                }
            }

            return startingPaths;
        }

        public List<Path> GetEndingPaths()
        {
            var startingTiles = GetOuterTiles();
            var endingPaths = new List<Path>();
            if (startingTiles == null)
            {
                return endingPaths;
            }

            foreach (var tile in startingTiles)
            {
                var paths = tile.GetPaths();
                if (paths == null)
                {
                    continue;
                }
                
                foreach (var path in paths)
                {
                    var endPos = path.End().position;
                    if (Math.Abs(endPos.X) < 1 || Math.Abs(endPos.Y) < 1 || Math.Abs(endPos.X - pictureBox.Width) < 1 || Math.Abs(endPos.Y - pictureBox.Height) < 1)
                    {
                        endingPaths.Add(path);
                    }
                }
            }

            return endingPaths;
        }

        public void CleanUp()
        {
            foreach (var tile in tiles)
            {
                tile.CleanUp();
            }
            lifes = new List<Life>();
            drawables = new List<IDrawable>();
            Stop();
        }

        public void Save(BinaryWriter writer)
        {
            _simulationMapDao.Save(this, writer);
        }
    }
}