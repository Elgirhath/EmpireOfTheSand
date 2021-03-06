﻿using UnityEngine;

namespace Assets.Units.Attacking
{
    public interface IAttackable
    {
        void Attack(int attackStrength);

        Vector2 Position { get; }

        bool IsDestroyed { get; }
    }
}