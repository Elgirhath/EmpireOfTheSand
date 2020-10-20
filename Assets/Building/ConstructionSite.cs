using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Assets.Map;
using Assets.Unit.Managers;
using Assets.Util;
using UnityEngine;

namespace Assets.Building
{
    public class ConstructionSite : MonoBehaviour
    {
        public IDictionary<TileType, int> requiredResources => buildPrefab.GetBuildCost();
        public IDictionary<TileType, int> deliveredResources = new Dictionary<TileType, int>
        {
            {TileType.Sand, 0},
            {TileType.Water, 0}
        };

        public Building buildPrefab;

        public void Build()
        {
            Instantiate(buildPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public IDictionary<TileType, int> GetRemainingResourcesToDeliver()
        {
            return requiredResources.Zip(deliveredResources, (a, b) => new {a.Key, Value = a.Value - b.Value}).Where(x => x.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
