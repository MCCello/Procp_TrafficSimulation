using ProCPTestAppTiles.forms.simulationform.livestatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.entities.stats
{
    class LiveStats: IAttachable
    {
        public LiveStatsControl liveStatsControl { get; set; }
        public Control mommyControl { get; set; }
        public ListBox listBoxStats { get; set; }
        public SimulationStatistics stats { get; set; }

        public LiveStats()
        {
            this.liveStatsControl = new LiveStatsControl(this);
            InitLiveStats();
        }

        public void AttachTo(Control mommyControl)
        {
            this.mommyControl = mommyControl;
        }

        public void DetachFrom()
        {
            this.mommyControl.Controls.Remove(this.liveStatsControl);
        }

        public void InitLiveStats()
        {
            listBoxStats = new ListBox {Size=new System.Drawing.Size(335, 277), Location= new System.Drawing.Point(0,0) };
            
        }
        public void AddToLB(string s)
        {
            listBoxStats.Items.Add(s);
        }
        public void ClearLB()
        {
            listBoxStats.Items.Clear();
        }

        private void RefreshListbox()
        {
            var simStats = stats;
            listBoxStats.Items.Clear();
            listBoxStats.Items.Add("Total distance : " + simStats.CalculateAllCarsDistanceTravelled());
            listBoxStats.Items.Add("Total cars in simulation: " + simStats.CalculateTotalCarsMoving());
            listBoxStats.Items.Add($"Simulation Duration: {simStats.SimulationDuration()}");
            listBoxStats.Items.Add($"Total cars which completed their route: {simStats.TotalCarsCompletedRoute()}");
            listBoxStats.Items.Add("---------------------------------");

            if (simStats.AverageCarLifeTime() != 0)
            {
                listBoxStats.Items.Add($"Average Car Life Time : {simStats.AverageCarLifeTime()} s");
            }
            else listBoxStats.Items.Add($"Average Car Life Time : No Cars have completed their journeys yet.");
            if (simStats.TotalTimeAllCarsSpent() != 0)
            {
                listBoxStats.Items.Add($"Total Car Life Time : {simStats.TotalTimeAllCarsSpent()} s");
            }
            else listBoxStats.Items.Add($"Total Car Life Time : No Cars have completed their journeys yet.");
            if (simStats.FastestCar() != 0)
            {
                listBoxStats.Items.Add($"Fastest Car in Simulation took: {simStats.FastestCar()} s");
            }
            else listBoxStats.Items.Add($"Fastest Car in Simulation:  No Cars have completed their journeys yet.");
            if (simStats.SlowestCar() != 0)
            {
                listBoxStats.Items.Add($"Slowest Car in Simulation took: {simStats.SlowestCar()} s");
            }
            else listBoxStats.Items.Add($"Slowest Car in Simulation:  No Cars have completed their journeys yet.");
            listBoxStats.Items.Add($"Average Speed For All Cars: {simStats.AverageSpeedOfCars()}  pixels/seconds");
        }
    }
}
