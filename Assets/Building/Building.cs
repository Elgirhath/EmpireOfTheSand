using Assets.Map;
using System.Collections.Generic;

namespace Assets.Building
{
    public abstract class Building : Structure
    {
        public abstract IDictionary<TileType, int> GetBuildCost();
    }
}