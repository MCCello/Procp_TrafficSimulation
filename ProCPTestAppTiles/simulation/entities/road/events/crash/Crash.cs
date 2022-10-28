using System;
using ProCPTestAppTiles.simulation.entities.life;

namespace ProCPTestAppTiles.simulation.entities.road.events.crash
{
    public class Crash
    {
        private Random r = new Random();
        public int crashCar()
        {
            int wwichCar = r.Next(1,70);
            return wwichCar;
        }
    }
}