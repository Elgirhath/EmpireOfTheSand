using Build;
using Map;
using UnityEngine;

namespace Ai
{
    public class ActionController : MonoBehaviour
    {
        private StorageBuilder storageBuilder;
        public Building sandCastlePrefab;

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
                RandomBuilder.Build(sandCastlePrefab, GetComponent<AiManager>().playerColor, GetComponent<AiBuildingManager>());
            }
            else if (action == Action.Attack)
            {

            }
            Debug.Log(action);
        }
    }
}