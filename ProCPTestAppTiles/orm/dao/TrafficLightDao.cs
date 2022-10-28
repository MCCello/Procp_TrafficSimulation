using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;

namespace ProCPTestAppTiles.orm.dao
{
    public class TrafficLightDao : IDao<TrafficLight>
    {
        public TrafficLight Load(BinaryReader reader, RoadPosition roadPosition)
        {
            TrafficLight trafficLight = null;
            try
            {
                trafficLight = new TrafficLight();
                trafficLight.objectId = reader.ReadInt32();
                trafficLight.seqNo = reader.ReadInt32();
                trafficLight.SetRoadPosition(roadPosition);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return trafficLight;
        }

        public void Save(TrafficLight o, BinaryWriter writer)
        {
            writer.Write(o.objectId);
            writer.Write(o.seqNo);
        }

        public TrafficLight GetByObjectId(Tile tile, int objectId)
        {
            return tile.GetRoadPositionGrid()?.Where(r => r.TrafficLight != null && r.TrafficLight.objectId == objectId).Select(r => r.TrafficLight).FirstOrDefault();
        }
    }
}