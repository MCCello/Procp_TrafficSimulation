using System.Drawing;
using ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard.tileblock;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.forms.mapcreatorform.tileblockboard.tileblocks
{
    public class TileBlockControl : ButtonControllable<TileBlock>
    {
        public TileBlockControl(TileBlock logic) : base(logic)
        {
        }

        /// <summary>
        /// highlight tile control
        /// </summary>
        public void Highlight()
        {
            Image = Utils.AdjustBrightness((Bitmap) Image, -69);
        }
        
        /// <summary>
        /// un-highlight tile control
        /// </summary>
        public void UnHighlight()
        {
            Image = new Bitmap(GetLogic().oImage);
        }
    }
}