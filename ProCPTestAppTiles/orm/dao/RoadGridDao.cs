using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;

namespace ProCPTestAppTiles.orm.dao
{
    public class RoadGridDao : IDao<RoadGrid>
    {
        private static RoadPositionDao _roadPositionDao = (RoadPositionDao) DaoFactory.GetByType<RoadPosition>();
        
        
        public RoadGrid Load(BinaryReader reader, Tile tile)
        {
            RoadGrid roadGrid = null;
            try
            {
                roadGrid = new RoadGrid();

                roadGrid.tile = tile;
                
                roadGrid.roadPositionGridDict = new Dictionary<int, List<RoadPosition>>();
                var dictCount = reader.ReadInt32();
                for (int i = 0; i < dictCount; i++)
                {
                    var laneCount = reader.ReadInt32();

                    var roadPosListCount = reader.ReadInt32();
                    var roadPosList = new List<RoadPosition>();
                    for (int j = 0; j < roadPosListCount; j++)
                    {
                        roadPosList.Add(_roadPositionDao.Load(reader, tile));
                    }

                    roadGrid.roadPositionGridDict[laneCount] = roadPosList;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return roadGrid;
        }

        public void Save(RoadGrid o, BinaryWriter writer)
        {
            writer.Write(o.roadPositionGridDict.Count);
            foreach (var keyValuePair in o.roadPositionGridDict)
            {
                var laneCount = keyValuePair.Key;
                var roadPosList = keyValuePair.Value;
                
                writer.Write(laneCount);
                
                writer.Write(roadPosList.Count);
                foreach (var roadPosition in roadPosList)
                {
                    roadPosition.Save(writer);
                }
            }
        }
    }
}