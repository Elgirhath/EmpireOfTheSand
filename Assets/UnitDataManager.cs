using System.Collections.Generic;
using Assets.Units;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public static class UnitDataManager
    {
        private static IDictionary<PlayerColor, ISet<Unit>> units = new Dictionary<PlayerColor, ISet<Unit>>();

        public static void AddUnit(Unit unit)
        {
            var color = unit.GetComponent<PlayerProperty>().playerColor;
            if (!units.ContainsKey(color))
            {
                units.Add(color, new HashSet<Unit>{unit});
            }
            else
            {
                units[color].Add(unit);
            }
        }

        public static void RemoveUnit(Unit unit)
        {
            var color = unit.GetComponent<PlayerProperty>().playerColor;
            units[color].Remove(unit);
            if (units[color].Count <= 0)
            {
                Debug.Log($"Player {color} has been defeated");
            }
        }

        public static ISet<Unit> GetUnits(PlayerColor color)
        {
            return units[color];
        }
    }
}
