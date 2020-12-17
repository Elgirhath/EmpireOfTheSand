using System.Linq;
using Pathfinding;
using UnityEngine;

namespace Units.Movement
{
    public class UnitCollisionHandler
    {
        public static bool WillCollide(UnitMovementController unit, Path path)
        {
            var units = GameObject.FindGameObjectsWithTag("Unit");
            var unitsInRange = units.Where(u => u != unit.gameObject)
                .Where(u => Vector2.Distance(u.transform.position, unit.transform.position) < unit.otherUnitRecalculateDistance);

            if (!unitsInRange.Any()) return false;

            var distanceToPath =
                unitsInRange.Min(u => path.vectorPath.Min(v => Vector2.Distance(v, u.transform.position)));

            return distanceToPath < unit.maxDistanceToPath;
        }

        private static void RecalculatePath(UnitMovementController unit, Path path)
        {
            unit.Stop();
            unit.SetDestination(path.vectorPath.Last());
        }

        public static void HandleCollision(UnitMovementController unit, Path path)
        {
            if (WillCollide(unit, path))
            {
                RecalculatePath(unit, path);
            }
        }
    }
}