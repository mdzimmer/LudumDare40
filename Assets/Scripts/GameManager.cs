﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager GetManager()
    {
        return FindObjectOfType<GameManager>();
    }

    public Currency currency;
    public Transform cameraHandle;
    public Transform debugPointer;
    public Grid grid;
    public Overlay overlay;
    public AudioSource nope;
    public AudioSource splash;

    Vector3 previousMousePosition = Vector3.zero;
    Selector curSelector;
    Ship buildGhost;
    int buildCost;
    Button curButton;
    bool running = false;
    List<Helicopter> helicopters;

    float MIN_SPAWN_INTERVAL = 2.0f;
    float MAX_SPAWN_INTERVAL = 4.0f;
    float SPAWN_BUFFER_DISTANCE = 15.0f;
    float HELICOPTER_HEIGHT = 1.0f;
    float GHOST_OPACITY = 0.5f;

    void Start()
    {
        grid = new Grid();
    }

    // Update is called once per frame
    void Update () {
        if (!running)
        {
            return;
        }
        Vector3 mousePosition = GetMousePosition();
        debugPointer.position = mousePosition;
        Vector2 mouseTile = grid.MouseToTile();
        Ship shipOnTile = grid.tiles.ContainsKey(mouseTile) ? grid.tiles[mouseTile] : null;
        Vector3 vectorOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 vectorEnd = GetMousePosition();
        Vector3 vector = vectorEnd - vectorOrigin;
        RaycastHit[] hits = Physics.RaycastAll(vectorOrigin, vector);
        bool hitButton = false;
        foreach (RaycastHit hit in hits)
        {
            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                if (button != curButton)
                {
                    button.HideDescription();
                }
                curButton = button;
                hitButton = true;
                break;
            }
        }
        if (!hitButton && curButton != null)
        {
            curButton.HideDescription();
            curButton = null;
        }
        if (curButton != null)
        {
            curButton.ShowDescription();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (curSelector != null && curButton != null)
            {
                if (curButton.DoAction())
                {
                    curSelector.Close();
                    curSelector = null;
                } else
                {
                    nope.Play();
                }
            }
            else if (buildGhost != null)
            {
                if (buildGhost.ValidBuild())
                {
                    buildGhost.Build();
                    buildGhost = null;
                    currency.IncrementValue(-buildCost);
                    splash.Play();
                } else
                {
                    nope.Play();
                }
            }
            else if (shipOnTile != null && curSelector == null)
            {
                curSelector = shipOnTile.GenerateSelector(mousePosition);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (curSelector != null)
            {
                curSelector.Close();
                curSelector = null;
            }
            previousMousePosition = GetMousePosition();
            if (buildGhost != null)
            {
                buildGhost.Stop();
                buildGhost = null;
            }
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = previousMousePosition - GetMousePosition();
            cameraHandle.Translate(delta);
            previousMousePosition = GetMousePosition();
        }
        if (buildGhost != null)
        {
            buildGhost.UpdateTiles(grid.MouseToTile());
            if (!buildGhost.ValidBuild())
            {
                buildGhost.SetTint(Color.red);
            } else
            {
                buildGhost.SetTint();
            }
        }
	}

    public void BeginGame()
    {
        running = true;
        Ship startShip = new Ship("test_ship", new Vector2(0, 0));
        startShip.Build();
        helicopters = new List<Helicopter>();
        StartCoroutine(SpawnHelicopters());
        currency.Initialize();
    }

    public void EndGame()
    {
        if (running)
        {
            running = false;
            overlay.GoToEnd();
        }
    }

    public void StartBuild(string shipName, int _buildCost)
    {
        buildGhost = new Ship(shipName, grid.MouseToTile());
        buildGhost.SetOpacity(GHOST_OPACITY);
        buildCost = _buildCost;
    }

    public GameObject CreatePrefab(string prefab)
    {
        return Instantiate(Resources.Load("Prefabs/" + prefab) as GameObject) as GameObject;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 vectorOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vectorOrigin.y = vectorOrigin.y / Mathf.Sqrt(2.0f);
        Vector3 vector = new Vector3(-1.0f / 3.0f, -1.0f / 3.0f, 1.0f / 3.0f).normalized;
        Vector3 planeNormal = new Vector3(0.0f, 1.0f, 0.0f);
        float t = -(Dot(vectorOrigin, planeNormal)) / Dot(vector, planeNormal);
        Vector3 intersectionPoint = vectorOrigin + t * vector;
        return intersectionPoint;
    }

    public void DoDestroy(GameObject go)
    {
        Destroy(go);
    }

    float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    IEnumerator SpawnHelicopters()
    {
        float elapsedTime = 0.0f;
        float timeToSpawn = 0.0f;
        while (true)
        {
            if (!running)
            {
                break;
            }
            if (elapsedTime >= timeToSpawn)
            {
                float spawnAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
                Vector3 spawnOffset = new Vector3(
                    Mathf.Cos(spawnAngle) * SPAWN_BUFFER_DISTANCE,
                    HELICOPTER_HEIGHT,
                    Mathf.Sin(spawnAngle) * SPAWN_BUFFER_DISTANCE
                );
                GameObject go = CreatePrefab("Helicopter");
                Vector3 cameraPosition = cameraHandle.position;
                cameraPosition.y = 0.0f;
                go.transform.position = cameraPosition + spawnOffset;
                Helicopter helicopter = go.GetComponent<Helicopter>();
                helicopter.Initialize();
                helicopters.Add(helicopter);
                elapsedTime = 0.0f;
                timeToSpawn = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
