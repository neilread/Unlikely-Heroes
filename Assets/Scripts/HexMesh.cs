using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    MeshCollider meshCollider;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();

        GetComponent<MeshRenderer>().material = Resources.Load("HexMaterial") as Material;
        
    }

    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();

        for(int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    private void Triangulate(HexCell cell)
    {
        if (cell != null)
        {
            //GetComponent<MeshRenderer>().material.SetColor("_Color", colors[Random.Range(0, colors.Length - 1)]);

            Vector3 center = cell.transform.localPosition;

            for (int i = 0; i < 6; i++)
            {
                AddTriangle(center, center + HexDimensions.corners[i], center + HexDimensions.corners[i + 1]);
            }
        }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    /*void OnMouseOver()
    {
        GetCellMousedOver();
    }*/

    // Gets cell mouse is currently over
    public void GetCellMousedOver()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 position = transform.InverseTransformPoint(hit.point);
            HexCoordinates coordinates = HexCoordinates.PositionToCoords(position);
            Debug.Log(GetComponentInParent<HexGrid>().GetCell(coordinates).coordinates);
        }
    }
}
