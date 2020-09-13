using System.Linq;
using Assets.Building;
using Assets.Map;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{
    public class TargetStorageProvider
    {
        private Transform unit;

        public TargetStorageProvider(Transform unit)
        {
            this.unit = unit;
        }

        public Storage GetTargetStorage(TileType tileType)
        {
            var storages = GameObject.FindGameObjectsWithTag("Storage").Select(storage => storage.GetComponent<Storage>());

            Storage targetStorage = null;
            var minDistance = Mathf.Infinity;

            foreach (var storage in storages.Where(s => s.Type == tileType))
            {
                var distance = GetDistance(storage.transform, unit);

                if (!(distance < minDistance)) continue;

                minDistance = distance;
                targetStorage = storage;
            }

            return targetStorage;
        }

        private float GetDistance(Transform storage, Transform unit)
        {
            return (storage.position - unit.position).magnitude;
        }
    }
}