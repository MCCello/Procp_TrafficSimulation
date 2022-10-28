using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.mapcreator.board;

namespace ProCPTestAppTiles.orm.dao
{
    public class MapCreatorDao : IDao<MapCreator>
    {
        private static BoardDao _boardDao = (BoardDao) DaoFactory.GetByType<Board>();
        
        public MapCreator Load(BinaryReader reader)
        {
            MapCreator mapCreator = null;
            try
            {
                mapCreator = new MapCreator
                {
                    Location = new Point(reader.ReadInt32(), reader.ReadInt32()),
                    Size = new Size(reader.ReadInt32(), reader.ReadInt32())
                };


                // MapCreator
                    // Board
                _boardDao.Load(reader, mapCreator);

                mapCreator.Init();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return mapCreator;
        }

        public void Save(MapCreator o, BinaryWriter writer)
        {
            // Control
                // Location
            writer.Write(o.Location.X);
            writer.Write(o.Location.Y);
            
                // Size
            writer.Write(o.Size.Height);
            writer.Write(o.Size.Width);
            
            
            // MapCreator
                // Board
            o.board.Save(writer);
            
                // BlockTileBoard 
            // No need to save it. Constant 4 tiles.
        }
    }
}