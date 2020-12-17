using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Build;
using Units;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerColor color;

    void Start()
    {
        
    }

    public Transform GetUnitParent()
    {
        return transform.Find("Units");
    }

    public Transform GetBuildingParent()
    {
        return transform.Find("Buildings");
    }

    public ICollection<Structure> GetBuildings()
    {
        return GetBuildingParent().GetComponentsInChildren<Structure>();
    }

    public static Player GetPlayer(PlayerColor color)
    {
        return GetPlayers().First(player => player.color == color);
    }

    public static IEnumerable<Player> GetPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player").Select(go => go.GetComponent<Player>());
    }
}
