using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RowModifierCard : Card
{
    public List<AttackRows> AttackRows { get; } = new List<AttackRows>();
    public float PowerQuotient { get; }
    protected override double Power { get => 0; set => throw new NotImplementedException(); }

    public RowModifierCard(string name, Factions faction, string imagePath, List<AttackRows> attackRows, float powerQuotient,IEffect dslEffect, Effects effect) : base(name, faction, imagePath, attackRows,dslEffect, effect)
    {
        PowerQuotient = powerQuotient;
        AttackRows = attackRows;
    }
}

