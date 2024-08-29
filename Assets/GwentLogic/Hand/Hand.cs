using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hand: IList<ICard>
{
    private List<Card> _handCards;
    public List<Card> Cards { get => _handCards; }
    public int NumberOfCards { get => _handCards.Count; }

    int ICollection<ICard>.Count => _handCards.Count;

    bool ICollection<ICard>.IsReadOnly => false;

    ICard IList<ICard>.this[int index] { get => _handCards[index]; set => _handCards[index]=value as Card; }

    public Hand(List<Card> handCards)
    {
        _handCards = handCards;
    }
    public void AddCard(Card card)
    {
     _handCards.Add(card);
    }
    public Card RemoveCard(int cardPosition)
    {
        Card card = _handCards[cardPosition];
        _handCards.RemoveAt(cardPosition);
        return card;
    }
    public Card RemoveCard(Card card)
    {
        _handCards.Remove(card);
        return card;
    }
    public void ShowCards()
    {
        for (int i = 0; i < NumberOfCards; i++)
        {
            Console.WriteLine($"{ i}:{ _handCards[i].Name}");
        }
    }

    int IList<ICard>.IndexOf(ICard item)
    {
        return _handCards.IndexOf(item as Card);
    }

    void IList<ICard>.Insert(int index, ICard item)
    {
        _handCards.Insert(index, item as Card); 
    }

    void IList<ICard>.RemoveAt(int index)
    {
        _handCards.RemoveAt(index);
    }

    void ICollection<ICard>.Add(ICard item)
    {
        _handCards.Add(item as Card);
    }

    void ICollection<ICard>.Clear()
    {
        _handCards.Clear();
    }

    bool ICollection<ICard>.Contains(ICard item)
    {
        return _handCards.Contains(item as Card);
    }

    void ICollection<ICard>.CopyTo(ICard[] array, int arrayIndex)
    {
        for (int i = 0; i < _handCards.Count; i++)
        {
            array[arrayIndex + i] = _handCards[i];
        }
    }

    bool ICollection<ICard>.Remove(ICard item)
    {
       return _handCards.Remove(item as Card);
    }

    IEnumerator<ICard> IEnumerable<ICard>.GetEnumerator()
    {
        return _handCards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _handCards.GetEnumerator();
    }
}
