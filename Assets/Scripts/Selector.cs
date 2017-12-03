using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
    public GameObject buildButtonOne;
    public GameObject buildButtonTwo;
    public GameObject buildButtonThree;
    public GameObject abilityButton;

    GameManager gm;
    //List<Element> elements;

    //float HORIZONTAL_OFFSET_DISTANCE = 0.8f;
    //float VERTICAL_OFFSET_DISTANCE = 0.8f;
    //float HORIZONTAL_OFFSET = 1f;
    //float VERTICAL_OFFSET = 1.5f;
    //float POP_IN_TIME = 0.05f;
    //float POP_OUT_TIME = 0.05f;
    //float MIN_SCALE = 0.001f;

    public void Initialize(Selection? ability, params Selection[] builds)
    {
        gm = GameManager.GetManager();
        //elements = new List<Element>();
        //elements.Add(CreateElement("Arrow", new Vector2(0, 0.35f)));
        //for (int i = 0; i < selections.Length; i++)
        //{
        //    Selection selection = selections[i];
        //    Vector2 offset = new Vector2();
        //    offset.x = (i - selections.Length / 2) * HORIZONTAL_OFFSET;
        //    offset.y = VERTICAL_OFFSET;
        //    elements.Add(CreateElement(selection.prefabName, offset));
        //}
        if (ability != null)
        {
            ApplySelection((Selection)ability, abilityButton);
        } else
        {
            //TODO clean up action elements
        }
        switch (builds.Length)
        {
            case 0:
                //TODO: clean up build elements
                break;
            case 1:
                ApplySelection(builds[0], buildButtonOne);
                buildButtonTwo.SetActive(false);
                buildButtonThree.SetActive(false);
                break;
            case 2:
                ApplySelection(builds[0], buildButtonOne);
                ApplySelection(builds[1], buildButtonTwo);
                buildButtonThree.SetActive(false);
                break;
            case 3:
                ApplySelection(builds[0], buildButtonOne);
                ApplySelection(builds[1], buildButtonTwo);
                ApplySelection(builds[2], buildButtonThree);
                break;
        }
        StartCoroutine(PopIn());
    }

    public void Close()
    {
        StopCoroutine(PopIn());
        StartCoroutine(PopOut());
    }

    //private Element CreateElement(string prefabName, Vector2 offset)
    //{
    //    GameObject go = gm.CreatePrefab(prefabName);
    //    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
    //    go.transform.position = (Vector2)transform.position + new Vector2(offset.x * HORIZONTAL_OFFSET_DISTANCE, offset.y * VERTICAL_OFFSET_DISTANCE);
    //    go.transform.parent = this.transform;
    //    go.SetActive(false);
    //    Element output = new Element(go, sr);
    //    return output;
    //}

    Sprite GetSprite(string spriteName)
    {
        return Resources.Load("Sprites/" + spriteName) as Sprite;
    }

    void ApplySelection(Selection selection, GameObject element)
    {
        //element.GetComponent<Image>().sprite = GetSprite((selection).spriteName);
        element.GetComponent<Button>().action = selection.action;
    }

    IEnumerator PopIn()
    {
        //Queue<Element> queue = new Queue<Element>();
        //foreach(Element element in elements)
        //{
        //    queue.Enqueue(element);
        //}
        //while (queue.Count != 0)
        //{
        //    Element element = queue.Dequeue();
        //    element.go.SetActive(true);
        //    Vector2 startScale = element.go.transform.localScale;
        //    element.go.transform.localScale = new Vector3(MIN_SCALE, MIN_SCALE, 1.0f);
        //    float elapsedTime = 0f;
        //    while (elapsedTime < POP_IN_TIME)
        //    {
        //        float curScale = Mathf.Max((elapsedTime / POP_IN_TIME) * startScale.x, MIN_SCALE);
        //        element.go.transform.localScale = new Vector3(curScale, curScale, 1.0f);
        //        elapsedTime += Time.deltaTime;
        //        yield return new WaitForEndOfFrame();
        //    }
        //    element.go.transform.localScale = startScale;
        //}
        yield return new WaitForEndOfFrame();
    }

    IEnumerator PopOut()
    {
        //Queue<Element> queue = new Queue<Element>();
        //foreach (Element element in elements)
        //{
        //    if (element.go.activeSelf)
        //    {
        //        queue.Enqueue(element);
        //    }
        //}
        //while (queue.Count != 0)
        //{
        //    Element element = queue.Dequeue();
        //    Vector2 startScale = element.go.transform.localScale;
        //    element.go.transform.localScale = new Vector3(MIN_SCALE, MIN_SCALE, 1.0f);
        //    float elapsedTime = 0f;
        //    while (elapsedTime < POP_OUT_TIME)
        //    {
        //        float curScale = (1.0f - (elapsedTime / POP_OUT_TIME)) * startScale.x;
        //        element.go.transform.localScale = new Vector3(curScale, curScale, 1.0f);
        //        elapsedTime += Time.deltaTime;
        //        yield return new WaitForEndOfFrame();
        //    }
        //    element.go.SetActive(false);
        //}
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }

    public struct Selection
    {
        //public string prefabName;
        //public abilitys.ability ability;

        //public Selection(string _prefabName, abilitys.ability _ability)
        //{
        //    prefabName = _prefabName;
        //    ability = _ability;
        //}
        public string spriteName;
        public Actions.Action action;
        public Color color;

        public Selection(string _spriteName, Actions.Action _action, Color _color)
        {
            spriteName = _spriteName;
            action = _action;
            color = _color;
        }
    }

    //struct Element
    //{
    //    public GameObject go;
    //    public SpriteRenderer sr;

    //    public Element(GameObject _go, SpriteRenderer _sr)
    //    {
    //        go = _go;
    //        sr = _sr;
    //    }
    //}
}
