using System;
using System.Collections.Generic;
using System.Linq;
using Build;
using Units;
using Units.Building;
using UnityEngine;

namespace Ai
{
    public static class BuildAssignmentManager
    {
        public static Dictionary<Construction, ISet<BuildingStateManager>> buildingAssignments = new Dictionary<Construction, ISet<BuildingStateManager>>();

        public static void AssignUnitToBuild(Construction construction, BuildingStateManager unit)
        {
            if (!buildingAssignments.ContainsKey(construction))
            {
                buildingAssignments.Add(construction, new HashSet<BuildingStateManager>());
            }

            buildingAssignments[construction].Add(unit);
        }

        public static void UnassignUnitFromBuild(Construction construction, BuildingStateManager unit)
        {
            if (!buildingAssignments.ContainsKey(construction))
            {
                Debug.LogError("Construction not present in dictionary");
                return;
            }
            if (!buildingAssignments[construction].Contains(unit))
            {
                Debug.LogError("Unit not assigned to build");
                return;
            }
            buildingAssignments[construction].Remove(unit);
        }

        public static void CompleteBuilding(Construction construction)
        {
            if (buildingAssignments.ContainsKey(construction))
            {
                buildingAssignments.Remove(construction);
            }
        }

        public static void CreateUnassigned(Construction construction)
        {
            buildingAssignments.Add(construction, new HashSet<BuildingStateManager>());
        }

        public static IEnumerable<Construction> GetUnassignedConstructionSites()
        {
            RemoveExpiredAssignments();
            return buildingAssignments.Where(pair => pair.Value.Count == 0).Select(pair => pair.Key);
        }

        // removes building state managers whose state turned to None
        private static void RemoveExpiredAssignments()
        {
            foreach (var construction in buildingAssignments.Keys.ToList())
            {
                if (construction == null)
                {
                    buildingAssignments.Remove(construction);
                }
            }

            foreach (var kvp in buildingAssignments)
            {
                foreach (var bsm in kvp.Value.ToList())
                {
                    if (Equals(bsm.State, BuildingState.None))
                    {
                        kvp.Value.Remove(bsm);
                    }
                }
            }
        }

        public static bool IsUnitAssigned(BuildingStateManager buildingStateManager)
        {
            return buildingAssignments.Values.Any(unitSet => unitSet.Contains(buildingStateManager));
        }
    }
}