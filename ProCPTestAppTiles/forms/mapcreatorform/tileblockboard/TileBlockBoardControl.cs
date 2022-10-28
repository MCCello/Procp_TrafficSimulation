using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.tileblockboard.tileblocks;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard;

namespace ProCPTestAppTiles.forms.mapcreatorform.tileblockboard
{
    public class TileBlockBoardControl : Control
    {
        //Properties
        public TileBlockBoard tileBlockBoard { get; set; }

        /// <summary>
        /// control for user to click on tiles 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TileBlockMouseDown(object sender, MouseEventArgs e)
        {
            var tileBlockControl = sender as TileBlockControl;
            if (tileBlockControl == null)
            {
                return;
            }
            
            MapCreator.selectedRoadType = tileBlockControl.GetLogic().roadType;
            tileBlockBoard.ResetAllHighlights();
            tileBlockControl.Highlight();
        }
    }
}