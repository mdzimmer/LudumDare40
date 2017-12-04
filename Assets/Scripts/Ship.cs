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
    Color startColor;

    float NO_HOVER_SCALE = 0.5f;
    float HOVER_SCALE = 0.6f;

    public Ship(string configFile, Vector2 originTile)
    {
        gm = GameManager.GetManager();
        grid = gm.grid;
        config = LoadConfig(configFile);
        model = gm.CreatePrefab(config.name);
        UpdateTiles(originTile);
        startColor = model.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_Color");
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
        SetOpacity(1.0f);
    }
    
    public Selector GenerateSelector(Vector3 position)
    {
        GameObject go = gm.CreatePrefab("Selector");
        go.transform.position = position;
        Vector3 popUp = new Vector3(1.0f / 3.0f, 1.0f / 3.0f, -1.0f / 3.0f).normalized * 1.0f;
        go.transform.position = new Vector3(go.transform.position.x + popUp.x, go.transform.position.y + popUp.y, go.transform.position.z + popUp.z);
        Selector selector = go.GetComponent<Selector>();
        selector.Initialize(config);
        return selector;
    }

    public bool ValidBuild()
    {
        foreach (Vector2 tile in tileLayout)
        {
            if (grid.tiles.ContainsKey(tile))
            {
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
                    return true;
                }
            }
        }
        return false;
    }

    public void Stop()
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

    public void SetOpacity(float opacity)
    {
        foreach (Transform child in model.transform)
        {
            MeshRenderer mr = child.gameObject.GetComponent<MeshRenderer>();
            Color color = mr.material.GetColor("_Color");
            color.a = opacity;
            mr.material.SetColor("_Color", color);
        }
    }

    public void SetTint(Color? tint = null)
    {
        if (tint == null)
        {
            tint = startColor;
        }
        Color appliedTint = (Color)tint;
        appliedTint.a = model.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_Color").a;
        foreach (Transform child in model.transform)
        {
            MeshRenderer mr = child.gameObject.GetComponent<MeshRenderer>();
            mr.material.SetColor("_Color", appliedTint);
        }
    }

    public static Config LoadConfig(string configFile)
    {
        return JsonUtility.FromJson<Config>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, configFile + ".json")));
    }

    public class Config
    {
        public string name;
        public string buildFlavor;
        public int[] usedTiles;
        public int[] unusedTiles;
        public int buildCost;
        public string[] canBuild;
        public string abilityName;
        public int abilityCost;
        public string abilityFlavor;
        public float abilityCooldown;
    }
}
