using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
    public Dictionary<string, GameObject> elements;

    GameManager gm;

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
            element.GetComponent<RectTransform>().anchoredPosition = new Vector2(buildWidth / (config.canBuild.Length + 1.0f), -11.5f);
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
        }
        return null;
    }

    Sprite GetSprite(string spriteName)
    {
        return Resources.Load("Sprites/" + spriteName) as Sprite;
    }

    void AddByEnable(Queue<GameObject> queue, string name, bool desiredEnabled)
    {
        GameObject go = elements[name];
        if (go.activeSelf == desiredEnabled)
        {
            queue.Enqueue(go);
        }
    }

    IEnumerator PopIn()
    {
        //Queue<GameObject> queue = new Queue<GameObject>();
        //AddByEnable()
        yield return new WaitForEndOfFrame();
    }

    IEnumerator PopOut()
    {
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }
}
