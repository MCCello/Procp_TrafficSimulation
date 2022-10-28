using System;
using ProCPTestAppTiles.simulation;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.mapcreator.board;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace ProCPTestAppTiles.orm.dao
{
    public static class DaoFactory
    {
        private static IDao<Board> _boardDao;
        private static IDao<Tile> _tileDao;
        private static IDao<MapCreator> _mapCreatorDao;
        private static IDao<Simulation> _simulationDao;
        private static IDao<SimulationMap> _simulationMapDao;
        private static IDao<TrafficLight> _trafficLightDao;
        private static IDao<RoadPosition> _roadPositionDao;
        private static IDao<Life> _lifeDao;
        private static IDao<RoadGrid> _roadGridDao;
        private static IDao<Paths> _pathsDao;
        private static IDao<Path> _pathDao;
        private static IDao<IntersectionTrafficLightLogic> _intersectionTrafficLightLogicDao;
        private static IDao<Queue> _queueDao;

        public static IDao<T> GetByType<T>() where T : ISaveable
        {
            Type type = typeof(T);
            
            // Tile
            if (type == typeof(Tile))
            {
                if (_tileDao == null)
                {
                    _tileDao = new TileDao();
                }

                return (IDao<T>) _tileDao;
            }
            
            // Board
            if (type == typeof(Board))
            {
                if (_boardDao == null)
                {
                    _boardDao = new BoardDao();
                }

                return (IDao<T>) _boardDao;
            }

            // MapCreator
            if (type == typeof(MapCreator))
            {
                if (_mapCreatorDao == null)
                {
                    _mapCreatorDao = new MapCreatorDao();
                }

                return (IDao<T>) _mapCreatorDao;
            }
            
            // Simulation
            if (type == typeof(Simulation))
            {
                if (_simulationDao == null)
                {
                    _simulationDao = new SimulationDao();
                }

                return (IDao<T>) _simulationDao;
            }
            
            // SimulationMap
            if (type == typeof(SimulationMap))
            {
                if (_simulationMapDao == null)
                {
                    _simulationMapDao = new SimulationMapDao();
                }

                return (IDao<T>) _simulationMapDao;
            }
            
            // TrafficLight
            if (type == typeof(TrafficLight))
            {
                if (_trafficLightDao == null)
                {
                    _trafficLightDao = new TrafficLightDao();
                }

                return (IDao<T>) _trafficLightDao;
            }
            
            // IntersectionTrafficLightLogic
            if (type == typeof(IntersectionTrafficLightLogic))
            {
                if (_intersectionTrafficLightLogicDao == null)
                {
                    _intersectionTrafficLightLogicDao = new IntersectionTrafficLightLogicDao();
                }

                return (IDao<T>) _intersectionTrafficLightLogicDao;
            }
            
            // RoadPosition
            if (type == typeof(RoadPosition))
            {
                if (_roadPositionDao == null)
                {
                    _roadPositionDao = new RoadPositionDao();
                }

                return (IDao<T>) _roadPositionDao;
            }
            
            // Life
            if (type == typeof(Life))
            {
                if (_lifeDao == null)
                {
                    _lifeDao = new LifeDao();
                }

                return (IDao<T>) _lifeDao;
            }
            
            // RoadGrid
            if (type == typeof(RoadGrid))
            {
                if (_roadGridDao == null)
                {
                    _roadGridDao = new RoadGridDao();
                }

                return (IDao<T>) _roadGridDao;
            }
            
            // Paths
            if (type == typeof(Paths))
            {
                if (_pathsDao == null)
                {
                    _pathsDao = new PathsDao();
                }

                return (IDao<T>) _pathsDao;
            }
            
            // Path
            if (type == typeof(Path))
            {
                if (_pathDao == null)
                {
                    _pathDao = new PathDao();
                }

                return (IDao<T>) _pathDao;
            }
            
            // Queue
            if (type == typeof(Queue))
            {
                if (_queueDao == null)
                {
                    _queueDao = new QueueDao();
                }

                return (IDao<T>) _queueDao;
            }
            
            
            // Not Supported Dao
            return null;
        }
    }
}