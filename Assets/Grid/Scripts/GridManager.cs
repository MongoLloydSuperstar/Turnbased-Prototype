using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNamespace
{
    
    public class GridManager : MonoBehaviour
    {
        #region Fields
        public static GridManager gridManager = null;

        public Vector3Int gridSize;
        public float gridScale = 1;

        private ScriptableObject[,,] gridTiles;


        #region ToDo/Ideas


        #endregion

        #endregion

        public ScriptableObject[,,] GridTiles { get => gridTiles; private set => gridTiles = value; }

        #region Internal Methods

        private void Awake()
        {
            if (gridManager == null) {
                gridManager = this;
            }
            else if (gridManager != this) {
                Debug.LogWarning("Tried to instantiate additional GridManager");
                Destroy(gameObject);
            }

            InitializeTileList();
        }


        private void InitializeTileList()
        {
            //Sets a default Grid if non exists
            if (gridSize == null || gridSize == Vector3.zero) {
                gridSize = new Vector3Int(10, 10, 10);
            }

            gridTiles = new GridTile[gridSize.x, gridSize.y, gridSize.z];

            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    for (int z = 0; z < gridSize.z; z++) {
                        GridTile tile = ScriptableObject.CreateInstance(typeof(GridTile)) as GridTile;
                        tile.Initialize(new Vector3(x, y, z));


                        gridTiles[x, y, z] = tile;
                    }
                }
            }
        }


        #endregion
    }
}