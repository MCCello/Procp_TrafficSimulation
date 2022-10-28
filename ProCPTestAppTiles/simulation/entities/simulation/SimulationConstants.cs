namespace ProCPTestAppTiles.simulation.entities.simulation
{
    class SimulationConstants
    {
        public static bool CAR_DRIVES_AROUND_CRASH = false; // Buggy hence not turned on
        
        public static int CRASH_VELOCITY = 180; // Minimum required velocity in order to crash with a car.
        
        public static int CAR_CRASH_PROC = 250; // out of 10,000
        public static int CAR_CRASH_PROC_MAX = 10_000; // out of 10,000
    }
}
