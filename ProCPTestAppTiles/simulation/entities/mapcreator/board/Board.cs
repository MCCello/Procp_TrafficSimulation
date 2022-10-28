using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.board;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils.tile;

namespace ProCPTestAppTiles.simulation.entities.mapcreator.board
{
    public class Board : Attachable<BoardControl>, ISaveable
    {
        private static BoardDao _boardDao = (BoardDao) DaoFactory.GetByType<Board>();

        public Tile[,] tiles { get; set; }

        public Board()
        {
        }
        
        public Board(Control mommyControl, Point location) : base(mommyControl, location)
        {
            Init();
        }
        
        protected override void InitTControl()
        {
            control = new BoardControl(this);
        }

        /// <summary>
        /// Initialises Board and populates Tiles within.
        /// </summary>
        public override void Init()
        {
            var x = 0;
            var y = 0;
            
            // Populate and Place Tiles
            if (tiles == null)
            {
                tiles = new Tile[BoardConstants.V_GRID_TILES, BoardConstants.H_GRID_TILES];
                for (var i = 0; i < tiles.GetLength(0); i++)
                {
                    for (var j = 0; j < tiles.GetLength(1); j++)
                    {
                        Tile tile = new Tile(GetControl(), new Point(x, y));
                        tiles[i, j] = tile;

                        tile.GetControl().MouseDown += GetControl().TileMouseDown;

                        x += TileConstants.TILE_WIDTH;
                    }

                    y += TileConstants.TILE_HEIGHT;
                    x = 0;
                }
            }

            // Determine Size of Board
            UpdateSize();
        }

        /// <summary>
        /// Saves the Board using the ORM.
        /// </summary>
        public void Save(BinaryWriter writer)
        {
            _boardDao.Save(this, writer);
        }

        /// <summary>
        /// Checks if the tile placement in the current board is valid for simulation.
        /// </summary>
        /// <returns>Return true if valid, false otherwise</returns>
        public bool IsValid()
        {
            var isFilled = tiles.Cast<Tile>().Any(tile => tile?.roadType != null);

            if (!isFilled)
            {
                return false;
            }
            
            foreach (var tile in tiles)
            {
                if (tile?.roadType == null)
                {
                    continue;
                }
                foreach (var allConnectedTile in TileUtils.GetAllConnectedTiles(tiles, tile))
                {
                    if (allConnectedTile?.roadType == null)
                    {
                        return false;
                    }
                    if (!TileUtils.GetAllConnectedTiles(tiles, allConnectedTile).Contains(tile))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}