using System.Drawing;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.mapcreatorform.board.tiles
{
    public class TileControl : ButtonControllable<Tile>
    {
        public TileControl(Tile logic) : base(logic)
        {
            Size = new Size(TileConstants.TILE_WIDTH, TileConstants.TILE_HEIGHT);
        }
    }
}