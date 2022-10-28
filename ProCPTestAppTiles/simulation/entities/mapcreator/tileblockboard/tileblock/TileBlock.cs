using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.mapcreatorform.tileblockboard.tileblocks;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;

namespace ProCPTestAppTiles.simulation.entities.mapcreator.tileblockboard.tileblock
{
    public class TileBlock : Attachable<TileBlockControl>
    {
        public RoadType roadType { get; set; }
        public Image oImage { get; set; }

        public TileBlock(RoadType roadType, Control mommyControl, Point location) : base(mommyControl, location)
        {
            Size = new Size(TileBlockConstants.TILE_BLOCK_WIDTH, TileBlockConstants.TILE_BLOCK_HEIGHT);
            this.roadType = roadType;
            Image = ResourceUtils.GetImageByRoadType(roadType);
            oImage = new Bitmap(Image);
        }

        protected override void InitTControl()
        {
            control = new TileBlockControl(this);
        }

        public override void Init()
        {
        }

        public Image Image
        {
            get => GetControl().Image;
            set => GetControl().Image = value;
        }
        
        public override string ToString()
        {
            return  $"[{GetType()}] " + 
                    $"Size={Size} "  +
                    $"Size={Image} " +
                    $"RoadType={roadType};";
        }
    }
}