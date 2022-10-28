using System;
using System.Drawing;

namespace ProCPTestAppTiles.simulation.entities
{
    public class Position
    {
        private double x;
        private double y;

        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Position()
        {
            this.x = 0;
            this.y = 0;
        }

        public double X
        {
            get => x;
            set => x = value;
        }
        
        public double Y
        {
            get => y;
            set => y = value;
        }

        /// <summary>
        /// Translate the 'this' Position by 'position'. In both x and y dimensions.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Returns the resulting Position as a new Position</returns>
        public Position Add(Position position)
        {
            return new Position(X + position.X, Y + position.Y);
        }

        public void AddWith(Position position)
        {
            X += position.X;
            Y += position.Y;
        }

        public PointF ToPoint()
        {
            return new PointF((float) x, (float) y);
        }

        public void FromPoint(PointF point)
        {
            x = Math.Round(point.X);
            y = Math.Round(point.Y);
        }
        
        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}