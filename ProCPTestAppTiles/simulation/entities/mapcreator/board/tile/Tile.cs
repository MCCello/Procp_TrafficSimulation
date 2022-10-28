using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.forms.mapcreatorform.board.tiles;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.tileconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;
using ProCPTestAppTiles.utils.tile;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.simulation.entities.mapcreator.board.tile
{
    public class Tile : Attachable<TileControl>, ISaveable
    {
        private static TileDao _tileDao = (TileDao) DaoFactory.GetByType<Tile>();

        public TileConfigData configData { get; set; }
        
        public int rotation { get; set; }
        public RoadType? roadType { get; set; }
        public RoadGrid roadGrid { get; set; }
        public Paths paths { get; set; }
        public IntersectionTrafficLightLogic ittl { get; set; }
        public int lanes = 3;


        public Tile()
        {
        }
        
        public Tile(Control mommyControl, Point location) : base(mommyControl, location)
        {
            Init();
        }

        protected override void InitTControl()
        {
            control = new TileControl(this);
        }
        
        public override void Init()
        {
        }

        
        public Image Image
        {
            get => GetControl().Image;
            set => GetControl().Image = value;
        }

        
        
        /// <summary>
        /// Sets the RoadType and resets the Rotation to 0.
        /// Updates Control to display changes.
        /// </summary>
        /// <param name="pRoadType"></param>
        public void SetRoadType(RoadType? pRoadType)
        {
            // Logic
            rotation = 0;
            roadType = pRoadType;
            paths = null;
            roadGrid = null;
            ittl = null;

            // Control
            Image = new Bitmap(ResourceUtils.GetImageByRoadType(roadType, lanes));
            UpdateControl();
        }

        /// <summary>
        /// Rotates the Image 90 degrees Clockwise.
        /// Updates Control to display changes.
        /// </summary>
        public void Rotate90DegreesCW()
        {
            if (GetRoadPositionGrid() != null)
            {
                AdjustRotation(-rotation);
            }
            
            // Logic
            rotation = (rotation + 90) % 360;

            if (GetRoadPositionGrid() != null)
            {
                AdjustRotation(rotation);
            }
            
            // Control
            Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            UpdateControl();
        }

        /// <summary>
        /// Rotates the Image 90 degrees Counter-Clockwise.
        /// Updates Control to display changes.
        /// </summary>
        public void Rotate90DegreesCCW()
        {
            if (GetRoadPositionGrid() != null && rotation != 0)
            {
                AdjustRotation(-rotation);
            }
            
            // Logic
            rotation = (rotation - 90) % 360;
            if (rotation < 0)
            {
                rotation = 360 - Math.Abs(rotation);
            }

            if (GetRoadPositionGrid() != null)
            {
                AdjustRotation(rotation);
            }

            // Control
            Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            UpdateControl();
        }

        /// <summary>
        /// Removes the current RoadType and resets the Rotation to 0.
        /// Updates Control to display changes.
        /// </summary>
        public void Clear()
        {
            // Logic
            roadType = null;
            rotation = 0;
            paths = null;
            roadGrid = null;
            ittl = null;

            // Control
            Image = null;
            UpdateControl();
        }

        
        
        /// <summary>
        /// Saves the Tile using ORM.
        /// </summary>
        public void Save(BinaryWriter writer)
        {
            _tileDao.Save(this, writer);
        }

        public void Refresh()
        {
            if (roadType == null)
            {
                return;
            }
            Image = new Bitmap(ResourceUtils.GetImageByRoadType(roadType, lanes));
            for (var i = 0; i < rotation; i += 90)
            {
                Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            UpdateControl();
        }

        public void AdjustRotation(float angle)
        {
            foreach (var entry in roadGrid.roadPositionGridDict)
            {
                Utils.RotateRoadPositions(entry.Value, angle, GetCenter());
            }
        }
        
        public void CorrectPositionsForLocationAndRotation()
        {
            if (roadGrid?.roadPositionGridDict == null)
            {
                return;
            }

            // Correct RoadGrid
            foreach (var entry in roadGrid.roadPositionGridDict)
            {
                foreach (var roadPosition in entry.Value)
                {
                    roadPosition.position.AddWith(new Position(Location.X, Location.Y));
                }

                Utils.RotateRoadPositions(entry.Value, rotation, GetCenter());
            }
        }

        
        
        public Point GetCenter()
        {
            var xCenter = Location.X + (Size.Width / 2);
            var yCenter = Location.Y + (Size.Height / 2);
            return new Point(xCenter, yCenter);
        }

        public List<RoadPosition> GetRoadPositionGrid()
        {
            if (roadType.Equals(RoadType.INTERSECTION))
            {
                return roadGrid?.GetGridByLane(3);
            }

            return roadGrid?.GetGridByLane(lanes);
        }

        public List<Path> GetPaths()
        {
            if (roadType.Equals(RoadType.INTERSECTION))
            {
                return paths?.GetPathsByLane(3);
            }

            return paths?.GetPathsByLane(lanes);
        }

        public void Start()
        {
            ittl?.Start();
        }

        public void Stop()
        {
            ittl?.Stop();
        }

        public List<Path> GetPathsByDirectionType(DirectionType inflow, DirectionType outflow, bool bypassOutflow = false)
        {
            return GetPaths()?.Where(p =>
                    inflow.Equals(p.GetDirectForPathPerFlowType(FlowType.INFLOW)) &&
                    (bypassOutflow || outflow.Equals(p.GetDirectForPathPerFlowType(FlowType.OUTFLOW))))
                .ToList();
        }


        public List<Life> GetAllLifes()
        {
            return GetRoadPositionGrid()?.Where(rp => rp.HasLife()).Select(rp => rp.oldLife).ToList();
        }

        public List<TrafficLight> GetAllTrafficLights()
        {
            var temp = new List<TrafficLight>();
            if (ittl != null)
            {
                foreach (var trafficLights in ittl.trafficLightSequence)
                {
                    foreach (var trafficLight in trafficLights)
                    {
                        temp.Add(trafficLight);
                    }
                }
            }

            return temp;
        }

        public void ApplyConfig()
        {
            configData?.ApplyToTile(this);
        }

        public void AddInfo() 
        {
            if (roadGrid == null || paths == null)
            {
                RoadXMLReader.AddTileInfoFromXML(this);
                CorrectPositionsForLocationAndRotation();
            }
                
            if (roadGrid != null)
            {
                roadGrid.roadPositionGridDict[0] = new List<RoadPosition>();
            }

            if (paths != null)
            {
                paths.pathsDict[0] = new List<Path>();
            }
        }

        public void CleanUp()
        {
            if (GetRoadPositionGrid() == null)
            {
                return;
            }
            
            foreach (var roadPosition in GetRoadPositionGrid())
            {
                roadPosition.oldLife = null;
                roadPosition.newLife = null;
                roadPosition.counter = 0;
            }

            ittl?.CleanUp();
        }
        
        public override string ToString()
        {
            return $"[{GetType().Name}] " +
                   $"RoadType={roadType};" +
                   $"Rotation={rotation};";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile other))
            {
                return false;
            }

            return other.Location.X == Location.X
                   && other.Location.Y == Location.Y
                   && other.roadType.Equals(roadType);
        }

   
    }
}