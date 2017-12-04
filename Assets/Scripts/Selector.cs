using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
    public Dictionary<string, GameObject> elements;
    public AudioSource boop;

    GameManager gm;

    float POP_IN_TIME = 0.1f;
    float POP_OUT_TIME = 0.1f;

    public void Initialize(Ship.Config config)
    {
        gm = GameManager.GetManager();
        elements = new Dictionary<string, GameObject>();
        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;
            elements[go.name] = go;
        }
        float buildWidth = 70.0f;
        if (config.abilityName != "") {
            elements["Ability"].GetComponent<Image>().sprite = GetSprite(config.abilityName);
            Button button = elements["Ability"].GetComponent<Button>();
            button.name = config.abilityName;
            button.cooldown = config.abilityCooldown;
            button.flavor = config.abilityFlavor;
            button.cost = config.abilityCost;
            button.action = NameToAction(config.abilityName);
        } else
        {
            elements["AbilityBox"].SetActive(false);
            elements["Ability"].SetActive(false);
            buildWidth = 100.0f;
        }
        if (buildWidth == 100.0f)
        {
            elements["BuildBox"].GetComponent<RectTransform>().sizeDelta = new Vector2(buildWidth, 27.22f);
        }
        for (int i = 0; i < 3; i++)
        {
            string name = "BuildOne";
            switch (i)
            {
                case 1:
                    name = "BuildTwo";
                    break;
                case 2:
                    name = "BuildThree";
                    break;
            }
            GameObject element = elements[name];
            if (config.canBuild.Length < i + 1)
            {
                element.SetActive(false);
                continue;
            }
            element.GetComponent<RectTransform>().anchoredPosition = new Vector2((buildWidth / (config.canBuild.Length + 1.0f)) * (i + 1), -11.5f);
            element.GetComponent<Button>().action = NameToAction(config.canBuild[i]);
            Ship.Config targetConfig = Ship.LoadConfig(config.canBuild[i]);
            element.GetComponent<Button>().cost = targetConfig.buildCost;
            element.GetComponent<Button>().name = targetConfig.name;
            element.GetComponent<Button>().flavor = targetConfig.buildFlavor;
        }
        elements["DescriptionBox"].SetActive(false);
        elements["Flavor"].SetActive(false);
        elements["Name"].SetActive(false);
        elements["Cost"].SetActive(false);
        StartCoroutine(PopIn());
    }

    public void Close()
    {
        StopCoroutine(PopIn());
        StartCoroutine(PopOut());
    }

    Actions.Action NameToAction(string name)
    {
        switch (name)
        {
            case "TestAbility":
                return Actions.TestAbility;
            case "test_ship":
                return Actions.TestBuild;
            case "longBoat":
                return Actions.longBoat;
            case "barracks":
                return Actions.barracks;
            case "berserkersHold":
                return Actions.berserkersHold;
            case "cheiftainsHold":
                return Actions.cheiftainsHold;
            case "danceHall":
                return Actions.danceHall;
            case "dragonPen":
                return Actions.dragonPen;
            case "drumBoat":
                return Actions.drumBoat;
            case "meadHall":
                return Actions.meadHall;
            case "shrine":
                return Actions.shrine;
            case "stadium":
                return Actions.stadium;
            case "wishingWell":
                return Actions.wishingWell;

        }
        return null;
    }

    Sprite GetSprite(string spriteName)
    {
        return Resources.Load("Sprites/" + spriteName) as Sprite;
    }

    void AddByEnable(Queue<List<ElementWithScale>> queue, bool desiredEnabled, params string[] names)
    {
        List<ElementWithScale> row = new List<ElementWithScale>();
        foreach (string name in names)
        {
            GameObject go = elements[name];
            if (go.activeSelf == desiredEnabled)
            {
                ElementWithScale col = new ElementWithScale();
                col.element = go;
                col.scale = go.GetComponent<RectTransform>().sizeDelta;
                row.Add(col);
            }
        }
        queue.Enqueue(row);
    }

    IEnumerator PopIn()
    {
        Queue<List<ElementWithScale>> queue = new Queue<List<ElementWithScale>>();
        AddByEnable(queue, true, "Arrow");
        AddByEnable(queue, true, "BuildBox", "AbilityBox");
        AddByEnable(queue, true, "BuildHeader", "AbilityHeader");
        AddByEnable(queue, true, "BuildOne", "BuildTwo", "BuildThree", "Ability");
        foreach (List<ElementWithScale> list in queue)
        {
            foreach (ElementWithScale element in list)
            {
                element.element.GetComponent<RectTransform>().sizeDelta = new Vector2(0.001f, 0.001f);
            }
        }
        while (queue.Count > 0)
        {
            List<ElementWithScale> list = queue.Dequeue();
            float timeElapsed = 0.0f;
            while (timeElapsed < POP_IN_TIME)
            {
                timeElapsed += Time.deltaTime;
                float progression = timeElapsed / POP_IN_TIME;
                foreach (ElementWithScale ews in list)
                {
                    ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(ews.scale.x * progression, 0.0f, 100.0f), Mathf.Clamp(ews.scale.y * progression, 0.0f, 100.0f));
                }
                yield return new WaitForEndOfFrame();
            }
            foreach (ElementWithScale ews in list)
            {
                ews.element.GetComponent<RectTransform>().sizeDelta = ews.scale;
            }
        }
        yield return new WaitForEndOfFrame();
    }

    public struct ElementWithScale
    {
        public GameObject element;
        public Vector2 scale;

        public ElementWithScale(GameObject _element, Vector2 _scale)
        {
            element = _element;
            scale = _scale;
        }
    }

    IEnumerator PopOut()
    {
        Queue<List<ElementWithScale>> queue = new Queue<List<ElementWithScale>>();
        AddByEnable(queue, true, "BuildOne", "BuildTwo", "BuildThree", "Ability");
        AddByEnable(queue, true, "BuildHeader", "AbilityHeader");
        AddByEnable(queue, true, "BuildBox", "AbilityBox");
        AddByEnable(queue, true, "Arrow");
        while (queue.Count > 0)
        {
            List<ElementWithScale> list = queue.Dequeue();
            float timeElapsed = 0.0f;
            while (timeElapsed < POP_OUT_TIME)
            {
                timeElapsed += Time.deltaTime;
                float progression = 1.0f - (timeElapsed / POP_OUT_TIME);
                foreach (ElementWithScale ews in list)
                {
                    ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(ews.scale.x * progression, 0.0f, 100.0f), Mathf.Clamp(ews.scale.y * progression, 0.0f, 100.0f));
                }
                yield return new WaitForEndOfFrame();
            }
            foreach (ElementWithScale ews in list)
            {
                ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(0.001f, 0.001f);
            }
        }
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }
}
