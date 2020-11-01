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

        public void Spawn(Vector3 location)
        {
            Instantiate(unitPrefab, location, Quaternion.identity);
        }
    }
}