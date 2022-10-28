using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.logiccontrolpattern
{
    public abstract class Controllable<TLogic> : Control
    {
        private TLogic logic { get; set; }

        public Controllable(TLogic logic)
        {
            this.logic = logic;
        }

        public TLogic GetLogic()
        {
            return logic;
        }
    }
}