using System.Collections.Generic;
using Assets.Unit.ResourceGathering;
using UnityEngine;

namespace Assets.Unit.Managers
{
    public class UnitSelectionManager : MonoBehaviour
    {
        public static UnitSelectionManager Instance { get; private set; }

        void Start()
        {
            Instance = this;
        }

        public ISet<ResourceGatheringFlowManager> SelectedUnits { get; } = new HashSet<ResourceGatheringFlowManager>();

        public void NotifyUnitSelection(ResourceGatheringFlowManager unit, bool selected)
        {
            if (selected)
            {
                SelectedUnits.Add(unit);
            }
            else
            {
                SelectedUnits.Remove(unit);
            }
        }
    }
}
