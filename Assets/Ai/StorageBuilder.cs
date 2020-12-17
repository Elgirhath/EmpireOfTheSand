using System.Linq;
using Build;
using Map;
using Units;
using UnityEngine;

namespace Ai
{
    public class StorageBuilder : MonoBehaviour
    {
        public Building waterStoragePrefab;
        public Building sandStoragePrefab;

        public void BuildStorage(TileType type)
        {
            var prefab = type == TileType.Sand ? sandStoragePrefab : waterStoragePrefab;

            var color = GetComponent<AiManager>().playerColor;

            var basePosition = GetBasePosition(color);

            var closestResourceLocation = GetClosestResourceLocation(type, basePosition);

            var buildPosition = GetTargetBuildPosition(basePosition, closestResourceLocation);

            GetComponent<AiBuildingManager>().Build(prefab, buildPosition);
        }

        private Vector2Int GetBasePosition(PlayerColor color)
        {
            return GameMap.Instance.Matrix.EnumerateWithIndeces().Where(tuple => tuple.tile?.structure?.CompareTag("Base") == true)
                .Where(tuple => tuple.tile.structure.GetComponent<PlayerProperty>().playerColor == color).Select(tuple => tuple.position).First();
        }

        private Vector2Int GetClosestResourceLocation(TileType type, Vector2Int basePosition)
        {
            var tileMatrix = GameMap.Instance.Matrix;

            Vector2Int closestResourceLocation = default;
            float closestResourceDistance = Mathf.Infinity;

            foreach (var item in tileMatrix.EnumerateWithIndeces())
            {
                if (item.tile.type != type) continue;

                var distance = Vector2.Distance(item.position, basePosition);

                if (distance < closestResourceDistance)
                {
                    closestResourceLocation = item.position;
                    closestResourceDistance = distance;
                }
            }

            return closestResourceLocation;
        }

        private Vector2Int GetTargetBuildPosition(Vector2Int basePosition, Vector2Int resourcePosition)
        {
            var extend = 2;

            var xMin = Mathf.Min(resourcePosition.x - extend, basePosition.x);
            var yMin = Mathf.Min(resourcePosition.y - extend, basePosition.y);
            var xMax = Mathf.Max(resourcePosition.x + extend, basePosition.x);
            var yMax = Mathf.Max(resourcePosition.y + extend, basePosition.y);

            return RandomBuildingPositionProvider.DrawRandomPosition(xMin, yMin, xMax, yMax);
        }
    }
}