using DSL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GwentLogic.CardFactory
{
    internal class CardFactory : ICardFactory
    {
        public ICard CreateCard(string name, string faction, string type, IList<string> range, double power, IEffect effect)
        {
            var rangeRows = range.Select(x => x == "Melee" ? AttackRows.M : x == "Siesge" ? AttackRows.S : AttackRows.R).ToList();
            var factionEnum = faction == "Goods" ? Factions.Goods : Factions.Bads;
            return type switch
            {
                "Gold" => new GoldUnityCard(name, factionEnum, "", (int)power, rangeRows, effect, Effects.None),
                "Silver" => new SilverUnityCard(name, factionEnum, "", (int)power, rangeRows, effect, Effects.None),
                "Decoy" => new DecoyCard(name, factionEnum, "", rangeRows, effect),
                "Boost" => new BoostCard(name, factionEnum, "", rangeRows, 2, effect),
                "Weather" => new WeatherCard(name, factionEnum, "", rangeRows, 0.5f, effect),
                "Clearing" => new ClearingCard(name, factionEnum, "", rangeRows, effect),
                "Leader" => new LeaderCard(name, factionEnum, "", effect, Effects.None),
                _ => throw new Exception($"Type {type} is not a valid game card type"),
            };
        }
    }
}
