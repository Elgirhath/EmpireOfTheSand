using System.Collections.Generic;
using Assets.Map;
using Assets.Unit.ResourceGathering;
using Assets.Util;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileType = Assets.Map.TileType;

namespace Assets.Unit.Managers
{
    public class UnitActionManager : MonoBehaviour
    {
        private ISet<GameObject> selectedUnits;
        private TileTypeRetriever _tileTypeRetriever;
        private Tilemap tilemap;

        private void Start()
        {
            selectedUnits = GetComponent<UnitSelectionManager>().SelectedUnits;
            tilemap = FindObjectOfType<Tilemap>();
            _tileTypeRetriever = tilemap.GetComponent<TileTypeRetriever>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonUp(1)) return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                var pos = ScreenToWorldConverter.ToWorldPosition(Input.mousePosition);
                var tile = tilemap.GetComponent<global::Map>().GetTileAtPosition(pos);

                if (tile.type == TileType.Default) return;
                CommandResourceGathering(tile);
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
                unit.GetComponent<ResourceGatheringFlowManager>().CleanDestinationResource();
                unit.GetComponent<UnitMovementController>().SetDestination(position);
            }
        }

        private void CommandResourceGathering(CustomTile tile)
        {
            foreach (var unit in selectedUnits)
            {
                unit.GetComponent<ResourceGatheringFlowManager>().SetDestinationResource(tile);
            }
        }
    }
}
