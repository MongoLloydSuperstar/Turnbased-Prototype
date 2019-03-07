using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNamespace
{

    public class Grid : MonoBehaviour
    {
        #region Fields
        public static Grid grid = null;

        public Vector3Int gridSize;
        public Vector3 cellSize = Vector3.one;

        public ScriptableObject[,,] GridTiles { get; private set; }


        #endregion

        #region Public methods and accessors
        // Get the local position of a cell
        public Vector3 CellToLocal(Vector3Int cellPosition)
        {
            return cellPosition;
        }

        // Get the world position of a cell
        public Vector3 CellToWorld(Vector3Int cellPosition)
        {
            return cellPosition + transform.position;
        }

        // Get the bounds of a cell
        public Bounds GetBoundsLocal(Vector3Int cellPosition)
        {
            return new Bounds(cellPosition, cellSize / 2.0f);
        }

        // Get a cell from local position
        public Vector3Int LocalToCell(Vector3 localPosition)
        {
            return new Vector3Int(
                Mathf.FloorToInt(localPosition.x * cellSize.x),
                Mathf.FloorToInt(localPosition.y * cellSize.y),
                Mathf.FloorToInt(localPosition.z * cellSize.z)
                );
        }

        // Get world position from local position
        public Vector3 LocalToWorld(Vector3 localPosition)
        {
            return localPosition + transform.position;
        }

        // Get a cell fro world position
        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            Vector3 localPosition = WorldToLocal(worldPosition);

            return new Vector3Int(
                Mathf.FloorToInt(localPosition.x * cellSize.x),
                Mathf.FloorToInt(localPosition.y * cellSize.y),
                Mathf.FloorToInt(localPosition.z * cellSize.z)
                );
        }

        // Get local position from world position
        public Vector3 WorldToLocal(Vector3 worldPosition)
        {
            return worldPosition - transform.position;
        }


        #endregion

        #region Internal Methods

        private void Awake()
        {
            if (grid == null) {
                grid = this;
            }
            else if (grid != this) {
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

            GridTiles = new GridTile[gridSize.x, gridSize.y, gridSize.z];

            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    for (int z = 0; z < gridSize.z; z++) {
                        GridTile tile = ScriptableObject.CreateInstance(typeof(GridTile)) as GridTile;
                        tile.Initialize(new Vector3(x, y, z));


                        GridTiles[x, y, z] = tile;
                    }
                }
            }
        }


        #endregion
    }
}