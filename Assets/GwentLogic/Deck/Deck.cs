using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;
using System.Collections;

public class Deck: IList<ICard>
{
    public int NumberOfCards { get => _deckCards.Count; }

    private readonly List<Card> _deckCards;
    private LeaderCard _leaderCard;
    public LeaderCard Leader { get => _leaderCard; }

    int ICollection<ICard>.Count => _deckCards.Count;

    bool ICollection<ICard>.IsReadOnly => false;

    ICard IList<ICard>.this[int index] { get => _deckCards[index]; set => _deckCards[index]=value as Card; }

    public Deck(string name, List<Card> deckCards,LeaderCard leader)
    {
        _deckCards = deckCards;
        _leaderCard = leader;

    }
    public Card FetchCard()
    {
        Card fectchedCard = _deckCards[^1];
        _deckCards.RemoveAt(_deckCards.Count-1);
        return fectchedCard;
       
    }
    public void Shuffle()
    {
        System.Random random = new(); 
        for (int i = NumberOfCards-1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (_deckCards[j], _deckCards[i]) = (_deckCards[i], _deckCards[j]);
        }
    }

    int IList<ICard>.IndexOf(ICard item)
    {
        return _deckCards.IndexOf(item as  Card);
    }

    void IList<ICard>.Insert(int index, ICard item)
    {
        _deckCards.Insert(index, item as Card);
    }

    void IList<ICard>.RemoveAt(int index)
    {
        _deckCards.RemoveAt(index);
    }

    void ICollection<ICard>.Add(ICard item)
    {
        _deckCards.Add(item as Card);
    }

    void ICollection<ICard>.Clear()
    {
        _deckCards.Clear();
    }

    bool ICollection<ICard>.Contains(ICard item)
    {
        return _deckCards.Contains(item as Card);
    }

    void ICollection<ICard>.CopyTo(ICard[] array, int arrayIndex)
    {
        for (int i = 0; i < _deckCards.Count; i++)
        {
            array[arrayIndex+i]= _deckCards[i];
        }
    }

    bool ICollection<ICard>.Remove(ICard item)
    {
       return _deckCards.Remove(item as Card);
    }

    IEnumerator<ICard> IEnumerable<ICard>.GetEnumerator()
    {
        return (IEnumerator<ICard>)_deckCards;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (this as IList<ICard>).GetEnumerator();    
    }
}

