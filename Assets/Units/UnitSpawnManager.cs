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
            var parent = Player.GetPlayer(color).GetUnitParent();
            var obj = Instantiate(unitPrefab, location, Quaternion.identity, parent);
            obj.GetComponent<PlayerProperty>().playerColor = color;
            if (color == PlayerColor.Black) // TODO: Make it more universal
            {
                obj.AddComponent<AiUnitController>();
            }
            return obj;
        }
    }
}