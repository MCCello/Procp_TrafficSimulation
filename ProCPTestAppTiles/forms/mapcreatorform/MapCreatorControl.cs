using System;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.mapcreatorform
{
    public class MapCreatorControl : Controllable<MapCreator>
    {
        public MapCreatorControl(MapCreator logic) : base(logic)
        {
        }

        public void startButton_Clicked(object sender, EventArgs e)
        {
            GetLogic().CreateSimulation();
        }
    }
}