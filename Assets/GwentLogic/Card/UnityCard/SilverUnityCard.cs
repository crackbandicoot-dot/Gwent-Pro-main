using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;
public class SilverUnityCard : UnityCard
{
    public override bool AffectedByEffects { get => true; }
    public SilverUnityCard(string name, Factions faction, string imagePath, int powerPoints, List<AttackRows> attackRows,IEffect dslEffect,Effects effect) : base(name, faction, imagePath, powerPoints, attackRows,dslEffect,effect)
    {
    } 
}


