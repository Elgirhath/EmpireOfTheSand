using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public class TileMatrix : IEnumerable
    {
        public class TilePositionPair
        {
            public CustomTile tile;
            public Vector2Int position;
        }

        public Vector2Int size;
        private CustomTile[,] tiles;
        private Vector2Int offset;

        public TileMatrix(BoundsInt bounds)
        {
            size = (Vector2Int)bounds.size;
            offset = -(Vector2Int)bounds.min;
            tiles = new CustomTile[size.x, size.y];
        }

        public static TileMatrix Generate(Tilemap[] tilemaps, TileTypeRetriever tileTypeRetriever)
        {
            var bounds = tilemaps[0].cellBounds;

            var tileMatrix = new TileMatrix(bounds);

            AssignTilesToCells(tileMatrix, tilemaps, tileTypeRetriever);

            AssignStructuresToCells(tileMatrix, tilemaps);

            return tileMatrix;
        }

        private static void AssignTilesToCells(TileMatrix matrix, Tilemap[] tilemaps, TileTypeRetriever tileTypeRetriever)
        {
            var bounds = tilemaps[0].cellBounds;

            foreach (var position in bounds.allPositionsWithin)
            {
                var worldPosition = tilemaps[0].GetCellCenterWorld(position);

                var positionOffsetted = (Vector2Int)position + matrix.offset;

                matrix.tiles[positionOffsetted.x, positionOffsetted.y] = new CustomTile
                {
                    position = worldPosition,
                    @base = null,
                    type = TileType.Default
                };

                foreach (var tilemap in tilemaps)
                {
                    var tile = tilemap.GetTile(position);

                    if (tile == null) continue;

                    matrix.tiles[positionOffsetted.x, positionOffsetted.y].@base = tile;
                    matrix.tiles[positionOffsetted.x, positionOffsetted.y].type = tileTypeRetriever.GetTileType(tile);
                }
            }
        }

        private static void AssignStructuresToCells(TileMatrix matrix, Tilemap[] tilemaps)
        {
            foreach (var childCollider in Player.GetPlayers().SelectMany(player => player.GetBuildings()).SelectMany(building => building.GetComponentsInChildren<Collider2D>()))
            {
                if (childCollider.isTrigger) continue;

                matrix.Assign(childCollider);
            }
        }

        public void Assign(GameObject gameObject)
        {
            foreach (var collider in gameObject.GetComponentsInChildren<Collider2D>())
            {
                if (collider.isTrigger) continue;

                Assign(collider);
            }
        }

        public void Assign(Collider2D collider)
        {
            var upCorner = collider.bounds.center + Vector3.up * collider.bounds.extents.y + Vector3.down * TilemapVectorConverter.TileHeight / 2f;
            var downCorner = collider.bounds.center + Vector3.down * collider.bounds.extents.y + Vector3.up * TilemapVectorConverter.TileHeight / 2f;

            var minTile = GameMap.Instance.GetCellPosition(downCorner);
            var maxTile = GameMap.Instance.GetCellPosition(upCorner);

            for (var x = minTile.x; x <= maxTile.x; x++)
            {
                for (var y = minTile.y; y <= maxTile.y; y++)
                {
                    var xOffset = x + offset.x;
                    var yOffset = y + offset.y;

                    if (xOffset < 0 || yOffset < 0 || xOffset > size.x || yOffset > size.y) continue;

                    tiles[xOffset, yOffset].structure = collider.gameObject;
                }
            }
        }

        private Vector2Int CellPositionToMatrixSpace(Vector2Int position)
        {
            return position + offset;
        }

        private Vector2Int CellPositionFromMatrixSpace(Vector2Int position)
        {
            return position - offset;
        }

        public IEnumerable<TilePositionPair> EnumerateWithIndeces()
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var worldSpacePosition = CellPositionFromMatrixSpace(new Vector2Int(x, y));
                    yield return new TilePositionPair
                    {
                        tile = tiles[x, y],
                        position = worldSpacePosition
                    };
                }
            }
        }

        public IEnumerator GetEnumerator() => tiles.GetEnumerator();


        public CustomTile GetCell(Vector2Int position)
        {
            var matrixSpacePosition = CellPositionToMatrixSpace(position);
            return tiles[matrixSpacePosition.x, matrixSpacePosition.y];
        }
    }
}