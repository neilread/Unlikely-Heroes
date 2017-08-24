using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public HexCell Location { get; private set; }

    // Changes the hero's position to new cell
    public void MoveToCell(HexCell newLocation)
    {
        Location = newLocation;
        transform.position = HexCoordinates.CellToPosition(Location);
    }
}
