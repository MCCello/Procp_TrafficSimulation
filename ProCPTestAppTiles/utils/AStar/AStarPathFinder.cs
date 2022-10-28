using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.simulation;
using ProCPTestAppTiles.utils.tile;

namespace ProCPTestAppTiles.utils.astar
{
    public class AStarPathFinder
    {
        private List<Node> allNodes { get; set; }
        private Simulation simulation { get; set; }

        private List<Node> openNodes { get; set; }
        private List<Node> closedNodes { get; set; }

        private Node startNode { get; set; }
        private Node endNode { get; set; }
        private bool found { get; set; }

        public AStarPathFinder(Simulation simulation)
        {
            this.simulation = simulation;
            allNodes = new List<Node>();
            openNodes = new List<Node>();
            closedNodes = new List<Node>();
        }

        public List<Node> FindPath(Tile startTile, Tile endTile)
        {
            // Populate & Initialise the Node Grid
            if (allNodes == null || allNodes.Count == 0)
            {
                InitAllNodes();
            }

            startNode = GetNodeByTile(startTile);
            endNode = GetNodeByTile(endTile);

            if (startNode == null || endNode == null)
            {
                Debug.WriteLine("StartNode or EndNode is null");
                return null;
            }

            DiscoverNode(startNode, null);
            Node curNode = null;
            while (openNodes.Count > 0)
            {
                curNode = GetBestNode();
                if (curNode.Equals(endNode))
                {
                    break;
                }
                ExploreNode(curNode);
                if (found)
                {
                    curNode = endNode;
                    break;
                }
            }

            return CreateNodeList(curNode);
        }
        
        
        private List<Node> CreateNodeList(Node node)
        {
            List<Node> nodeList = new List<Node> { node };
            var curNode = node;
            while (curNode.cameFrom != null)
            {
                nodeList.Add(curNode.cameFrom);
                curNode = curNode.cameFrom;
            }

            nodeList.Reverse();
            return nodeList;
        }

        private Node GetBestNode()
        {
            if (openNodes == null || openNodes.Count == 0)
            {
                return null;
            }

            if (openNodes.Count > 1)
            {
                openNodes.Sort();
            }
            
            return openNodes[0];
        }

        private void ExploreNode(Node node)
        {
            // Place node in closedNode list
            if (!closedNodes.Contains(node))
            {
                closedNodes.Add(node);
            }

            // Remove node from openNode list
            if (openNodes.Contains(node))
            {
                openNodes.Remove(node);
            }
            
            // Check surroundings
            CheckSurroundings(node);
        }

        private void CheckSurroundings(Node node)
        {
            var tiles = GetTiles();
            if (tiles == null)
            {
                Debug.WriteLine("Tiles returned null");
                return;
            }

            var connectedNodes = TileUtils.GetAllConnectedTiles(GetTiles(), node.tile)
                .Select(GetNodeByTile).ToList();

            foreach (var connectedNode in connectedNodes)
            {
                if (connectedNode.Equals(endNode))
                {
                    found = true;
                }
                DiscoverNode(connectedNode, node);
            }
        }

        private void DiscoverNode(Node node, Node cameFrom)
        {
            var newGCost = cameFrom?.gCost + 10 ?? 0;
            var newHCost = CalculateHCost(node);
            var newFCost = newGCost + newHCost;
            if (!openNodes.Contains(node) && !closedNodes.Contains(node) || newFCost < node.GetFCost())
            {
                node.Discover(cameFrom, newHCost);
            }
            
            if (!openNodes.Contains(node) && !closedNodes.Contains(node))
            {
                openNodes.Add(node);
            }
        }

        private void InitAllNodes()
        {
            PopulateAllNodes();
        }

        private void PopulateAllNodes()
        {
            var tiles = GetTiles();
            if (tiles == null)
            {
                Debug.WriteLine("Tiles returned null");
                return;
            }

            foreach (var tile in tiles)
            {
                var node = new Node(tile);
                allNodes.Add(node);
            }
        }

        private Tile[,] GetTiles()
        {
            return simulation?.simulationMap?.tiles;
        }

        private Node GetNodeByTile(Tile tile)
        {
            if (allNodes == null || allNodes.Count == 0)
            {
                Debug.WriteLine("Trying to retrieve Node from Tile whilst 'allNodes' is null or empty");
                return null;
            }
            return allNodes.First(n => n.tile.Equals(tile));
        }

        private int CalculateHCost(Node node)
        {
            var tiles = GetTiles();
            
            var curNodeCoordinates = Utils.CoordinatesOf(tiles, node.tile);
            var endNodeCoordinates = Utils.CoordinatesOf(tiles, endNode.tile);
            
            var endNodeX = endNodeCoordinates.Item2;
            var endNodeY = endNodeCoordinates.Item1;
            
            var curNodeX = curNodeCoordinates.Item2;
            var curNodeY = curNodeCoordinates.Item1;

            var xDiff = Math.Abs(endNodeX - curNodeX) * 10;
            var yDiff = Math.Abs(endNodeY - curNodeY) * 10;

            return (int) Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
        }
    }
}