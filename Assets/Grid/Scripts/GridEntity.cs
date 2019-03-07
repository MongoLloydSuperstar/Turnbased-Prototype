using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNamespace
{
    public class GridEntity : MonoBehaviour
    {
        public Vector3 gridPosition = new Vector3();
        public Vector3 targetOnGrid = new Vector3();
        public bool lockToGrid = false;
        

        

        private void FindGridPosition()
        {
            gridPosition = new Vector3(
                Mathf.Floor(transform.position.x * Grid.grid.cellSize.x),
                Mathf.Floor(transform.position.y * Grid.grid.cellSize.y),
                Mathf.Floor(transform.position.z * Grid.grid.cellSize.z)
                );
        }

        private void SetGridPosition()
        {
            transform.position = gridPosition;
        }

        private void Update()
        {
            UpdatePosition();

            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("World To Cell: " + GridNamespace.Grid.grid.WorldToCell(transform.position));
                Debug.Log("World To Local: " + GridNamespace.Grid.grid.WorldToLocal(transform.position));
            }
        }

        private void UpdatePosition()
        {
            FindGridPosition();
            if (lockToGrid) {
                SetGridPosition();
            }
        }

        private void MoveToPosition()
        {

        }
    }
}

