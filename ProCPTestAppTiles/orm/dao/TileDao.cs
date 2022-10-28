using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.orm.dao
{
    public class TileDao : IDao<Tile>
    {
        private static RoadGridDao _roadGridDao = (RoadGridDao) DaoFactory.GetByType<RoadGrid>();
        private static PathsDao _pathsDao = (PathsDao) DaoFactory.GetByType<Paths>();
        private static PathDao _pathDao = (PathDao) DaoFactory.GetByType<Path>();
        private static IntersectionTrafficLightLogicDao _intersectionTrafficLightLogicDao = (IntersectionTrafficLightLogicDao) DaoFactory.GetByType<IntersectionTrafficLightLogic>();
        
        /// <summary>
        /// Loads a tile.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Tile Load(BinaryReader reader)
        {
            Tile tile = null;
            try
            {
                tile = new Tile
                {
                    Location = new Point(reader.ReadInt32(), reader.ReadInt32()),
                    Size = new Size(reader.ReadInt32(), reader.ReadInt32())
                };


                // Tile
                var roadTypeInt = reader.ReadInt32();
                var lanes = reader.ReadInt32();
                var rotation = reader.ReadInt32();
                
                RoadType? roadType = null;
                if (roadTypeInt != -1)
                {
                    roadType = (RoadType) roadTypeInt;
                }

                tile.roadType = roadType;
                tile.lanes = lanes;
                tile.rotation = rotation;

                // RoadGrid
                if (reader.ReadBoolean())
                {
                    tile.roadGrid = _roadGridDao.Load(reader, tile);
                }

                // Paths
                if (reader.ReadBoolean())
                {
                    tile.paths = _pathsDao.Load(reader, tile);
                }
                
                // IntersectionTrafficLightLogic
                if (reader.ReadBoolean())
                {
                    tile.ittl = _intersectionTrafficLightLogicDao.Load(reader, tile);
                }
                
                
                tile.Init();
                tile.Refresh();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return tile;
        }

        /// <summary>
        /// Saves a tile.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="writer"></param>
        public void Save(Tile o, BinaryWriter writer)
        {
            // Control
                // Location
            writer.Write(o.Location.X);
            writer.Write(o.Location.Y);
            
                // Size
            writer.Write(o.Size.Height);
            writer.Write(o.Size.Width);
            
            
            // Tile
                // RoadType
            writer.Write(o.roadType != null ? (int) o.roadType : -1);
                // Lanes
            writer.Write(o.lanes);
                // Rotation
            writer.Write(o.rotation);
            
                // RoadGrid
            writer.Write(o.roadGrid != null);
            o.roadGrid?.Save(writer);
            
                // Paths
            writer.Write(o.paths != null);
            o.paths?.Save(writer);
            
                // IntersectionTrafficLightLogic
            writer.Write(o.ittl != null);
            o.ittl?.Save(writer);
        }
    }
}