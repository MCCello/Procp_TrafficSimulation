using System.IO;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace ProCPTestAppTiles.orm
{
    public static class ORMManager
    {
        /// <summary>
        /// Saves the created map on the board.
        /// </summary>
        /// <param name="mapCreator"></param>
        public static void SaveMapCreator(MapCreator mapCreator, string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                mapCreator.Save(writer);
            }
        }

        public static MapCreator LoadMapCreator(string filename)
        {
            MapCreator mapCreator = null;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                MapCreatorDao _mapCreatorDao = (MapCreatorDao) DaoFactory.GetByType<MapCreator>();
                mapCreator = _mapCreatorDao.Load(reader);
            }

            return mapCreator;
        }

        
        
        public static void SaveSimulation(Simulation simulation, string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                simulation.Save(writer);
            }
        }

        public static Simulation LoadSimulation(string filename)
        {
            Simulation simulation = null;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                SimulationDao _simulationDao = (SimulationDao) DaoFactory.GetByType<Simulation>();
                simulation = _simulationDao.Load(reader);
            }

            return simulation;
        }
    }
}