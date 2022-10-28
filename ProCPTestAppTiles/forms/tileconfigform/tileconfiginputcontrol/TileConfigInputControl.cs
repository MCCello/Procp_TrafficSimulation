using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.tileconfigform.tileconfiginputcontrol
{
    public class TileConfigInputControl : Controllable<TileConfigInput>
    {
        public TileConfigInputControl(TileConfigInput logic) : base(logic)
        {
        }
    }
}