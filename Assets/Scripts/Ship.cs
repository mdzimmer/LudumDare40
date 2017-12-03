using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship {
    GameManager gm;
    Type type;
    List<GameObject> sprites; //TEMPORARY
    float NO_HOVER_SCALE = 0.8f;
    float HOVER_SCALE = 1.0f;

    public Ship(GameManager _gm, Grid grid, Type _type, Vector2 originTile)
    {
        gm = _gm;
        type = _type;
        List<Vector2> layout = new List<Vector2>();
        sprites = new List<GameObject>(); //TEMPORARY
        Color tintColor = Color.grey; //TEMPORARY
        switch (type)
        {
            case Type.CAPITAL:
                layout = new List<Vector2>{
                    new Vector2(0,0),
                    new Vector2(1,0),
                    new Vector2(2,0)
                };
                tintColor = Color.grey;
                break;
            case Type.ONE_BY_ONE:
                layout = new List<Vector2>{
                    new Vector2(0,0)
                };
                tintColor = Color.red;
                break;
            case Type.ONE_BY_TWO:
                layout = new List<Vector2>{
                    new Vector2(0,0),
                    new Vector2(1,0)
                };
                tintColor = Color.blue;
                break;
            case Type.L_SHAPE:
                layout = new List<Vector2>{
                    new Vector2(0,0),
                    new Vector2(1,0),
                    new Vector2(2,0),
                    new Vector2(2,1)
                };
                tintColor = Color.green;
                break;
        }
        foreach (Vector2 layoutTile in layout)
        {
            Vector2 gridTile = originTile + layoutTile;
            grid.tiles[gridTile] = this;
            //TEMPORARY tile logic
            GameObject testTile = gm.CreatePrefab("TestTile");
            testTile.transform.position = grid.tileToPosition(gridTile);
            testTile.transform.localScale = new Vector3(NO_HOVER_SCALE, NO_HOVER_SCALE, 1.0f);
            sprites.Add(testTile);
            testTile.GetComponent<SpriteRenderer>().color = tintColor;
        }
    }

    public void BeginHover()
    {
        foreach (GameObject sprite in sprites)
        {
            sprite.transform.localScale = new Vector3(HOVER_SCALE, HOVER_SCALE, 1.0f);
        }
    }

    public void EndHover()
    {
        foreach (GameObject sprite in sprites)
        {
            sprite.transform.localScale = new Vector3(NO_HOVER_SCALE, NO_HOVER_SCALE, 1.0f);
        }
    }

    public enum Type
    {
        NONE,
        CAPITAL,
        ONE_BY_ONE,
        ONE_BY_TWO,
        L_SHAPE
    }
}
