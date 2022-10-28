using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ProCPTestAppTiles.simulation.entities;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.road;


namespace ProCPTestAppTiles.utils
{
    public class Utils
    {
        public static Random random = new Random();

        /// Code taken from:
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        

        public static Size GetCorrectSize(Control control)
        {
            var maxY = 0;
            var maxX = 0;
            foreach (var subControl in control.Controls)
            {
                maxY = Math.Max(maxY, ((Control) subControl).Bottom);
                maxX = Math.Max(maxX, ((Control) subControl).Right);
            }
            
            return new Size(maxX, maxY);
        }
        
        
        /// <summary>
        /// Returns a random number. uBound is Excl
        /// </summary>
        /// <param name="lBound"></param>
        /// <param name="uBound"></param>
        /// <returns></returns>
        public static int GetRandom(int lBound, int uBound)
        {
            if (lBound == uBound)
            {
                return lBound;
            }

            if (lBound > uBound)
            {
                var temp = uBound;
                uBound = lBound;
                lBound = temp;
            }
            return random.Next(lBound, uBound);
        }


        /// <summary>
        /// Checks if some action succeeds, given a chance out of 'max'. Which default is 100.
        /// </summary>
        /// <param name="chance"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool SucceedProp(int chance, int max = 100)
        {
            return random.Next(max) < chance;
        }
        

        /// <summary>
        /// Retrieves Coordinates of an object within a 2D Array. And returns the values as a Tuple
        /// </summary>
        /// <param name="array2D"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Tuple<int, int> CoordinatesOf<T>(T[,] array2D, T value)
        {
            for (int y = 0; y < array2D.GetLength(0); y++)
            {
                for (int x = 0; x < array2D.GetLength(1); x++)
                {
                    if (array2D[y, x].Equals(value))
                    {
                        return Tuple.Create(y, x);
                    }
                }
            }

            return null;
        }
        
        
        /// <summary>
        /// Rotates all the roadPositions by a certain amount (angle) from the center.
        /// </summary>
        /// <param name="roadPositions"></param>
        /// <param name="angle"></param>
        /// <param name="center"></param>
        public static void RotateRoadPositions(List<RoadPosition> roadPositions, float angle, Point center)
        {
            roadPositions.ForEach(r => RotateRoadPosition(r, angle, center));
        }

        
        /// <summary>
        /// Rotate a RoadPosition by a certain amount (angle) from the center.
        /// </summary>
        /// <param name="roadPosition"></param>
        /// <param name="angle"></param>
        /// <param name="center"></param>
        public static void RotateRoadPosition(RoadPosition roadPosition, float angle, Point center)
        {
            PointF[] points = {roadPosition.position.ToPoint()};
            using (Matrix m = new Matrix())
            {
                m.RotateAt(angle, center);
                m.TransformPoints(points);
                roadPosition.position.FromPoint(points[0]);
            }
        }
        
        
        /// <summary>
        /// Used with Dictionaries. Returns the value of (key). If key doesn't exist, return default (def).
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key, TValue def)
        {
            return dict.ContainsKey(key) ? dict[key] : def;
        }
        
        
        /// <summary>
        /// Returns a random item from the <param name="list"></param> given.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Random item within the list.</returns>
        public static T GetRandomFromCollection<T>(IList<T> list)
        {
            if (list != null && list.Count > 0)
            {
                return list[random.Next(list.Count)];
            }

            return default(T);
        }

       
        // Adjusting the Brightness of an Image.
        // Code taken from:
        // https://www.codeproject.com/Questions/243422/Csharp-Image-Processing-Brightness-Adjustment-Trac
        /// <summary>
        /// Adjust brightness of Image <param name="Image"></param> by <param name="Value"></param>
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Value"></param>
        /// <returns>Adjusted Bitmap</returns>
        public static Bitmap AdjustBrightness(Bitmap Image, int Value)
        {
            Bitmap TempBitmap = Image;
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            float FinalValue = Value / 255.0f;

            float[][] FloatColorMatrix ={
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
            };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, 
                TempBitmap.Width, TempBitmap.Height), 0, 0, 
                TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, 
                Attributes);
            
            Attributes.Dispose();
            NewGraphics.Dispose();

            return NewBitmap;
        }


        /// <summary>
        /// Calculates and Returns the distance between two positions using a straight line.
        /// </summary>
        /// <param name="fPos"></param>
        /// <param name="tPos"></param>
        /// <returns>Distance of a Straight line between 2 Positions</returns>
        public static double CalculateDistanceBetweenPoints(Position fPos, Position tPos)
        {
            var xDiff = Math.Abs(tPos.X - fPos.X);
            var yDiff = Math.Abs(tPos.Y - fPos.Y);

            return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
        }
        
        
        /// <summary>
        /// Calculates and Return the Angle between 2 positions with respect to fPos, using Math.Atan2.
        /// </summary>
        /// <param name="fPos"></param>
        /// <param name="tPos"></param>
        /// <returns>Angle of 2 Positions</returns>
        public static double GetAngleOfTwoPositions(Position fPos, Position tPos)
        {
            // TODO -> Fix  Math.Atan2
            // return 0;

            var fPosX = fPos.X;
            var fPosY = fPos.Y;
            var tPosX = tPos.X;
            var tPosY = tPos.Y;

            if (fPosX == tPosX && fPosY == tPosY)
            {
                // Positions are the same
                return -1337;
            }
            if (fPosX == tPosX)
            {
                // X is the same
                return tPosY - fPosY > 0 ? 0 : 180;
            }

            if (fPosY == tPosY)
            {
                // Y is the same
                return tPosX - fPosX > 0 ? 90 : 270;
            }

            var dX = tPosX - fPosX;
            var dY = tPosY - fPosY;
            
            var angle = Math.Atan2(dY, dX) * (180.0 / Math.PI);

            return angle;
        }

        
    }
}