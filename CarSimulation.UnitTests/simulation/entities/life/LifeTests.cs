using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;

namespace CarSimulation.UnitTests.simulation.entities.life
{
    [TestClass]
    public class LifeTests
    {
        private Life life;
        private Path startPath;
        private Path endPath;
        private Tile startTile;
        private Tile endTile;

        
        /// <summary>
        /// Ran before every test. To initialise the context to be tested.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // arrange
            startTile = new Tile();
            startPath = new Path(startTile, new List<RoadPosition>
            {
                new RoadPosition(10, 10), 
                new RoadPosition(20, 20),
                new RoadPosition(30, 30),
                new RoadPosition(40, 40),
            });
            
            endTile = new Tile {Location = new Point(120, 120)};
            endPath = new Path(endTile, new List<RoadPosition>
            {
                new RoadPosition(50, 50),
                new RoadPosition(60, 60),
                new RoadPosition(70, 70),
            });
            
            life = new Car(startPath, endPath);
        }

        
        /// <summary>
        /// Ran after every test. To clean up any manipulation done by the test, and start anew.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            life = null;
            startPath = null;
            endPath = null;
            startTile = null;
            endTile = null;
        }
        
        
        /// <summary>
        /// Checking if the next RoadPosition is equal to the next one in the list and therefore works
        /// </summary>
        [TestMethod]
        public void GetNextRoadPosition_NextRoadPositionGiven_ReturnsObjectRoadPosition()
        {
            // act
            var nextRoadPosition = life.GetNextRoadPosition();
            var listRoadPosition = startPath.path[1];
            
            // assert
            Assert.AreEqual(listRoadPosition, nextRoadPosition);
        }
 

        [TestMethod]
        public void SetNewPath_NewPathIsCurrentPath_Succeeded()
        {
            // act
            life.SetNewPath(endPath);
            
            // assert
            Assert.AreEqual(endPath, life.currentPath);
        }


        [TestMethod]
        public void Move_CarIsNotMovingMoveToNextRoadPos_Succeeded()
        {
            // act
            life.Move();
            
            // assert
            Assert.IsTrue(life.IsMoving());
        }
        
        
        [TestMethod]
        public void Move_CarIsNotMoving_Fail()
        {
            // act
            
            // assert
            Assert.IsFalse(life.IsMoving());
        }

        
        [TestMethod]
        public void AddDistanceTraveled_AddedDistance_Succeeded()
        {
            // act
            life.AddDistanceTraveled(69);
            
            // assert
            Assert.AreEqual(life.distanceTraveled, 69);
        }
    }
}
