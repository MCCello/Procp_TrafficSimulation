using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.orm.dao
{
    public class SimulationMapDao : IDao<SimulationMap>
    {
        private static TileDao _tileDao = (TileDao) DaoFactory.GetByType<Tile>();
        private static LifeDao _lifeDao = (LifeDao) DaoFactory.GetByType<Life>();
        private static PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        private static RoadPositionDao _roadPositionDao = (RoadPositionDao) DaoFactory.GetByType<RoadPosition>();
        
        public SimulationMap Load(BinaryReader reader, Simulation simulation)
        {
            SimulationMap simulationMap = null;
            try
            {
                simulationMap = new SimulationMap
                {
                    Location = new Point(reader.ReadInt32(), reader.ReadInt32()),
                    Size = new Size(reader.ReadInt32(), reader.ReadInt32())
                };

                simulationMap.tiles = new Tile[reader.ReadInt32(), reader.ReadInt32()];
                simulationMap.Init();

                // Tiles
                for (var i = 0; i < simulationMap.tiles.GetLength(0); i++)
                {
                    for (var j = 0; j < simulationMap.tiles.GetLength(1); j++)
                    {
                        Tile tile = _tileDao.Load(reader);
                        simulationMap.tiles[i, j] = tile;
                        tile.AttachTo(simulationMap.GetControl());
                    }
                }

                // Lifes
                var lifesCount = reader.ReadInt32();
                for (int i = 0; i < lifesCount; i++)
                {
                    var life = _lifeDao.Load(reader);
                    if (life == null)
                    {
                        Debug.WriteLine("Life is null");
                        continue;
                    }
                    
                    simulationMap.AddLife(life);
                }
                
                // Link References
                foreach (var life in simulationMap.lifes)
                {
                    life.curRoadPosition = life.curRoadPosition.objectId == -1 ? null : _roadPositionDao.GetByObjectId(simulationMap, life.curRoadPosition.objectId);
                    life.currentPath = life.currentPath.objectId == -1 ? null : _pathDao.GetByObjectId(simulationMap, life.currentPath.objectId);
                    life.endingPath = life.endingPath.objectId == -1 ? null : _pathDao.GetByObjectId(simulationMap, life.endingPath.objectId);
                }
                foreach (var tile in simulationMap.tiles)
                {
                    foreach (var roadPosition in tile.GetRoadPositionGrid() ?? new List<RoadPosition>())
                    {
                        roadPosition.oldLife = roadPosition.oldLife.objectId == -1 ? null : _lifeDao.GetByObjectId(simulationMap, roadPosition.oldLife.objectId);
                        roadPosition.newLife = roadPosition.newLife.objectId == -1 ? null : _lifeDao.GetByObjectId(simulationMap, roadPosition.newLife.objectId);
                    }
                }
                foreach (var tile in simulationMap.tiles)
                {
                    foreach (var trafficLight in tile.GetAllTrafficLights())
                    {
                        simulationMap.drawables.Add(trafficLight);
                    }
                }


                simulationMap.pictureBox.Image = simulationMap.CreateMapImageFromTiles();
                simulation.simulationMap = simulationMap;
                simulationMap.AttachTo(simulation.GetControl());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return simulationMap;
        }

        public void Save(SimulationMap o, BinaryWriter writer)
        {
            // Control
                // Location
            writer.Write(o.Location.X);
            writer.Write(o.Location.Y);
            
                // Size
            writer.Write(o.Size.Height);
            writer.Write(o.Size.Width);
            
            
            // SimulationMap
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

                // Lifes
            writer.Write(o.lifes.Count);
            foreach (var life in o.lifes)
            {
                life.Save(writer);
            }
        }
    }
}