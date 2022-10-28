using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.orm.dao;
using Point = System.Drawing.Point;

namespace ProCPTestAppTiles.simulation.entities.road.trafficlight
{
    public class TrafficLight : IDrawable, ISaveable
    {
        private static TrafficLightDao _trafficLightDao = (TrafficLightDao) DaoFactory.GetByType<TrafficLight>();
        
        public static int WIDTH = 5;
        public static int HEIGHT = 5;
        
        public int objectId { get; set; }
        private TrafficLightState state = TrafficLightState.RED; // Default RED
        private RoadPosition roadPosition;
        private bool isOn = false;
        public int seqNo { get; set; }
       
        public delegate void StateChanged(TrafficLight trafficLight, TrafficLightEventArgs e);
        public event StateChanged stateChanged;

        //Constructor
        public TrafficLight()
        {
        }

        //Properties
        public TrafficLightState State
        {
            get => state;
            set => state = value;
        }

        [Obsolete("Using Setter for RoadPosition is deprecated. Use 'SetRoadPosition' instead.")]
        public RoadPosition RoadPosition
        {
            get => roadPosition;
            set => roadPosition = value;
        }

        /// <summary>
        /// Creates a roadPosition for current traffic light. Creates a link between traffic light and roadPosition.
        /// </summary>
        /// <param name="roadPosition"></param>
        public void SetRoadPosition(RoadPosition roadPosition)
        {
            if (roadPosition != null)
            {
                roadPosition.TrafficLight = this;
            }
            RoadPosition = roadPosition;
        }

        /// <summary>
        /// Returns boolean value if the traffic light is green
        /// </summary>
        /// <returns>bool</returns>
        public bool IsGreen()
        {
            return State.Equals(TrafficLightState.GREEN);
        }

        public void TurnGreen()
        {
            var oldState = State;
            State = TrafficLightState.GREEN;
            stateChanged?.Invoke(this, new TrafficLightEventArgs(this, oldState, State));
        }

        public void TurnRed()
        {
            var oldState = State;
            State = TrafficLightState.RED;
            stateChanged?.Invoke(this, new TrafficLightEventArgs(this, oldState, State));
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] " +
                   $"State= {State}";
        }

        public void Save(BinaryWriter writer)
        {
            _trafficLightDao.Save(this, writer);
        }

        public void Draw(PaintEventArgs e)
        {
            Color color = IsGreen() ? Color.Green : Color.Red;
            using (var pen = new Pen(color, WIDTH))
            {
                var position = roadPosition;
                var g = e.Graphics;
                pen.Alignment = PenAlignment.Inset;
                g.DrawRectangle(pen, (float)position.position.X, (float)position.position.Y, WIDTH, HEIGHT);
            }
        }

        public Rect GetRect()
        {
            return new Rect((float)roadPosition.position.X, (float)roadPosition.position.Y, WIDTH, HEIGHT);
        }
    }
}