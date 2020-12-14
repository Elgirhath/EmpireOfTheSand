using System;
using UnityEngine;

namespace Assets.Map
{
    /// <summary>
    /// Provides utility for handling tilemap-space vectors, where tile's edges are of length 1 and are perpendicular to each other
    /// </summary>
    public static class TilemapVectorConverter
    {
        public static Vector2 TilemapXAxis { get; private set; }
        public static Vector2 TilemapYAxis { get; private set; }

        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }

        static TilemapVectorConverter()
        {
            InitializeTilemapAxis();
        }

        private static void InitializeTilemapAxis()
        {
            var refCellPosition = GameMap.Instance.GetCellCenterWorld(new Vector3Int(0, 1, 0));
            TilemapXAxis = GameMap.Instance.GetCellCenterWorld(new Vector3Int(1, 1, 0)) - refCellPosition;
            TilemapYAxis = GameMap.Instance.GetCellCenterWorld(new Vector3Int(0, 0, 0)) - refCellPosition;

            TileWidth = Mathf.Abs(TilemapXAxis.x * 2);
            TileHeight = Mathf.Abs(TilemapXAxis.y * 2);
        }

        public static Vector2 WorldToTilemapVector(Vector2 vector)
        {
            var b = (vector.y * TilemapXAxis.x - TilemapXAxis.y * vector.x) / (TilemapYAxis.y * TilemapXAxis.x - TilemapXAxis.y * TilemapYAxis.x);
            var a = (vector.x - TilemapYAxis.x * b) / TilemapXAxis.x;

            return new Vector2(a, b);
        }

        public static Vector2 TilemapToWorldVector(Vector2 vector)
        {
            return TilemapXAxis * vector.x + TilemapYAxis * vector.y;
        }

        public static Vector2 NormalizeRelativeToTilemap(Vector2 vector)
        {
            return TilemapToWorldVector(WorldToTilemapVector(vector).normalized);
        }
    }
}