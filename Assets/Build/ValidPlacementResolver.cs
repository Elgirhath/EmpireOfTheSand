using Map;
using UnityEngine;

namespace Build
{
    public class ValidPlacementResolver
    {

        public static bool IsPlacementValid(Vector2Int position)
        {
            var tileMatrix = GameMap.Instance.Matrix;

            var cell = tileMatrix.GetCell(position);

            if (cell.structure != null) return false;
            if (cell.type == TileType.Water) return false;

            return true;
        }
    }
}