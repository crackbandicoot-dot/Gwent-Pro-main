using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Search;
using UnityEngine;
using Random = System.Random;

public class Board : IList<ICard>
{
    private AttackRows[] rows = new[]
    {
        AttackRows.M,
        AttackRows.R,
        AttackRows.S
    };
#nullable enable

    #region Properties

    #region Effects Properties
    public UnityCard? PowerfulCard
    {
        get
        {
            UnityCard? powerfulCard = default;
            for (int p = 0; p < amountOfPlayers; p++)
            {
                foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
                {
                    UnityCard? powerfulCardInCurrentRow = PowerfulCardInRow(p, row);
                    if (powerfulCardInCurrentRow != default && powerfulCardInCurrentRow.IsPowerfulThan(powerfulCard))
                        powerfulCard = powerfulCardInCurrentRow;
                }
            }
            return powerfulCard;
        }
    }
    public int NumberOfUnities {
        get
        {
            int res = 0;
            for (int p = 0; p < amountOfPlayers; p++)
            {
                res += unityCardGrids[p].Keys.Sum(row => GetAmountOfUnitiesInRow(p, row));
            }
            return res;
        }
    }
    public float AmountOfPowerPoints
    {
        get => GetPlayerScore(0) + GetPlayerScore(1);
    }
    public int MinimumNumberOfUnitiesInRow
    {
        get
        {
            int res = int.MaxValue;
            for (int p = 0; p < amountOfPlayers; p++)
            {
                foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
                {
                    int amountOfUnitiesInCurrentRow = GetAmountOfUnitiesInRow(p, row);
                    if (amountOfUnitiesInCurrentRow < res && amountOfUnitiesInCurrentRow != 0) res = amountOfUnitiesInCurrentRow;
                }
            }
            Debug.Log("Fila con menor cantidad de unidades es" + (res == int.MaxValue ? 0 : res));
            return res == int.MaxValue ? 0 : res;
        }
    }
    public float PowerAverage
    {
        get => AmountOfPowerPoints / NumberOfUnities;
    }
    private IList<ICard>[] fields = new Field[2];
    int ICollection<ICard>.Count => 39;

    bool ICollection<ICard>.IsReadOnly => false;

    ICard IList<ICard>.this[int index] {
        get
        {
            if(index<0||index>=39)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                int setcion = index / 18;
                if (setcion < 2)
                { 
                    return fields[setcion][index % fields[setcion].Count];
                }
                else
                {
                    
                    return weatherCardGrid[rows[index % 18]]!;
                }
                    
            }
        }
        set {
            if ( index<0 || index >= 39)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                int setcion = index /18;
                if (setcion < 2)
                    fields[setcion][index % fields[setcion].Count] = value;
                else
                {
                    var weather = value as WeatherCard;
                    PlaceCard(weather!, weather!.AttackRows.PickRandom());
                }
            }
        } 
    }

    #endregion

    #region Specific Board Properties
    private static int amountOfPlayers = 2;

    private static int numberOfColumns = 5;

    private Dictionary<AttackRows, UnityCard?[]>[] unityCardGrids = new Dictionary<AttackRows, UnityCard?[]>[amountOfPlayers];

    private Dictionary<AttackRows, BoostCard?>[] boostCardGrid = new Dictionary<AttackRows, BoostCard?>[amountOfPlayers];

    public Dictionary<AttackRows, WeatherCard?> weatherCardGrid = new Dictionary<AttackRows, WeatherCard?>();

   /// private LeaderCard[] leaderCards = new LeaderCard[amountOfPlayers];
    #endregion

    #endregion
    #region Methods

    #region Constructors
    public Board()
    {
        for (int playerNumber = 0; playerNumber < amountOfPlayers; playerNumber++)
        {
            unityCardGrids[playerNumber] = new Dictionary<AttackRows, UnityCard?[]>();
            boostCardGrid[playerNumber] = new Dictionary<AttackRows, BoostCard?>();
            foreach (AttackRows attackRow in Enum.GetValues(typeof(AttackRows)))
            {
                unityCardGrids[playerNumber].Add(attackRow, new UnityCard?[numberOfColumns]);
                boostCardGrid[playerNumber].Add(attackRow, default);
            }
        }
        foreach (AttackRows attackRow in Enum.GetValues(typeof(AttackRows)))
        {
            weatherCardGrid.Add(attackRow, default);
        }
        for (int i = 0; i < amountOfPlayers; i++)
        {
            //fields = new Field[2];
            fields[i] = new Field(this,unityCardGrids[i], boostCardGrid[i], i);
        }
    }
    #endregion
    public IList<ICard> FieldOfPlayer(int player)
    {
        return fields[player];
    }
    #region Effect Methods
    public void SetPowerPointsToAverage()
    {
        float Average = PowerAverage;
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    UnityCard? currentCard = unityCardGrids[playerID][row][col];
                    if (currentCard != default)
                    { currentCard.ModifyPoints(Average/currentCard.PowerPoints);}
                }
            }
        }
    }
    public void ApplyBoostEffectInRow(int playerID, AttackRows row, float increaseQuotient)
    => ModifyPointsInRow(playerID, row, increaseQuotient);
    public void ApplyWeatherInRow(AttackRows row, float decreaseQuotient)
    {
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            ModifyPointsInRow(playerID, row, decreaseQuotient);
        }
    }
    public UnityCard? WeakCardOfPlayer(int playerID)
    {
        UnityCard? weakCard = default;
        foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
        {
            UnityCard? weakCardInCurrentRow = WeakCardInRow(playerID, row);
            if (weakCardInCurrentRow != default && weakCardInCurrentRow.IsWeakThan(weakCard))
                weakCard = weakCardInCurrentRow;
        }
        return weakCard;
    }
    public int FindAllCardsEqualsTo(UnityCard inputCard)
     => unityCardGrids.Sum(kvp => kvp.Values.Sum(row => row.Count(card => card != null && card.Name == inputCard.Name)));
    public int GetAmountOfUnitiesInRow(int playerNumber, AttackRows row)
    => unityCardGrids[playerNumber][row].Count(card => card != default);
    public void ClearRowWithMinimumNumberOfUnities()
    {
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
                int NumberOfUnitiesInCurrentRow = GetAmountOfUnitiesInRow(playerID, row);
                if (NumberOfUnitiesInCurrentRow == MinimumNumberOfUnitiesInRow)
                {
                    ClearUnityRow(playerID, row, false);
                    return;
                }
            }
        }
    }
    private UnityCard? PowerfulCardInRow(int playerID, AttackRows row)
    {
        UnityCard? powerfulCard = default;
        foreach (UnityCard? currentCard in unityCardGrids[playerID][row])
        {
            if (currentCard != default && currentCard.IsPowerfulThan(powerfulCard))
                powerfulCard = currentCard;
        }
        return powerfulCard;
    }
    private UnityCard? WeakCardInRow(int playerID, AttackRows row)
    {
        UnityCard? weakCard = default;
        foreach (UnityCard? currentCard in unityCardGrids[playerID][row])
        {
            if (currentCard != default && currentCard.IsWeakThan(weakCard))
                weakCard = currentCard;
        }
        return weakCard;
    }
    #endregion

    #region Specific Board Methods

    public void ModifyPointsInRow(int playerID,AttackRows row, float modifier)
    {
        for (int col = 0; col < numberOfColumns; col++)
        {
            UnityCard? currentCard = unityCardGrids[playerID][row][col];
            if (currentCard!= default) currentCard.ModifyPoints(modifier);
        }
    }
    public float GetPlayerScore(int playerNumber) 
    => unityCardGrids[playerNumber].Keys.Sum(row => GetScoreInRow(playerNumber, row));
    public float GetScoreInRow(int playerNumber,AttackRows row)
    => unityCardGrids[playerNumber][row].Sum(unity => unity==default? 0: (float)(unity as ICard).Power);
    public void PlaceCard(UnityCard unityCard,int playerID,AttackRows row)
    {
        for (int col = 0; col < numberOfColumns; col++)
        {
            if (unityCardGrids[playerID][row][col]==default)
            {
                unityCardGrids[playerID][row][col] = unityCard;
                BoostCard? boostCard = boostCardGrid[playerID][row];
                WeatherCard? weatherCard = weatherCardGrid[row];
                if (boostCard != default) unityCard.ModifyPoints(boostCard.PowerQuotient);
                if (weatherCard != default) unityCard.ModifyPoints(weatherCard.PowerQuotient);
                return;
            }
        }
        
    }
    public void PlaceCard(WeatherCard weatherCard,AttackRows row)
    {
        weatherCardGrid[row] = weatherCard;
    }
    private void ClearUnityRow(int playerID,AttackRows row,bool removeGoldUnities=true)
    {
        if (removeGoldUnities)
        {
            unityCardGrids[playerID][row] = new UnityCard?[numberOfColumns];
        }
        else
        {
            for (int col = 0; col < numberOfColumns; col++)
            {
                UnityCard? currentCard = unityCardGrids[playerID][row][col];
                if (currentCard != null && currentCard.AffectedByEffects)
                    RemoveUnityCard(playerID, row, col);
            }
        }            
    }
    private void ClearWeatherRow(AttackRows row)
    {
        weatherCardGrid[row] = default;
    }
    private void ClearBoostRow(int playerID,AttackRows row)
    {
        boostCardGrid[playerID][row] = default;
    }
    public bool RemoveFirstOccurrenceOf(UnityCard unityCard)
    {
        for (int playerID = 0; playerID < amountOfPlayers ; playerID++)
        {
            if (RemoveFirstOccurrenceOf(playerID, unityCard)) return true;
        }
        return false;
    }
    public bool RemoveFirstOccurrenceOf(int playerID,UnityCard unityCard)
    {
        foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
        {
            for (int col = 0; col < numberOfColumns; col++)
            {
                UnityCard? currentCard = unityCardGrids[playerID][row][col];
                if (currentCard!=default && currentCard.EqualsByProperties(unityCard))
                {
                    RemoveUnityCard(playerID, row, col);
                    return true;
                }      
            }
        }
        return false;
    }
    public void PlaceCard(BoostCard card,int playerNumber,AttackRows row)
    {
        boostCardGrid[playerNumber][row]=card;
    }
    public UnityCard RemoveUnityCard(int playerNumber,AttackRows row,int col)
    {
        UnityCard res = unityCardGrids[playerNumber][row][col]!;
        unityCardGrids[playerNumber][row][col]=default;
        return res;
    }
    public void RemoveWeatherCard(AttackRows row)
    {
        WeatherCard? removedCard = weatherCardGrid[row];
        if (removedCard!=default)
        {
            for (int playerID = 0; playerID < amountOfPlayers; playerID++)
            {
                ModifyPointsInRow(playerID, row, 1 / removedCard.PowerQuotient);
            }
        }
        weatherCardGrid[row] =default;
    }
    public void RemoveBoostCard(int playerID,AttackRows row)
    {
        BoostCard? removedCard = boostCardGrid[playerID][row];
        if (removedCard!=default)
        {
            ModifyPointsInRow(playerID, row, 1 / removedCard.PowerQuotient);
        }
        boostCardGrid[playerID][row] = default;
    }
    
    public bool HasCard(Card card)
    {
        if (card is UnityCard unityCard) return HasCard(unityCard);
        else if (card is BoostCard boostCard) return HasCard(boostCard);
        else if (card is WeatherCard weatherCard) return HasCard(weatherCard);
        else if (card is LeaderCard) return true;
        else return false; 
    }
    private bool HasCard(UnityCard unityCard)
    {
        for (int playerID = 0; playerID < amountOfPlayers;playerID ++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    UnityCard? currentCard = unityCardGrids[playerID][row][col];
                    if (currentCard == unityCard) return true;
                }
            }
        }
        return false;
    }
    private bool HasCard(BoostCard boostCard)
    {
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
                if (boostCardGrid[playerID][row] == boostCard) return true;
            }
        }
        return false;
    }
    private bool HasCard(WeatherCard weatherCard)
    {
        foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
        {
            if (weatherCardGrid[row]==weatherCard) return true;
        }
        return false;
    }
    private void ClearUnityGrid()
    {
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
              ClearUnityRow(playerID, row);
            }
        }  
    }
    private void ClearWeatherGrid()
    {
        foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
        {
           ClearWeatherRow(row);
        }
    }
    private void ClearBoostGrid()
    {
        for (int playerID = 0; playerID < amountOfPlayers; playerID++)
        {
            foreach (AttackRows row in Enum.GetValues(typeof(AttackRows)))
            {
                ClearBoostRow(playerID, row);
            }
        }
    }
    public void Clear()
    {
        ClearUnityGrid();
        ClearWeatherGrid();
        ClearBoostGrid();
    }

    int IList<ICard>.IndexOf(ICard item)
    {
        for (int i = 0; i < (this as IList<ICard>).Count; i++)
        {
            if ((this as IList<ICard>)[i]==item)
            {
                return i;
            }
        }
        return -1;
    }

    void IList<ICard>.Insert(int index, ICard item)
    {
        (this as IList<ICard>)[index] = item;
    }

    void IList<ICard>.RemoveAt(int index)
    {
        (this as IList<ICard>)[index] = null;
    }

    void ICollection<ICard>.Add(ICard item)
    {
        if (item is WeatherCard weather)
        {
            PlaceCard(weather, weather.AttackRows.PickRandom());
        }
        else if(item is BoostCard or UnityCard)
        {
            fields[new[] { 0, 1 }.PickRandom()].Add(item);
        }
        else
        {
            throw new Exception("That type of card is not playable in this board");
        }
    }

    void ICollection<ICard>.Clear()
    {
        for (int i = 0; i < amountOfPlayers; i++)
        {
            fields[i].Clear();
        }
        foreach (var key in weatherCardGrid.Keys)
        {
            weatherCardGrid[key] = null;
        }
    }

    bool ICollection<ICard>.Contains(ICard item)
    {
        return (this as IList<ICard>).IndexOf(item) != -1;
    }

    void ICollection<ICard>.CopyTo(ICard[] array, int arrayIndex)
    {
        for (int i = 0; i < (this as IList<ICard>).Count ; i++)
        {
            array[i + arrayIndex] = (this as IList<ICard>)[i];
        }
    }

    bool ICollection<ICard>.Remove(ICard item)
    {
        bool removed = fields[0].Remove(item) || fields[1].Remove(item);
        if (removed) {
            Debug.Log("Removed is false");
            return true; }
        else
        {
            foreach (var r in weatherCardGrid.Keys)
            {
                if (weatherCardGrid[r] == item)
                {
                    weatherCardGrid[r] = null;
                    return true;
                }
            }
            Debug.Log("Removes is false");
            return false;
        } 
    }

    IEnumerator<ICard> IEnumerable<ICard>.GetEnumerator()
    { 
        for (int i = 0; i < (this as IList<ICard>).Count; i++)
        {
            var current = (this as IList<ICard>)[i];
            if (current is not null)
            {
                yield return (this as IList<ICard>)[i];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (this as IList<ICard>).GetEnumerator();
    }
    #endregion

    #endregion
}