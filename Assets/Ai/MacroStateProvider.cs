using System.Linq;
using Assets.Building;
using Assets.Units;
using UnityEngine;

namespace Assets.Ai
{
    public class MacroStateProvider
    {
        private PlayerColor playerColor;

        public MacroStateProvider(PlayerColor playerColor)
        {
            this.playerColor = playerColor;
        }

        public MacroState GetState()
        {
            var state = new MacroState
            {
                CastleCount = GetPlayersGameObjectCountOfType<SandCastle>(),
                SandStorageCount = GetPlayersGameObjectCountOfType<Storage>(),
                UnitCount = GetPlayersGameObjectCountOfType<Unit>(),
                WaterStorageCount = GetPlayersGameObjectCountOfType<Storage>()
            };
            return state;
        }

        private int GetPlayersGameObjectCountOfType<T>() where T : Component
        {
            return Object.FindObjectsOfType<T>()
                .Count(obj => obj.GetComponent<PlayerProperty>().playerColor == playerColor);
        }
    }
}