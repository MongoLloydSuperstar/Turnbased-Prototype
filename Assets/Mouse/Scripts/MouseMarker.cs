using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using MouseSystem;

public class MouseMarker : GridEntity
{
    public float selectionMarkerOffset = 1.0f;
    public Vector3 selectionMarkerScale = new Vector3(0.4f, 0.4f, 0.4f);
    private GameObject selectionMarker;

    private MeshRenderer markerMeshRenderer;
    private Camera cam;
    private bool isHit;

    private List<GridEntity> cellContent;
    private GridEntity selectedUnit;
    

    void Start()
    {
        cam = Camera.main;
        InitializeMarker();
    }

    void Update()
    {
        MarkerPosition();
        Selection();
    }


    private void Selection()
    {
        if (Input.GetMouseButtonDown(0)) {
            GridCell cell = Grid3D.GridCells[gridPosition.x, gridPosition.y, gridPosition.z] as GridCell;
            cellContent = cell.GridEntities;

            if (cellContent.Count > 0) {
                SelectUnit();

                // Create marker
                Vector3 unitPos = selectedUnit.MeshObject.transform.position;
                selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                selectionMarker.transform.position = unitPos + Vector3.up * selectionMarkerOffset;
                selectionMarker.transform.localScale = selectionMarkerScale;
            }
            else {
                DeselectUnit();
                Destroy(selectionMarker);
            }
        }


    }

    private void SelectUnit()
    {
        foreach (GridEntity gridEntity in cellContent) {
            if (gridEntity.GetComponentInParent<Transform>().CompareTag("PlayerUnit")) {
                selectedUnit = gridEntity;
            }
        }
    }

    private void DeselectUnit()
    {
        selectedUnit = null;
    }

    private void MarkerPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            isHit = true;
            gridPosition = Grid3D.WorldToCell(raycastHit.point);
        }
        else {
            isHit = false;
        }

        if (isHit) {
            Vector3 worldPos = Grid3D.CellToWorld(gridPosition) + Vector3.Scale(Grid3D.instance.cellSize, new Vector3(1, 0, 1)) / 2;
            Vector3 yOffset = Vector3.up * (Grid3D.instance.FloorHeight + 0.001f);
            worldPos += yOffset;

            transform.position = worldPos;
            markerMeshRenderer.enabled = true;
        }
        else {
            markerMeshRenderer.enabled = false;
        }
    }

    private void InitializeMarker()
    {
        markerMeshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().enabled = false;
    }
}
