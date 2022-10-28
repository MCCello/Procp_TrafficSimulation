using System;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.tileblockboard;
using ProCPTestAppTiles.forms.mapcreatorform.tileblockboard.tileblocks;
using ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard.tileblock;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard
{
    public class TileBlockBoard : Attachable<TileBlockBoardControl>
    {
        public TileBlockBoard()
        {
            Init();
        }

        public TileBlockBoard(Control mommyControl, Point location) : base(mommyControl, location)
        {
            Init();
        }

        protected override void InitTControl()
        {
            control = new TileBlockBoardControl {tileBlockBoard = this};
        }

        /// <summary>
        /// Initialises the TileBlockBoard and Populates it.
        /// </summary>
        public override void Init()
        {
            var x = 0;
            var y = 0;
            
            // Populate and Place TileBlocks
            var vals = Enum.GetValues(typeof(RoadType));
            foreach (RoadType roadType in vals)
            {
                TileBlock tileBlock = new TileBlock(roadType, GetControl(), new Point(x, y));
                tileBlock.GetControl().MouseDown += GetControl().TileBlockMouseDown;
                
                y += TileBlockConstants.TILE_BLOCK_HEIGHT;
            }
            
            // Determine Size of TileBlockBoard
            UpdateSize();
        }

        /// <summary>
        /// Unhighlights all TileBlocks within the TileBlockBoardControl.
        /// </summary>
        public void ResetAllHighlights()
        {
            foreach (var control in GetControl().Controls)
            {
                if (control is TileBlockControl blockControl)
                {
                    blockControl.UnHighlight();
                }
            }
        }
    }
}