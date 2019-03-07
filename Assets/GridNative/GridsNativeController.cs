using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridsNativeController : MonoBehaviour
{
    [SerializeField] private Vector3 cellSize = Vector3.one;
    private List<Transform> grids = new List<Transform>();

    public List<Transform> Grids { get => grids; set => grids = value; }

        
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            Grid grid = child.GetComponent<Grid>();

            if (grid != null) {
                grids.Add(child);
                child.position = Vector3.up * i * cellSize.y;
                grid.cellSize = new Vector3(cellSize.x, cellSize.z, cellSize.y); //Z and Y switch places because Swizzle
            }
        }        
    }
}
