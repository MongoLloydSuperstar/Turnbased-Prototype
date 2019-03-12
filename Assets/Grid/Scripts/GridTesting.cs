using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class GridTesting : MonoBehaviour
{
    public List<GridCell> gridCellList = new List<GridCell>();
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //AddCells();
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Grid grid = GetComponent<Grid>();
            Debug.Log("Native Local Bounds: " + grid.GetBoundsLocal(new Vector3Int(2, 0, 1)));
        }

    }

    private void AddCells()
    {
        gridCellList.Clear();

        ScriptableObject[,,] cells = GridSystem.Grid3D.instance.GridCells;
        foreach (GridCell gridCell in cells) {
            gridCellList.Add(gridCell);
        }
    }
}
