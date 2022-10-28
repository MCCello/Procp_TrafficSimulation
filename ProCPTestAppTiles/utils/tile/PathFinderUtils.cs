using System.Collections.Generic;
using ProCPTestAppTiles.enums;
using ProCPTestAppTiles.simulation.entities.life;
using ProCPTestAppTiles.simulation.entities.mapcreator.board.tile;
using ProCPTestAppTiles.simulation.entities.paths;
using ProCPTestAppTiles.utils.astar;

namespace ProCPTestAppTiles.utils.tile
{
    public class PathFinderUtils
    {
        public static List<Path> GetNextPathByAStar(Life life, List<Node> nodeList, Tile[,] tiles)
        {
            var tile = nodeList[0].tile; // current Tile
            if (nodeList.Count > 1)
            {
                var nextNode = nodeList[1];
                var directionTypeFromCurToNextTile =
                    TileUtils.GetDirectionTypeByNextTile(tiles, tile, nextNode.tile);

                if (directionTypeFromCurToNextTile == null)
                {
                    return null;
                }

                var nextPaths = nextNode.tile.GetPathsByDirectionType((DirectionType) directionTypeFromCurToNextTile,
                    (DirectionType) life.endingPath.GetDirectForPathPerFlowType(FlowType.OUTFLOW));
                
                
                if (nodeList.Count > 2)
                {
                    var directionTypeFromNextToNextNextTile =
                        TileUtils.GetDirectionTypeByNextTile(tiles, nextNode.tile, nodeList[2].tile);
                    
                    if (directionTypeFromNextToNextNextTile == null)
                    {
                        return null;
                    } 
                    nextPaths = nextNode.tile.GetPathsByDirectionType((DirectionType) directionTypeFromCurToNextTile,
                        (DirectionType) directionTypeFromNextToNextNextTile);
                }

                return nextPaths;
            }

            return null;
        }
    }
}