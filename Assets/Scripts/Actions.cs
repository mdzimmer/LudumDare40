using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions {
    public delegate void Action(int cost);

    public static void TestAbility(int cost)
    {
        GameManager.GetManager().currency.IncrementValue(-cost);
    }

    public static void TestBuild(int cost)
    {
        GameManager.GetManager().StartBuild("test_ship", cost);
    }
}
