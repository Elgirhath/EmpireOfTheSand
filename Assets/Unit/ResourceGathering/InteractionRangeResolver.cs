using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Unit.ResourceGathering
{
    public class InteractionRangeResolver
    {
        private Tilemap tilemap;
        private Vector3 tilemapRightVector;
        private Vector3 tilemapUpVector;

        public InteractionRangeResolver(Tilemap tilemap)
        {
            this.tilemap = tilemap;

            var refCellPosition = tilemap.GetCellCenterWorld(new Vector3Int(0, 1, 0));
            tilemapRightVector = tilemap.GetCellCenterWorld(new Vector3Int(1, 1, 0)) - refCellPosition;
            tilemapUpVector = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0)) - refCellPosition;
        }

        public bool IsPointInInteractionRange(Vector3Int tilePosition, Vector3 point, float range)
        {
            var tileCenterPoint = tilemap.GetCellCenterWorld(tilePosition);

            tileCenterPoint.z = point.z; // discard z

            // get distance along tilemap's axis
            var distanceTilemapX = Mathf.Abs(Vector3.Dot(point - tileCenterPoint, tilemapRightVector.normalized));
            var distanceTilemapY = Mathf.Abs(Vector3.Dot(point - tileCenterPoint, tilemapUpVector.normalized));

            var maxDistanceTilemapX = tilemapRightVector.magnitude / 2 + range;
            var maxDistanceTilemapY = tilemapUpVector.magnitude / 2 + range;

            return !(distanceTilemapX > maxDistanceTilemapX || distanceTilemapY > maxDistanceTilemapY);
        }
    }
}