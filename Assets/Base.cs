using System.Linq;
using Build;
using Units;
using UnityEngine;

public class Base : Structure
{
    public static Base GetBase(PlayerColor color) => GameObject.FindGameObjectsWithTag("Base")
        .Select(go => go.GetComponent<Base>()).Single(b => b.GetComponent<PlayerProperty>().playerColor == color);
}