using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.entities.mapcreator.board
{
    public class BoardConstants
    {
        public const int H_GRID_TILES = 5; // Amount of Tiles Horizontally
        public const int V_GRID_TILES = 4; // Amount of Tiles Vertically

        public const Keys ROTATE_CW = Keys.Shift;
        public const Keys ROTATE_CCW = Keys.Control;
        public const Keys CLEAR = Keys.Shift & Keys.Control;
    }
}