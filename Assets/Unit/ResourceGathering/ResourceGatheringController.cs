using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Building;
using Assets.Map;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{
    public class ResourceGatheringController : MonoBehaviour
    {
        public Dictionary<TileType, int> resourceCounts;

        public float gatheringFrequency = 1f;
        public int maxResourceCount = 1;
        public TileType? targetResource = null;
        public bool IsGathering => targetResource != null;

        public ResourceGatheringController()
        {
            resourceCounts = new Dictionary<TileType, int>()
            {
                {TileType.Sand, 0},
                {TileType.Water, 0}
            };
        }

        public void StartGathering(TileType tileType)
        {
            if (targetResource != null) throw new InvalidOperationException("Already gathering");

            targetResource = tileType;
            StartCoroutine(Gather());
        }

        public void Deliver(Storage storage)
        {
            var amountToDeliver = Mathf.Min(resourceCounts[storage.Type], storage.Capacity - storage.Size);
            storage.Size += amountToDeliver;
            resourceCounts[storage.Type] -= amountToDeliver;
        }

        private IEnumerator Gather()
        {
            while (targetResource != null && resourceCounts[targetResource.Value] < maxResourceCount)
            {
                yield return new WaitForSeconds(gatheringFrequency);
                resourceCounts[targetResource.Value]++;
            }

            targetResource = null;
        }
    }
}