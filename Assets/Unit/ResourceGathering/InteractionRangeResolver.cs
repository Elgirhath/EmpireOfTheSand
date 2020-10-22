﻿using Assets.Map;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{
    public class InteractionRangeResolver
    {
        private Vector3 tilemapRightVector;
        private Vector3 tilemapUpVector;

        private static InteractionRangeResolver instance;
        public static InteractionRangeResolver Instance => instance ?? (instance = new InteractionRangeResolver());

        private InteractionRangeResolver()
        {
            var tilemap = GameMap.Instance.tilemap;
            var refCellPosition = tilemap.GetCellCenterWorld(new Vector3Int(0, 1, 0));
            tilemapRightVector = tilemap.GetCellCenterWorld(new Vector3Int(1, 1, 0)) - refCellPosition;
            tilemapUpVector = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0)) - refCellPosition;
        }

        public bool IsPointInInteractionRange(Vector3Int tilePosition, Vector3 point, float range)
        {
            var tileCenterPoint = GameMap.Instance.tilemap.GetCellCenterWorld(tilePosition);

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