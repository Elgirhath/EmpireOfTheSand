using System.Collections.Generic;
using Assets.Map;
using Assets.Unit.ResourceGathering;
using Assets.Util;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                var tile = tilemap.GetTile(tilemap.GetComponent<CellPositionProvider>().GetCellPosition(pos));
                var tileType = _tileTypeRetriever.GetTileType(tile);

                if (tileType == TileType.DEFAULT) return;
                CommandResourceGathering(pos);
            }
            else
            {
                CommandMovement(ScreenToWorldConverter.ToWorldPosition(Input.mousePosition));
            }
        }

        public void NotifyOnMouseButtonUp(int buttonNumber, GameObject obj)
        {
            Debug.Log(obj.name);
        }

        private void CommandMovement(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
                unit.GetComponent<ResourceGatheringFlowManager>().CleanDestinationResource();
                unit.GetComponent<UnitMovementController>().SetDestination(position);
            }
        }

        private void CommandResourceGathering(Vector3 position)
        {
            foreach (var unit in selectedUnits)
            {
                unit.GetComponent<ResourceGatheringFlowManager>().SetDestinationResource(position);
            }
        }
    }
}
