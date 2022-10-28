using ProCPTestAppTiles.simulation.entities.stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProCPTestAppTiles.forms.simulationform.livestatistics
{
    class LiveStatsControl : Control 
    {
        LiveStats liveStats;

        public LiveStatsControl(LiveStats liveStats)
        {
            this.liveStats = liveStats;
        }
    }
}
