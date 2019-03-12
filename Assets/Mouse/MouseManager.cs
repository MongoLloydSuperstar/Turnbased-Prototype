using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class MouseManager : MonoBehaviour
{
    private GameObject cellMarker;
    MeshRenderer markerMeshRenderer;
    private Camera cam;
    private bool isHit;
    private Vector3Int targetCell;

    void Start()
    {
        cam = Camera.main;
        InitializeMarker();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {

            if (Input.GetMouseButtonDown(0)) {
                
            }
            

            isHit = true;
            targetCell = Grid3D.instance.WorldToCell(raycastHit.point);
        }
        else {
            isHit = false;
        }


        

        if (isHit) {
            Vector3 worldPos = Grid3D.instance.CellToWorld(targetCell) + Vector3.Scale(Grid3D.instance.cellSize, new Vector3(1, 0, 1)) / 2;
            Vector3 yOffset = Vector3.up * (Grid3D.instance.FloorHeight + 0.001f);
            worldPos += yOffset;

            cellMarker.transform.position = worldPos;
            markerMeshRenderer.enabled = true;
        }
        else {
            markerMeshRenderer.enabled = false;
        }

    }

    private void InitializeMarker()
    {
        cellMarker = Grid3D.instance.CreateQuad(
                        Vector3.Scale(Grid3D.instance.GetGridCenter(), new Vector3(1, 0, 1)),
                        Quaternion.Euler(90 * Vector3.right),
                        Vector3.one
                        );

        markerMeshRenderer = cellMarker.GetComponent<MeshRenderer>();
        cellMarker.GetComponent<MeshRenderer>().enabled = false;

        BuildTexture();
    }

    private void BuildTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        Texture2D textureStandard = new Texture2D(1, 1);
        Color markerColor = Color.red;
        Color standard = Color.white;
        markerColor.a = 0.5f;

        texture.SetPixel(1, 1, markerColor);
        textureStandard.SetPixel(1, 1, standard);

        texture.Apply();
        markerMeshRenderer.sharedMaterials[0].mainTexture = textureStandard;
        markerMeshRenderer.material.mainTexture = texture;
    }

}
