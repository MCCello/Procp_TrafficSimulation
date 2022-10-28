using System;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform;
using ProCPTestAppTiles.forms.simulationform;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.simulation;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.simulation;

namespace ProCPTestAppTiles.forms.menustrip
{
    public class SimMenuStrip : MenuStrip, IAttachable
    {
        //Properties
        public Control mommyControl { get; set; }
        
        private ToolStripMenuItem fileToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem saveMapToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem loadMapToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem saveSimulationToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem loadSimulationToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem helpToolStripMenuItem = new ToolStripMenuItem();
        //Constructors
        public SimMenuStrip(Control mommyControl)
        {
            this.mommyControl = mommyControl;
            InitSimMenuStrip();
        }

       /// <summary>
       /// Control method 
       /// </summary>
       /// <param name="mommyControl"></param>
        public void AttachTo(Control mommyControl)
        {
            this.mommyControl = mommyControl;
            mommyControl.Controls.Add(this);
        }
        /// <summary>
        /// Control detach
        /// </summary>
        public void DetachFrom()
        {
            mommyControl.Controls.Remove(this);
            mommyControl = null;
        }

        /// <summary>
        /// Initializes menu strip of simulation and populates it with "File" "Help" "Save map" "Load Map"
        /// </summary>
        private void InitSimMenuStrip()
        {
            SuspendLayout();
            
            // Menu Strip
            Items.AddRange(new ToolStripItem[]
            {
                fileToolStripMenuItem, helpToolStripMenuItem
            });
            Location = new Point(0, 0);
            Size = new Size(250, 24);
            TabIndex = 1;
            
            // File ToolStripMenuItem
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { });
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = @"File";
            
            // Help ToolStripMenu Item
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = @"Help";
            
            // saveMapToolStripMenuItem
            if (mommyControl is MapCreatorControl)
            {
                fileToolStripMenuItem.DropDownItems.Add(saveMapToolStripMenuItem);
                saveMapToolStripMenuItem.Size = new Size(152, 22);
                saveMapToolStripMenuItem.Text = @"Save Map";
                saveMapToolStripMenuItem.Click += HandleSaveMap;
            }

            // loadMapToolStripMenuItem
            fileToolStripMenuItem.DropDownItems.Add(loadMapToolStripMenuItem);
            loadMapToolStripMenuItem.Size = new Size(152, 22);
            loadMapToolStripMenuItem.Text = @"Load Map";
            loadMapToolStripMenuItem.Click += HandleLoadMap;
            
            // saveSimulationToolStripMenuItem
            if (mommyControl is SimulationControl)
            {
                fileToolStripMenuItem.DropDownItems.Add(saveSimulationToolStripMenuItem);
                saveSimulationToolStripMenuItem.Size = new Size(152, 22);
                saveSimulationToolStripMenuItem.Text = @"Save Simulation";
                saveSimulationToolStripMenuItem.Click += HandleSaveSimulation;
            }
            
            // loadSimulationToolStripMenuItem
            fileToolStripMenuItem.DropDownItems.Add(loadSimulationToolStripMenuItem);
            loadSimulationToolStripMenuItem.Size = new Size(152, 22);
            loadSimulationToolStripMenuItem.Text = @"Load Simulation";
            loadSimulationToolStripMenuItem.Click += HandleLoadSimulation;

            ResumeLayout(false);
            PerformLayout();
        }
        
        

        private void HandleSaveSimulation(object sender, EventArgs e)
        {
            var simulationControl = mommyControl as SimulationControl;
            var simulation = simulationControl?.GetLogic();
            if (simulation == null)
            {
                return;
            }
            using (var d = new SaveFileDialog())
            {
                d.Title = @"Save Simulation Map";
                
                d.ShowDialog();

                if (d.FileName != "")
                {
                    ORMManager.SaveSimulation(simulation, d.FileName);
                }
            }
        }
        
        private void HandleLoadSimulation(object sender, EventArgs e)
        {
            using (var d = new OpenFileDialog())
            {
                d.Title = @"Open Simulation Map";
                
                d.ShowDialog();

                if (d.FileName != "")
                {
                    var form = new Form();
                    var simulation = ORMManager.LoadSimulation(d.FileName);
                    simulation.AttachTo(form);
                    simulation.Start();

                    form.AutoSize = true;
                    form.Show();

                    var graphs = new Graphs(simulation)
                    {
                        StartPosition = FormStartPosition.Manual,
                        Location = new Point(simulation.simulationMap.pictureBox.Right + 500,
                            simulation.simulationMap.pictureBox.Bottom - 300)
                    };
                    graphs.AutoSize = true;
                    graphs.Show();
                }
            }
        }

        /// <summary>
        /// Opens save file dialog to handle saving maps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSaveMap(object sender, EventArgs e)
        {
            var mapCreatorControl = mommyControl as MapCreatorControl;
            var mapCreator = mapCreatorControl?.GetLogic();
            if (mapCreator == null)
            {
                return;
            }
            using (var d = new SaveFileDialog())
            {
                d.Title = @"Save Simulation Map";
                
                d.ShowDialog();

                if (d.FileName != "")
                {
                    ORMManager.SaveMapCreator(mapCreator, d.FileName);
                }
            }
        }

        /// <summary>
        /// Using File dialog open/load map.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleLoadMap(object sender, EventArgs e)
        {
            MapCreator mc = null;
            switch (mommyControl)
            {
                case MapCreatorControl control:
                    mc = control.GetLogic();
                    if (mc == null)
                    {
                        return;
                    }
                    break;
                case SimulationControl control:
                    var simulation = control.GetLogic();
                    if (simulation == null)
                    {
                        return;
                    }
                    mc = simulation.mapCreator;
                    break;
            }

            if (mc == null)
            {
                return;
            }
            
            using (var d = new OpenFileDialog())
            {
                d.Title = "Open Simulation Map";
                
                d.ShowDialog();

                if (d.FileName != "")
                {
                    var mommyControl = mc.mommyControl;
                    mc.DetachFrom();
                    mc = ORMManager.LoadMapCreator(d.FileName);
                    mc.AttachTo(mommyControl);
                }
            }
        }
    }
}