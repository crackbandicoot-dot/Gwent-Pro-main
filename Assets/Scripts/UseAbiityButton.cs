using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class UseAbilityButton : MonoBehaviour
{
    GameManager gameManager { get => GameObject.Find("GameManager").GetComponent<GameManager>(); }
    public int playerID;
    // Start is called before the first frame update
    public void OnClick()
    {
        gameManager.ExecuteTurnAsync(TurnActions.UseAbility, playerID);
    }
}
