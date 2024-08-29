using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static string Player1Name;
    public static string Player2Name;
    public static string Player1Faction;
    public static string Player2Faction;

    public static bool Player1Ready = false;
    public static bool Player2Ready = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player1Ready && Player2Ready)
        {
            SceneManager.LoadScene("Battlefield");
        }
    }

    public static void SetPlayer1Name(string name)
    { Player1Name = name; }

    public static void SetPlayer2Name(string name)
    { Player2Name = name; }

    public static void SetPlayer1Faction(string faction)
    { Player1Faction = faction;}

    public static void SetPlayer2Faction(string faction)
    { Player2Faction = faction;}
}
