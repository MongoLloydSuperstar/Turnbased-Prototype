using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridSystem
{
    public class Grid3D : MonoBehaviour
    {
        #region Fields
        public static Grid3D instance = null;

        public Vector3Int gridSize;
        public Vector3 cellSize = Vector3.one;

        private float floorHeight = 0.1f;
        private GameObject floorCube;

        public static ScriptableObject[,,] GridCells { get; private set; }
        public GameObject FloorQuad { get => FloorQuad; private set => FloorQuad = value; }
        public GameObject FloorCube { get => floorCube; set => floorCube = value; }
        public float FloorHeight { get => floorHeight; private set => floorHeight = value; }
        
        #endregion

        #region Public methods and accessors
        // Get the local position of a cell
        public static Vector3 CellToLocal(Vector3Int cellPosition)
        {
            return cellPosition;
        }

        // Get the world position of a cell
        public static Vector3 CellToWorld(Vector3Int cellPosition)
        {
            return cellPosition + instance.transform.position;
        }

        // Get the bounds of a cell
        public static Bounds GetBoundsLocal(Vector3Int cellPosition)
        {
            return new Bounds(cellPosition, instance.cellSize);
        }

        // Get a cell from local position
        public static Vector3Int LocalToCell(Vector3 localPosition)
        {
            return new Vector3Int(
                Mathf.FloorToInt(localPosition.x * instance.cellSize.x),
                Mathf.FloorToInt(localPosition.y * instance.cellSize.y),
                Mathf.FloorToInt(localPosition.z * instance.cellSize.z)
                );
        }

        // Get world position from local position
        public static Vector3 LocalToWorld(Vector3 localPosition)
        {
            return localPosition + instance.transform.position;
        }

        // Get a cell fro world position
        public static Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return new Vector3Int(
                (int)(Mathf.Floor(worldPosition.x / instance.cellSize.x) * instance.cellSize.x),
                (int)(Mathf.Floor(worldPosition.y / instance.cellSize.y) * instance.cellSize.y),
                (int)(Mathf.Floor(worldPosition.z / instance.cellSize.z) * instance.cellSize.z)
                );
        }

        // Get local position from world position
        public static Vector3 WorldToLocal(Vector3 worldPosition)
        {
            return worldPosition - instance.transform.position;
        }

        public static Vector3 GetCellOffset(Vector3 cellSize)
        {
            return cellSize / 2;
        }

        public static Vector3 GetGridCenter(Vector3Int gridSize)
        {
            return (Vector3)gridSize / 2;
        }

        public static GameObject CreateQuad(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            GameObject fQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            fQuad.transform.position = pos;
            fQuad.transform.rotation = rot;
            fQuad.transform.localScale = scale;

            return fQuad;
        }

        public static GameObject CreateCube(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            GameObject fCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fCube.transform.position = pos;
            fCube.transform.rotation = rot;
            fCube.transform.localScale = scale;

            return fCube;
        }


        #endregion

        #region Internal Methods

        private void Awake()
        {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Debug.LogWarning("Tried to instantiate additional Grid3D");
                Destroy(gameObject);
            }

            InitializeTileList();
        }

        private void Start()
        {
            floorCube = CreateCube(
                Vector3.Scale(GetGridCenter(gridSize), new Vector3(1, 0, 1)) + (Vector3.up * (floorHeight / 2)),
                Quaternion.identity,
                new Vector3(gridSize.x, floorHeight, gridSize.z)
                );

            //FloorQuad = CreateQuad(
            //    Vector3.Scale(GetGridCenter(), new Vector3(1, Mathf.Epsilon, 1)), // Epsilon adds the smallest float value to put it on top. Float rounding issue if y is exactly 0
            //    Quaternion.Euler(90f * Vector3.right), 
            //    new Vector3(gridSize.x, gridSize.z, 1)
            //    );
        }

        private void Update()
        {
            DGetCellsHoldingEntity();
        }



        private void InitializeTileList()
        {
            //Sets a default (10, 10, 10) Grid if non exists
            if (gridSize == null || gridSize == Vector3.zero) {
                gridSize = new Vector3Int(10, 10, 10);
            }

            GridCells = new GridCell[gridSize.x, gridSize.y, gridSize.z];

            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    for (int z = 0; z < gridSize.z; z++) {
                        GridCell cell = ScriptableObject.CreateInstance(typeof(GridCell)) as GridCell;
                        cell.Initialize(new Vector3(x, y, z));


                        GridCells[x, y, z] = cell;
                    }
                }
            }
        }


        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            Color gridColor = Color.red;
            gridColor.a = 0.5f;
            Gizmos.color = gridColor;

            DrawGrid(gridSize.x, gridSize.z, cellSize.x, cellSize.z);
            DrawCorners();
            DrawCenter();
        }


        private void DrawGrid(int xMax, int zMax, float xCel, float zCel)
        {
            Vector3 transformOffset = Vector3.Scale((transform.position + cellSize), new Vector3(1, 0, 1));
            float heightOffset = 0;
            int i = 0;
            int j = 0;

            for (i = 0; i < xMax + 1; i++) {
                Vector3 iFrom = new Vector3(i - xCel, heightOffset, 0 - zCel) + transformOffset;
                Vector3 iTo = new Vector3(i - xCel, heightOffset, zMax - zCel) + transformOffset;

                Gizmos.DrawLine(iFrom, iTo);
            }
            for (j = 0; j < zMax + 1; j++) {
                Vector3 jFrom = new Vector3(0 - xCel, heightOffset, j - zCel) + transformOffset;
                Vector3 jTo = new Vector3(xMax - xCel, heightOffset, j - zCel) + transformOffset;

                Gizmos.DrawLine(jFrom, jTo);
            }
        }

        private void DrawCorners()
        {
            Vector3[] corners =
                { Vector3.zero, gridSize,
                new Vector3(0, 0, gridSize.z), new Vector3(0, gridSize.y, 0),new Vector3(gridSize.x, 0, 0),
                new Vector3(0, gridSize.y, gridSize.z), new Vector3(gridSize.x, 0, gridSize.z), new Vector3(gridSize.x, gridSize.y, 0)
            };
            float radius = 0.1f;

            foreach (Vector3 corner in corners) {
                Gizmos.DrawSphere(corner, radius);
            }
        }

        private void DrawCenter()
        {
            Gizmos.DrawSphere(GetGridCenter(gridSize), 0.1f);
        }

        #endregion

        #region Debug

        private void DGetCellsHoldingEntity()
        {
            if (Input.GetKeyDown(KeyCode.G)) {
                foreach (ScriptableObject scriptableObject in GridCells) {
                    GridCell gridCell = scriptableObject as GridCell;

                    if (gridCell.GridEntities.Count != 0) {
                        Debug.Log(gridCell.GridPosition);
                        foreach (GridEntity gridEntity in gridCell.GridEntities) {
                            Debug.Log(gridEntity);
                        }
                    }
                }
            }
        }

        #endregion
    }

}