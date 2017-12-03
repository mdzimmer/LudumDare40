using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions {
    public delegate void Action();

    public static void TestAbility()
    {
        GameManager.GetManager().currency.IncrementValue(-10);
    }

    public static void TestBuild()
    {
        GameManager.GetManager().StartBuild("test_ship");
    }
}
