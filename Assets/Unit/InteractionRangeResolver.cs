using Assets.Map;
using UnityEngine;

namespace Assets.Unit
{
    public class InteractionRangeResolver
    {
        private static InteractionRangeResolver instance;
        public static InteractionRangeResolver Instance => instance ?? (instance = new InteractionRangeResolver());

        public bool IsPointInInteractionRange(Vector3Int tilePosition, Vector3 point, float range)
        {
            var tileCenterPoint = GameMap.Instance.GetCellCenterWorld(tilePosition);

            tileCenterPoint.z = point.z; // discard z

            // get distance along tilemap's axis
            var decomposedVector = TilemapVectorConverter.WorldToTilemapVector(point - tileCenterPoint);

            var maxDistanceTilemapX = 0.5f + range;
            var maxDistanceTilemapY = 0.5f + range;

            return !(Mathf.Abs(decomposedVector.x) > maxDistanceTilemapX || Mathf.Abs(decomposedVector.y) > maxDistanceTilemapY);
        }
    }
}