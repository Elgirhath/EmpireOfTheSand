using Assets.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Building
{
    public abstract class Building : MonoBehaviour
    {
        public abstract IDictionary<TileType, int> GetBuildCost();
    }
}