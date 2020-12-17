﻿using System.Collections.Generic;
using System.Linq;
using Map;

namespace Build
{
    public class Construction : Structure
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
            var building = StructureBuildManager.Instance.Build(buildPrefab, transform.position, transform.parent);
            building.GetComponent<PlayerProperty>().playerColor = GetComponent<PlayerProperty>().playerColor;
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