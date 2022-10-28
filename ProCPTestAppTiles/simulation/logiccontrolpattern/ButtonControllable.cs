using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.logiccontrolpattern
{
    public class ButtonControllable<TLogic> : Button
    {
        private TLogic logic { get; set; }

        public ButtonControllable(TLogic logic)
        {
            this.logic = logic;
        }

        public TLogic GetLogic()
        {
            return logic;
        }
    }
}