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
        gridTileList.Clear();

        ScriptableObject[,,] tiles = GridNamespace.Grid.grid.GridTiles;
        foreach (GridTile gridTile in tiles) {
            gridTileList.Add(gridTile);
        }


    }
}
