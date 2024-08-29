using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Image image;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCard(Card card)
    {
      image.sprite = Resources.Load<Sprite>(card.Name);
      if(image.sprite == default)
      {
            name.text = card.Name;
            name.color = ((ICard)(card)).Type switch
            {
                "Gold" => Color.yellow,
                "Silver" => Color.gray,
                "Decoy" => Color.magenta,
                "Weather" => Color.red,
                "Leader" => Color.black,
                "Boost" => Color.green,
                "Clearing" => Color.cyan,
                _ => throw new Exception()
            }; 
      }
      if (card is UnityCard unity && card is not DecoyCard) textMeshPro.text = ((int)((card as ICard).Power)).ToString();
      else  textMeshPro.text = "";
      this.card = card;
    }
}
