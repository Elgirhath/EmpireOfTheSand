using System.Collections.Generic;
using System.Linq;
using Assets.Building;
using Assets.Map;
using Pathfinding;
using UnityEngine;

namespace Assets.Units.ResourceGathering
{

    public class TargetStorageProvider
    {
        public enum ActionType
        {
            CollectFrom,
            DeliverTo
        }

        private readonly Transform unit;

        public TargetStorageProvider(Transform unit)
        {
            this.unit = unit;
        }

        public Storage GetTargetStorage(TileType tileType, ActionType actionType)
        {
            var storages = GameObject.FindGameObjectsWithTag("Storage").Select(storage => storage.GetComponent<Storage>())
                .Where(s => s != null).Where(s => s.GetComponent<PlayerProperty>().playerColor == GameManager.Instance.playerColor);

            var availableStorages = new List<Storage>();

            foreach (var storage in storages.Where(s => s.Type == tileType))
            {
                if (actionType == ActionType.DeliverTo)
                {
                    if (storage.Size >= storage.Capacity) continue;
                }
                else
                {
                    if (storage.Size <= 0) continue;
                }

                availableStorages.Add(storage);
            }

            var storagesOrdered = availableStorages.OrderBy(storage => Vector2.Distance(storage.transform.position, unit.transform.position));

            foreach (var storage in storagesOrdered)
            {
                if (IsPathPossible(unit.transform.position, storage.transform.position))
                {
                    return storage;
                }
            }

            return null;
        }

        private bool IsPathPossible(Vector2 startPosition, Vector2 endPosition)
        {
            var startNode = AstarPath.active.GetNearest(startPosition, NNConstraint.Default).node;
            var endNode = AstarPath.active.GetNearest(endPosition, NNConstraint.Default).node;
            if (Vector3.Distance((Vector3)endNode.position, endPosition) > 0.5f) return false;

            return PathUtilities.IsPathPossible(startNode, endNode);
        }
    }
}