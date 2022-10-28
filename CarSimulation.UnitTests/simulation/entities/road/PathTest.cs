using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;

namespace CarSimulation.UnitTests.simulation.entities.road
{
    [TestClass]
    public class PathTest
    {
        private Tile tile;
        private Path path;
        private List<RoadPosition> roadPositions;

        
        /// <summary>
        /// Ran before every test. To initialise the context to be tested.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // arrange
            tile = new Tile();
            roadPositions = new List<RoadPosition>
            {
                new RoadPosition(10, 10),
                new RoadPosition(20, 20),
                new RoadPosition(30, 30),
                new RoadPosition(40, 40),
                new RoadPosition(50, 50),
                new RoadPosition(60, 60),
                new RoadPosition(70, 70),
            };
            path = new Path(tile, roadPositions);
        }

        
        /// <summary>
        /// Ran after every test. To clean up any manipulation done by the test, and start anew.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            tile = null;
            path = null;
        }
        
        
        [TestMethod]
        public void Start_NotStarting_ReturnNull()
        {
            // arrange
            path = new Path(tile, new List<RoadPosition>());
            
            // act
            var pathStart = path.Start();
            
            // assert
            Assert.IsNull(pathStart); 
        }
        
        
        [TestMethod]
        public void End_NotEnd_ReturnNull()
        {
            // arrange
            path = new Path(tile, new List<RoadPosition>());
            
            // act
            var pathEnd = path.End();
            
            // assert
            Assert.IsNull(pathEnd); 
        }

        
        [TestMethod]
        public void Start_Starting_ReturnRoadPosition()
        {
            // act
            var pathStart = path.Start();
            var roadPositionStart = roadPositions[0];
            
            // assert
            Assert.AreEqual(pathStart, roadPositionStart);
        }

        
        [TestMethod]
        public void End_Ending_ReturnRoadPosition()
        {
            // act
            var pathEnd = path.End();
            var roadPositionEnd = roadPositions[roadPositions.Count - 1];
            
            // assert
            Assert.AreEqual(pathEnd, roadPositionEnd);
        }
    }
}
