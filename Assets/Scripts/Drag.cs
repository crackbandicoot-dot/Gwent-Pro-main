using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{    
    public static GameObject DraggedCard;
    public Transform originalParent;
    public Vector3 originalPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
      originalParent = this.transform.parent;
      originalPosition = this.transform.position;
      this.transform.SetParent(this.transform.root);
      this.transform.SetAsLastSibling();
      DraggedCard = this.gameObject;
      this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.SetParent(originalParent);
        this.transform.position = originalPosition;
        if (this.GetComponent<CardDisplay>().card is ClearingCard clearingCard)
        {
            if(gameManager.Player1.IsPlaying) gameManager.ExecuteTurnAsync(TurnActions.PlayCard,0,AttackRows.M,clearingCard);   
            else gameManager.ExecuteTurnAsync(TurnActions.PlayCard,1, AttackRows.M, clearingCard);
            Destroy(this.gameObject);
        }
       
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
