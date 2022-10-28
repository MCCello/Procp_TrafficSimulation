namespace ProCPTestAppTiles.simulation.entities.road.trafficlight
{
    /// <summary>
    /// Class contains data that the event wants to pass back
    /// </summary>
    public class TrafficLightEventArgs
    {
        private TrafficLight source;
        private TrafficLightState? oldState;
        private TrafficLightState? newState;

        //Constructor
        public TrafficLightEventArgs(TrafficLight trafficLight, TrafficLightState? oldState, TrafficLightState? newState)
        {
            this.source = trafficLight;
            this.oldState = oldState;
            this.newState = newState;
        }

        //Properties
        public TrafficLight Source
        {
            get => source;
            set => source = value;
        }

        public TrafficLightState? OldState
        {
            get => oldState;
            set => oldState = value;
        }

        public TrafficLightState? NewState
        {
            get => newState;
            set => newState = value;
        }
         
        public override string ToString()
        {
            return  $"Source= {Source} " +
                    $"Old State= {OldState} " +
                    $"New State= {NewState}";
        }
    }
}