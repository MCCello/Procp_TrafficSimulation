using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.board;
using ProCPTestAppTiles.simulation;

namespace ProCPTestAppTiles.forms.menustrip
{
    public class SimContextMenuStrip : ContextMenuStrip, IAttachable
    {
        //Properties
        public Control mommyControl { get; set; }
        
        private ToolStripMenuItem rotateToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem rotateCCWToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem clearToolStripMenuItem = new ToolStripMenuItem();
        private ToolStripMenuItem configureTile = new ToolStripMenuItem();
        
        //Constructor
        public SimContextMenuStrip(Control mommyControl)
        {
            this.mommyControl = mommyControl;
            InitSimContextMenuStrip();
        }

        /// <summary>
        /// Creating the context for the menu strip, so importing the variables, methods, etc to the clicks of the map form
        /// </summary>
        private void InitSimContextMenuStrip()
        {
            // contextMenuStrip1
            Items.AddRange(new ToolStripItem[]
                {rotateToolStripMenuItem, rotateCCWToolStripMenuItem, clearToolStripMenuItem, configureTile});
            Name = "contextMenuStrip1";
            Size = new System.Drawing.Size(139, 70);
            
            // rotateToolStripMenuItem
            rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
            rotateToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            rotateToolStripMenuItem.Text = @"Rotate CW";
            rotateToolStripMenuItem.Click += ((BoardControl) mommyControl).rotateToolStripMenuItem_Click;
            
            // rotateCCWToolStripMenuItem
            rotateCCWToolStripMenuItem.Name = "rotateCCWToolStripMenuItem";
            rotateCCWToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            rotateCCWToolStripMenuItem.Text = @"Rotate CCW";
            rotateCCWToolStripMenuItem.Click += ((BoardControl) mommyControl).rotateCCWToolStripMenuItem_Click;
            
            // clearToolStripMenuItem
            clearToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            clearToolStripMenuItem.Text = @"Clear";
            clearToolStripMenuItem.Click += ((BoardControl) mommyControl).clearToolStripMenuItem_Click;

            // editLanesToolStripMenuItem
            configureTile.Name = "Configure Tile";
            configureTile.Size = new System.Drawing.Size(138, 22);
            configureTile.Text = @"Configure Tile";
            configureTile.Click += ((BoardControl) mommyControl).configureTile_Click;

        }
        
        /// <summary>
        /// attach mommyControl
        /// </summary>
        /// <param name="mommyControl"></param>
        public void AttachTo(Control mommyControl)
        {
            this.mommyControl = mommyControl;
            mommyControl.Controls.Add(this);
        }
        
        /// <summary>
        /// detach mommyControl
        /// </summary>
        public void DetachFrom()
        {
            mommyControl.Controls.Remove(this);
            mommyControl = null;
        }
    }
}