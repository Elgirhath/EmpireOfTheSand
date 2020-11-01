using System.Collections.Generic;
using Assets.Map;
using UnityEngine;

namespace Assets.Units.ResourceGathering
{
    public class ResourceHolder : MonoBehaviour
    {
        public Dictionary<TileType, int> resourceCounts;
        public Dictionary<TileType, int> resourceCapacity = new Dictionary<TileType, int>
        {
            {TileType.Sand, 1},
            {TileType.Water, 1}
        };

        public int maxResourceCount = 1;

        public ResourceHolder()
        {
            resourceCounts = new Dictionary<TileType, int>()
            {
                {TileType.Sand, 0},
                {TileType.Water, 0}
            };
        }
    }
}