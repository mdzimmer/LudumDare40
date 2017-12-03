using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    Grid grid;
    Ship curHover;
    int buildNumber = 0; //TEMPORARY

	// Use this for initialization
	void Start () {
        grid = new Grid();
        new Ship(this, grid, Ship.Type.CAPITAL, new Vector2(0, 0));
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 mouseTile = grid.mouseToTile();
        Ship shipOnTile = grid.tiles.ContainsKey(mouseTile) ? grid.tiles[mouseTile] : null;
        if (curHover != null && (shipOnTile == null || shipOnTile != curHover))
        {
            curHover.EndHover();
            curHover = null;
        }
        if (shipOnTile != null)
        {
            if (curHover == null || curHover != shipOnTile)
            {
                curHover = shipOnTile;
                curHover.BeginHover();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {

            //TEMPORARY
            Ship.Type buildType = Ship.Type.NONE;
            switch (buildNumber)
            {
                case 0:
                    break;
                case 1:
                    buildType = Ship.Type.ONE_BY_ONE;
                    break;
                case 2:
                    buildType = Ship.Type.ONE_BY_TWO;
                    break;
                case 3:
                    buildType = Ship.Type.L_SHAPE;
                    break;
            }
            new Ship(this, grid, buildType, mouseTile);
        }
	}

    public GameObject CreatePrefab(string prefab)
    {
        return Instantiate(Resources.Load("Prefabs/" + prefab) as GameObject) as GameObject;
    }
}
