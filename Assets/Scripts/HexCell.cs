using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{

    public int X { get; private set; }
    public int Y { get; private set; }

    public HexCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }

    public static Vector2 CellToPosition(HexCell cell)
    {
        return CoordsToPosition(cell.coordinates);
    }

    public static Vector2 CoordsToPosition(HexCoordinates coords)
    {
        Vector2 position;
        position.x = (coords.X + coords.Y * 0.5f) * (HexDimensions.outerRadius * 1.5f);
        position.y = coords.Y * (HexDimensions.outerRadius * 1.5f);

        return position;
    }

    public static HexCoordinates PositionToCoords(Vector3 position)
    {
        float offset = position.y / (HexDimensions.outerRadius * 3f);
        float x = position.x / (HexDimensions.innerRadius * 2f) - offset;
        float y = position.y / (HexDimensions.innerRadius * 2f) - offset;

        return new HexCoordinates(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ")";
    }

    public override bool Equals(object obj)
    {
        HexCoordinates coordinates = (HexCoordinates)obj;
        return X == coordinates.X && Y == coordinates.Y;
    }
}

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public bool IsVillainCell { get; set; }

    public override string ToString()
    {
        return coordinates.ToString();
    }
}

public static class HexDimensions
{
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners = 
    {
        new Vector3(0f, outerRadius, 0f),
        new Vector3(innerRadius, 0.5f * outerRadius, 0f),
        new Vector3(innerRadius, -0.5f * outerRadius, 0f),
        new Vector3(0f, -outerRadius, 0f),
        new Vector3(-innerRadius, -0.5f * outerRadius, 0f),
        new Vector3(-innerRadius, 0.5f * outerRadius, 0f),
        new Vector3(0f, outerRadius, 0f)
    };
}
