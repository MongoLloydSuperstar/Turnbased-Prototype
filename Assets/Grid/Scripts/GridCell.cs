using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridSystem
{
    public class GridCell : ScriptableObject
    {
        private Vector3 gridPosition;
        private List<GridEntity> gridEntities = new List<GridEntity>();
        private bool isGround = false;

        #region Get/Set
        public List<GridEntity> GridEntities { get => gridEntities; set => gridEntities = value; }
        public Vector3 GridPosition { get => gridPosition; private set => gridPosition = value; }

        #endregion


        public void Initialize(Vector3 gp) {            
            gridPosition = gp;

            string positionString = gp.x + ", " + gp.y + ", " + gp.z;
            name = "Cell - " + positionString;
        }

        public void AddToCell(GridEntity gridEntity)
        {
            gridEntities.Add(gridEntity);
        }

        public void RemoveFromCell(GridEntity gridEntity)
        {
            gridEntities.Remove(gridEntity);
        }
    }
}
