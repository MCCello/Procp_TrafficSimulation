using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using Path = ProCPTestAppTiles.simulation.entities.paths.Path;

namespace ProCPTestAppTiles.utils.tile
{
    public class RoadXMLReader
    {
        public static void AddTileInfoFromXML(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            
            var path = ResourceUtils.GetXmlPathByRoadType(tile.roadType);
            if (path == null)
            {
                return;
            }
            
            using (XmlReader reader = XmlReader.Create(path, new XmlReaderSettings()))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            switch (reader.Name)
                            {
                                case "grids":
                                    tile.roadGrid = GetGridsFromXML(tile, reader);
                                    break;
                                
                                case "pathList":
                                    tile.paths = GetPathsFromXML(tile, reader);
                                    break;
                            }
                            break;
                    }
                }
            }

            if (tile.ittl != null && tile.ittl.trafficLightSequence.Count == 0)
            {
                var trafficLights = tile.GetRoadPositionGrid().Where(r => r.TrafficLight != null).Select(r => r.TrafficLight).ToList();
                List<int> sequenceList = trafficLights.Select(t => t.seqNo).OrderBy(i => i).Distinct().ToList();
                var trafficLightSequence = new List<List<TrafficLight>>();
                foreach (var seq in sequenceList)
                {
                    var trafficLightSequencePart = trafficLights.Where(t => t.seqNo == seq).ToList();
                    trafficLightSequence.Add(trafficLightSequencePart);
                }

                tile.ittl.trafficLightSequence = trafficLightSequence;
            }
        }

        private static Paths GetPathsFromXML(Tile tile, XmlReader reader)
        {
            var roadGrid = tile.roadGrid;
            
            var d = new Dictionary<int, List<Path>>();
            
            // Read <pathList>
            while (reader.Read())
            {
                if (reader.NodeType.Equals(XmlNodeType.EndElement))
                {
                    break;
                }
                if (reader.Name != "paths")
                {
                    continue;
                }

                var laneStr = reader.GetAttribute("lanes");
                if (int.TryParse(laneStr, out var lanes))
                {
                    var paths = new List<Path>();
                    
                    // Read <paths>
                    while (reader.Read())
                    {
                        if (reader.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            break;
                        }
                        if (reader.Name != "path")
                        {
                            continue;
                        }
                        
                        // Read <path>
                        var roadPositionList = new List<RoadPosition>();
                        while (reader.Read())
                        {
                            if (reader.NodeType.Equals(XmlNodeType.EndElement))
                            {
                                break;
                            }
                            if (reader.Name != "roadposition")
                            {
                                continue;
                            }
                            
                            // Read <roadposition>
                            var xStr = reader.GetAttribute("x");
                            var yStr = reader.GetAttribute("y");

                            if (int.TryParse(xStr, out var x) && int.TryParse(yStr, out var y))
                            {
                                roadPositionList.Add(roadGrid.GetRoadPositionFromGrid(lanes, x, y));
                            }
                            else
                            {
                                Debug.WriteLine("Error Trying to parse XML File's Grid Positions");
                            }
                        }

                        var path = new Path(tile, roadPositionList) { objectId = SimulationMap.GetNextObjectId() };
                        paths.Add(path);
                    }

                    d[lanes] = paths;
                }
            }


            return new Paths(d);
        }

        private static RoadGrid GetGridsFromXML(Tile tile, XmlReader reader)
        {
            var d = new Dictionary<int, List<RoadPosition>>();
            
            // Read <grids>
            while (reader.Read())
            {
                if (reader.NodeType.Equals(XmlNodeType.EndElement))
                {
                    break;
                }
                if (reader.Name != "grid")
                {
                    continue;
                }

                // Read <grid>
                var laneStr = reader.GetAttribute("lanes");
                if (int.TryParse(laneStr, out var lane))
                {
                    var l = new List<RoadPosition>();
                    while (reader.Read())
                    {
                        if (reader.NodeType.Equals(XmlNodeType.EndElement))
                        {
                            break;
                        }
                        if (reader.Name != "roadposition")
                        {
                            continue;
                        }

                        // Read <roadposition>
                        
                        var xStr = reader.GetAttribute("x");
                        var yStr = reader.GetAttribute("y");

                        if (int.TryParse(xStr, out var x) && int.TryParse(yStr, out var y))
                        {
                            if (!l.Any(rp => rp.position.X == x && rp.position.Y == y))
                            {
                                var pos = new RoadPosition(x, y) { objectId = SimulationMap.GetNextObjectId() };
                                l.Add(pos);
                                var trafficLight = reader.GetAttribute("tl");
                                if (trafficLight != null)
                                {
                                    if (tile.ittl == null)
                                    {
                                        tile.ittl = new IntersectionTrafficLightLogic();
                                    }
                                    
                                    var tl = new TrafficLight {seqNo = int.Parse(trafficLight), objectId = SimulationMap.GetNextObjectId()};
                                    pos.SetTrafficLight(tl);
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Error Trying to parse XML File's Grid Positions");
                        }
                    }

                    d[lane] = l;
                }
            }
            
            return new RoadGrid(tile, d);
        }
    }
}