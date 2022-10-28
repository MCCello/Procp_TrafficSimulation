using System;
using System.Collections.Generic;
using System.IO;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;

namespace ProCPTestAppTiles.simulation.entities.paths
{
    public class Path : ISaveable
    {
        private static PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        
        public int objectId { get; set; }
        public Tile tile { get; set; }
        public List<RoadPosition> path { get; set; }

        public Path()
        {
        }

        public Path(Tile tile, List<RoadPosition> path)
        {
            this.tile = tile;
            this.path = path;
        }

        public RoadPosition Start()
        {
            if (path == null || path.Count == 0)
            {
                return null;
            }
            return path[0];
        }

        public RoadPosition End()
        {
            if (path == null || path.Count == 0)
            {
                return null;
            }
            return path[path.Count - 1];
        }

        public bool IsEndPosition(RoadPosition roadPosition)
        {
            return roadPosition.Equals(End());
        }

        public RoadPosition GetNext(RoadPosition roadPosition)
        {
            if (roadPosition == null || IsEndPosition(roadPosition))
            {
                return null;
            }

            var idx = path.IndexOf(roadPosition);
            return path[idx + 1];
        }
        
        public DirectionType? GetDirectForPathPerFlowType(FlowType flowType)
        {
            var endingPath = flowType.Equals(FlowType.INFLOW) ? Start() : End();
            var endingPathPos = endingPath.position;

            if(Math.Abs(endingPathPos.X - tile.Location.X) < 2)
            {
                return flowType.Equals(FlowType.INFLOW) ? DirectionType.RIGHT : DirectionType.LEFT;
            }

            if (Math.Abs(endingPathPos.X - (tile.Location.X + tile.Size.Width)) < 2)
            {
                return flowType.Equals(FlowType.INFLOW) ? DirectionType.LEFT : DirectionType.RIGHT;
            }

            if (Math.Abs(endingPathPos.Y - tile.Location.Y) < 2)
            {
                return flowType.Equals(FlowType.INFLOW) ? DirectionType.DOWN : DirectionType.UP;
            }

            if (Math.Abs(endingPathPos.Y - (tile.Location.Y + tile.Size.Height)) < 2)
            {
                return flowType.Equals(FlowType.INFLOW) ? DirectionType.UP : DirectionType.DOWN;
            }

            return null;
        }

        public void Save(BinaryWriter writer)
        {
            _pathDao.Save(this, writer);
        }
    }
}