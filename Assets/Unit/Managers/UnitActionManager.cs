using System.Collections.Generic;
using Assets.Building;
using Assets.Map;
using Assets.Unit.Building;
using Assets.Unit.ResourceGathering;
using Assets.Util;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;
using Physics2D = UnityEngine.Physics2D;
using TileType = Assets.Map.TileType;

namespace Assets.Unit.Managers
{
    public class UnitActionManager : MonoBehaviour
    {
        private ISet<GameObject> selectedUnits;

        private void Start()
        {
            selectedUnits = GetComponent<UnitSelectionManager>().SelectedUnits;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonUp(1)) return;

            var pos = ScreenToWorldConverter.ToWorldPosition(Input.mousePosition);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                var tile = GameMap.Instance.tilemap.GetComponent<GameMap>().GetTileAtPosition(pos);

                if (tile.type == TileType.Default) return;
                CommandResourceGathering(tile);
            }
            else
            {
                var hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.transform?.GetComponent<ConstructionSite>() != null)
                {
                    CommandBuilding(hit.transform.GetComponent<ConstructionSite>());
                }
                else
                {
                    CommandMovement(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
                }
            }
        }

        private void CommandMovement(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
                CleanCommands(unit);
                unit.GetComponent<UnitMovementController>().SetDestination(position);
            }
        }

        private void CommandBuilding(ConstructionSite building)
        {
            foreach (var unit in selectedUnits)
            {
                CleanCommands(unit);
                unit.GetComponent<BuildingFlowManager>().AssignToBuild(building);
            }
        }

        private void CommandResourceGathering(CustomTile tile)
        {
            foreach (var unit in selectedUnits)
            {
                CleanCommands(unit);
                unit.GetComponent<ResourceGatheringFlowManager>().SetDestinationResource(tile);
            }
        }

        private void CleanCommands(GameObject unit)
        {
            unit.GetComponent<ResourceGatheringFlowManager>().CleanDestinationResource();
            unit.GetComponent<BuildingFlowManager>().CleanCommands();
        }
    }
}
