using System.Windows.Forms;

namespace ProCPTestAppTiles.simulation.entities
{
    public interface IDrawable
    {
        /// <summary>
        /// Paints on the sender using e as the PaintEventArgs
        /// </summary>
        /// <param name="e"></param>
        void Draw(PaintEventArgs e);
    }
}