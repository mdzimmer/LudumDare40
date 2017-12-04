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

    GameManager gm;
    Currency currency;
    public Dictionary<string, GameObject> elements;

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
        elements["DescriptionBox"].SetActive(true);
        elements["Name"].SetActive(true);
        elements["Name"].GetComponent<Text>().text = name;
        elements["Cost"].SetActive(true);
        elements["Cost"].GetComponent<Text>().text = cost + " gold";
        elements["Flavor"].SetActive(true);
        elements["Flavor"].GetComponent<Text>().text = flavor;
    }

    public void HideDescription()
    {
        elements["DescriptionBox"].SetActive(false);
        elements["Name"].SetActive(false);
        elements["Cost"].SetActive(false);
        elements["Flavor"].SetActive(false);
    }
}
