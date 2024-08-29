using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSL;
using DSL.Interfaces;
using System.Linq;
using System;

public abstract class Card : ICard
{
    private readonly List<AttackRows> range;
    private readonly IEffect dslEffect;

    public string Name { get;}
    public Factions Faction {get;}
    public string ImagePath {get;}
    protected abstract double Power { get; set; }
    public Effects Effect { get;}

    string ICard.Name => Name;

    string ICard.Faction => Faction.ToString();

    string ICard.Type { get
        {
            var typeString = GetType().ToString();
            return typeString switch
            {
                "GoldUnityCard" => "Gold",
                "SilverUnityCard" => "Silver",
                "LeaderCard" => "Leader",
                "WeatherCard" => "Weather",
                "ClearingCard" => "Clearing",
                "DecoyCard" => "Decoy",
                "BoostCard" => "Boost",
                _ => throw new Exception($"Type {typeString} type is not valid for the compiler")
            } ;
        } 
    }

    IList<string> ICard.Range => range.Select(x => x==AttackRows.M?"Melee": x == AttackRows.R ? "Ranged": "Siesge").ToList();

    double ICard.Power { get => Power; set => Power=value; }

    IEffect ICard.Effect => dslEffect;

    public Card(string name, Factions faction, string imagePath, List<AttackRows> range ,
         IEffect dslEffect,Effects effect=Effects.None)
    {
        Name = name;
        Faction = faction;
        ImagePath = imagePath;
        this.range = range;
        this.dslEffect = dslEffect;
        Effect = effect;
    }
    
    public virtual bool EqualsByProperties(Card otherCard)
        => this.Name == otherCard.Name && this.Faction==otherCard.Faction && this.ImagePath==ImagePath && this.Effect==Effect;
    
}