using UnityEngine;

namespace Util
{
    public class DebugExtension
    {
        public static void DrawCross(Vector2 position, float radius, Color color, float duration)
        {
            Debug.DrawRay(position, Vector3.up * radius, color, duration);
            Debug.DrawRay(position, Vector3.right * radius, color, duration);
            Debug.DrawRay(position, Vector3.down * radius, color, duration);
            Debug.DrawRay(position, Vector3.left * radius, color, duration);
        }
        public static void DrawCross(Vector2 position, float radius, Color color)
        {
            Debug.DrawRay(position, Vector3.up * radius, color);
            Debug.DrawRay(position, Vector3.right * radius, color);
            Debug.DrawRay(position, Vector3.down * radius, color);
            Debug.DrawRay(position, Vector3.left * radius, color);
        }

        public static void DrawCross(Vector2 position, float radius) => DrawCross(position, radius, Color.white);
        public static void DrawCross(Vector2 position) => DrawCross(position, 1f, Color.white);
    }
}