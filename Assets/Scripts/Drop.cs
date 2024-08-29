using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drop : MonoBehaviour,IDropHandler
{
    public AttackRows row;

    public int playerID;
    
    GameManager gameManager { get => GameObject.Find("GameManager").GetComponent<GameManager>(); }

    public void OnDrop(PointerEventData eventData)
    {
        int numberOfCardsInRow = this.GetComponent<HorizontalLayoutGroup>().transform.childCount;   
        GameObject cardToDrop = Drag.DraggedCard;
        if (numberOfCardsInRow<6 && cardToDrop.GetComponent<CardDisplay>().card is UnityCard unity && unity.AttackRows.Contains(row) && cardToDrop.GetComponent<Drag>().originalParent.parent.name ==this.transform.parent.parent.name && this.transform.parent.name == "unityGrid")
        {
            DropCardWithRotation(cardToDrop);
            gameManager.ExecuteTurnAsync(TurnActions.PlayCard,playerID, row, cardToDrop.GetComponent<CardDisplay>().card);
        }

        else if (numberOfCardsInRow == 0 && cardToDrop.GetComponent<CardDisplay>().card is BoostCard boost && boost.AttackRows.Contains(row) && cardToDrop.GetComponent<Drag>().originalParent.parent.name == this.transform.parent.parent.name && this.transform.parent.name == "boostColumn")
        {
            DropCardWithoutRotation(cardToDrop);
            gameManager.ExecuteTurnAsync(TurnActions.PlayCard, playerID, row, cardToDrop.GetComponent<CardDisplay>().card);
        }
        else if (numberOfCardsInRow == 0 && cardToDrop.GetComponent<CardDisplay>().card is WeatherCard weather && weather.AttackRows.Contains(row) && this.transform.parent.name == "weatherGrid")
        {
            DropCardWithRotation(cardToDrop);
            if (gameManager.Player1.IsPlaying) gameManager.ExecuteTurnAsync( TurnActions.PlayCard,0, row, cardToDrop.GetComponent<CardDisplay>().card);
            else gameManager.ExecuteTurnAsync(TurnActions.PlayCard, 1, row, cardToDrop.GetComponent<CardDisplay>().card);
        }
    }
    private void DropCardWithoutRotation(GameObject cardToDrop)
    {
        cardToDrop.transform.SetParent(this.GetComponent<HorizontalLayoutGroup>().transform);
        cardToDrop.transform.position = this.transform.position;
        cardToDrop.GetComponent<CanvasGroup>().blocksRaycasts = true;
        cardToDrop.GetComponent<Drag>().enabled = false;
    }
    void DropCardWithRotation(GameObject cardToDrop)
    {
        cardToDrop.transform.SetParent(this.GetComponent<HorizontalLayoutGroup>().transform);
        cardToDrop.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        cardToDrop.GetComponent<CanvasGroup>().blocksRaycasts = true;
        cardToDrop.GetComponent<Drag>().enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
