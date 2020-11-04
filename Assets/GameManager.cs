using System.Collections.Generic;
using System.Linq;
using Assets.Units;
using UnityEngine;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }
        public PlayerColor playerColor;

        public IList<Unit> Units { get; private set; }

        private void Start()
        {
            Instance = this;
            Units = GameObject.FindGameObjectsWithTag("Unit").Select(go => go.GetComponent<Unit>()).Where(unit => unit.PlayerColor == playerColor).ToArray();
        }
    }
}
