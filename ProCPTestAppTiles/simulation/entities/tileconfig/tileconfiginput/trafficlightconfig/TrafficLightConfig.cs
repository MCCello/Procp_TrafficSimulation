using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProCPTestAppTiles.forms.tileconfigform;
using ProCPTestAppTiles.forms.tileconfigform.tileconfiginputcontrol;
using ProCPTestAppTiles.forms.tileconfigform.tileconfiginputcontrol.trafficlightconfigcontrol;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;
using ProCPTestAppTiles.simulation.logiccontrolpattern;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ProCPTestAppTiles.simulation.entities.tileconfig.tileconfiginput.trafficlightconfig
{
    public class TrafficLightConfig : Attachable<TrafficLightConfigControl>
    {
        public Tile tile { get; set; }
        public List<TextBox> textBoxes { get; set; }
        
        public TrafficLightConfig(Tile tile, Control mommyControl, Point location) : base(mommyControl, location)
        {
            this.tile = tile;
            Init();
        }

        protected override void InitTControl()
        {
            control = new TrafficLightConfigControl(this);
        }

        public override void Init()
        {
            var x = 0;
            var y = 0;
            
            // Label
            var label = new Label {Text = @"Traffic Light Sequence", Size = new Size(200, 20), Location = new Point(x, y)};
            y += label.Size.Height;
            AddControl(label);
            
            
            // Textboxes
            textBoxes = new List<TextBox>
            {
                new TextBox()
            };
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
                textBox.Size = new Size(200, 20);
                textBox.Location = new Point(x, y);
                textBox.ReadOnly = true;
                y += textBox.Size.Height;
                textBox.Click += GetControl().TextBox_Click;
                AddControl(textBox);
            }
            
            UpdateSize();
        }

        public void ClickedLastTextBox()
        {
            if (textBoxes.Count < TrafficLightConfigConstants.MAX_TRAFFIC_LIGHT_ROW)
            {
                AddTextBox();
            }
        }

        public bool IsLastTextBox(TextBox textBox)
        {
            return textBox.Equals(textBoxes[textBoxes.Count - 1]);
        }

        public TextBox GetFocusedTextBox()
        {
            if (textBoxes.Count == 1)
            {
                return textBoxes[0];
            }
            return textBoxes.First(tb => tb.Focused);
        }

        private void AddTextBox()
        {
            var lastTextBox = textBoxes[textBoxes.Count - 1];
            var x = lastTextBox.Location.X;
            var y = lastTextBox.Bottom;

            var newTextBox = new TextBox {Size = new Size(200, 20), Location = new Point(x, y), ReadOnly = true};
            newTextBox.Click += GetControl().TextBox_Click;
            textBoxes.Add(newTextBox);
            AddControl(newTextBox);
            UpdateControl();
            UpdateSize();
        }

        public TileConfig GetTileConfig()
        {
            var tileConfigInputControl = mommyControl as TileConfigInputControl;
            var tileConfigControl = tileConfigInputControl?.GetLogic().mommyControl as TileConfigControl;

            return tileConfigControl?.GetLogic();
        }
        
        public List<List<TrafficLight>> GetTrafficLightSequence()
        {
            var trafficLightSequence = new List<List<TrafficLight>>();
            var tileConfig = GetTileConfig();
            var trafficLights = tileConfig.GetTrafficLights();
            
            foreach (var textBox in textBoxes)
            {
                var textParsed = textBox.Text.Split(',');
                if (textParsed.Length == 0)
                {
                    continue;
                }
                var trafficLightPerSequence = new List<TrafficLight>();
                foreach (var objectId in textParsed)
                {
                    var numberStr = objectId.Trim();
                    int number = int.TryParse(numberStr, out number) ? number : -1;
                    if (number == -1)
                    {
                        continue;
                    }

                    var trafficLight = trafficLights.Find(tf => tf.objectId == number);
                    if (trafficLight == null)
                    {
                        continue;
                    }

                    trafficLightPerSequence.Add(trafficLight);
                }

                if (trafficLightPerSequence.Count > 0)
                {
                    trafficLightSequence.Add(trafficLightPerSequence);
                }
            }

            return trafficLightSequence;
        }
    }
}