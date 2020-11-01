using Assets.Map;
using System.Collections.Generic;
using Assets.Units;
using UnityEngine;

namespace Assets.Building
{
    public class SandCastle : Building
    {
        public override IDictionary<TileType, int> GetBuildCost()
        {
            return new Dictionary<TileType, int>
            {
                {TileType.Sand, 4}
            };
        }

        public void Start()
        {
            UnitSpawnManager.Instance.Spawn(transform.position + Vector3.right * 1f); //temporary solution
        }
    }
}