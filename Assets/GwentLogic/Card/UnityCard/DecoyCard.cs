using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;
public class DecoyCard : UnityCard
{
    public DecoyCard(string name, Factions faction, string imagePath,
        List<AttackRows> attackRows,IEffect dslEffect) : base(name, faction, imagePath, 0, attackRows, dslEffect, Effects.DecoyEffect)
    {

    }
    public override bool AffectedByEffects => false;
}


