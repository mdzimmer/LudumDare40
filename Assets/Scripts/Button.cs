using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour {
    public Actions.Action action;
    public int cost;
    public string name;
    public string flavor;
    public float cooldown;
    public Dictionary<string, GameObject> elements;

    GameManager gm;
    Currency currency;
    bool showing = false;
    List<Selector.ElementWithScale> ewsList;


    float SHOW_DESCRIPTION_TIME = 0.1f;
    float HIDE_DESCRIPTION_TIME = 0.1f;

    void Start()
    {
        gm = GameManager.GetManager();
        currency = gm.currency;
        elements = transform.parent.gameObject.GetComponent<Selector>().elements;
        ewsList = new List<Selector.ElementWithScale>()
        {
            Load("DescriptionBox"),
            Load("Name"),
            Load("Cost"),
            Load("Flavor")
        };
    }

	public bool DoAction()
    {
        if (currency.goalValue < cost)
        {
            return false;
        }
        action(cost);
        return true;
    }

    public void ShowDescription()
    {
        if (showing)
        {
            return;
        }
        showing = true;
        StopCoroutine(DoHideDescription());
        elements["DescriptionBox"].SetActive(true);
        elements["Name"].SetActive(true);
        elements["Name"].GetComponent<Text>().text = name;
        elements["Cost"].SetActive(true);
        elements["Cost"].GetComponent<Text>().text = cost + " gold";
        elements["Flavor"].SetActive(true);
        elements["Flavor"].GetComponent<Text>().text = flavor;
        StartCoroutine(DoShowDescription());
    }

    public void HideDescription()
    {
        if (!showing)
        {
            return;
        }
        showing = false;
        StopCoroutine(DoShowDescription());
        StartCoroutine(DoHideDescription());
    }

    Selector.ElementWithScale Load(string name)
    {
        GameObject go = elements[name];
        Selector.ElementWithScale output = new Selector.ElementWithScale(
            go,
            go.GetComponent<RectTransform>().sizeDelta
            );
        return output;
    }

    IEnumerator DoShowDescription()
    {
        float elapsedtime = 0.0f;
        while (elapsedtime < SHOW_DESCRIPTION_TIME)
        {
            elapsedtime += Time.deltaTime;
            float progress = elapsedtime / SHOW_DESCRIPTION_TIME;
            foreach (Selector.ElementWithScale ews in ewsList)
            {
                ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(ews.scale.x * progress, ews.scale.y * progress);
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (Selector.ElementWithScale ews in ewsList)
        {
            ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(ews.scale.x, ews.scale.y);
        }
    }

    IEnumerator DoHideDescription()
    {
        float elapsedtime = 0.0f;
        while (elapsedtime < SHOW_DESCRIPTION_TIME)
        {
            elapsedtime += Time.deltaTime;
            float progress = 1.0f - Mathf.Min((elapsedtime / SHOW_DESCRIPTION_TIME), 1.0f);
            foreach (Selector.ElementWithScale ews in ewsList)
            {
                ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(ews.scale.x * progress, ews.scale.y * progress);
            }
            yield return new WaitForEndOfFrame();
        }
        elements["DescriptionBox"].SetActive(false);
        elements["Name"].SetActive(false);
        elements["Cost"].SetActive(false);
        elements["Flavor"].SetActive(false);
        foreach (Selector.ElementWithScale ews in ewsList)
        {
            ews.element.GetComponent<RectTransform>().sizeDelta = new Vector2(ews.scale.x, ews.scale.y);
        }
    }
}
