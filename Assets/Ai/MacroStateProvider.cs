using System.Collections.Generic;
using System.Linq;
using Build;
using Map;
using Units;
using UnityEngine;

namespace Ai
{
    public class MacroStateProvider : MonoBehaviour
    {
        private IList<Player> players;

        private void Start()
        {
            players = Player.GetPlayers().ToList();
        }

        public object GetState(PlayerColor currentPlayerColor)
        {
            var rewards = RewardProvider.GetRewards();

            return new
            {
                PlayerData = players.Select(player => new
                {
                    Color = player.color.ToString(),
                    Current = player.color == currentPlayerColor,
                    Reward = rewards[player.color],
                    State = GetPlayerState(player)
                }),
                GameEnded = false
            };
        }

        private PlayerState GetPlayerState(Player player)
        {
            return new PlayerState
            {
                CastleCount = player.GetComponentsInChildren<SandCastle>().Length,
                SandStorageCount =
                    player.GetComponentsInChildren<Storage>().Count(s => s.Type == TileType.Sand),
                UnitCount = player.GetComponentsInChildren<Unit>().Length,
                WaterStorageCount = player.GetComponentsInChildren<Storage>()
                    .Count(s => s.Type == TileType.Water)
            };
        }
    }
}