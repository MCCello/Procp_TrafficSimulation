using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.tileconfigform;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;
using ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput;
using ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput.trafficlightconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using ProCPTestAppTiles.utils;
using ProCPTestAppTiles.utils.tile;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ProCPTestAppTiles.simulation.entities.tileconfig
{
    public class TileConfig : Attachable<TileConfigControl>
    {
        private Dictionary<Rect, TrafficLight> trafficLights { get; set; }
        
        public Tile tile { get; set; }
        public Tile realTile { get; set; }
        public PictureBox pictureBox { get; set; }
        public TileConfigInput tileConfigInput { get; set; }
        public Button buttonSave { get; set; }
        public Button buttonCancel { get; set; }

        public TileConfigData data { get; set; }
        
        public TileConfig(Tile tile, Control mommyControl, Point location) : base(mommyControl, location)
        {
            this.realTile = tile;
            this.tile = new Tile();
            this.tile.SetRoadType(tile.roadType);
            this.tile.Location = tile.Location;
            this.tile.Size = tile.Size;
            this.tile.rotation = tile.rotation;
            
            data = new TileConfigData(this.tile.lanes);
            
            Init();
        }

        protected override void InitTControl()
        {
            control = new TileConfigControl(this);
        }

        public override void Init()
        {
            tile.AddInfo();
            trafficLights = new Dictionary<Rect, TrafficLight>();
            
            
            var x = 0;
            var y = 0;
            
            
            // Gap
            x += TileConfigConstants.LEFT_BORDER_TO_PICTUREBOX;
            y += TileConfigConstants.TOP_BORDER_TO_PICTUREBOX;
            
            
            // PictureBox
            var image = new Bitmap(tile.Image);
            var resizedImage = Utils.ResizeImage(image, image.Width * TileConfigConstants.SCALE_ZOOM, image.Height * TileConfigConstants.SCALE_ZOOM);
            
            pictureBox = new PictureBox {Image = new Bitmap(resizedImage), Size = resizedImage.Size, Location = new Point(x, y)};
            pictureBox.MouseClick += GetControl().PictureBox_MouseClick;
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.Invalidate();
            AddControl(pictureBox);
            x += pictureBox.Size.Width;
            
            
            // Gap
            x += TileConfigConstants.PICTUREBOX_TO_TILE_CONFIG_INPUT;
            
            
            // TileConfigInput
            tileConfigInput = new TileConfigInput(tile, GetControl(), new Point(x, y));
            y += tileConfigInput.Size.Height;
            
            
            // Gap
            y += TileConfigConstants.TILE_CONFIG_INPUT_TO_BUTTON_SAVE;
            
            
            // Button Save
            buttonSave = new Button {Size = new Size(70, 40), Location = new Point(x, y), Text = @"Save"};
            buttonSave.Click += GetControl().ButtonSave_Click;
            x += buttonSave.Size.Width;
            AddControl(buttonSave);
            
            // Gap
            x += TileConfigConstants.BUTTON_SAVE_TO_BUTTON_CANCEL;
            
            // Button Cancel
            buttonCancel = new Button {Size = new Size(70, 40), Location = new Point(x, y), Text = @"Cancel"};
            buttonCancel.Click += GetControl().ButtonCancel_Click;
            x += buttonCancel.Size.Width;
            AddControl(buttonCancel);
            
            
            UpdateSize();
        }

        public int GetLanes()
        {
            return tileConfigInput?.GetLanes() ?? 0;
        }

        public List<List<TrafficLight>> GetTrafficLightSequence()
        {
            return tileConfigInput?.GetTrafficLightSequence();
        }

        public void Save()
        {
            data.lanes = GetLanes();

            var tls = GetTrafficLightSequence();
            if (tls.Count > 0)
            {
                data.itll = new IntersectionTrafficLightLogic(tls);
            }
            data.paths = tile.paths;
            data.roadGrid = tile.roadGrid;
            realTile.configData = data;
            realTile.ApplyConfig();
        }

        public void ClickedOnTile(Position position)
        {
            var trafficLight = GetTrafficLightByPosition(position);

            if (trafficLight == null)
            {
                // If user did not click on a TrafficLight
                return;
            }

            var trafficLightConfig = GetTrafficLightConfig();

            var focusedTextBox = trafficLightConfig?.GetFocusedTextBox();
            if (focusedTextBox == null)
            {
                return;
            }

            if (focusedTextBox.Text.Contains(trafficLight.objectId.ToString()))
            {
                return;
            }
            
            focusedTextBox.Text += $"{trafficLight.objectId}, ";
        }

        public TrafficLight GetTrafficLightByPosition(Position position)
        {
            foreach (var keyValuePair in trafficLights)
            {
                var rect = keyValuePair.Key;
                var trafficLight = keyValuePair.Value;

                if (rect.Contains(position.X, position.Y))
                {
                    return trafficLight;
                }
            }

            return null;
        }

        public TrafficLightConfig GetTrafficLightConfig()
        {
            return tileConfigInput?.trafficLightConfig;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var ittl = tile?.ittl;
            if (ittl == null)
            {
                return;
            }

            if (!(sender is PictureBox pictureBox))
            {
                return;
            }
            
            var originX = 0;
            var originY = 0;
            
            foreach (var trafficLight in ittl.GetAllTrafficLights())
            {
                
                var trafficLightPosition = trafficLight.RoadPosition.position;
                var trafficLightPositionX = trafficLightPosition.X;
                var trafficLightPositionY = trafficLightPosition.Y;

                var diffX = trafficLightPositionX - originX;
                var diffY = trafficLightPositionY - originY;

                var newX = originX + (TileConfigConstants.SCALE_ZOOM * diffX);
                var newY = originY + (TileConfigConstants.SCALE_ZOOM * diffY);
                
                if (trafficLight.objectId == 0)
                {
                    trafficLight.objectId = SimulationMap.GetNextObjectId();
                }
                
                Color color = trafficLight.objectId % 2 == 0 ? Color.Green : Color.Red;
                using (var pen = new Pen(color, TrafficLight.WIDTH))
                {
                    var g = e.Graphics;
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawRectangle(pen, (int) newX, (int) newY, TrafficLight.WIDTH * TileConfigConstants.SCALE_ZOOM, TrafficLight.WIDTH * TileConfigConstants.SCALE_ZOOM);
                    Rect rect = new Rect(newX, newY, TrafficLight.WIDTH * TileConfigConstants.SCALE_ZOOM, TrafficLight.WIDTH * TileConfigConstants.SCALE_ZOOM);
                    
                    trafficLights[rect] = trafficLight;
                }
            }
        }

        public List<TrafficLight> GetTrafficLights()
        {
            return trafficLights.Values.ToList();
        }
    }
}