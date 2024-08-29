using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public static class GameEffects
{
    #nullable enable
    #region Unities and Special Cards Effects
    public static void RemovePowerfulCard(Board board)
    {
      if (board.PowerfulCard != null && board.PowerfulCard.AffectedByEffects) 
         board.RemoveFirstOccurrenceOf(board.PowerfulCard);
    }
    public static void RemoveWeakCard(Player player, Board board)
    {
        int OponentID = Player.GetOpponentID(player.PlayerID);
        UnityCard? rivalWeakCard = board.WeakCardOfPlayer(OponentID);
        if (rivalWeakCard != null && rivalWeakCard.AffectedByEffects)
            board.RemoveFirstOccurrenceOf(OponentID, rivalWeakCard);
    }
    public static void ClearRowWithMinimumNumberOfUnities(Board board)
    => board.ClearRowWithMinimumNumberOfUnities();
    public static void MultiplyByN(UnityCard unityCard, Board board) => unityCard.ModifyPoints(board.FindAllCardsEqualsTo(unityCard));
    public static void SetPointsToAverage(Board board)
    => board.SetPowerPointsToAverage();
    public static void FetchOneCard(Player player) => player.Fetch(1);
    public static void WeatherEffect(Board board, AttackRows row, float decreaseQuotient)
    => board.ApplyWeatherInRow(row, decreaseQuotient);
    public static void BoostEffect(Board board, int playerID, AttackRows row, float increaseQuotient)
    => board.ApplyBoostEffectInRow(playerID, row, increaseQuotient);
    public static void DecoyEffect(Player player,AttackRows row, int col)
    {
        player.AddToHand(player._board.RemoveUnityCard(player.PlayerID, row, col));
    }
    public static void  ClearWeatherEffect(Board board,AttackRows row)
    {
        board.RemoveWeatherCard(row);
    }
    public static void WinInTieCase(Player player)
    {
        player.AllowWinInTieCase();
    }
    #endregion

    
}

