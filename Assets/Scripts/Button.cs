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
    
    float SHOW_DESCRIPTION_TIME = 0.25f;
    float HIDE_DESCRIPTION_TIME = 0.25f;

    void Start()
    {
        gm = GameManager.GetManager();
        currency = gm.currency;
        elements = transform.parent.gameObject.GetComponent<Selector>().elements;
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
        //StopCoroutine(DoHideDescription());
        elements["DescriptionBox"].SetActive(true);
        elements["Name"].SetActive(true);
        elements["Name"].GetComponent<Text>().text = name;
        elements["Cost"].SetActive(true);
        elements["Cost"].GetComponent<Text>().text = cost + " gold";
        elements["Flavor"].SetActive(true);
        elements["Flavor"].GetComponent<Text>().text = flavor;
        //StartCoroutine(DoShowDescription());
    }

    public void HideDescription()
    {
        //StopCoroutine(DoShowDescription());
        elements["DescriptionBox"].SetActive(false);
        elements["Name"].SetActive(false);
        elements["Cost"].SetActive(false);
        elements["Flavor"].SetActive(false);
        //StartCoroutine(DoHideDescription());
    }

    //Selector.ElementWithScale Load(string name)
    //{
    //    GameObject go = elements[name];
    //    Selector.ElementWithScale output = new Selector.ElementWithScale(
    //        go,
    //        go.GetComponent<RectTransform>().sizeDelta
    //        );
    //}

    //IEnumerator DoShowDescription()
    //{
    //    List<Selector.ElementWithScale> elements = new List<Selector.ElementWithScale>()
    //    {
    //        Load("Description"),
    //        Load("Name"),
    //        Load("Cost"),
    //        Load("Flavor")
    //    };
    //    float elapsedtime = 0.0f;
    //    while (elapsedtime < SHOW_DESCRIPTION_TIME)
    //    {
    //        elapsedtime += Time.deltaTime;
    //        float progress = elapsedtime / SHOW_DESCRIPTION_TIME;
    //        foreach(Selector.ElementWithScale ews in elements)
    //        {

    //        }
    //    }
    //}

    //IEnumerator DoHideDescription()
    //{

    //}
}
