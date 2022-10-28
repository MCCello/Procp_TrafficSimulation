using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.forms.menustrip;
using ProCPTestAppTiles.forms.simulationform;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.simulation.entities.road.events.crash;
using ProCPTestAppTiles.utils;
using ProCPTestAppTiles.utils.astar;
using ProCPTestAppTiles.utils.tile;
using Timer = System.Windows.Forms.Timer;

namespace ProCPTestAppTiles.simulation.entities.simulation
{
    public class Simulation : Attachable<SimulationControl>, ISaveable
    {
        private static SimulationDao _simulationDao = (SimulationDao) DaoFactory.GetByType<Simulation>();
        
        public SimMenuStrip simMenuStrip { get; set; }
        public Button heatmapButton { get; set; }
        public Button pauseButton { get; set; }
        public MapCreator mapCreator { get; set; }
        public Timer timer { get; set; }
        public Timer removeCrashedCarTimer { get; set; }
        public SimulationMap simulationMap { get; set; }
        public Queue queue { get; set; }
        public int lifeSize { get; set; }
        public int carsAmount { get; set; }
        public Stopwatch stop1 { get; set; }
        public List<CrashReport> crashReports { get; set; }
        


        public Simulation()
        {
            
        }
        
        public Simulation(MapCreator mapCreator, Control mommyControl, Point location) : base(mommyControl, location)
        {
            // Init Fields
            timer = new Timer {Interval = 20};
            removeCrashedCarTimer = new Timer { Interval = 10000 };
            this.mapCreator = mapCreator;
            stop1 = new Stopwatch();
            carsAmount = GetCarsToSimulate();
            
            
            // Init Simulation
            Init();
        }

        protected override void InitTControl()
        {
            control = new SimulationControl(this);
        }

        /// <summary>
        /// Initialise the Simulation.
        /// </summary>
        public override void Init()
        {
            crashReports = new List<CrashReport>();
            Form form = mommyControl as Form;

            if (form != null)
            {
                Size = form.Size;
            }
            
            timer = new Timer {Interval = 20};
            removeCrashedCarTimer = new Timer { Interval = 5000 };
            
            queue = new Queue();
            
            stop1 = new Stopwatch();
            stop1.Start();
            
            
            var x = 0;
            var y = 0;
            
            // Populate Panels
            
            // MenuStrip
            simMenuStrip = new SimMenuStrip(GetControl()) {Location = new Point(x, y)};
            simMenuStrip.AttachTo(GetControl());
            y += simMenuStrip.Height;
            if (form != null)
            {
                form.MainMenuStrip = simMenuStrip;
            }
            
            // SimulationMap
            if (simulationMap == null)
            {
                simulationMap = new SimulationMap(mapCreator.board.tiles, GetControl(), new Point(x, y));
            }
            x += simulationMap.Size.Width;
            
            // SimulationStatistics
            //Heatmap button
            heatmapButton = new Button { Size = new Size(80, 80), Text = @"Show HeatMap", Location = new Point(x, y) };
            heatmapButton.Click += GetControl().HeatmapButton_Clicked;
            AddControl(heatmapButton);
            y += heatmapButton.Height;
            heatmapButton.BringToFront();

            
            pauseButton = new Button { Size = new Size(80, 80), Text = IsPaused() ? @"Start" : @"Pause", Location = new Point(x, y) };
            pauseButton.Click += GetControl().PauseButton_Clicked;
            AddControl(pauseButton);
            y += pauseButton.Height;
            pauseButton.BringToFront();
            
            
            
            // Determine Size of SimulationControl
            UpdateSize();
            
            
            
            // # # #
            // # Logic
            // #
            

            // Add TimerTick Method to Tick event, to constantly call Invalidate() on pictureBox.
            timer.Tick += TimerTick;
            
            // Random Event
            removeCrashedCarTimer.Tick += RemoveCrashedCar;


            carsAmount = GetCarsToSimulate();
        }


        public bool IsPaused()
        {
            return !timer.Enabled;
        }


        /// <summary>
        /// Starts the Simulation.
        /// </summary>
        public void Start()
        {
            simulationMap.Start();
            timer.Start();
            removeCrashedCarTimer.Start();
            GetControl().UpdatePauseButton();
        }

        /// <summary>
        /// Stops the Simulation.
        /// </summary>
        public void Stop()
        {
            simulationMap.Stop();
            timer.Stop();
            removeCrashedCarTimer.Stop();
            GetControl().UpdatePauseButton();
        }

        /// <summary>
        /// Invalidates the pictureBox.
        /// Is called through the timer within the class. Causes the pictureBox to refresh and invoke the 'Paint' event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            
            simulationMap.Refresh(); // Refresh Map

            
            // Release Queue
            if (queue.Count() > 0)
            {
                ReleaseFirstInQueue();
            }
            
            // Random Event :: Crash Odds replaced
           
        }
        private void RemoveCrashedCar(object sender, EventArgs e)
        {
            simulationMap.lifes.OfType<Car>().Where(c => !c.enabled).ToList().ForEach(RemoveLife);
        }

        public int GetCarsToSimulate()
        {
            int cars = int.TryParse(mapCreator?.numberOfCars?.Text ?? "0", out cars) ? cars : 0;
            carsAmount = cars;
            return cars;
        }

        public void AddAmountOfCars(int amount)
        {
            lifeSize += amount;
            for (int i = 0; i < amount; i++)
            {
                var randStartingPath = Utils.GetRandomFromCollection(simulationMap.GetStartingPaths());
                var randStartingPathInflowDirection = randStartingPath.GetDirectForPathPerFlowType(FlowType.INFLOW);
                var startTile = randStartingPath.tile;
                var randEndingPath = Utils.GetRandomFromCollection(simulationMap.GetEndingPaths().Where(p => p.tile != startTile).ToList());
                var endTile = randEndingPath.tile;
                
                var completePath = new AStarPathFinder(this).FindPath(startTile, endTile);

                if (completePath.Count > 1)
                {
                    var nextTile = completePath[1].tile;
                    var directionTypeFromCurToNextTile =
                        TileUtils.GetDirectionTypeByNextTile(simulationMap.tiles, startTile, nextTile);
                    if (randStartingPathInflowDirection == null || directionTypeFromCurToNextTile == null)
                    {
                        return;
                    }

                    var xList = startTile.GetPathsByDirectionType(
                        (DirectionType) randStartingPathInflowDirection,
                        (DirectionType) directionTypeFromCurToNextTile);
                    randStartingPath = Utils.GetRandomFromCollection(xList);
                }
                
                if (!randStartingPath.Start().isFree())
                {
                    queue.Add(randStartingPath, randEndingPath);
                    continue;
                }
                
                Car car = new Car(randStartingPath, randEndingPath);
                AddLife(car);
            }
            
        }

        private void ReleaseFirstInQueue()
        {
            var path = Utils.GetRandomFromCollection(queue.queue);
            var startPath = path.Item1;
            var endPath = path.Item2;
            if (startPath == null || endPath == null)
            {
                queue.Remove(path);
                return;
            }
            if (!startPath.Start().isFree())
            {
                return;
            }
            
            queue.Remove(path);
            Car car = new Car(startPath, endPath);
            AddLife(car);
        }

        /// <summary>
        /// Safely adds instances of the Life class to the Simulation.
        /// </summary>
        /// <param name="life"></param>
        private void AddLife(Life life)
        {
            simulationMap.AddLife(life);
        }

        /// <summary>
        /// Safely removes instances of the Life class from the Simulation.
        /// </summary>
        /// <param name="life"></param>
        private void RemoveLife(Life life)
        {
            simulationMap.RemoveLife(life);
        }

        public void ShowHeatMap()
        {
            HeatMap heatMap = new HeatMap(this);
            heatMap.Show();
        }

        public void CleanUp()
        {
            queue = new Queue();
            simulationMap.CleanUp();
        }

        public void Save(BinaryWriter writer)
        {
            _simulationDao.Save(this, writer);
        }
    }
}