using System;
using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities;
using ProCPTestAppTiles.simulation.entities.tileconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.forms.tileconfigform
{
    public class TileConfigControl : Controllable<TileConfig>
    {
        public TileConfigControl(TileConfig logic) : base(logic)
        {
        }

        public void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            GetLogic().ClickedOnTile(new Position(e.X, e.Y));
        }

        public void ButtonSave_Click(object sender, EventArgs e)
        {
            GetLogic().Save();
            var p = Parent as Form;
            p.Close();
        }

        public void ButtonCancel_Click(object sender, EventArgs e)
        {
            var p = Parent as Form;
            p.Close();
        }
    }
}