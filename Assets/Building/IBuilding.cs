using Assets.Map;
using System.Collections.Generic;

namespace Assets.Building
{
    public interface IBuilding
    {
        IDictionary<TileType, int> GetBuildCost();
    }
}