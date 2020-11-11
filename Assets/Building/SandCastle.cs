using Assets.Map;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Units;
using UnityEngine;

namespace Assets.Building
{
    public class SandCastle : Building
    {
        public List<Unit> boundUnits = new List<Unit>();

        public override IDictionary<TileType, int> GetBuildCost()
        {
            return new Dictionary<TileType, int>
            {
                {TileType.Sand, 4}
            };
        }

        public override void OnBuild()
        {
            var unit = UnitSpawnManager.Instance.Spawn(transform.position + Vector3.right * 1f); //temporary solution
            boundUnits.Add(unit.GetComponent<Unit>());
        }

        public override void Destroy()
        {
            base.Destroy();
            foreach (var unit in boundUnits)
            {
                unit.Destroy();
            }
        }
    }
}