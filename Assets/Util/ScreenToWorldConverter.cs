using UnityEngine;

namespace Util
{
    public static class ScreenToWorldConverter
    {
        public static Vector3 ToWorldPosition(Vector3 screenPosition)
        {
            var destPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            destPosition.z = 0f;
            return destPosition;
        }
    }
}
