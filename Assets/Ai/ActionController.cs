using Assets.Building;
using Assets.Map;
using UnityEngine;

namespace Assets.Ai
{
    public class ActionController : MonoBehaviour
    {
        private StorageBuilder storageBuilder;
        public GameObject sandCastlePrefab;

        private void Start()
        {
            storageBuilder = GetComponent<StorageBuilder>();
        }

        public void ExecuteAction(Action action)
        {
            if (action == Action.BuildSandStorage)
            {
                storageBuilder.BuildStorage(TileType.Sand);
            }
            else if (action == Action.BuildWaterStorage)
            {
                storageBuilder.BuildStorage(TileType.Water);
            }
            else if (action == Action.BuildCastle)
            {
                RandomBuilder.Build(sandCastlePrefab, GetComponent<AiManager>().playerColor);
            }
            Debug.Log(action);
        }
    }
}