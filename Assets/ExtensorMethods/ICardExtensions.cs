using DSL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ExtensorMethods
{
    public static class ICardExtensions
    {
        public static ICard Clone(this ICard card,ICardFactory cardFactory)
        {
            return cardFactory.CreateCard(card.Name, card.Faction,card.Type,card.Range,card.Power,card.Effect);
        }
        public static IEnumerable<ICard> GetClones(this ICard card,ICardFactory cardFactory,int amountOfCopies)
        {
            for (int i = 0; i < amountOfCopies; i++)
            {
                yield return Clone(card, cardFactory);
            }
        }
    }
}
