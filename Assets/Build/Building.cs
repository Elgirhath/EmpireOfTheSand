using System.Collections.Generic;
using Map;

namespace Build
{
    public abstract class Building : Structure
    {
        public abstract IDictionary<TileType, int> GetBuildCost();

        public virtual void OnBuild() {}
    }
}