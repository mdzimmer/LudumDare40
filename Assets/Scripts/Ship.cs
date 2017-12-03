using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ship {
    public List<Vector2> usedTiles;
    public List<Vector2> unusedTiles;

    List<Vector2> tileLayout;
    GameManager gm;
    Config config;
    Grid grid;
    GameObject model;

    float NO_HOVER_SCALE = 0.5f;
    float HOVER_SCALE = 0.6f;

    public Ship(Grid _grid, string configFile, Vector2 originTile)
    {
        gm = GameManager.GetManager();
        config = JsonUtility.FromJson<Config>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, configFile + ".json")));
        model = gm.CreatePrefab(config.name);
        grid = _grid;
        UpdateTiles(originTile);
        //model.transform.position = grid.TileToPosition(originTile);
    }

    public void Build()
    {
        tileLayout = new List<Vector2>();
        tileLayout.AddRange(usedTiles);
        tileLayout.AddRange(unusedTiles);
        foreach (Vector2 tile in tileLayout)
        {
            grid.tiles[tile] = this;
        }
    }
    
    public Selector GenerateSelector(Vector3 position)
    {
        //TEMPORARY
        GameObject go = gm.CreatePrefab("Selector");
        go.transform.position = position;
        Vector3 popUp = new Vector3(1.0f / 3.0f, 1.0f / 3.0f, -1.0f / 3.0f).normalized * 1.0f;
        go.transform.position = new Vector3(go.transform.position.x + popUp.x, go.transform.position.y + popUp.y, go.transform.position.z + popUp.z);
        Selector selector = go.GetComponent<Selector>();
        selector.Initialize(
            new Selector.Selection(
                "spears",
                Actions.TestAbility,
                Color.red
                ),
            new Selector.Selection(
                "party",
                Actions.TestBuild,
                Color.blue
                ),
            new Selector.Selection(
                "beer",
                Actions.TestBuild,
                Color.green
                )
            );
        return selector;
    }

    public bool ValidBuild()
    {
        foreach (Vector2 tile in tileLayout)
        {
            //Debug.Log(tile);
            if (grid.tiles.ContainsKey(tile))
            {
                //Debug.Log("occupied " + tile);
                return false;
            }
        }
        foreach (Vector2 tile in usedTiles)
        {
            List<Vector2> neighbors = grid.GetNeighbors(tile);
            foreach (Vector2 neighbor in neighbors)
            {
                Ship neighborShip = grid.tiles[neighbor];
                if (neighborShip != this && neighborShip.usedTiles.Contains(neighbor))
                {
                    //Debug.Log(tile + " : " + neighbor);
                    return true;
                }
            }
        }
        return false;
    }

    public void Destroy()
    {
        gm.DoDestroy(model);
        foreach (Vector2 layoutTile in tileLayout)
        {
            grid.tiles.Remove(layoutTile);
        }
    }

    public void UpdateTiles(Vector2 originTile)
    {
        tileLayout = new List<Vector2>();
        usedTiles = new List<Vector2>();
        unusedTiles = new List<Vector2>();
        for (int i = 0; i < config.usedTiles.Length; i += 2)
        {
            usedTiles.Add(new Vector2(config.usedTiles[i], config.usedTiles[i + 1]) + originTile);
        }
        for (int i = 0; i < config.unusedTiles.Length; i += 2)
        {
            unusedTiles.Add(new Vector2(config.unusedTiles[i], config.unusedTiles[i + 1]) + originTile);
        }
        model.transform.position = grid.TileToPosition(originTile);
        tileLayout.AddRange(usedTiles);
        tileLayout.AddRange(unusedTiles);
    }

    class Config
    {
        public string name;
        public int[] usedTiles;
        public int[] unusedTiles;
        public string abilityName;
        public int abilityCost;
        public string abilityFlavor;
        public float abilityCooldown;
    }
}
