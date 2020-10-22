using System;
using System.Collections;
using Assets.Building;
using Assets.Map;
using System.Linq;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{

    public class TargetStorageProvider
    {
        private enum ActionType
        {
            CollectFrom,
            DeliverTo
        }

        private readonly Transform unit;

        public TargetStorageProvider(Transform unit)
        {
            this.unit = unit;
        }

        public Storage GetStorageToDeliverTo(TileType tileType)
        {
            return GetTargetStorage(tileType, ActionType.DeliverTo);
        }

        public Storage GetStorageToCollectFrom(TileType tileType)
        {
            return GetTargetStorage(tileType, ActionType.CollectFrom);
        }

        private Storage GetTargetStorage(TileType tileType, ActionType actionType)
        {
            var storages = GameObject.FindGameObjectsWithTag("Storage").Select(storage => storage.GetComponent<Storage>()).Where(s => s != null);

            Storage targetStorage = null;
            var minDistance = Mathf.Infinity;

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

                var distance = GetDistance(storage.transform);

                if (!(distance < minDistance)) continue;

                minDistance = distance;
                targetStorage = storage;
            }

            return targetStorage;
        }

        private float GetDistance(Transform storage)
        {
            return (storage.position - unit.position).magnitude;
        }
    }
}