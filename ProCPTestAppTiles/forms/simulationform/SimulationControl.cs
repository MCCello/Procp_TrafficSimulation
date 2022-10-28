using System;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.simulationform
{
    public class SimulationControl : Controllable<Simulation>
    {
        public SimulationControl(Simulation logic) : base(logic)
        {
        }

        public void HeatmapButton_Clicked(object sender, EventArgs e)
        {
            GetLogic().ShowHeatMap();
        }

        public void PauseButton_Clicked(object sender, EventArgs e)
        {
            if (GetLogic().IsPaused())
            {
                GetLogic().Start();
            }
            else
            {
                GetLogic().Stop();
            }
            UpdatePauseButton();
        }

        public void Form_Closing(object sender, EventArgs e)
        {
            GetLogic().CleanUp();
        }

        public void UpdatePauseButton()
        {
            GetLogic().pauseButton.Text = GetLogic().IsPaused() ? @"Start" : @"Pause";
        }
    }
}