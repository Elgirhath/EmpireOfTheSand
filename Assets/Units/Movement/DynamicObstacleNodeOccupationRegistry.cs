using System.Collections.Generic;
using Pathfinding;

namespace Assets.Units.Movement
{
    public class DynamicObstacleNodeOccupationRegistry
    {
        private class OccupancyData
        {
            public bool InitialWalkability { get; set; }
            public int UnitsCount { get; set; }
        }

        private static readonly IDictionary<GraphNode, OccupancyData> Occupancy = new Dictionary<GraphNode, OccupancyData>();

        /// <summary>
        /// Registers a new unit on the node.
        /// </summary>
        /// <returns>True if node was not occupied, false otherwise.</returns>
        /// <param name="node"></param>
        public static bool Push(GraphNode node)
        {
            if (!Occupancy.ContainsKey(node) || Occupancy[node].UnitsCount <= 0)
            {
                var data = new OccupancyData
                {
                    InitialWalkability = node.Walkable,
                    UnitsCount = 1
                };
                Occupancy.Add(node, data);
                return true;
            }
            Occupancy[node].UnitsCount++;
            return false;
        }

        /// <summary>
        /// Unregisters a unit from the node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>True if the unit was the last one to leave the node and node's collider should be recalculated. False otherwise.</returns>
        public static bool Pull(GraphNode node)
        {
            Occupancy[node].UnitsCount--;
            if (Occupancy[node].UnitsCount > 0) return false;

            node.Walkable = Occupancy[node].InitialWalkability;
            Occupancy.Remove(node);
            return true;
        }
    }
}