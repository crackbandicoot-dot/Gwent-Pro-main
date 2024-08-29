using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButton : MonoBehaviour
{
    GameManager gameManager { get => GameObject.Find("GameManager").GetComponent<GameManager>(); }
    public int playerID;
    // Start is called before the first frame update
    public void OnClick()
    {
        gameManager.ExecuteTurnAsync(TurnActions.Pass,playerID);
    }
    
}
