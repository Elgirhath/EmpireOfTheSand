using System.Collections.Generic;
using System.Linq;
using Build;
using Map;
using Units;
using UnityEngine;

namespace Ai
{
    public class ResourceManager : MonoBehaviour
    {
        private PlayerColor color;

        private void Start()
        {
            color = GetComponent<AiManager>().playerColor;
        }

        public IDictionary<TileType, int> GetTotalResources()
        {
            var totalResources = new Dictionary<TileType, int>();

            var storages = GameObject.FindGameObjectsWithTag("Storage")
                .Where(s => s.GetComponent<PlayerProperty>().playerColor == color).Select(s => s.GetComponent<Storage>());

            foreach (var storage in storages)
            {
                if (!totalResources.ContainsKey(storage.Type))
                {
                    totalResources.Add(storage.Type, 0);
                }
                totalResources[storage.Type] += storage.Size;
            }

            return totalResources;
        }

        public bool AreRequiredResourcesAvailable(Construction construction)
        {
            var totalResources = GetTotalResources();

            foreach (var kvp in construction.GetRemainingResourcesToDeliver())
            {
                if (totalResources.ContainsKey(kvp.Key) && totalResources[kvp.Key] > 0) return true;
            }

            return false;
        }
    }
}