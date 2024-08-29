using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;
public abstract class SpecialCard : Card
{
    public List<AttackRows> AttackRows { get; } = new List<AttackRows>();
    protected override double Power { get => 0; set => throw new Exception(); }

    public SpecialCard(string name, Factions faction, string imagePath,List<AttackRows> attackRows,IEffect dslEffect,Effects effect) : base(name,faction,imagePath,attackRows,dslEffect,effect)
    {
       AttackRows = attackRows;
    }
}

