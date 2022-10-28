using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.tileconfigform.tileconfiginputcontrol;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput.trafficlightconfig;
using ProCPTestAppTiles.simulation.logiccontrolpattern;

namespace ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput
{
    public class TileConfigInput : Attachable<TileConfigInputControl>
    {
        public Tile tile { get; set; }
        public TrafficLightConfig trafficLightConfig { get; set; }
        public TextBox textBox { get; set; }
        
        public TileConfigInput(Tile tile, Control mommyControl, Point location) : base(mommyControl, location)
        {
            this.tile = tile;
            Init();
        }

        protected override void InitTControl()
        {
            control = new TileConfigInputControl(this);
        }

        public override void Init()
        {
            var x = 0;
            var y = 0;
            
            
            // TrafficLightConfig
            trafficLightConfig = new TrafficLightConfig(tile, GetControl(), new Point(x, y));
            y += trafficLightConfig.Size.Height;
            
            
            // Gap
            y += TileConfigInputConstants.TRAFFIC_LIGHT_CONFIG_TO_LABEL;
            
            
            // Label
            var label = new Label {Location = new Point(x, y), Size = new Size(100, 17), Text = @"Lanes:"};
            y += label.Size.Height;
            AddControl(label);
                
            // Gap
            y += TileConfigInputConstants.LABEL_TO_TEXTBOX;
            
            // Textbox
            textBox = new TextBox {Location = new Point(x, y), Size = new Size(100, 30), Text = tile.lanes.ToString()};
            y += textBox.Size.Height;
            AddControl(textBox);
            
            
            UpdateSize();
        }

        public int GetLanes()
        {
            int lanes = int.TryParse(textBox.Text, out lanes) ? lanes : 3;
            return Math.Min(3, Math.Max(0, lanes));
        }

        public List<List<TrafficLight>> GetTrafficLightSequence()
        {
            return trafficLightConfig?.GetTrafficLightSequence();
        }
    }
}