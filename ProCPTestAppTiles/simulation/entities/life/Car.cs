using System.Drawing;
using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities.paths;

namespace ProCPTestAppTiles.simulation.entities.life
{
    public class Car : Life
    {
        // Draw Info
        public static int CAR_HEIGHT = 5; // 15;
        public static int CAR_WIDTH = 5; // 5;
        public Color CAR_COLOR = Color.Black;
        public static int CAR_PEN_WIDTH = 3;

        public Car()
        {
        }

        public Car(Path startingPath, Path endingPath) : base(startingPath, endingPath)
        {
        }

        public override void crash()
        {
            this.enabled = false;
            this.CAR_COLOR = Color.HotPink;
        }
        
        public void autoPech()
        {
            this.enabled = false;
            this.CAR_COLOR = Color.LimeGreen;
        }

        /// <summary>
        /// Draws car which is 'a' life.
        /// </summary>
        /// <param name="e"></param>
        public override void Draw(PaintEventArgs e)
        {
            using (var pen = new Pen(CAR_COLOR, CAR_PEN_WIDTH))
            {
                var position = PrepareDrawing();
                var g = e.Graphics;


                // TODO -> Figure out rotational translations needed to be performed to properly showcase cars moving
                // g.TranslateTransform((float) position.X + (CAR_WIDTH / 2), (float) position.Y + (CAR_HEIGHT / 2));
                // g.RotateTransform((float) Rotation);
                // g.TranslateTransform((float) -position.X - (CAR_WIDTH / 2), (float) -position.Y - (CAR_HEIGHT / 2));
                // g.DrawRectangle(pen, (float) position.X, (float) position.Y, CAR_WIDTH, CAR_HEIGHT);
                
                // Hotfix for not correctly drawing rotational translations. Draw a circle!
                g.DrawEllipse(pen, (float) position.X, (float) position.Y, CAR_WIDTH, CAR_HEIGHT);
            }
        }
    }
}