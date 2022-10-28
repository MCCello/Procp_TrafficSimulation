using System;
using System.Windows.Forms;
using LiveCharts.Wpf;
using Brushes = System.Windows.Media.Brushes;
using System.Windows;
using System.Windows.Media;
using ProCPTestAppTiles.simulation.entities.simulation;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles
{
    public partial class Graphs : Form
    {
        public Simulation sim;
        public string totaltime;
        private Button buttonSave;
        public Utils utils;
        private double maxVelocity;
        private double avgVelocity;
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        private Tuple<int,int> indexTileAvg;
        private Tuple<int, int> indexTileDelay;
        private Tuple<int,int> indexTile;
        private int numbrOfPassedCars = 0;
        private int numbrOfCrashedCarsInTile = 0;
        private int numbrOfAllCrashed = 0;


        public Graphs(Simulation simulation)
        {
            InitializeComponent();
            timer1.Interval = 300;
            timer1.Start();

            sim = simulation;

            CreateAngularG();
            CreateGauge();
            SaveButton();
        }

        public void Start()
        {
            timer1.Start();
        }
        
        public void Stop()
        {
            timer1.Stop();
        }

        public void CreateAngularG()
        {
            
            angularGauge1.FromValue = 50;
            angularGauge1.ToValue = 250;
            angularGauge1.TicksForeground = Brushes.White;
            angularGauge1.Base.Foreground = Brushes.White;
            angularGauge1.Base.FontWeight = FontWeights.Bold;
            angularGauge1.Base.FontSize = 16;
            angularGauge1.SectionsInnerRadius = 0.5;

            angularGauge1.Sections.Add(new AngularSection
            {
                FromValue = 50,
                ToValue = 250,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,20,147))
            });
            
        }

        public void CreateGauge()
        {
            solidGauge1.Uses360Mode = true;
            solidGauge1.From = 0;
            solidGauge1.To = sim.carsAmount;
            solidGauge1.Value = sim.carsAmount;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            solidGauge1.Value = sim.simulationMap.lifes.Count;
            if (sim.simulationMap.lifes.Count != 0)
            {
              
                    angularGauge1.Value = sim.simulationMap.simstats.CalculateAverageSpeed();
               
               // Debug.WriteLine(sim.simulationMap.simstats.CalculateAverageSpeed());
            }

            if (CheckForLastLife())
            {
                TimeSpan ts = sim.stop1.Elapsed;
                totaltime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                lblTotalTime.Text = totaltime;
             
                var tile = sim.simulationMap.simstats.GetTileWithMostCrashes(sim.simulationMap.tiles, sim);
                var amountDelay = sim.simulationMap.simstats.TotalAmountOfDelay(sim.simulationMap.lifes, sim);
                MaxMaxVelocity();
                AvgVelocity();
                lblCarCrash.Text = sim.simulationMap.simstats.NrOfCrashedCars().ToString();
                if(numbrOfAllCrashed < sim.simulationMap.simstats.NrOfCrashedCars())
                {
                    numbrOfAllCrashed = sim.simulationMap.simstats.NrOfCrashedCars();
                }
              
                if (numbrOfCrashedCarsInTile < NrofCrashedCarsTile())
                {
                    numbrOfCrashedCarsInTile = NrofCrashedCarsTile();
                }
                if (GetIndexTile() != null)
                {
                    lblTileCrash.Text = GetIndexTile().ToString();
                }
                indexTile = GetIndexTile();
                //  Debug.Write(avgVelocity + " --");
                // Debug.Write(amountDelay);
                if (GetIndexOfDelayTile() != null)
                {
                    lblTileStopped.Text = GetIndexOfDelayTile().ToString();
                    
                }
                
                //Debug.Write(GetIndexOfDelayTile());
                if (GetIndexOfAvgTile() != null)
                {
                    lblHighAvgSpeedTile.Text = GetIndexOfAvgTile().ToString();
                    indexTileAvg = GetIndexOfAvgTile();
                }
                ;
                if (numbrOfPassedCars < nrOfPassedCars())
                {
                    numbrOfPassedCars = nrOfPassedCars();
                }
                tltldistance.Text = "Total Distance Traveled: " + sim.simulationMap.simstats.CalculateAllCarsDistanceTravelled() + "km";



            }
            else sim.stop1.Stop();
            indexTileDelay = GetIndexOfDelayTile();
            indexTileAvg = GetIndexOfAvgTile();
            

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
        public bool CheckForLastLife()
        {
            if (sim.simulationMap.lifes.Count > 0)
            {
                return true;
            }
            else return false;
        }

        private void SaveButton()
        {
            
            buttonSave = new Button { Text = @"Save Data"};
            buttonSave.Location = new System.Drawing.Point(400,500);
            buttonSave.Click += new EventHandler(buttonClickSaveData);
            Controls.Add(buttonSave);
        }

        void buttonClickSaveData(object sender, EventArgs e)
        {
          
         

            saveFileDialog1.Filter = "Text File | *.txt | XML-File | *.xml | Json files (*.json)|*.json";
            saveFileDialog1.Title = "Save Statistics Data in chosen file";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                System.Windows.MessageBox.Show("Saving Canceled");

            }
            else
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                writer.Write(FormatFileText());
                writer.Dispose();
                writer.Close();
            }


        }


        public string FormatFileText()
        {
            string a = "Simulation End - Statistics@" + "-----------------------------------------" + "@@Total Time duration of simulation: " + totaltime + " @Average Speed of Cars in simulation: " + sim.simulationMap.simstats.CalculateAverageSpeed() + "@Total amount of cars in Simulation: "
                 + sim.carsAmount + "@@The coordinates of the Tile with the most crashes is " + GetIndexTile() + "@with a number of: " + numbrOfCrashedCarsInTile + " @Max Velocity in that tile was: " + maxVelocity
                 + "@@The number of cars that passed through the Tile with most incidents: " + numbrOfPassedCars + "@Duration of Delay by all cars waiting for lane "
                 + sim.simulationMap.simstats.TotalAmountOfDelay(sim.simulationMap.lifes, sim)
                 + "@Average velocity of all crashed cars:  " + avgVelocity + "@Number of All crashed cars: " + numbrOfAllCrashed
                 + "@Tile that had cars with most delay: " + indexTileDelay + "@Tile that has the highest Average velocity of cars passing through: "+ indexTileAvg  + "@As you can see on the heatmap, the lane in tile: " + GetIndexOfMostUsedTile() + " had most activity ";
            a = a.Replace("@", "" + System.Environment.NewLine);
            return a;
        }
        

        private void Graphs_Load(object sender, EventArgs e)
        {

        }

        
        
        public Tuple<int,int> GetIndexTile()
        {

            var tileWithMostCrashes = sim.simulationMap.simstats.GetTileWithMostCrashes(sim.simulationMap.tiles, sim);
            var coordinated = Utils.CoordinatesOf(sim.simulationMap.tiles, tileWithMostCrashes);
            return coordinated;

        }

        public Tuple<int, int> GetIndexOfMostUsedTile()
        {
            var t = sim.simulationMap.simstats.GetMostUsedTile(sim);
            var coordinates = Utils.CoordinatesOf(sim.simulationMap.tiles, t);
            return coordinates;
        }
        
        public Tuple<int, int> GetIndexOfDelayTile()
        {
            var tilewithmostdelay = sim.simulationMap.simstats.GetTileWithMostStops(sim.simulationMap.tiles, sim, sim.simulationMap.lifes);
            var coordinates = Utils.CoordinatesOf(sim.simulationMap.tiles, tilewithmostdelay);
            return coordinates;
        }

        public Tuple<int, int> GetIndexOfAvgTile()
        {
            var tilez = sim.simulationMap.simstats.GetTileWithHighestAvgVelocity(sim.simulationMap.tiles, sim, sim.simulationMap.lifes);
            var coordinatez = Utils.CoordinatesOf(sim.simulationMap.tiles, tilez);
            return coordinatez;
        }

        public int NrofCrashedCarsTile()
        {
            var tilez = sim.simulationMap.simstats.GetTileWithMostCrashes(sim.simulationMap.tiles, sim);
            var nr = sim.simulationMap.simstats.NumberOfCrashedCarsInTileWithMostCrashedCars(tilez, sim.simulationMap.lifes);
            return nr;
        }

        public double MaxVelocity()
        {
            var tileWithMostCrashes = sim.simulationMap.simstats.GetTileWithMostCrashes(sim.simulationMap.tiles, sim);
            if (sim.simulationMap.lifes.Count(l => !l.enabled) > 0) { var nr = sim.simulationMap.simstats.GetMaxVelocityInMostCrashedTile(tileWithMostCrashes, sim); return nr; }
            return 0;
        }

        public int nrOfPassedCars()
        {
            var tileWithMostCrashes = sim.simulationMap.simstats.GetTileWithMostCrashes(sim.simulationMap.tiles, sim);
            var nr = sim.simulationMap.simstats.NrOfCarsThatPassedThroughCrashedTile(tileWithMostCrashes, sim.simulationMap.lifes);
            return nr;
        }

        public void MaxMaxVelocity()
        {
            double nr = MaxVelocity();
            if(nr > maxVelocity)
            {
                maxVelocity = nr;
            }
            
        }

        public void AvgVelocity()
        {
            if (sim.simulationMap.lifes.Count(l => !l.enabled) > 0)
            {
                avgVelocity = sim.simulationMap.simstats.GetAverageVelocityOfAllCrashedCars(sim);}
            
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
