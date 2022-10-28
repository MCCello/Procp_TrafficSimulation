using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator;

namespace ProCPTestAppTiles
{
    public partial class Form1 : Form
    {
        public MapCreator mapCreator { get; set; }
        
        public Form1()
        {
            InitializeComponent();
            mapCreator = new MapCreator(this, new Point());
            mapCreator.AttachTo(this);
        }
    }
}