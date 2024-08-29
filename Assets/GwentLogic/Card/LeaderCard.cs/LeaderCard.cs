using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;

public class LeaderCard : Card
{
    public LeaderCard(string name, Factions faction, string imagePath,IEffect dslEffect,Effects effects) : base(name, faction, imagePath,new(),dslEffect,effects)
    {
    }

    protected override double Power { get => 0; set => throw new Exception(); }
}

