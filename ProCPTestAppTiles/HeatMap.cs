using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.heatmap;
using ProCPTestAppTiles.simulation.entities.simulation;

namespace ProCPTestAppTiles
{
    public partial class HeatMap : Form
    {
        int width;
        int height;
        ColorRamp heatType;
        List<HeatPoint> points;
        public static int HEAT_POINT_RADIUS = 15;
        public static float HEAT_POINT_OPACITY = 0.6f;
        public Simulation simulation;

        public HeatMap(Simulation simulation)
        {
            InitializeComponent();
            this.simulation = simulation;
            BackgroundImage = simulation.simulationMap.pictureBox.Image;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Location = new Point();
            Size = simulation.simulationMap.Size;
        }

        List<HeatPoint> RandomPoints()
        {
            var result = new List<HeatPoint>();

            var simulationMap = simulation.simulationMap;
            if (simulationMap == null)
            {
                return result;
            }

            var tiles = simulationMap.tiles;
            if (tiles == null)
            {
                return result;
            }

            foreach (var tile in tiles)
            {
                var grid = tile.GetRoadPositionGrid();
                if (grid == null)
                {
                    continue;
                }
                
                foreach (var roadPosition in grid)
                {
                    if (roadPosition.counter <= 0)
                    {
                        continue;
                    }
                    var pos = roadPosition.position;
                    var point = new HeatPoint
                    {
                        X = (float) pos.X,
                        Y = (float) pos.Y,
                        W = 1
                    };
                    var counter = roadPosition.counter;
                    var amountOfLifes = simulation.lifeSize;
                    var visitR = (float) counter / amountOfLifes * 100f;
                    for (int i = 0; i < visitR / 10; i++)
                    {
                        result.Add(point);
                    }
                }
            }

            return result;
        }

        private async void make4Maps(int width, int height, int radius, List<HeatPoint> points, float opacity, ColorRamp cr)
        {
            var hmMaker = new HeatMapMaker
            {
                Width = width,
                Height = height,
                Radius = radius,
                ColorRamp = cr,
                HeatPoints = points,
                Opacity = opacity
            };
            pictureBox1.BackgroundImage = await hmMaker.MakeHeatMap();
        }

        private void HeatMap_Load_1(object sender, EventArgs e)
        {
            width = pictureBox1.Width;
            height = pictureBox1.Height;
            points = RandomPoints();
            heatType = ColorRamp.RAINBOW;
            make4Maps(width, height, HEAT_POINT_RADIUS, points, HEAT_POINT_OPACITY, heatType);
        }
    }
}
