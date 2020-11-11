using Assets.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Building
{
    public class ConstructionSite : Structure
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
            var building = Instantiate(buildPrefab, transform.position, Quaternion.identity);
            building.GetComponent<PlayerProperty>().playerColor = GameManager.Instance.playerColor;
            building.OnBuild();
            Destroy(gameObject);
        }

        public IDictionary<TileType, int> GetRemainingResourcesToDeliver()
        {
            return requiredResources.Zip(deliveredResources, (a, b) => new { a.Key, Value = a.Value - b.Value }).Where(x => x.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
