using DSL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class ClearingCard : SpecialCard
{
    public ClearingCard(string name, Factions faction, string imagePath, List<AttackRows> attackRows,IEffect dslEffect) : base(name, faction, imagePath, attackRows,dslEffect, Effects.ClearWeather)
    {
    }
}
