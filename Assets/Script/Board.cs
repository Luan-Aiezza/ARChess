using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform gridAnchor; // referência ao objeto invisível
    public int gridSize = 8;
    public float cellSize = 0.05f;

    void Awake()
    {
        if (gridAnchor == null)
            Debug.LogError("GridAnchor não está configurado no Board!");
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        return gridAnchor.position
            + gridAnchor.right * (x * cellSize + cellSize / 2f)
            + gridAnchor.forward * (y * cellSize + cellSize / 2f);
    }
}
