using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;

namespace ProCPTestAppTiles.simulation.entities.road.trafficlight
{
    public class IntersectionTrafficLightLogic : ISaveable
    {
        private static IntersectionTrafficLightLogicDao _intersectionTrafficLightLogicDao = (IntersectionTrafficLightLogicDao) DaoFactory.GetByType<IntersectionTrafficLightLogic>();
        
        public static int GREEN_TIME = 6000; // 6sec
        public static int DELAY_TIME = 2000; // 2sec


        public List<List<TrafficLight>> trafficLightSequence { get; set; } //List that contains the list of traffic lights. Will be used in next iterations
        public int cursor { get; set; }     // Pointer for the positions inside the list
        public Timer timer { get; set; }    // Traffic light timer
        public Timer delayTimer { get; set; } // Delay so the cars do not start moving while other cars in intersection

        //Constructor
        public IntersectionTrafficLightLogic()
        {
            this.trafficLightSequence = new List<List<TrafficLight>>();
            cursor = 0;
            
            timer = new Timer {Interval = DELAY_TIME};
            timer.Tick += StartSequence;

            delayTimer = new Timer {Interval = GREEN_TIME};
            delayTimer.Tick += DelayTimerTick;
        }

        public IntersectionTrafficLightLogic(List<List<TrafficLight>> trafficLightSequence)
        {
            this.trafficLightSequence = trafficLightSequence;
            cursor = 0;
            
            timer = new Timer {Interval = DELAY_TIME};
            timer.Tick += StartSequence;

            delayTimer = new Timer {Interval = GREEN_TIME};
            delayTimer.Tick += DelayTimerTick;
        }

        /// <summary>
        /// Starts Intersection Logic.
        /// </summary>
        public void Start()
        {
            delayTimer.Start();
        }

        /// <summary>
        /// Stops Intersection Logic
        /// </summary>
        public void Stop()
        {
            delayTimer.Stop();
        }
        
        private void StartSequence(object sender, EventArgs e)
        {
            if (trafficLightSequence.Count == 0)
            {
                return;
            }
            
            // Turn TrafficLights
            foreach (var trafficLightList in trafficLightSequence[cursor])
            {
                trafficLightList.TurnGreen();
            }

            cursor++;
            if (cursor >= trafficLightSequence.Count)
            {
                cursor = 0;
            }

            // Timer Management
            timer.Stop();
            delayTimer.Start();
        }

        private void DelayTimerTick(object sender, EventArgs e)
        {
            // Turn TrafficLights
            TurnAllRed();
            
            // Timer Management
            delayTimer.Stop();
            timer.Start();
        }

        /// <summary>
        /// Turns all the traffic lights Red
        /// </summary>
        private void TurnAllRed()
        {
            foreach (var trafficLightList in trafficLightSequence)
            {
                foreach (var trafficLight in trafficLightList)
                {
                    trafficLight.TurnRed();
                }
            }
        }
        
        public List<TrafficLight> GetAllTrafficLights()
        {
            var temp = new List<TrafficLight>();
            foreach (var trafficLights in trafficLightSequence)
            {
                foreach (var trafficLight in trafficLights)
                {
                    temp.Add(trafficLight);
                }
            }

            return temp.Distinct().ToList();
        }

        public void CleanUp()
        {
            TurnAllRed();
            cursor = 0;
            Stop();
        }

        public void Save(BinaryWriter writer)
        {
            _intersectionTrafficLightLogicDao.Save(this, writer);
        }
    }
}

