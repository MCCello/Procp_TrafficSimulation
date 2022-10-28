using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCPTestAppTiles.utils.astar;
using System.Drawing;
using System.IO;
using ProCPTestAppTiles.orm;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.simulation.entities.simulation.simulationmap;

namespace CarSimulation.UnitTests.simulation.astar
{
    [TestClass]
    public class AStarTests
    {
        private Simulation simulation;

        private void InitSimulation(int number)
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/resources/SimulationMap" + number;
            var mapCreator = ORMManager.LoadMapCreator(path);
            simulation = new Simulation(mapCreator, null, Point.Empty);
        }
        
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            simulation = null;
        }

        [TestMethod]
        public void TestAStar_PathFinding_Test1()
        {
            InitSimulation(1);
            var tiles = simulation.simulationMap.tiles;
            
            var aStarPathFinder = new AStarPathFinder(new Simulation {simulationMap = new SimulationMap {tiles = tiles}});
            var nodeList = aStarPathFinder.FindPath(tiles[0, 0], tiles[3, 3]);
            Assert.IsTrue(nodeList.Count == 7);
        }
        
        [TestMethod]
        public void TestAStar_PathFinding_Test2()
        {
            InitSimulation(2);
            var tiles = simulation.simulationMap.tiles;
            
            var aStarPathFinder = new AStarPathFinder(new Simulation {simulationMap = new SimulationMap {tiles = tiles}});
            var nodeList = aStarPathFinder.FindPath(tiles[0, 2], tiles[3, 4]);
            Assert.IsTrue(nodeList.Count == 10);
        }
        
        [TestMethod]
        public void TestAStar_PathFinding_Test3()
        {
            InitSimulation(3);
            var tiles = simulation.simulationMap.tiles;
            
            var aStarPathFinder = new AStarPathFinder(new Simulation {simulationMap = new SimulationMap {tiles = tiles}});
            var nodeList = aStarPathFinder.FindPath(tiles[0, 0], tiles[2, 0]);
            Assert.IsTrue(nodeList.Count == 11);
        }
        
        [TestMethod]
        public void TestAStar_PathFinding_Test4()
        {
            InitSimulation(4);
            var tiles = simulation.simulationMap.tiles;
            
            var aStarPathFinder = new AStarPathFinder(new Simulation {simulationMap = new SimulationMap {tiles = tiles}});
            var nodeList = aStarPathFinder.FindPath(tiles[0, 4], tiles[0, 0]);
            Assert.IsTrue(nodeList.Count == 9);
        }
    }
}