using UnityEngine;

namespace Assets.Util.Selection
{
    public class RectangleLasso : MonoBehaviour
    {
        private const int thickness = 1;

        public static RectangleLasso Instantiate(Vector2 startPos)
        {
            var obj = new GameObject();
            var lasso = obj.AddComponent<RectangleLasso>();
            lasso.rectTransform = obj.AddComponent<RectTransform>();
            lasso.rectTransform.pivot = Vector2.zero;
            lasso.rectTransform.position = startPos;
            return lasso;
        }

        private RectTransform rectTransform;

        public void SetEndPos(Vector2 endPos)
        {
            rectTransform.pivot = Vector2.zero;
            var size = endPos - (Vector2)rectTransform.position;
            rectTransform.sizeDelta = new Vector2(size.x, -size.y);
        }

        private void OnGUI()
        {
            var guiRect = GetGuiRect(rectTransform.position, (Vector2)rectTransform.position + rectTransform.sizeDelta);

            DrawLasso(guiRect, new Color(1f, 1f, 1f, 0.7f), thickness);
        }

        private void DrawLasso(Rect rect, Color color, int thickness)
        {
            var lines = new[]
            {
                new Rect(rect.xMin, rect.yMin, rect.width - thickness, thickness), //top
                new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), //right
                new Rect(rect.xMin, rect.yMax - thickness, rect.width - thickness, thickness), //bottom
                new Rect(rect.xMin, rect.yMin + thickness, thickness, rect.height - 2 * thickness), //left
            };

            foreach (var line in lines)
            {
                DrawRectangle(line, color);
            }
        }

        private void DrawRectangle(Rect rect, Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            var style = new GUIStyle();
            style.normal.background = texture;

            GUI.Box(rect, GUIContent.none, style);
        }

        private Rect GetGuiRect(Vector2 start, Vector2 end)
        {
            var x = start.x;
            var y = Screen.height - start.y;
            var width = end.x - start.x;
            var height = end.y - start.y;

            if (height < 0)
            {
                y += height;
                height = -height;
            }

            if (width < 0)
            {
                x += width;
                width = -width;
            }

            return new Rect(x, y, width, height);
        }
    }
}