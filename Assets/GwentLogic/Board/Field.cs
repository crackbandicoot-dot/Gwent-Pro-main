
using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Field : IList<ICard>
    {
        private readonly AttackRows[] rows= new[]{AttackRows.M,
        AttackRows.R,AttackRows.S};
    private readonly Board board;
    private readonly Dictionary<AttackRows, UnityCard[]> unitiesGrid;
    private readonly Dictionary<AttackRows, BoostCard> boostGrid;
    private readonly int playerID;

    public Field(Board board, Dictionary<AttackRows, UnityCard?[]> unitiesGrid, Dictionary<AttackRows, BoostCard?> boostGrid,int playerID)
        {
        this.board = board;
        this.unitiesGrid = unitiesGrid;
        this.boostGrid = boostGrid;
        this.playerID = playerID;
    }

        public ICard this[int index] {
        get
        {
            if (index < 0 || index >= 18)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                var row = rows[index /6];
                var col = index % 6;
                if (col==0)
                {
                    return boostGrid[row];
                }
                else
                {
                    return unitiesGrid[row][col-1];
                }
            }
        }
        set
        {
            
            if (index < 0 || index >= 18)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                var row = rows[index / 6];
                var col = index % 6;
                if (value is null)
                {
                    if (col == 0)
                    {
                        boostGrid[row]=null;
                    }
                    else
                    {
                        unitiesGrid[row][col-1] = null;
                    }
                }
                else
                {
                    if (col == 0)
                    {
                        board.PlaceCard(value as BoostCard, playerID, row);
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"Poniendo {value} en la fila {row}");
                        board.PlaceCard(value as UnityCard, playerID, row);
                    }
                }
            }
        }
    }

        public int Count => 18;

        public bool IsReadOnly => false;

        public void Add(ICard item)
        {
            if (item is BoostCard boostCard)
            { 
                board.PlaceCard(boostCard,playerID,boostCard.AttackRows.PickRandom());
            }
            else if(item is UnityCard card)
            {
                board.PlaceCard(card,playerID,card.AttackRows.PickRandom());
            }
            else
            {
                throw new Exception("Only units and boost cards are allowed");
            }
    }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = default;
            }
        }

        public bool Contains(ICard item)
        {
        return (this as IEnumerable<ICard>).Contains(item);
        }

        public void CopyTo(ICard[] array, int arrayIndex)
        {
            int i = 0;
            foreach (var item in array)
            {
                array[i+arrayIndex] = item;
            }
        }

        public IEnumerator<ICard> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
              if (this[i]!=null)
              {
                yield return this[i];
              }
            }
        }

        public int IndexOf(ICard item)
        {
            for (int i = 0; i < Count; i++)
            {
            if (item as Card== this[i])
                    return i;
            }
            return -1;
        }

        public void Insert(int index, ICard item)
        {
            this[index] = item;
        }

        public bool Remove(ICard item)
        {
           for (int i = 0; i < Count; i++)
           {
                if (this[i] == item)
                {
                    this[i] = null;
                    return true;
                }
           }
           return false;
        }

        public void RemoveAt(int index)
        {
             this[index]=null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

