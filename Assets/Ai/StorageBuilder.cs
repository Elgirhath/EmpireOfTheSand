using System.Linq;
using Build;
using Map;
using Units;
using UnityEngine;
using Util;

namespace Ai
{
    public class StorageBuilder : MonoBehaviour
    {
        public Building waterStoragePrefab;
        public Building sandStoragePrefab;

        public float minResourceDistanceToStorage;
        public int buildAreaRadius;

        public void BuildStorage(TileType type)
        {
            var prefab = type == TileType.Sand ? sandStoragePrefab : waterStoragePrefab;

            var color = GetComponent<AiManager>().playerColor;

            var basePosition = GetBasePosition(color);

            var closestResourceLocation = GetClosestResourceLocation(type, basePosition);

            var buildPosition = GetTargetBuildPosition(closestResourceLocation);

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

            return tileMatrix.EnumerateWithIndeces().Where(item => item.tile.type == type)
                .Where(item => GetDistanceToClosestStorage(item.tile) > minResourceDistanceToStorage)
                .ArgMin(item => Vector2.Distance(item.position, basePosition)).position;
        }

        private float GetDistanceToClosestStorage(CustomTile tile)
        {
            return GetComponent<Player>().GetBuildingParent().GetComponentsInChildren<Storage>()
                .Where(storage => storage.Type == tile.type)
                .Min(storage => Vector2.Distance(storage.Position, tile.position));
        }

        private Vector2Int GetTargetBuildPosition(Vector2Int resourcePosition)
        {
            var xMin = resourcePosition.x - buildAreaRadius;
            var yMin = resourcePosition.y - buildAreaRadius;
            var xMax = resourcePosition.x + buildAreaRadius;
            var yMax = resourcePosition.y + buildAreaRadius;

            return RandomBuildingPositionProvider.DrawRandomPosition(xMin, yMin, xMax, yMax);
        }
    }
}