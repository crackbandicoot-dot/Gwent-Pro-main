using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    GameManager gameManager { get => GameObject.Find("GameManager").GetComponent<GameManager>(); }
    public GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //ameManager.Player1.Fetch(10);
        //GameManager.Player2.Fetch(10);
        if (this.transform.parent.name == "Player1")
        {
            gameManager.Player1.Fetch(10);
            UpdateHand();
        }
        else
        {
            gameManager.Player2.Fetch(10);
            UpdateHand();
        }
        /*if (this.transform.parent.name == "Player1")
        {
            GameManager.Player1.Fetch(10);
            List<Card> hand = GameManager.Player1.PlayerHand.Cards;
            foreach (Card card in hand)
            {
                var currentCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentCard.GetComponent<CardDisplay>().UpdateCard(card);
                currentCard.transform.SetParent(this.transform);
            }
        }
        else
        {
            GameManager.Player2.Fetch(10);
            List<Card> hand = GameManager.Player2.PlayerHand.Cards;
            foreach (Card card in hand)
            {
                var currentCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentCard.GetComponent<CardDisplay>().UpdateCard(card);
                currentCard.transform.SetParent(this.transform);
            }
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent.name == "Player1")
        {
            if(gameManager.Player1.IsPlaying)
                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
            else
                this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        else if(this.transform.parent.name == "Player2")
        {
            if(gameManager.Player2.IsPlaying)
                this.GetComponent <CanvasGroup>().blocksRaycasts = true;
            else
                this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void UpdateHand()
    {
        List<Card> backendHand;
        if (this.transform.parent.name == "Player1")
        {
            backendHand = gameManager.Player1.Hand.Cards;
        }
        else
        {
            backendHand = gameManager.Player2.Hand.Cards;
        }
        var frontendHand= GameObject.FindGameObjectsWithTag("Card").Where(card => card.transform.parent.name == this.transform.name);
        foreach (var frontendCard in frontendHand)
        {
            if (!backendHand.Contains(frontendCard.GetComponent<CardDisplay>().card))
            {
                Destroy(frontendCard);
            }
        }
        foreach (Card backendCard in backendHand)
        {
            bool backendCardIsInFrontend = false;
            foreach (var frontendCard in frontendHand)
            {
                if (frontendCard.GetComponent<CardDisplay>().card == backendCard)
                { 
                    backendCardIsInFrontend = true;
                    break;
                }
            }
            if (!backendCardIsInFrontend)
            {
                var currentCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                currentCard.GetComponent<CardDisplay>().UpdateCard(backendCard);
                currentCard.transform.SetParent(this.transform);
            }
        }
       
    }

}
