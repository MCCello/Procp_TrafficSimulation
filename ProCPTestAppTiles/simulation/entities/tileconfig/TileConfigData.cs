using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;

namespace ProCPTestAppTiles.simulation.entities.tileconfig
{
    public class TileConfigData
    {
        public IntersectionTrafficLightLogic itll { get; set; }
        public RoadGrid roadGrid { get; set; }
        public Paths paths { get; set; }
        public int lanes { get; set; }

        public TileConfigData(int lanes)
        {
            this.lanes = lanes;
        }

        public void ApplyToTile(Tile tile)
        {
            if (itll != null)
            {
                tile.ittl = itll;
            }

            tile.roadGrid = roadGrid;
            tile.paths = paths;
            tile.lanes = lanes;
        }
    }
}