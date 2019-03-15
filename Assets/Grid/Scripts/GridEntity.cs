using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridEntity : MonoBehaviour
    {
        public Vector3 moveTo = new Vector3();
        public Vector3Int gridPosition = new Vector3Int();
        public Vector3Int previousGridPosition = new Vector3Int();
        public Vector3 grid = new Vector3();
        public bool lockToGrid = false;

        private GameObject meshObject;
        private GameObject target;

        public GameObject MeshObject { get => meshObject; set => meshObject = value; }
        public GameObject Target { get => target; set => target = value; }


        private void Start()
        {
            foreach (Transform t in transform) {
                if (t.CompareTag("Target")) {
                    target = t.gameObject;
                }
                else if (t.CompareTag("Mesh")) {
                    meshObject = t.gameObject;
                }
            }
        }

        private void Update()
        {
            UpdatePosition();
            TestChangedPosition();

            DebugSpaceKey();

            previousGridPosition = gridPosition;
        }


        private void UpdatePosition()
        {
            transform.position = Grid3D.instance.transform.position;
            FindGridPosition();

            if (lockToGrid) {
                LockToGrid();
            }
            else {
                meshObject.transform.position = target.transform.position + MeshOffset();
            }

        }

        private Vector3 MeshOffset()
        {
            Vector3 floorOffset = Vector3.up * Grid3D.instance.FloorHeight;
            Vector3 cellOffset = Grid3D.instance.cellSize / 2;
            Mesh mesh = meshObject.GetComponent<MeshFilter>().mesh;

            Vector3 sizeOffset = Vector3.Scale(meshObject.transform.localScale, mesh.bounds.extents);
            Vector3 meshOffset = new Vector3(cellOffset.x, (sizeOffset.y + floorOffset.y), cellOffset.z);

            return meshOffset;
        }

        private void FindGridPosition()
        {
            gridPosition = ClampPosition(Grid3D.WorldToCell(target.transform.position));
        }

        private Vector3Int ClampPosition(Vector3Int gp)
        {
            Vector3 border = Grid3D.instance.gridSize - Vector3.one;

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
            meshObject.transform.position = gridPosition + MeshOffset();
        }

        public void TestChangedPosition()
        {
            if (gridPosition != previousGridPosition) {
                GridCell previousCell = Grid3D.GridCells[previousGridPosition.x, previousGridPosition.y, previousGridPosition.z] as GridCell;
                GridCell newCell = Grid3D.GridCells[gridPosition.x, gridPosition.y, gridPosition.z] as GridCell;

                previousCell.RemoveFromCell(this);
                newCell.AddToCell(this);

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
                Debug.Log("World To Cell: " + Grid3D.WorldToCell(transform.position));
                Debug.Log("World To Local: " + Grid3D.WorldToLocal(transform.position));
                Debug.Log("Cell To World: " + Grid3D.CellToWorld(new Vector3Int(1, 0, 1)));
                Debug.Log("Bounds Local: " + Grid3D.GetBoundsLocal(new Vector3Int(2, 0, 1)));
            }
        }

        #endregion

    }
}

