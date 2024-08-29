using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSL.Interfaces;
public class WeatherCard : RowModifierCard
{
    public WeatherCard(string name, Factions faction, string imagePath, List<AttackRows> attackRows, float penalizationQuotient,IEffect dslEffect) :
    base(name, faction, imagePath, attackRows, penalizationQuotient,dslEffect, Effects.WeatherEffect)
    {

    }
}

