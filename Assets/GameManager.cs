using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public PlayerColor playerColor;

    public IList<Unit> Units { get; private set; } = new List<Unit>();

    private void Start()
    {
        Instance = this;
    }
}