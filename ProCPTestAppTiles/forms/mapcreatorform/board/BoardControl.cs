using System;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.board.tiles;
using ProCPTestAppTiles.forms.menustrip;
using ProCPTestAppTiles.simulation.entities.mapcreator;
using ProCPTestAppTiles.simulation.entities.mapcreator.board;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.tileconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.forms.mapcreatorform.board
{
    public class BoardControl : Controllable<Board>
    {
        // Properties
        public SimContextMenuStrip simContextMenuStrip { get; set; }
        
        //Constructor
        public BoardControl(Board logic) : base(logic)
        {
            simContextMenuStrip = new SimContextMenuStrip(this);
        }
        
        /// <summary>
        /// checks for tileControl 
        /// rotates tiles on shift / ctrl click
        /// checks mouse input for right click drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TileMouseDown(object sender, MouseEventArgs e)
        {
            TileControl tileControl = sender as TileControl;
            if (tileControl == null)
            {
                return;
            }

            Tile tile = tileControl.GetLogic();
            
            var mouseButton = e.Button;

            var hasShiftDown = ModifierKeys.HasFlag(BoardConstants.ROTATE_CW); // Shift
            var hasCtrlDown = ModifierKeys.HasFlag(BoardConstants.ROTATE_CCW); // Ctrl
            
            
            switch (mouseButton)
            {
                case MouseButtons.Left:
                    if (tileControl.Image != null && (hasCtrlDown || hasShiftDown))
                    {
                        if (hasCtrlDown && hasShiftDown)
                        {
                            tile.Clear();
                            break;
                        }
                        
                        // Shift -> Rotate Clockwise 90*
                        if (hasShiftDown)
                        {
                            tile.Rotate90DegreesCW();
                            break;
                        }
                        
                        // Control -> Rotate CounterClockwise 90*
                        if (hasCtrlDown)
                        {
                            tile.Rotate90DegreesCCW();
                            break;
                        }
                    }
                    
                    var roadType = GetSelectedRoadType();
                    if (roadType != null)
                    {
                        tile.SetRoadType(roadType);
                    }
                    break;

                case MouseButtons.Right:
                    if (tileControl.Image != null)
                    {
                        simContextMenuStrip.Show(tileControl, new Point(e.X, e.Y));
                    }
                    break;
            }
        }
        /// <summary>
        /// returns a selected roadType from left screen input
        /// </summary>
        /// <returns>selectedRoadType from left screen</returns>
        private RoadType GetSelectedRoadType()
        {
            return MapCreator.selectedRoadType;
        }
        
        /// <summary>
        /// Rotating method for tiles 90 CW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void rotateToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                ContextMenuStrip contextMenuStrip = toolStripItem.Owner as ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    TileControl tileControl = contextMenuStrip.SourceControl as TileControl;
                    if (tileControl != null)
                    {
                        tileControl.GetLogic().Rotate90DegreesCW();
                    }
                }
            }
        }
        
        /// <summary>
        /// Rotating method for tiles 90 CCW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void rotateCCWToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                ContextMenuStrip contextMenuStrip = toolStripItem.Owner as ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    TileControl tileControl = contextMenuStrip.SourceControl as TileControl;
                    if (tileControl != null)
                    {
                        tileControl.GetLogic().Rotate90DegreesCCW();
                    }
                }
            }
        }
        
        /// <summary>
        /// click out of strip down menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void clearToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                ContextMenuStrip contextMenuStrip = toolStripItem.Owner as ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    TileControl tileControl = contextMenuStrip.SourceControl as TileControl;
                    if (tileControl != null)
                    {
                        tileControl.GetLogic().Clear();
                    }
                }
            }
        }
        
        public void configureTile_Click(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = sender as ToolStripItem;
            if (toolStripItem != null)
            {
                ContextMenuStrip contextMenuStrip = toolStripItem.Owner as ContextMenuStrip;
                if (contextMenuStrip != null)
                {
                    TileControl tileControl = contextMenuStrip.SourceControl as TileControl;
                    if (tileControl != null)
                    {
                        var tile = tileControl.GetLogic();
                        var form = new Form();

                        new TileConfig(tile, form, Point.Empty);
                        form.Size = Utils.GetCorrectSize(form);
                        form.AutoSize = true;
                        form.Show();
                    }
                }
            }
        }
    }
}