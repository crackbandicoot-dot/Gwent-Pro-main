using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 position =new Vector3(-720,0,0);
    private Vector3 scale = new Vector3(5f, 5f, 5f);
    private GameObject cardToShow;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardToShow = Instantiate(this.gameObject,new Vector3(0,0,0),Quaternion.identity);
        cardToShow.transform.SetParent(this.transform.root);
        cardToShow.transform.localPosition = position;
        cardToShow.transform.localScale = scale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(cardToShow);
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
