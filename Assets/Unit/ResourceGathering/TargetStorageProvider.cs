using Assets.Building;
using Assets.Map;
using System.Linq;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{
    public class TargetStorageProvider
    {
        private readonly Transform unit;

        public TargetStorageProvider(Transform unit)
        {
            this.unit = unit;
        }

        public Storage GetTargetStorage(TileType tileType)
        {
            var storages = GameObject.FindGameObjectsWithTag("Storage").Select(storage => storage.GetComponent<Storage>()).Where(s => s != null);

            Storage targetStorage = null;
            var minDistance = Mathf.Infinity;

            foreach (var storage in storages.Where(s => s.Type == tileType))
            {
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