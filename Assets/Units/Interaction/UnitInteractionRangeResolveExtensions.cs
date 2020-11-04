using Assets.Building;
using Assets.Map;
using Assets.Units.Movement;
using UnityEngine;

namespace Assets.Units
{
    public static class UnitInteractionRangeResolveExtensions
    {
        public static bool IsInInteractionRange(this UnitMovementController unit, Vector3Int tilePosition, float range)
        {
            return InteractionRangeResolver.IsPointInInteractionRange(tilePosition, unit.transform.position, range);
        }

        public static bool IsInInteractionRange(this UnitMovementController unit, CustomTile tile, float range)
        {
            var tileIndex = GameMap.Instance.GetCellPosition(tile.position);
            return unit.IsInInteractionRange(tileIndex, range);
        }
        public static bool IsInInteractionRange(this UnitMovementController unit, Structure structure, float range)
        {
            return unit.IsInInteractionRange(structure.tile, range);
        }
    }
}