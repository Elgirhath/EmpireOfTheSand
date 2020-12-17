using Map;
using UnityEngine;

namespace Build
{
    public class StructureBuildManager : MonoBehaviour
    {
        public AstarPath pathfinder;

        public static StructureBuildManager Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

        public T Build<T>(T prefab, Vector3Int position, Transform parent) where T : Object
        {
            var cellCenter = GameMap.Instance.GetCellCenterWorld(position);
            return Build(prefab, cellCenter, parent);
        }

        public T Build<T>(T prefab, Vector3 position, Transform parent) where T : Object
        {
            var worldPosition = GameMap.Instance.SnapToGrid(position);
            var obj = Instantiate(prefab, worldPosition, Quaternion.identity, parent);
            RebakeNavMesh();
            AssignToTilemapMatrix(obj);
            return obj;
        }

        private void RebakeNavMesh()
        {
            foreach (var graph in pathfinder.graphs)
            {
                graph.Scan();
            }
        }

        private static void AssignToTilemapMatrix(Object obj)
        {
            var gameObject = obj as GameObject;

            if (gameObject == null)
            {
                gameObject = ((Component) obj).gameObject;
            }

            GameMap.Instance.Matrix.Assign(gameObject);
        }
    }
}