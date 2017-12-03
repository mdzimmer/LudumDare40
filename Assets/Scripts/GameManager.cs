using System.Collections;
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

    Grid grid;
    Vector3 previousMousePosition = Vector3.zero;
    Selector curSelector;
    Ship buildGhost;

    // Use this for initialization
    void Start() {
        grid = new Grid();
        Ship startShip = new Ship(grid, "test_ship", new Vector2(0, 0));
        startShip.Build();
        //for (int i = -1; i < 2; i++)
        //{
        //    for (int j = -1; j < 2; j++)
        //    {
        //        GameObject go = CreatePrefab("TestCube");
        //        go.transform.position = grid.TileToPosition(new Vector2(i, j));
        //        go.name = i + ", " + j;
        //    }
        //}
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePosition = GetMousePosition();
        debugPointer.position = mousePosition;
        Vector2 mouseTile = grid.MouseToTile();
        Ship shipOnTile = grid.tiles.ContainsKey(mouseTile) ? grid.tiles[mouseTile] : null;
        if (Input.GetMouseButtonUp(0))
        {
            if (curSelector != null)
            {
                Vector3 vectorOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                vectorOrigin.y = vectorOrigin.y / Mathf.Sqrt(2.0f);
                Vector3 vector = new Vector3(-1.0f / 3.0f, -1.0f / 3.0f, 1.0f / 3.0f).normalized;
                RaycastHit[] hits = Physics.RaycastAll(vectorOrigin, vector);
                //Debug.DrawLine(vectorOrigin, vectorOrigin + vector * 10.0f, Color.red, 100.0f);
                foreach(RaycastHit hit in hits)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    Button button = hit.collider.GetComponent<Button>();
                    if (button != null)
                    {
                        //Debug.Log("button");
                        button.action();
                        curSelector.Close();
                        break;
                    }
                }
            }
            else if (buildGhost != null)
            {
                if (buildGhost.ValidBuild())
                {
                    //Debug.Log("build");
                    buildGhost.Build();
                    buildGhost = null;
                }
            }
            else if (shipOnTile != null)
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
                buildGhost.Destroy();
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
            //Debug.Log("update");
        }
	}

    public void StartBuild(string shipName)
    {
        buildGhost = new Ship(grid, shipName, grid.MouseToTile());
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

    float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public void DoDestroy(GameObject go)
    {
        Destroy(go);
    }
}
