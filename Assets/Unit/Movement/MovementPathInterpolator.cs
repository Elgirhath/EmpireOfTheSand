using System.Collections.Generic;
using UnityEngine;

namespace Assets.Unit.Movement
{
    public class MovementPathInterpolator
    {
        public static IList<Vector3> SmoothOut(IList<Vector3> waypointList, float triggerThreshold)
        {
            var smoothedOut = new List<Vector3> { waypointList[0] };
            for (var i = 1; i < waypointList.Count - 1; i++)
            {
                var start = Vector3.Lerp(waypointList[i], waypointList[i - 1], triggerThreshold);
                var end = Vector3.Lerp(waypointList[i], waypointList[i + 1], triggerThreshold);

                smoothedOut.Add(start);
                smoothedOut.Add(end);
            }
            smoothedOut.Add(waypointList[waypointList.Count - 1]);
            return smoothedOut;
        }
    }
}