using System.Collections.Generic;
using System.Linq;
using Build;
using Units;

namespace Ai
{
    public static class RewardProvider
    {
        public static IDictionary<PlayerColor, float> GetScores()
        {
            var players = Player.GetPlayers().ToList();

            var scores = new Dictionary<PlayerColor, float>();

            foreach (var player in players)
            {
                var buildings = player.GetBuildings();
                float score = buildings.Count + player.GetUnitParent().childCount;
                var resourceCount = buildings.OfType<Storage>().Sum(s => s.Size);
                score += resourceCount / 5f;
                scores.Add(player.color, score);
            }

            return scores;
        }

        public static IDictionary<PlayerColor, float> GetRewards()
        {
            var scores = GetScores();
            return scores.ToDictionary(kvp => kvp.Key, kvp => GetReward(scores, kvp.Key));
        }

        private static float GetReward(IDictionary<PlayerColor, float> scores, PlayerColor player)
        {
            return (scores[player] - scores.Where(kvp => kvp.Key != player).Max(kvp => kvp.Value)) / scores.Values.Max();
        }
    }
}