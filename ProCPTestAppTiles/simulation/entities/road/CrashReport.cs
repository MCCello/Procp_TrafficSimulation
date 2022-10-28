using System.Collections.Generic;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;

namespace ProCPTestAppTiles.simulation.entities.road
{
    public class CrashReport
    {
        private List<Life> lifes { get; set; }
       

        public CrashReport(params Life[] cars)
        {
            lifes = cars.ToList();
        }

        public double GetMaxVelocity()
        {
            return lifes.Select(l => l.velocity).Max();
        }

        public double GetAverageVelocity()
        {
            return lifes.Select(l => l.velocity).Average();
        }

        public double GetMinVelocity()
        {
            return lifes.Select(l => l.velocity).Min();
        }

        public Tile GetTile()
        {
            return lifes.Select(l => l.currentPath.tile).First();
        }

        public Path GetPath()
        {
            return lifes.Select(l => l.currentPath).First();
        }

        public Position GetPosition()
        {
            return lifes.Select(l => l.curRoadPosition.position).First();
        }

      
       
    }
}