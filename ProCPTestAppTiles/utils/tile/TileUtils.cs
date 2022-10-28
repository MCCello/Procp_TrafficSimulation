using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.simulation.entities;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.simulation.entities.road;

namespace ProCPTestAppTiles.utils.tile
{
    public static class TileUtils
    {

        public static Tile GetTileAboveThisTile(Tile[,] tiles, Tile tile)
        {
            return GetTileTranslationFromThisTile(tiles, tile, DirectionType.UP);
        }


        public static Tile GetTileBelowThisTile(Tile[,] tiles, Tile tile)
        {
            return GetTileTranslationFromThisTile(tiles, tile, DirectionType.DOWN);
        }


        public static Tile GetTileLeftThisTile(Tile[,] tiles, Tile tile)
        {
            return GetTileTranslationFromThisTile(tiles, tile, DirectionType.LEFT);
        }


        public static Tile GetTileRightThisTile(Tile[,] tiles, Tile tile)
        {
            return GetTileTranslationFromThisTile(tiles, tile, DirectionType.RIGHT);
        }


        public static Tile GetTileTranslationFromThisTile(Tile[,] tiles, Tile tile, DirectionType directionType)
        {
            var indexOfTile = Utils.CoordinatesOf(tiles, tile);
            var indexOfTileY = indexOfTile.Item1;
            var indexOfTileX = indexOfTile.Item2;
            switch (directionType)
            {
                case DirectionType.UP:
                    indexOfTileY--;
                    break;
                case DirectionType.DOWN:
                    indexOfTileY++;
                    break;
                case DirectionType.LEFT:
                    indexOfTileX--;
                    break;
                case DirectionType.RIGHT:
                    indexOfTileX++;
                    break;
            }

            if (indexOfTileX < 0 || indexOfTileX >= tiles.GetLength(1) || indexOfTileY < 0 ||
                indexOfTileY >= tiles.GetLength(0))
            {
                return null;
            }

            return tiles[indexOfTileY, indexOfTileX];
        }


        public static List<DirectionType> GetAllDirectionTypesByTile(Tile tile)
        {
            var directionTypes = new List<DirectionType>();
            foreach (var roadPosition in tile.GetRoadPositionGrid())
            {
                var x = roadPosition.position.X;
                var y = roadPosition.position.Y;

                if (Math.Abs(x - tile.Location.X) < 2)
                {
                    directionTypes.Add(DirectionType.LEFT);
                }

                if (Math.Abs(y - tile.Location.Y) < 2)
                {
                    directionTypes.Add(DirectionType.UP);
                }

                if (Math.Abs(x - (tile.Location.X + tile.Size.Width)) < 2)
                {
                    directionTypes.Add(DirectionType.RIGHT);
                }

                if (Math.Abs(y - (tile.Location.Y + tile.Size.Height)) < 2)
                {
                    directionTypes.Add(DirectionType.DOWN);
                }
            }

            return directionTypes.Distinct().ToList();
        }


        public static List<Tile> GetAllConnectedTiles(Tile[,] tiles, Tile tile)
        {
            var tileList = new List<Tile>();
            var directionTypes = GetAllDirectionTypesByTile(tile);

            foreach (var directionType in directionTypes)
            {
                var availableTile = GetTileTranslationFromThisTile(tiles, tile, directionType);
                if (availableTile == null)
                {
                    continue;
                }

                tileList.Add(availableTile);
            }

            return tileList;
        }


        public static DirectionType? GetDirectionTypeByNextTile(Tile[,] tiles, Tile currentTile, Tile nextTile)
        {
            var coordinateCurrTile = Utils.CoordinatesOf(tiles, currentTile);
            var coordinateNextTile = Utils.CoordinatesOf(tiles, nextTile);

            var coordinateCurrTileX = coordinateCurrTile.Item2;
            var coordinateCurrTileY = coordinateCurrTile.Item1;

            var coordinateNextTileX = coordinateNextTile.Item2;
            var coordinateNextTileY = coordinateNextTile.Item1;

            var diffX = coordinateNextTileX - coordinateCurrTileX;
            var diffY = coordinateNextTileY - coordinateCurrTileY;

            if (diffX == -1 && diffY == 0)
            {
                return DirectionType.LEFT;
            }

            if (diffX == 1 && diffY == 0)
            {
                return DirectionType.RIGHT;
            }

            if (diffX == 0 && diffY == -1)
            {
                return DirectionType.UP;
            }

            if (diffX == 0 && diffY == 1)
            {
                return DirectionType.DOWN;
            }

            return null;
        }


        public static Tuple<RoadPosition, Path> GetNearestRoadPosition(RoadPosition position, Tile tile, Path path,
            Tile[,] tiles)
        {
            bool isVertical = path.GetDirectForPathPerFlowType(FlowType.OUTFLOW) == DirectionType.UP ||
                              path.GetDirectForPathPerFlowType(FlowType.OUTFLOW) == DirectionType.DOWN;

            var pos = position.position;
            var pathsInTileWithSameInflow = tile.GetPaths().Where(p =>
                p.GetDirectForPathPerFlowType(FlowType.INFLOW)
                    .Equals(path.GetDirectForPathPerFlowType(FlowType.INFLOW))).ToList();
            var pathsInTileWithSameOutflow = tile.GetPaths().Where(p =>
                p.GetDirectForPathPerFlowType(FlowType.OUTFLOW)
                    .Equals(path.GetDirectForPathPerFlowType(FlowType.OUTFLOW))).ToList();
            var pathOutFlowDirection = path.GetDirectForPathPerFlowType(FlowType.OUTFLOW);
            var width = 12;
            var height = 8;
            var rect = new Rect(pos.X - (width / 2), pos.Y - height - 2, width, height);
            var availableRoadPositions = new List<Tuple<RoadPosition, Path>>();

            if (pathOutFlowDirection != null)
            {
                var nextTile = GetTileTranslationFromThisTile(tiles, tile, (DirectionType) pathOutFlowDirection);
                if (nextTile != null)
                {
                    pathsInTileWithSameInflow.AddRange(nextTile.GetPathsByDirectionType(
                        (DirectionType) pathOutFlowDirection, (DirectionType) pathOutFlowDirection, true));
                }
            }

            switch (path.GetDirectForPathPerFlowType(FlowType.OUTFLOW))
            {
                case DirectionType.UP:
                    rect = new Rect(pos.X - (width / 2), pos.Y - height - 2, width, height);
                    break;
                case DirectionType.DOWN:
                    rect = new Rect(pos.X - (width / 2), pos.Y + height + 2, width, height);
                    break;
                case DirectionType.LEFT:
                    width = 8;
                    height = 12;
                    rect = new Rect(pos.X + width - 2, pos.Y - (height / 2), width, height);
                    break;
                case DirectionType.RIGHT:
                    width = 8;
                    height = 12;
                    rect = new Rect(pos.X + 2, pos.Y - (height / 2), width, height);
                    break;
            }

            foreach (var p in pathsInTileWithSameInflow)
            {
                if (p.Equals(path))
                {
                    continue;
                }

                var inRect = p.path.Where(pp => rect.Contains(pp.position.X, pp.position.Y) && !pp.HasLife()).ToList();
                foreach (var roadPosition in inRect)
                {
                    availableRoadPositions.Add(Tuple.Create(roadPosition, p));
                }
            }

            if (availableRoadPositions.Count <= 0)
            {
                return null;
            }

            var bestRoadPosition =
                availableRoadPositions
                    .OrderBy(t => isVertical ? t.Item1.position.Y : t.Item1.position.X)
                    .Last();

            return bestRoadPosition;
        }
    }
}