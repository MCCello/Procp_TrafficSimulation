using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCPTestAppTiles.simulation.entities.road;
using ProCPTestAppTiles.simulation.entities.road.trafficlight;

namespace CarSimulation.UnitTests.simulation.entities.road
{
    [TestClass]
    public class TrafficLightTests
    {
        //methodname_scenario_expectedbehavior()

        private RoadPosition roadPosition;
        private TrafficLight trafficLight;
        
        
        /// <summary>
        /// Ran before every test. To initialise the context to be tested.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            roadPosition = new RoadPosition(40, 40);
            trafficLight = new TrafficLight();
        }


        /// <summary>
        /// Ran after every test. To clean up any manipulation done by the test, and start anew.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            roadPosition = null;
            trafficLight = null;
        }
        

        [TestMethod]
        public void SetRoadPosition_RoadPositionIsSet_Succeeds()
        {
            // act
            trafficLight.SetRoadPosition(roadPosition);
            
            // assert 
            Assert.AreEqual(roadPosition, trafficLight.RoadPosition);
        }
        
        
        [TestMethod]
        public void SetRoadPosition_TrafficLightIsSet_Succeeds()
        {
            // act
            trafficLight.SetRoadPosition(roadPosition);
            
            // assert 
            Assert.AreEqual(trafficLight, roadPosition.TrafficLight);
        }


        [TestMethod]
        public void IsGreen_TrafficLightIsGreen_ReturnsTrue()
        {
            //act
            trafficLight.TurnGreen();
            
            //Assert
            Assert.IsTrue(trafficLight.IsGreen());
        }
        
        
        [TestMethod]
        public void IsRed_TrafficLightIsRed_ReturnsTrue()
        {
            //act
            trafficLight.TurnRed();
            
            //Assert
            Assert.IsFalse(trafficLight.IsGreen());
        }
    }
}
