using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.orm.dao
{
    public class PathDao : IDao<Path>
    {
        private static RoadPositionDao _roadPositionDao = (RoadPositionDao) DaoFactory.GetByType<RoadPosition>();
        
        public Path Load(BinaryReader reader, Tile tile)
        {
            Path path = null;
            try
            {
                path = new Path();
                path.tile = tile;
                path.objectId = reader.ReadInt32();

                var pathListCount = reader.ReadInt32();
                path.path = new List<RoadPosition>();
                for (int i = 0; i < pathListCount; i++)
                {
                    var objectId = reader.ReadInt32();

                    path.path.Add(_roadPositionDao.GetByObjectId(tile, objectId));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return path;
        }

        public void Save(Path o, BinaryWriter writer)
        {
            writer.Write(o.objectId);
            
            writer.Write(o.path.Count);
            foreach (var roadPosition in o.path)
            {
                writer.Write(roadPosition.objectId);
            }
        }
        
        public Path GetByObjectId(SimulationMap simulationMap, int objectId)
        {
            return simulationMap.tiles
                .Cast<Tile>()
                .Where(tile => tile.GetPaths() != null)
                .SelectMany(tile => tile.GetPaths())
                .FirstOrDefault(path => path.objectId == objectId);
        }
    }
}