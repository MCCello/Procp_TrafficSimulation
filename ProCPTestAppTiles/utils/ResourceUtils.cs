using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using ProCPTestAppTiles.simulation.entities.road;

namespace ProCPTestAppTiles.utils
{
    public class ResourceUtils
    {
        private static Bitmap IMAGE_STRAIGHT_ROAD;
        private static Bitmap IMAGE_STRAIGHT_ROAD2;
        private static Bitmap IMAGE_STRAIGHT_ROAD3;

        private static Bitmap IMAGE_BENDING_ROAD;
        private static Bitmap IMAGE_BENDING_ROAD2;
        private static Bitmap IMAGE_BENDING_ROAD3;

        private static Bitmap IMAGE_T_SECTION_ROAD;
        private static Bitmap IMAGE_T_SECTION_ROAD2;
        private static Bitmap IMAGE_T_SECTION_ROAD3;

        private static Bitmap IMAGE_INTERSECTION_ROAD;
        private static Bitmap IMAGE_INTERSECTION_ROAD2;
        private static Bitmap IMAGE_INTERSECTION_ROAD3;

        private static Bitmap IMAGE_TESTING_CROSSING;
        
        
        /// <summary>
        /// Initialises ResourceUtils and assigning values to the available static variables of the class.
        /// </summary>
        public static void InitResourceUtils()
        {
            var assembly = Assembly.GetExecutingAssembly();
            #region Straight tile
            // Straight Tile 1 Lane
            Stream straight = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.StraightTile.png");
            IMAGE_STRAIGHT_ROAD = new Bitmap(straight);

            // Straight Tile 2 Lanes
            Stream straight2 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.StraightTile2.png");
            IMAGE_STRAIGHT_ROAD2 = new Bitmap(straight2);

            // Straight Tile 3 Lanes
            Stream straight3 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.StraightTile3.png");
            IMAGE_STRAIGHT_ROAD3 = new Bitmap(straight3);
            #endregion

            #region Bend Tile
            // Bend Tile 1 Lane
            Stream bend = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.BendTile.png");
            IMAGE_BENDING_ROAD = new Bitmap(bend);
            //Bend Tile 2 Lanes
            Stream bend2 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.BendTile2.png");
            IMAGE_BENDING_ROAD2 = new Bitmap(bend2);
            //Bend Tile 3 Lanes
            Stream bend3 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.BendTile3.png");
            IMAGE_BENDING_ROAD3 = new Bitmap(bend3);
            #endregion

            #region T Section Tile
            // T Junction 1 Lane
            Stream tJunction = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.T_JunctionTile.png");
            IMAGE_T_SECTION_ROAD = new Bitmap(tJunction);
            //T Junction 2 Lanes
            Stream tJunction2 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.T_JunctionTile2.png");
            IMAGE_T_SECTION_ROAD2 = new Bitmap(tJunction2);
            //T Junction 3 Lanes
            Stream tJunction3 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.T_JunctionTile3.png");
            IMAGE_T_SECTION_ROAD3 = new Bitmap(tJunction3);
            #endregion

            #region Intersection Tile
            // Intersection 1 Lane
            Stream intersection = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.IntersectionTile.png");
            IMAGE_INTERSECTION_ROAD = new Bitmap(intersection);
            //Intersection 2 Lanes
            Stream intersection2 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.IntersectionTile2.png");
            IMAGE_INTERSECTION_ROAD2 = new Bitmap(intersection2);
            //Intersection 3 Lanes
            Stream intersection3 = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.IntersectionTile3.png");
            IMAGE_INTERSECTION_ROAD3 = new Bitmap(intersection3);
            #endregion
            //RedLight
            // Stream redtraffic = assembly.GetManifestResourceStream()



            // TESTING
            Stream test = assembly.GetManifestResourceStream("ProCPTestAppTiles.resources.CrossingTest200x200.png");
            IMAGE_TESTING_CROSSING = new Bitmap(test);
        }

        
        public static Bitmap GetStraightRoadImage(int lanes)
        {
            if (IMAGE_STRAIGHT_ROAD == null)
            {
                InitResourceUtils();
            }
            if (lanes == 1) return IMAGE_STRAIGHT_ROAD;
            if (lanes == 2) return IMAGE_STRAIGHT_ROAD2;
            if (lanes == 3) return IMAGE_STRAIGHT_ROAD3;
            return null;
        }

        
        public static Bitmap GetBendingRoadImage(int lanes)
        {
            if (IMAGE_BENDING_ROAD == null)
            {
                InitResourceUtils();
            }
            if (lanes == 1) return IMAGE_BENDING_ROAD;
            if (lanes == 2) return IMAGE_BENDING_ROAD2;
            if (lanes == 3) return IMAGE_BENDING_ROAD3;
            return null; 
        }
        
        
        public static Bitmap GetTSectionRoadImage(int lanes)
        {
            if (IMAGE_T_SECTION_ROAD == null)
            {
                InitResourceUtils();
            }
            if (lanes == 1) return IMAGE_T_SECTION_ROAD;
            if (lanes == 2) return IMAGE_T_SECTION_ROAD2;
            if (lanes == 3) return IMAGE_T_SECTION_ROAD3;
            return null;
        }
        
        
        public static Bitmap GetIntersectionRoadImage(int lanes)
        {
            if (IMAGE_INTERSECTION_ROAD == null)
            {
                InitResourceUtils();
            }
            if (lanes == 1) return IMAGE_INTERSECTION_ROAD3;
            if (lanes == 2) return IMAGE_INTERSECTION_ROAD3;
            if (lanes == 3) return IMAGE_INTERSECTION_ROAD3;
            return null;
        }

        
        /// <summary>
        /// Gets the appropriate Image by the roadType given.
        /// Returns null if the roadType is not supported.
        /// </summary>
        /// <param name="roadType"></param>
        /// <returns>The correct Image as Bitmap</returns>
        public static Bitmap GetImageByRoadType(RoadType? roadType, int lanes=3)
        {
            switch (roadType)
            {
                case RoadType.STRAIGHT:
                    return GetStraightRoadImage(lanes);
                case RoadType.BEND:
                    return GetBendingRoadImage(lanes);
                case RoadType.T_SECTION:
                    return GetTSectionRoadImage(lanes);
                case RoadType.INTERSECTION:
                    return GetIntersectionRoadImage(lanes);
            }
            return null;
        }
        
        public static string GetXmlPathByRoadType(RoadType? roadType)
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/resources/";
            switch (roadType)
            {
                case RoadType.STRAIGHT:
                    return path + "StraightTile.xml";
                case RoadType.BEND:
                    return path + "BendTile.xml";
                case RoadType.T_SECTION:
                    return path + "T_JunctionTile.xml";
                case RoadType.INTERSECTION:
                    return path + "IntersectionTile.xml";
            }
            return null;
        }
    }
}