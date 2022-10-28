using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace ProCPTestAppTiles.orm.dao
{
    public class RoadPositionDao : IDao<RoadPosition>
    {
        private static TrafficLightDao _trafficLightDao = (TrafficLightDao) DaoFactory.GetByType<TrafficLight>();

        public RoadPosition Load(BinaryReader reader, Tile tile)
        {
            RoadPosition roadPosition = null;
            try
            {
                roadPosition = new RoadPosition((int) reader.ReadDouble(), (int) reader.ReadDouble());
                roadPosition.objectId = reader.ReadInt32();
                
                if (reader.ReadBoolean())
                {
                    roadPosition.TrafficLight = _trafficLightDao.Load(reader, roadPosition);
                }
                
                roadPosition.oldLife = new Car {objectId = reader.ReadInt32()};
                roadPosition.newLife = new Car {objectId = reader.ReadInt32()};
                
                roadPosition.counter = reader.ReadInt32();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return roadPosition;
        }

        public void Save(RoadPosition o, BinaryWriter writer)
        {
            writer.Write(o.position.X);
            writer.Write(o.position.Y);
            
            writer.Write(o.objectId);

            writer.Write(o.TrafficLight != null);
            o.TrafficLight?.Save(writer);
            
            writer.Write(o.oldLife?.objectId ?? -1);
            writer.Write(o.newLife?.objectId ?? -1);
            
            writer.Write(o.counter);
        }

        public RoadPosition GetByObjectId(Tile tile, int objectId)
        {
            return tile.GetRoadPositionGrid().FirstOrDefault(r => r.objectId == objectId);
        }
        
        public RoadPosition GetByObjectId(SimulationMap simulationMap, int objectId)
        {
            return simulationMap.tiles
                .Cast<Tile>()
                .Where(tile => tile.GetRoadPositionGrid() != null)
                .SelectMany(tile => tile.GetRoadPositionGrid())
                .FirstOrDefault(roadPosition => roadPosition.objectId == objectId);
        }
    }
}