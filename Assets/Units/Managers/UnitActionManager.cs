using System.Collections.Generic;
using System.Linq;
using Assets.Building;
using Assets.Map;
using Assets.Units.Attacking;
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
                var objectHit = PointSelector.PointSelect(Input.mousePosition);

                if (objectHit == null)
                {
                    CommandMovement(pos);
                    return;
                }

                var enemyBuilding = objectHit.GetComponent<Structure>();

                if (enemyBuilding != null && enemyBuilding.GetComponent<PlayerProperty>().playerColor != GameManager.Instance.playerColor)
                {
                    CommandAttack(enemyBuilding);
                    return;
                }

                var constructionSite = objectHit.GetComponent<ConstructionSite>();
                if (constructionSite != null)
                {
                    CommandBuilding(constructionSite);
                    return;
                }

                var unit = objectHit.GetComponent<SoakingStateManager>();

                if (unit != null && unit.GetComponent<PlayerProperty>().playerColor != GameManager.Instance.playerColor)
                {
                    CommandAttack(unit);
                }

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

        private void CommandAttack(IAttackable enemy)
        {
            foreach (var unit in SelectDryUnits())
            {
                CleanCommands(unit);
                unit.GetComponent<AttackingStateManager>().Attack(enemy);
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
            unit.GetComponent<AttackingStateManager>().CleanCommands();
            unit.GetComponent<BuildingStateManager>().CleanCommands();
        }

        private IEnumerable<GameObject> SelectDryUnits()
        {
            return selectedUnits.Where(unit => (SoakingState) unit.GetComponent<SoakingStateManager>().State == SoakingState.None);
        }
    }
}
