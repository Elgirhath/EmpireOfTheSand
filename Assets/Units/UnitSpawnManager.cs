using Ai;
using UnityEngine;

namespace Units
{
    public class UnitSpawnManager : MonoBehaviour
    {
        public static UnitSpawnManager Instance { get; private set; }
        public GameObject unitPrefab;

        private void Start()
        {
            Instance = this;
        }

        public GameObject Spawn(Vector3 location, PlayerColor color)
        {
            var player = Player.GetPlayer(color);
            var unitParent = player.GetUnitParent();
            var obj = Instantiate(unitPrefab, location, Quaternion.identity, unitParent);
            obj.GetComponent<PlayerProperty>().playerColor = color;
            if (player.GetComponent<AiManager>() != null) // TODO: Make it more universal
            {
                obj.AddComponent<AiUnitController>();
            }
            return obj;
        }
    }
}