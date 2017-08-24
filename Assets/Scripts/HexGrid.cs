using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [Range(1,34)]
    public int radius = 1;

    public HexCell cellPrefab;

    HexMesh hexMesh;

    public HexCell[] Cells { get; private set; }

    public void Awake()
    {
        hexMesh = GetComponentInChildren<HexMesh>();
        Cells = new HexCell[GetNumCells(radius)];

        for(int y = radius, i = 0; y >= -radius; y--)
        {
            Transform row = new GameObject("y = " + y).transform;
            row.SetParent(transform, false);

            for(int x = -radius + (y < 0 ? y * -1 : 0); x <= radius - (y > 0 ? y : 0); x++)
            {
                CreateCell(new HexCoordinates(x, y), i++, row);
            }
        }
    }

    public void Start()
    {
        hexMesh.Triangulate(Cells);
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            hexMesh.GetCellMousedOver();
        }
    }

    // Returns cell object based on coordinates
    // Returns null if coordinates do not map to a cell
    public HexCell GetCell(HexCoordinates coordinates)
    {
        HexCell cell = null;

        if (CoordsInGrid(coordinates))
        {
            // First index of row for rectangular grid
            int index = (-coordinates.Y + radius) * (2 * radius + 1);

            // Disregard missing cells below row 0
            index -= Triangular(radius);

            if (coordinates.Y > 0)
            {
                // Add cells from row y to row 0
                index += Triangular(coordinates.Y);
            }
            else if (coordinates.Y < 0)
            {
                // Disregard missing cells in row 0 to y-1
                index -= Triangular(-coordinates.Y - 1);
            }

            // Account for column
            index += coordinates.X + radius + Mathf.Min(coordinates.Y, 0);

            cell = Cells[index];
        }

        return cell;
    }

    // Returns true if the passed coordinates map to a cell in the grid
    public bool CoordsInGrid(HexCoordinates c)
    {
        return Mathf.Abs(c.Y) <= radius && Mathf.Abs(c.X) <= radius && (c.Y == 0 || c.Y > 0 && c.X <= radius - c.Y || c.Y < 0 && c.X >= -radius - c.Y);
    }

    // Returns an array of all cells adjacent to parameter cell
    public HexCell[] GetNeighbours(HexCell cell)
    {
        List<HexCell> cellList = new List<HexCell>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x != y)
                {
                    HexCell neighbour = GetCell(new HexCoordinates(cell.coordinates.X + x, cell.coordinates.Y + y));
                    if(neighbour != null)
                    {
                        cellList.Add(neighbour);
                    }
                }
            }
        }

        return cellList.ToArray();
    }

    // Instantiates new cell
    private void CreateCell(HexCoordinates coords, int i, Transform row)
    {
        /*Vector2 position;
        position.x = (x + y * 0.5f) * (HexDimensions.outerRadius * 1.5f);
        position.y = y * (HexDimensions.outerRadius * 1.5f);*/

        HexCell cell = Instantiate<HexCell>(cellPrefab, HexCoordinates.CoordsToPosition(coords), cellPrefab.transform.rotation);
        cell.transform.name = "Cell: " + "(" + coords.X + ", " + coords.Y + ")";
        cell.transform.SetParent(row, false);
        Cells[i] = cell;
        cell.coordinates = coords;

        // Villain is at origin cell
        cell.IsVillainCell = coords.X == 0 && coords.Y == 0;
    }

    // Returns the cells that the heroes will spawn in
    public HexCell[] GetHeroSpawnCells()
    {
        HexCell[] heroCells = new HexCell[3];

        heroCells[0] = Cells[0];// Top left
        heroCells[1] = Cells[Cells.Length / 2 + radius];// Center right
        heroCells[2] = GetCell(new HexCoordinates(0, -radius));// Bottom left

        return heroCells;
    }

    // Returns triangular number of n (1 + 2 + 3 ... + n)
    public static int Triangular(int n)
    {
        return (n * n + n) / 2;
    }

    // Returns the number of hex cells in a hexagon grid of radius r
    public static int GetNumCells(int r)
    {
        return 3 * (r * r + r) + 1;
    }
}
