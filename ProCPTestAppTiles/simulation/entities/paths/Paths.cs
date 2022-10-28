using System.Collections.Generic;
using System.IO;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.simulation.entities.paths
{
    public class Paths : ISaveable
    {
        private static PathsDao _pathsDao = (PathsDao) DaoFactory.GetByType<Paths>();
        
        public Dictionary<int, List<Path>> pathsDict { get; set; } // <Lane, List<Path>>

        public Paths()
        {
        }

        public Paths(Dictionary<int, List<Path>> pathsDict)
        {
            this.pathsDict = pathsDict;
        }
        
        public List<Path> GetPathsByLane(int lane)
        {
            return Utils.GetValueOrDefault(pathsDict, lane, null);
        }

        public void Save(BinaryWriter writer)
        {
            _pathsDao.Save(this, writer);
        }
    }
}