
using System;
using Assets.Building;
using Assets.Map;
using Assets.Units;
using Assets.Util;
using UnityEngine;
using Random = System.Random;

namespace Assets.Ai
{
    public static class RandomBuilder
    {
        private static readonly int randomFieldBoundExtends = 2;

        public static void Build(GameObject prefab, PlayerColor color)
        {
            var tileMatrix = GameMap.Instance.Matrix;

            var xMin = int.MaxValue;
            var yMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMax = int.MinValue;

            foreach (var item in tileMatrix.EnumerateWithIndeces())
            {
                var structure = item.tile.structure;
                if (structure == null) continue;
                if (structure.GetComponent<PlayerProperty>()?.playerColor != color) continue;

                var position = item.position;

                if (position.x < xMin) xMin = position.x;
                if (position.y < yMin) yMin = position.y;
                if (position.x > xMax) xMax = position.x;
                if (position.y > yMax) yMax = position.y;
            }

            var buildPosition = RandomBuildingPositionProvider.DrawRandomPosition(xMin - randomFieldBoundExtends, yMin - randomFieldBoundExtends, xMax + randomFieldBoundExtends, yMax + randomFieldBoundExtends);

            StructureBuildManager.Instance.Build(prefab, (Vector3Int) buildPosition);
        }
    }
}