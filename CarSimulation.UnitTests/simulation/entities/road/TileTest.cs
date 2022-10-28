using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.road;

namespace CarSimulation.UnitTests.simulation.entities.road
{
    [TestClass]
    public class TileTest
    {
        private Tile tile;

        
        /// <summary>
        /// Ran before every test. To initialise the context to be tested.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            tile = new Tile();
        }

        
        /// <summary>
        /// Ran after every test. To clean up any manipulation done by the test, and start anew.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            tile = null;
        }
        
        
        [TestMethod]
        [DataRow(RoadType.STRAIGHT)]
        [DataRow(RoadType.BEND)]
        [DataRow(RoadType.T_SECTION)]
        [DataRow(RoadType.INTERSECTION)]
        public void SetRoadType_RoadTypeIsSet_Succeeded(RoadType roadType)
        {
            // act
            tile.SetRoadType(roadType);
            
            // assert
            Assert.AreEqual(tile.roadType, roadType);
        }
    }

}
