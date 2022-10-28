using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation
{
    public interface IAttachable
    {
        /// <summary>
        /// Attach 'this' to the 'mommyControl'.
        /// </summary>
        /// <param name="mommyControl"></param>
        void AttachTo(Control mommyControl);

        /// <summary>
        /// Detach 'this' from the 'mommyControl'.
        /// </summary>
        void DetachFrom();
    }
}