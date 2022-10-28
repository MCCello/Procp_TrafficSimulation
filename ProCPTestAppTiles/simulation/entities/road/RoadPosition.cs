using System;
using System.IO;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;

namespace ProCPTestAppTiles.simulation.entities.road
{
    public class RoadPosition : ISaveable
    {
        private static RoadPositionDao _roadPositionDao = (RoadPositionDao) DaoFactory.GetByType<RoadPosition>();

        public int objectId { get; set; }
        private TrafficLight trafficLight;
        public Position position { get; set; }
        public Life oldLife { get; set; }
        public Life newLife { get; set; }
        public int counter { get; set; }

        //constructors
        public RoadPosition(int x, int y)
        {
            this.position = new Position(x, y);
        }

        public RoadPosition(Position position)
        {
            this.position = position;
        }
        
        //properties
        [Obsolete("Using Setter for TrafficLight is deprecated. Use 'SetTrafficLight' instead.")]
        public TrafficLight TrafficLight
        {
            get => trafficLight;
            set => trafficLight = value;
        }

        /// <summary>
        /// creates a traffic light in this road position. Also initializes trafficLight.Roadposition so they are both linked to each other.
        /// </summary>
        /// <param name="trafficLight"></param>
        public void SetTrafficLight(TrafficLight trafficLight)
        {
            if (trafficLight != null)
            {
                trafficLight.RoadPosition = this;
            }
            TrafficLight = trafficLight;
        }

        /// <summary>
        /// returns true if road position is not occupied by life, or if traffic light on this road position is green
        /// </summary>
        /// <returns>returns false if (RoadPosition.Life!=null || RoadPosition.TrafficLight is red  </returns>
        public bool isFree()
        {
            // Check if Traffic Light exists  AND  is Red
            if (TrafficLight != null && !TrafficLight.IsGreen())
            {
                return false;
            }

            if (newLife == null)
            {
                return oldLife == null || oldLife.IsMoving();
            }

            return false;
        }

        public bool HasLife()
        {
            return oldLife != null;
        }

        public override string ToString()
        {
            return $"[{this.GetType().Name}] " +
                   $"Pos= {this.position}; " +
                   $"Life= {this.oldLife};";
        }

        public void Save(BinaryWriter writer)
        {
            _roadPositionDao.Save(this, writer);
        }
    }
}