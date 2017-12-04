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

    public static void longBoat(int cost)
    {
        GameManager.GetManager().StartBuild("longBoat", cost);
    }

    public static void barracks(int cost)
    {
        GameManager.GetManager().StartBuild("barracks", cost);
    }

    public static void berserkersHold(int cost)
    {
        GameManager.GetManager().StartBuild("berserkersHold", cost);
    }

    public static void cheiftainsHold(int cost)
    {
        GameManager.GetManager().StartBuild("cheiftainsHold", cost);
    }

    public static void danceHall(int cost)
    {
        GameManager.GetManager().StartBuild("danceHall", cost);
    }

    public static void dragonPen(int cost)
    {
        GameManager.GetManager().StartBuild("dragonPen", cost);
    }

    public static void drumBoat(int cost)
    {
        GameManager.GetManager().StartBuild("drumBoat", cost);
    }

    public static void meadHall(int cost)
    {
        GameManager.GetManager().StartBuild("meadHall", cost);
    }

    public static void shrine(int cost)
    {
        GameManager.GetManager().StartBuild("shrine", cost);
    }

    public static void stadium(int cost)
    {
        GameManager.GetManager().StartBuild("stadium", cost);
    }

    public static void wishingWell(int cost)
    {
        GameManager.GetManager().StartBuild("wishingWell", cost);
    }
}
