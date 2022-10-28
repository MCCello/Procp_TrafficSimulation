using System.Collections.Generic;
using System.IO;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.simulation.entities.road
{
    public class RoadGrid : ISaveable
    {
        private static RoadGridDao _roadGridDao = (RoadGridDao) DaoFactory.GetByType<RoadGrid>();
        
        public Tile tile { get; set; }
        public Dictionary<int, List<RoadPosition>> roadPositionGridDict { get; set; } // <Lane, RoadPositions>

        public RoadGrid()
        {
        }

        public RoadGrid(Tile tile, Dictionary<int, List<RoadPosition>> roadPositionsGridList)
        {
            this.tile = tile;
            this.roadPositionGridDict = roadPositionsGridList;
        }

        public List<RoadPosition> GetGridByLane(int lane)
        {
            return Utils.GetValueOrDefault(roadPositionGridDict, lane, null);
        }

        public RoadPosition GetRoadPositionFromGrid(int lane, int x, int y)
        {
            var grid = GetGridByLane(lane);
            foreach (var r in grid)
            {
                if (r.position.X.Equals(x) && r.position.Y.Equals(y))
                {
                    return r;
                }
            }

            return null;
        }

        public void Save(BinaryWriter writer)
        {
            _roadGridDao.Save(this, writer);
        }
    }
}