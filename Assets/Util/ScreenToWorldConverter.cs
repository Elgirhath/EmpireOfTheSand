using UnityEngine;

namespace Assets.Util
{
    public static class ScreenToWorldConverter
    {
        public static Vector3 ToWorldPosition(Vector3 screenPosition)
        {
            var destPosition = Input.mousePosition;
            destPosition.z = 0f;
            return Camera.main.ScreenToWorldPoint(destPosition);
        }
    }
}
