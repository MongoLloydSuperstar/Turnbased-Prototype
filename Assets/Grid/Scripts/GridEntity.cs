using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNamespace
{
    public class GridEntity : MonoBehaviour
    {
        public Vector3 moveTo = new Vector3();
        public Vector3Int gridPosition = new Vector3Int();
        public Vector3Int previousGridPosition = new Vector3Int();
        public Vector3 grid = new Vector3();
        public bool lockToGrid = false;

        private GameObject mesh;
        private GameObject target;

        public GameObject Mesh { get => mesh; set => mesh = value; }
        public GameObject Target { get => target; set => target = value; }

        private void Start()
        {
            foreach (Transform t in transform) {
                if (t.CompareTag("Target")) {
                    target = t.gameObject;
                }
                else if (t.CompareTag("Mesh")) {
                    mesh = t.gameObject;
                }
            }
        }

        private void Update()
        {
            previousGridPosition = gridPosition;

            UpdatePosition();
            TestChangedPosition();

            DebugSpaceKey();
        }

        

        private void UpdatePosition()
        {
            transform.position = Grid3D.grid.transform.position;
            FindGridPosition();
            if (lockToGrid) {
                LockToGrid();
            }
            else {
                mesh.transform.position = target.transform.position;
            }

        }

        private void FindGridPosition()
        {
            gridPosition = ClampPosition(Grid3D.grid.WorldToCell(target.transform.position));
        }

        private Vector3Int ClampPosition(Vector3Int gp)
        {
            Vector3 border = Grid3D.grid.gridSize - Vector3.one;

             Vector3Int clampedPosition = new Vector3Int
                (
                (int)Mathf.Clamp(gp.x, 0, border.x),
                (int)Mathf.Clamp(gp.y, 0, border.y),
                (int)Mathf.Clamp(gp.z, 0, border.z)
                );

            return clampedPosition;
        }

        private void LockToGrid()
        {
            mesh.transform.position = gridPosition + Grid3D.grid.GetCellOffset();
        }

        public void TestChangedPosition()
        {
            if (gridPosition != previousGridPosition) {
                GridTile previousTile = Grid3D.grid.GridTiles[previousGridPosition.x, previousGridPosition.y, previousGridPosition.z] as GridTile;
                GridTile newTile = Grid3D.grid.GridTiles[gridPosition.x, gridPosition.y, gridPosition.z] as GridTile;

                previousTile.RemoveFromTile(this);
                newTile.AddToTile(this);

                transform.hasChanged = false;
            }            
        }

        private void MoveToPosition()
        {

        }
        

        #region Debug

        private void DebugSpaceKey()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("World To Cell: " + Grid3D.grid.WorldToCell(transform.position));
                Debug.Log("World To Local: " + Grid3D.grid.WorldToLocal(transform.position));
                Debug.Log("Cell To World: " + Grid3D.grid.CellToWorld(new Vector3Int(1, 0, 1)));
                Debug.Log("Bounds Local: " + Grid3D.grid.GetBoundsLocal(new Vector3Int(2, 0, 1)));
            }
        }

        #endregion

    }
}

