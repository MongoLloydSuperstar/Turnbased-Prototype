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
                Mathf.Floor(transform.position.x) * GridManager.gridManager.gridScale,
                Mathf.Floor(transform.position.y) * GridManager.gridManager.gridScale,
                Mathf.Floor(transform.position.z) * GridManager.gridManager.gridScale
                );
        }

        private void SetGridPosition()
        {
            transform.position = gridPosition;
        }

        private void Update()
        {
            UpdatePosition();
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

