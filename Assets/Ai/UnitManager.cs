using System.Collections.Generic;
using System.Linq;
using Units;

namespace Ai
{
    public class UnitManager
    {
        public static ICollection<Unit> GetFreeUnits(PlayerColor color)
        {
            var units = UnitDataManager.GetUnits(color);
            return units.Where(unit => !unit.GetComponent<AiUnitController>().IsBuilding).ToList();
        }
    }
}