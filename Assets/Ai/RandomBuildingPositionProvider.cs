using Assets.Building;
using Assets.Map;
using UnityEngine;
using Random = System.Random;

namespace Assets.Ai
{
    public static class RandomBuildingPositionProvider
    {
        public static Vector2Int DrawRandomPosition(int xMin, int yMin, int xMax, int yMax)
        {
            var tileMatrix = GameMap.Instance.Matrix;

            var random = new Random();

            while (true)
            {
                var x = random.Next(xMin, xMax + 1);
                var y = random.Next(yMin, yMax + 1);

                var position = new Vector2Int(x, y);

                if (ValidPlacementResolver.IsPlacementValid(position))
                {
                    return position;
                }
            }
        }
    }
}