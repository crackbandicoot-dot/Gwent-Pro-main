using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlayersManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public Button OkButton;

    // Start is called before the first frame update
    void Start()
    {
        OkButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerName.text.Length > 4)
        {
            if((this.transform.parent.name == "Player1" && GameData.Player1Faction != null) || (this.transform.parent.name == "Player2" && GameData.Player2Faction != null))
            OkButton.interactable = true;
        }

        else
        {
            OkButton.interactable = false;
        }
    }

    public void SetName(string Player)
    {
        if(Player == "1")
        {
            GameData.SetPlayer1Name(PlayerName.text);
            GameData.Player1Ready = true;
        }

        else if(Player == "2")
        {
            GameData.SetPlayer2Name(PlayerName.text);
            GameData.Player2Ready = true;
        }
    }
}
