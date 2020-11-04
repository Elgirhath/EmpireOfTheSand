using System.Collections.Generic;
using System.Linq;
using Assets.Building;
using Assets.Map;
using Assets.Units.Building;
using Assets.Units.Fighting;
using Assets.Units.Movement;
using Assets.Units.ResourceGathering;
using Assets.Units.Soaking;
using Assets.Util;
using Assets.Util.Selection;
using UnityEngine;

namespace Assets.Units.Managers
{
    public class UnitActionManager : MonoBehaviour
    {
        private IEnumerable<GameObject> selectedUnits; //using IEnumerable to update when selectedUnits are changed

        private void Start()
        {
            selectedUnits = GetComponent<SelectionManager>().unitSelectionManager.SelectedUnits.Select(unit => unit.gameObject);
        }

        private void Update()
        {
            if (!Input.GetMouseButtonUp(1)) return;

            var pos = ScreenToWorldConverter.ToWorldPosition(Input.mousePosition);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                var tile = GameMap.Instance.GetTileAtPosition(pos);

                if (tile.type == TileType.Default) return;
                CommandResourceGathering(tile);
            }
            else
            {
                var hit = Physics2D.Raycast(pos, Vector2.zero);
                var constructionSite = hit.transform?.GetComponent<ConstructionSite>();
                if (constructionSite != null)
                {
                    CommandBuilding(constructionSite);
                    return;
                }

                var unit = hit.transform?.GetComponent<Unit>();

                if (unit != null && unit.PlayerColor != GameManager.Instance.playerColor)
                {
                    CommandAttack(unit);
                    return;
                }

                CommandMovement(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
            }
        }

        private void CommandMovement(Vector3 position)
        {
            foreach (var unit in SelectDryUnits())
            {
                CleanCommands(unit);
                unit.GetComponent<UnitMovementController>().SetDestination(position);
            }
        }

        private void CommandAttack(Unit enemy)
        {
            foreach (var unit in SelectDryUnits())
            {
                CleanCommands(unit);
                unit.GetComponent<FightingStateManager>().Attack(enemy);
            }
        }

        private void CommandBuilding(ConstructionSite building)
        {
            foreach (var unit in SelectDryUnits())
            {
                CleanCommands(unit);
                unit.GetComponent<BuildingStateManager>().AssignToBuild(building);
            }
        }

        private void CommandResourceGathering(CustomTile tile)
        {
            foreach (var unit in SelectDryUnits())
            {
                CleanCommands(unit);
                unit.GetComponent<ResourceGatheringStateManager>().SetDestinationResource(tile);
            }
        }

        private void CleanCommands(GameObject unit)
        {
            unit.GetComponent<ResourceGatheringStateManager>().CleanCommands();
            unit.GetComponent<FightingStateManager>().CleanCommands();
            unit.GetComponent<BuildingStateManager>().CleanCommands();
        }

        private IEnumerable<GameObject> SelectDryUnits()
        {
            return selectedUnits.Where(unit => (SoakingState) unit.GetComponent<SoakingStateManager>().State == SoakingState.None);
        }
    }
}
