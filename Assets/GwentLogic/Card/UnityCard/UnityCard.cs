using DSL.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityCard : Card
{
    public abstract bool AffectedByEffects { get; }

    private float _powerPoints;

    private float _quotient = 1 ;
    
    public List<AttackRows> AttackRows { get; } = new List<AttackRows>();
    public float PowerPoints { get => _powerPoints * _quotient; }
    public float PowerQuotient { get => _quotient; }
    protected override double Power { get => PowerPoints; set => _powerPoints=(float)value; }

    public UnityCard(string name, Factions faction, string imagePath, float powerPoints, List<AttackRows> attackRows,IEffect dslEffect, Effects effect) 
        : base(name, faction, imagePath,attackRows,dslEffect,effect)
    {
        _powerPoints = powerPoints;
        AttackRows = attackRows;
    }
    public void ModifyPoints(float modifier)
    {
        if (AffectedByEffects && modifier>0)
        {
            _quotient *=modifier;
        }    
    }
   
    public bool IsPowerfulThan(UnityCard otherUnity)
    {
        if (otherUnity == default) return true;
        else return this.PowerPoints > otherUnity.PowerPoints;
    }
    public bool IsWeakThan(UnityCard otherUnity)
    {
        if (otherUnity == default) return true;
        else return this.PowerPoints < otherUnity.PowerPoints;
    }
    public bool EqualsByProperties(UnityCard otherCard)
    {
        return base.EqualsByProperties(otherCard) && this.PowerPoints==otherCard.PowerPoints;
    }

}

