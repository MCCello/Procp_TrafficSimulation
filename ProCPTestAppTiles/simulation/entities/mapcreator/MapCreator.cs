using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform;
using ProCPTestAppTiles.forms.menustrip;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.mapcreator.board;
using ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.simulation.entities.mapcreator
{
    public class MapCreator : Attachable<MapCreatorControl>, ISaveable
    {
        private static MapCreatorDao _mapCreatorDao = (MapCreatorDao) DaoFactory.GetByType<MapCreator>();
        
        public SimMenuStrip simMenuStrip { get; set; }
        public TileBlockBoard tileBlockBoard { get; set; }
        public Board board { get; set; }
        public Button startButton { get; set; }
        public ListBox lbStats { get; set; }
        public Simulation simulation { get; set; }
        public TextBox numberOfCars { get; set; }
        public Label numberOfCarsLbl { get; set; }
        
        public static RoadType selectedRoadType { get; set; }

        public MapCreator()
        {
        }
        
        public MapCreator(Control mommyControl, Point location) : base(mommyControl, location)
        {
            Init();
        }

        protected override void InitTControl()
        {
            control = new MapCreatorControl(this);
        }


        /// <summary>
        /// Initialises MapCreator and Populates Panels within it.
        /// </summary>
        public override void Init()
        {
            Form form = mommyControl as Form;

            if (form != null)
            {
                Size = form.Size;
            }

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


            // TileBlockBoard
            tileBlockBoard = new TileBlockBoard(GetControl(), new Point(x, y));
            x += tileBlockBoard.Size.Width;


            // Gap Between TileBlockBoard and Board
            x += MapCreatorConstants.TILE_BLOCK_BOARD_X_BOARD_GAP;


            // Board
            if (board == null)
            {
                board = new Board(GetControl(), new Point(x, y));
            }
            x += board.Size.Width;

            
            // Gap Between board and Start Button
            x += MapCreatorConstants.BOARD_X_START_BUTTON_GAP;
            
            
            // Start Button
            startButton = new Button {Size = new Size(80, 80), Text = @"Start Simulation", Location = new Point(x, y)};
            AddControl(startButton);
            startButton.Click += GetControl().startButton_Clicked;
            x += startButton.Width;

            //label textbox
            y += startButton.Height + 10;
            numberOfCarsLbl = new Label { Location = new Point(x - startButton.Width, y), Text = "Number of cars to be simulated:", AutoSize = true };
            AddControl(numberOfCarsLbl);

            //textbox number of cars
            y += numberOfCarsLbl.Height + 10;
            numberOfCars = new TextBox { Size = new Size(100, 20), Location = new Point(x-startButton.Width, y)};
            AddControl(numberOfCars);

            // Determine Size of MapCreatorControl
            UpdateSize();
        }

        public void Save(BinaryWriter writer)
        {
            _mapCreatorDao.Save(this, writer);
        }
        
        
        public void CreateSimulation()
        {
            Form form = new Form();
            simulation = new Simulation(this, form, Point.Empty);
            
            if (!board.IsValid())
            {
                MessageBox.Show(@"Current Map Configuration is not correct");
                return;
            }

            form.Size = Utils.GetCorrectSize(form);
            form.AutoSize = true;
            form.Show();
            form.FormClosing += simulation.GetControl().Form_Closing;
            
            simulation.AddAmountOfCars(simulation.GetCarsToSimulate());
            simulation.Start();
            
            // Statistics Graphics
            var graphs = new Graphs(simulation);
            simulation.simulationMap.graphs = graphs;
            graphs.StartPosition = FormStartPosition.Manual;
            graphs.Location = new Point(simulation.simulationMap.pictureBox.Right+500, simulation.simulationMap.pictureBox.Bottom-300);
            graphs.Show();
        }
    }
}