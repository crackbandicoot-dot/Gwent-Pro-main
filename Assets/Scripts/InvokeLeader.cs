using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InvokeLeader : MonoBehaviour
{
    public GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (this.transform.parent.name == "Player1")
        {
            var leader1 = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            leader1.GetComponent<CardDisplay>().UpdateCard(gameManager.Player1.Leader);
            leader1.transform.SetParent(this.transform);
            leader1.GetComponent<Drag>().enabled = false;
        }
        else
        {
            var leader2 = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            leader2.GetComponent<CardDisplay>().UpdateCard(gameManager.Player2.Leader);
            leader2.transform.SetParent(this.transform);
            leader2.GetComponent<Drag>().enabled = false;
        }
        

    }
}

