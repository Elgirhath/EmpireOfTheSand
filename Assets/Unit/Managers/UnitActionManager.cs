using System.Collections.Generic;
using Assets.Unit.ResourceGathering;
using Assets.Util;
using UnityEngine;

namespace Assets.Unit.Managers
{
    public class UnitActionManager : MonoBehaviour
    {
        private ISet<ResourceGatheringFlowManager> selectedUnits;

        private void Start()
        {
            selectedUnits = GetComponent<UnitSelectionManager>().SelectedUnits;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonUp(1)) return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                CommandResourceGathering(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
            }
            else
            {
                CommandMovement(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
            }
        }

        private void CommandMovement(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
//                unit.SetDestination(position);
            }
        }

        private void CommandResourceGathering(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
                unit.SetDestinationResource(position);
            }
        }
    }
}
