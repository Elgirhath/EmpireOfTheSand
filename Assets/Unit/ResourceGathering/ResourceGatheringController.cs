using System;
using System.Collections;
using UnityEngine;

namespace Assets.Unit.ResourceGathering
{
    public class ResourceGatheringController : MonoBehaviour
    {
        public float gatheringFrequency = 1f;
        public int resourceCount = 0;
        public int maxResourceCount = 1;
        public bool isGathering = false;

        public void StartGathering()
        {
            if (isGathering) throw new InvalidOperationException("Already gathering");

            isGathering = true;
            StartCoroutine(Gather());
        }

        private IEnumerator Gather()
        {
            while (resourceCount < maxResourceCount)
            {
                yield return new WaitForSeconds(gatheringFrequency);
                resourceCount++;
            }

            isGathering = false;
        }
    }
}