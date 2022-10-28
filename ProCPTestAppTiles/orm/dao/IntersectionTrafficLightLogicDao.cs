using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;

namespace ProCPTestAppTiles.orm.dao
{
    public class IntersectionTrafficLightLogicDao : IDao<IntersectionTrafficLightLogic>
    {
        private static TrafficLightDao _trafficLightDao = (TrafficLightDao) DaoFactory.GetByType<TrafficLight>();
        
        public IntersectionTrafficLightLogic Load(BinaryReader reader, Tile tile)
        {
            IntersectionTrafficLightLogic intersectionTrafficLightLogic = null;
            try
            {
                intersectionTrafficLightLogic = new IntersectionTrafficLightLogic();

                intersectionTrafficLightLogic.cursor = reader.ReadInt32();

                var trafficLightSequenceCount = reader.ReadInt32();
                intersectionTrafficLightLogic.trafficLightSequence = new List<List<TrafficLight>>();
                for (int i = 0; i < trafficLightSequenceCount; i++)
                {
                    var trafficLightsCount = reader.ReadInt32();
                    var trafficLights = new List<TrafficLight>();
                    for (int j = 0; j < trafficLightsCount; j++)
                    {
                        var objectId = reader.ReadInt32();
                        trafficLights.Add(_trafficLightDao.GetByObjectId(tile, objectId));
                    }
                    
                    intersectionTrafficLightLogic.trafficLightSequence.Add(trafficLights);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return intersectionTrafficLightLogic;
        }

        public void Save(IntersectionTrafficLightLogic o, BinaryWriter writer)
        {
            writer.Write(o.cursor);
            
            writer.Write(o.trafficLightSequence.Count);
            foreach (var trafficLights in o.trafficLightSequence)
            {
                writer.Write(trafficLights.Count);
                foreach (var trafficLight in trafficLights)
                {
                    writer.Write(trafficLight.objectId);
                }
            }
        }
    }
}