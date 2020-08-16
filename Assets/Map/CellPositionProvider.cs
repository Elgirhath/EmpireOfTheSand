using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPositionProvider : MonoBehaviour
{
    private GridLayout gridLayout;

    private void Start()
    {
        gridLayout = GetComponentInParent<GridLayout>();
    }

    public Vector3Int GetCellPosition(Vector3 worldPosition)
    {
        worldPosition.z = transform.position.z;
        return gridLayout.WorldToCell(worldPosition);
    }
}
