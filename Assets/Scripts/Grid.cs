using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public Dictionary<Vector2, Ship> tiles;

    GameManager gm;

    float TILE_SIZE = 1.0f;

	public Grid()
    {
        gm = GameManager.GetManager();
        tiles = new Dictionary<Vector2, Ship>();
    }

    public Vector2 MouseToTile()
    {
        Vector3 worldMousePosition = gm.GetMousePosition();
        Vector2 output = new Vector2(
            (int)Mathf.Floor(worldMousePosition.z / TILE_SIZE),
            (int)Mathf.Floor(-worldMousePosition.x / TILE_SIZE)
            );
        return output;
    }

    public Vector3 TileToPosition(Vector2 tile)
    {
        Vector3 output = new Vector3(
            -tile.y * TILE_SIZE - TILE_SIZE / 2.0f,
            0.0f,
            tile.x * TILE_SIZE + TILE_SIZE / 2.0f
            );
        return output;
    }

    public List<Vector2> GetNeighbors(Vector2 tile)
    {
        List<Vector2> output = new List<Vector2>();
        List<Vector2> permutations = new List<Vector2>()
        {
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1)
        };
        foreach (Vector2 permutation in permutations)
        {
            Vector2 neighborTile = tile + permutation;
            if (tiles.ContainsKey(neighborTile))
            {
                output.Add(neighborTile);
            }
        }
        return output;
    }
}
