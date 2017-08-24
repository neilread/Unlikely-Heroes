using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    const int NumHeroes = 3;

    public HexGrid gridPrefab;
    public Hero heroPrefab;

    public Hero[] Heroes { get; private set; }

    private HexGrid grid;

	// Use this for initialization
	void Awake ()
    {
        grid = Instantiate<HexGrid>(gridPrefab);
        grid.name = "Grid";
        grid.transform.SetParent(transform);

        // Adjusts camera to fit grid
        Camera.main.orthographicSize = 15 * (grid.radius + 1);

        GameObject heroesObj = new GameObject("Heroes");
        heroesObj.transform.SetParent(transform);

        Heroes = new Hero[NumHeroes];
        HexCell[] spawns = grid.GetHeroSpawnCells();

        for (int i = 0; i < NumHeroes; i++)
        {
            Heroes[i] = Instantiate<Hero>(heroPrefab);
            Heroes[i].transform.SetParent(heroesObj.transform);
            Heroes[i].transform.name = "Hero " + (i + 1);
            Heroes[i].MoveToCell(spawns[i]);
        }
	}
}
