using Assets.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Building
{
    public class Storage : Building
    {
        public int sandCost;
        public int waterCost;

#pragma warning disable 649
        [SerializeField]
        private TileType type;

        [SerializeField]
        private int capacity;
#pragma warning restore 649

        public TileType Type => type;

        public int Capacity => capacity;

        [SerializeField]
        private int size;

        public int Size
        {
            get => size;
            set => size = value;
        }

        public override IDictionary<TileType, int> GetBuildCost()
        {
            return new Dictionary<TileType, int>
            {
                { TileType.Sand, sandCost },
                { TileType.Water, waterCost }
            };
        }
    }
}