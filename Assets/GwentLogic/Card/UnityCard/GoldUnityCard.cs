using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;


public class GoldUnityCard : UnityCard
{

    public override bool AffectedByEffects { get => false; }
    public GoldUnityCard(string name, Factions faction, string imagePath, int powerPoints, List<AttackRows> attackRows,IEffect dslEffect, Effects effect) : base(name, faction, imagePath, powerPoints, attackRows, dslEffect, effect)
    {

    }   
}
