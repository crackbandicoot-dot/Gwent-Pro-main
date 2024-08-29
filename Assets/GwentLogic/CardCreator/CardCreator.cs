using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GwentLogic.CardFactory;
using DSL;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Unity.VisualScripting;
using DSL.Interfaces;
using Assets.ExtensorMethods;

public class CardContainer
{
    private int ammountOfPlayers=2;
    public  List<LeaderCard> goodsLeaders = new List<LeaderCard>(); 
    public  List<List<Card>> goodsDecks = new List<List<Card>>();
    public CardContainer()
    {
        //stringyPoints\",\r\n\tParams :\r\n\t{{\r\n\t\tQuotientarget in targets\r\n\t\t\ttarget.Power=target.Power*Quotient;\r\n\t}}\r\n}}\r\neffect\r\n{{\r\n\tName: \"Kill\",\r\n\tAction : (targets,context) =>{{\r\n\t\tfor target in targets\r\n\t\t{{\r\n\t\t\tcontext.Board.Remove(target);\r\n\t\t}}\r\n\t}}\r\n}}\r\neffect \r\n{{\r\n\tName: \"Draw\",\r\n\tParams:{{\r\n\t\tAmount : Number\r\n\t}},\r\n\tAction :(targets,context) =>{{\r\n\t\ti = 0;\r\n\t\twhile(i++<Amount)\r\n\t\t{{\r\n\t\t\tunit = context.Deck.Pop();\r\n\t\t\tcontext.Hand.Push(unit);\r\n\t\t}}\r\n\t}}\r\n}}\r\neffect\r\n{{\r\n\tName: \"PrintInfo\",\r\n\tAction: (targets,context) =>\r\n\t{{\r\n\t\tprint(\"Printing info ...\");\r\n\t\tfor unit in targets{{\r\n\t\t\tprint(\"Unit\"@@unit.Name@@\"has\"@@unit.Power@@\"Points\");\r\n\t\t}}\r\n\t}}\r\n}}\r\ncard\r\n{{\r\n\tType:\"Silver\",\r\n\tName : \"Wendy\",\r\n\tFaction: \"Goods\",\r\n\tPower : 4,\r\n\tRange:[\"Melee\"],\r\n\tOnActivation:\r\n\t[\r\n\t\t{{\r\n\t\t\tEffect : \r\n\t\t\t{{\r\n\t\t\t\tName:\"ModifyPoints\",\r\n\t\t\t\tQuotient :2\r\n\t\t\t}},\r\n\t\t\;
        StreamReader sd = new(@"Assets\Decks\Test.txt");

        string x = sd.ReadToEnd();
        var cardFactory = new CardFactory();
        var d = Compiler.Compile(x, cardFactory, Debug.Log);
        for (int i = 0; i < ammountOfPlayers; i++)
        {
            goodsDecks.Add(new List<Card>());
            foreach (var card in d)
            {
                switch(card.Type)
                {
                    case "Gold":
                        goodsDecks[i].AddRange(card.GetClones(cardFactory, 2));
                        break;
                    case "Silver":
                        goodsDecks[i].AddRange(card.GetClones(cardFactory, 3));
                        break;
                    case "Leader":
                        goodsLeaders.Add(card as LeaderCard);
                        break;
                    default:
                        goodsDecks[i].Add(card.Clone(cardFactory) as Card);
                        break;
                }
            }
        }
    }
}


