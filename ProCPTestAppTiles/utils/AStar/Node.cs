using System;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;

namespace ProCPTestAppTiles.utils.astar
{
    public class Node : IComparable<Node>
    {
        public Tile tile { get; set; }
        public Node cameFrom { get; set; }
        public int hCost { get; set; }
        public int gCost { get; set; }
        
        public Node(Tile tile)
        {
            this.tile = tile;
        }

        public void Discover(Node cameFrom, int hCost)
        {
            this.cameFrom = cameFrom;
            this.hCost = hCost;
            CalculateGCost();
        }

        private void CalculateGCost()
        {
            gCost = cameFrom?.gCost + 10 ?? 0;
        }

        public int GetFCost()
        {
            return hCost + gCost;
        }
        
        public int CompareTo(Node other)
        {
            // First Compare on F Cost
            if (GetFCost() == other.GetFCost())
            {
                // If F cost is equal, compare on H cost
                return hCost - other.hCost;
            }

            return GetFCost() - other.GetFCost();
        }

        public override string ToString()
        {
            var tileLocation = tile.Location;
            var fCost = GetFCost();
            return $"FCost={fCost}; Loc={tileLocation}";
        }
    }
}