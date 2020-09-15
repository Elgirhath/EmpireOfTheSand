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

#pragma warning disable 649
        [SerializeField]
        private int waterRequired;
        [SerializeField]
        private int sandRequired;
#pragma warning restore 649

        public Dictionary<TileType, int> requiredResources;
        public Dictionary<TileType, int> deliveredResources = new Dictionary<TileType, int>
        {
            {TileType.Sand, 0},
            {TileType.Water, 0}
        };

        private void Start()
        {
            requiredResources = new Dictionary<TileType, int>
            {
                {TileType.Sand, sandRequired},
                {TileType.Water, waterRequired}
            };
        }

        public Dictionary<TileType, int> GetRemainingResourcesToDeliver()
        {
            return requiredResources.Zip(deliveredResources, (a, b) => new {a.Key, Value = a.Value - b.Value}).Where(x => x.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
