using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridNamespace;

public class GridTesting : MonoBehaviour
{
    public List<GridTile> gridTileList = new List<GridTile>();
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //AddTiles();
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Grid grid = GetComponent<Grid>();
            Debug.Log("Native Local Bounds: " + grid.GetBoundsLocal(new Vector3Int(2, 0, 1)));
        }

    }

    private void AddTiles()
    {
        gridTileList.Clear();

        ScriptableObject[,,] tiles = GridNamespace.Grid3D.grid.GridTiles;
        foreach (GridTile gridTile in tiles) {
            gridTileList.Add(gridTile);
        }
    }
}
