using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.mapcreator.board;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;

namespace ProCPTestAppTiles.orm.dao
{
    public class BoardDao : IDao<Board>
    {
        private static TileDao _tileDao = (TileDao) DaoFactory.GetByType<Tile>();
        
        /// <summary>
        /// Loads a saved board.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Board Load(BinaryReader reader, MapCreator mapCreator)
        {
            Board board = null;
            try
            {
                board = new Board
                {
                    Location = new Point(reader.ReadInt32(), reader.ReadInt32()),
                    Size = new Size(reader.ReadInt32(), reader.ReadInt32())
                };

                
                // Board
                board.tiles = new Tile[reader.ReadInt32(), reader.ReadInt32()];
                for (int i = 0; i < board.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < board.tiles.GetLength(1); j++)
                    {
                        Tile tile = _tileDao.Load(reader);
                        board.tiles[i, j] = tile;
                        tile.GetControl().MouseDown += board.GetControl().TileMouseDown;
                        tile.AttachTo(board.GetControl());
                    }
                }
                
                board.Init();
                
                mapCreator.board = board;
                board.AttachTo(mapCreator.GetControl());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return board;
        }


        /// <summary>
        /// Saves a board.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="writer"></param>
        public void Save(Board o, BinaryWriter writer)
        {
            // Control
                // Location
            writer.Write(o.Location.X);
            writer.Write(o.Location.Y);
            
                // Size
            writer.Write(o.Size.Height);
            writer.Write(o.Size.Width);
            
            
            // Board
                // Tiles
            writer.Write(o.tiles.GetLength(0));
            writer.Write(o.tiles.GetLength(1));
            for (int i = 0; i < o.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < o.tiles.GetLength(1); j++)
                {
                    Tile tile = o.tiles[i, j];
                    tile.Save(writer);
                }
            }
        }
    }
}