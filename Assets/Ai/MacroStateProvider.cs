using System.Linq;
using Build;
using Units;
using UnityEngine;

namespace Ai
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
                State = new MacroState.StateClass()
                {
                    CastleCount = GetPlayersGameObjectCountOfType<SandCastle>(),
                    SandStorageCount = GetPlayersGameObjectCountOfType<Storage>(),
                    UnitCount = GetPlayersGameObjectCountOfType<Unit>(),
                    WaterStorageCount = GetPlayersGameObjectCountOfType<Storage>()
                },
                GameEnded = false,
                Reward = 1f
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