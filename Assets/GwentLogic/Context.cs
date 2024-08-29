using DSL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GwentLogic
{
    internal class Context : IContext
    {
        public int TriggerPlayer => 0;

        public IList<ICard> Board => throw new NotImplementedException();

        public IList<ICard> DeckOfPlayer(int player)
        {
            throw new NotImplementedException();
        }

        public IList<ICard> FieldOfPlayer(int player)
        {
            throw new NotImplementedException();
        }

        public IList<ICard> GraveYardOfPlayer(int player)
        {
            throw new NotImplementedException();
        }

        public IList<ICard> HandOfPlayer(int player)
        {
            return new List<ICard>() { new LeaderCard("Dipper", Factions.Goods, "", new VoidEffect(), Effects.FetchOneCard) as ICard };
        }
    }
}
