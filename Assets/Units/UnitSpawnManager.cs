using UnityEngine;

namespace Assets.Units
{
    public class UnitSpawnManager : MonoBehaviour
    {
        public static UnitSpawnManager Instance { get; private set; }
        public GameObject unitPrefab;

        private void Start()
        {
            Instance = this;
        }

        public GameObject Spawn(Vector3 location)
        {
            return Instantiate(unitPrefab, location, Quaternion.identity);
        }
    }
}