using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.logiccontrolpattern
{
    public interface IAttachable<TControl> where TControl : Control
    {
        void AttachTo(Control mommyControl);

        void DetachFrom();

        void AddControl(Control babyControl);

        void RemoveControl(Control babyControl);
        
        TControl GetControl();

        void UpdateControl();
    }
}