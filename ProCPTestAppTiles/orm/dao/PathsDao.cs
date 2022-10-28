using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.orm.dao
{
    public class PathsDao : IDao<Paths>
    {
        private static PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        
        
        public Paths Load(BinaryReader reader, Tile tile)
        {
            Paths paths = null;
            try
            {
                paths = new Paths();

                paths.pathsDict = new Dictionary<int, List<Path>>();
                var dictCount = reader.ReadInt32();
                for (int i = 0; i < dictCount; i++)
                {
                    var laneCount = reader.ReadInt32();

                    var pathListCount = reader.ReadInt32();
                    var pathList = new List<Path>();
                    for (int j = 0; j < pathListCount; j++)
                    {
                        pathList.Add(_pathDao.Load(reader, tile));
                    }

                    paths.pathsDict[laneCount] = pathList;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return paths;
        }

        public void Save(Paths o, BinaryWriter writer)
        {
            writer.Write(o.pathsDict.Count);
            foreach (var keyValuePair in o.pathsDict)
            {
                var laneCount = keyValuePair.Key;
                var pathList = keyValuePair.Value;

                writer.Write(laneCount);
                
                writer.Write(pathList.Count);
                foreach (var path in pathList)
                {
                    path.Save(writer);
                }
            }
        }
    }
}