using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public Dictionary<Vector2, Ship> tiles;

    int TILE_HALF_HEIGHT = 1;
    int TILE_HALF_WIDTH = 2;

	public Grid()
    {
        tiles = new Dictionary<Vector2, Ship>();
    }

    public Vector2 mouseToTile()
    {
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int mapX = (int)(worldMousePosition.x / TILE_HALF_WIDTH + worldMousePosition.y / TILE_HALF_HEIGHT) / 2;
        int mapY = (int)(worldMousePosition.y / TILE_HALF_HEIGHT - worldMousePosition.x / TILE_HALF_WIDTH) / 2;
        return new Vector2(mapX, mapY);
    }

    public Vector2 tileToPosition(Vector2 tile)
    {
        float positionX = (tile.x - tile.y) * TILE_HALF_WIDTH;
        float positionY = (tile.x + tile.y) * TILE_HALF_HEIGHT;
        return new Vector2(positionX, positionY);
    }
}
