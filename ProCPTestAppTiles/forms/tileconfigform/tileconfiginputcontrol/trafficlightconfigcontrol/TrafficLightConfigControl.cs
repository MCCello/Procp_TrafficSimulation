using System;
using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput.trafficlightconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.tileconfigform.tileconfiginputcontrol.trafficlightconfigcontrol
{
    public class TrafficLightConfigControl : Controllable<TrafficLightConfig>
    {
        public TrafficLightConfigControl(TrafficLightConfig logic) : base(logic)
        {
        }

        public void TextBox_Click(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null || !GetLogic().IsLastTextBox(textBox))
            {
                return;
            }

            GetLogic().ClickedLastTextBox();
        }
    }
}