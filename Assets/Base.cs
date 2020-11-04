﻿using System.Linq;
using Assets.Building;
using Assets.Units;
using UnityEngine;

namespace Assets
{
    public class Base : Structure
    {
        public PlayerColor playerColor;

        public static Base GetBase(PlayerColor color) => GameObject.FindGameObjectsWithTag("Base")
            .Select(go => go.GetComponent<Base>()).Single(b => b.playerColor == color);
    }
}
