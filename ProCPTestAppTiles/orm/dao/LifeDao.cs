using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.simulation.entities;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.orm.dao
{
    public class LifeDao : IDao<Life>
    {
        private static PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        
        public Life Load(BinaryReader reader)
        {
            Life life = null;
            try
            {
                life = new Car();
                
                life.objectId = reader.ReadInt32();

                life.curRoadPosition = new RoadPosition(0, 0) {objectId = reader.ReadInt32()};
                
                life.currentPath = new Path {objectId = reader.ReadInt32()};
                life.endingPath = new Path {objectId = reader.ReadInt32()};
                
                life.velocity = reader.ReadDouble();
                life.distanceTraveled = reader.ReadDouble();
                life.enabled = reader.ReadBoolean();

                var vPositionsCount = reader.ReadInt32();
                life.vPositions = new List<Position>();
                for (int i = 0; i < vPositionsCount; i++)
                {
                    life.vPositions.Add(new Position(reader.ReadDouble(), reader.ReadDouble()));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return life;
        }

        public void Save(Life o, BinaryWriter writer)
        {
            writer.Write(o.objectId);
            
            writer.Write(o.curRoadPosition?.objectId ?? -1);
            
            writer.Write(o.currentPath?.objectId ?? -1);
            writer.Write(o.endingPath?.objectId ?? -1);
            
            writer.Write(o.velocity);
            writer.Write(o.distanceTraveled);
            writer.Write(o.enabled);
            
            writer.Write(o.vPositions.Count);
            foreach (var vPosition in o.vPositions)
            {
                writer.Write(vPosition.X);
                writer.Write(vPosition.Y);
            }
        }

        public Life GetByObjectId(SimulationMap simulationMap, int objectId)
        {
            return (
                from Tile tile in simulationMap.tiles 
                where tile.GetRoadPositionGrid() != null
                from roadPosition in tile.GetRoadPositionGrid() 
                where roadPosition.HasLife() && roadPosition.oldLife.objectId == objectId 
                select roadPosition.oldLife).FirstOrDefault();
        }
    }
}