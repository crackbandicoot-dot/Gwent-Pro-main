using DSL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IContext
{
      
     #nullable enable
    // Start is called before the first frame update
    public Board board;
    public Player Player1;
    public Player Player2;

    public TextMeshProUGUI Player1Name;
    public TextMeshProUGUI Player2Name;
    public Image Player1Victory1;
    public Image Player1Victory2;
    public Image Player2Victory1;
    public Image Player2Victory2;

    public TextMeshProUGUI M1;
    public TextMeshProUGUI M2;
    public TextMeshProUGUI S1;
    public TextMeshProUGUI S2;
    public TextMeshProUGUI R1;
    public TextMeshProUGUI R2;

    public TextMeshProUGUI TotalScorePlayer1;
    public TextMeshProUGUI TotalScorePlayer2;

    public Button Player1PassButton;
    public Button Player2PassButton;
    public Button Player1AbilityButton;
    public Button Player2AbilityButton;

    public TextMeshProUGUI Message;

    public GameObject cardPrefab;
    public GameObject GameOverScreen;
    private readonly Player[] players = new Player[2];
    private readonly Button[] passButtons = new Button[2];
    private readonly Button[] abilityButtons = new Button[2];

    private int RoundNumber = 1;
    private Player? pastRoundWinnner;
    private bool CurrentRoundOver { get => Player1.Passed && Player2.Passed; }
    private bool GameOver { get => RoundNumber == 4 || Player1.NumberOfVictories == 2 || Player2.NumberOfVictories == 2; }

    int IContext.TriggerPlayer => players[0].IsPlaying ? 0 : 1;

    IList<ICard> IContext.Board => board;

    private void Awake()
    {
       
       
        CardContainer cardContainer = new CardContainer();
        Deck deck1 = new(GameData.Player1Faction, cardContainer.goodsDecks[0], cardContainer.goodsLeaders[0]); ;
        Deck deck2 = new(GameData.Player2Faction, cardContainer.goodsDecks[1], cardContainer.goodsLeaders[1]); ; 
       // Deck deck1 = new Deck(GameData.Player1Faction, d1.Select(x => (Card)(x)).ToList(), (LeaderCard)d1.First(x => x.Type == "Leader"));
       // Deck deck2 = new Deck(GameData.Player2Faction, d1.Select(x => (Card)(x)).ToList(), (LeaderCard)d1.First(x => x.Type == "Leader"));

        deck1.Shuffle();
        deck2.Shuffle();
        board = new Board();
        Player1 = new Player(deck1, board, GameData.Player1Name);
        Player2 = new Player(deck2, board, GameData.Player2Name);
        players[0] = Player1;
        players[1] = Player2;
        passButtons[0] = Player1PassButton;
        passButtons[1] = Player2PassButton;
        abilityButtons[0] = Player1AbilityButton;
        abilityButtons[1] = Player2AbilityButton;
        Player startPlayer = WhoStarts();
        startPlayer.BeginTurn();
        ShowMessage(startPlayer.PlayerName + " Turn");
        EnableButton(passButtons[startPlayer.PlayerID]);
        EnableButton(abilityButtons[startPlayer.PlayerID]);
    }
    void Start()
    {
        Time.timeScale = 1; // Ensure the game starts with normal time scale
        SetPlayerNames();
        DisableVictoriesImages();
        UpdatePoints();
    }

    public void ExecuteTurnAsync( TurnActions action,int playerID=0,AttackRows row= AttackRows.M, Card? card = default)
    {
        switch (action)
        {
            case TurnActions.PlayCard:
                PlayCard(playerID, row, card!);
                break;
            case TurnActions.Pass:
                Pass(playerID);
                break;
            case TurnActions.UseAbility:
                if (players[playerID].LeaderEffectUsed) { ShowMessage("Ability already used in this round"); return; }
                UseLeaderAbility(playerID);
                break;
        }
        UpdateFrontend();
        if (!players[Player.GetOpponentID(playerID)].Passed)
        {
            ChangeTurn(players[playerID], players[Player.GetOpponentID(playerID)]);
            UpdateFrontend();
        }
        if (CurrentRoundOver)
        {
            players[playerID].EndTurn();
            ChangeRound();
            UpdateFrontend();
        }
       /* if (gameOver)
        {
            //await Task.Delay(1000);
            UpdateFrontend();
            EndGame();
        } */

        
    }
    #region AuxiliarMethods
    private void UseLeaderAbility(int playerID)
    {
        players[playerID].UseLeaderAbility(this);
    }
    private void Pass(int playerID)
    {
        players[playerID].Pass();
        ShowMessage(players[playerID] + "Passed");
    }
    private void PlayCard(int playerID,AttackRows row,Card card)
    {
        players[playerID].PlayCard(card,row,this);
    }
    private void BeginRound()
    {
        if (GameOver)
        {
            EndGame();
        }
        else
        {
            foreach (var player in players)
            {
                player.BeginRound();
            }
            Player firstPlayerInTurn = WhoStarts();
            firstPlayerInTurn.BeginTurn();
            EnableButton(passButtons[firstPlayerInTurn.PlayerID]);
            EnableButton(abilityButtons[firstPlayerInTurn.PlayerID]);
            ShowMessage(firstPlayerInTurn.PlayerName + " Turn");
            UpdateFrontend();
        }
    }
    private void EnableButton(Button button)
    {
        button.interactable = true;
    }
    private void DisableButton(Button button)
    {
        button.interactable = false;
    }
    private Player WhoStarts()
    {
        if (pastRoundWinnner != null) return pastRoundWinnner;
        else
        {
            System.Random random = new();
            return players[random.Next(players.Length)];
        }
    }
    private void EndRound()
    {   
        pastRoundWinnner = FindRoundWinner();
        pastRoundWinnner?.HandleVictory();
        board.Clear();
        foreach (var player in players)
        {
            DisableButton(passButtons[player.PlayerID]);
            DisableButton(abilityButtons[player.PlayerID]);
        }
        UpdateFrontend();
    }
    private Player? FindRoundWinner()
    {
        float player1Score = board.GetPlayerScore(0);
        float player2Score = board.GetPlayerScore(1);
        if (player1Score < player2Score) return Player2;
        else if (player1Score > player2Score) return Player1;
        else if (Player1.WinsInTieCase && !Player2.WinsInTieCase) return Player1;
        else if (!Player1.WinsInTieCase && Player2.WinsInTieCase) return Player2;
        return null;
    }
    private Player? FindGameWinner()
    {
        
        if (Player1.NumberOfVictories < Player2.NumberOfVictories) return Player2;
        else if (Player1.NumberOfVictories > Player2.NumberOfVictories) return Player1;
        return null;
    }
    private void ChangeTurn(Player ActivePlayer,Player RivalPlayer)
    {
        ActivePlayer.EndTurn();
        DisableButton(passButtons[ActivePlayer.PlayerID]);
        DisableButton(abilityButtons[ActivePlayer.PlayerID]);
        RivalPlayer.BeginTurn();
        EnableButton(passButtons[RivalPlayer.PlayerID]);
        EnableButton(abilityButtons[RivalPlayer.PlayerID]);
        ShowMessage(RivalPlayer.PlayerName + " Turn");
    }
    private void ChangeRound()
    {
        EndRound();
        RoundNumber++;
        BeginRound();
    }
    private void UpdateFrontend()
    {
        UpdateBoardState();
        UpdateHandsState();
        UpdatePoints();
        AddVictoryImage();
    }
    private void SetPlayerNames()
    {
        Player1Name.text = Player1.PlayerName;
        Player2Name.text = Player2.PlayerName;
    }
    private void DisableVictoriesImages()
    {
        Player1Victory1.gameObject.SetActive(false);
        Player1Victory2.gameObject.SetActive(false);
        Player2Victory1.gameObject.SetActive(false);
        Player2Victory2.gameObject.SetActive(false);
    }
    private void UpdatePoints()
    {
        UpdateCardPoints();
        UpdateRowPoints();
        UpdatePlayerPoints();
    }
    private void UpdatePlayerPoints()
    {
        TotalScorePlayer1.text = ((int)Player1.PowerPoints).ToString();
        TotalScorePlayer2.text = ((int)Player2.PowerPoints) .ToString();
    }
    private void UpdateRowPoints()
    {
        M1.text = ((int)board.GetScoreInRow(0, AttackRows.M)).ToString();
        M2.text = ((int)board.GetScoreInRow(1, AttackRows.M)).ToString();
        R1.text = ((int)board.GetScoreInRow(0, AttackRows.R)).ToString();
        R2.text = ((int)board.GetScoreInRow(1, AttackRows.R)).ToString();
        S1.text = ((int)board.GetScoreInRow(0, AttackRows.S)).ToString();
        S2.text = ((int)board.GetScoreInRow(1, AttackRows.S)).ToString();
    }
    private void UpdateCardPoints()
    {
        
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            card.GetComponent<CardDisplay>().UpdateCard(card.GetComponent<CardDisplay>().card);
        }
    }
    private void UpdateBoardState()
    {
        var frontendCards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject frontendCard in frontendCards)
        {
            if (!frontendCard.transform.parent.CompareTag("Hand"))
            {
                Card backendCard = frontendCard.GetComponent<CardDisplay>().card;
                if (!board.HasCard(backendCard)) Destroy(frontendCard); 
            }
        }
        var rows = GameObject.FindGameObjectsWithTag("Row");
        var slots = GameObject.FindGameObjectsWithTag("Slot");
        
        foreach (var card in board)
        {
            bool isInFrontend = frontendCards.Where(c => !c.transform.parent.CompareTag("Hand")).Any(c => c.GetComponent<CardDisplay>().card == card);
            if (card is UnityCard unit && !isInFrontend)
            {
                var row = rows
                    .First(r => r.GetComponent<Drop>().row == unit.AttackRows[0]
                    && r.GetComponent<Drop>().playerID==(this as IContext).TriggerPlayer);
                var c =Instantiate(cardPrefab, row.transform);
                c.GetComponent<CardDisplay>().card = card as Card;
            }
            else if(card is BoostCard boostCard && !isInFrontend)
            {
                var slot = slots
                    .First(s => s.GetComponent<Drop>().row == boostCard.AttackRows[0]
                    && s.GetComponent<Drop>().playerID == (this as IContext).TriggerPlayer);
               var c = Instantiate(cardPrefab, slot.transform);
            }
            
        }
    }
    private void UpdateHandsState()
    {
        var frontendHands = GameObject.FindGameObjectsWithTag("Hand");
        foreach (var frontendHand in frontendHands)
        {
            frontendHand.GetComponent<HandScript>().UpdateHand();
        }
        
    }
    private void EndGame()
    {
        Player? winner = FindGameWinner();
        GameOverScreen.GetComponent<GameOverScreen>().GameOver(winner==null?"":winner.PlayerName);
    }
    private void AddVictoryImage()
    {
        if (Player1.NumberOfVictories > 0)
            Player1Victory1.gameObject.SetActive(true);

        if (Player1.NumberOfVictories == 2)
            Player1Victory2.gameObject.SetActive(true);

        if (Player2.NumberOfVictories > 0)
            Player2Victory1.gameObject.SetActive(true);

        if (Player2.NumberOfVictories == 2)
            Player2Victory2.gameObject.SetActive(true);
    }    
    private async void ShowMessage(string message,int wait =0)
    {
       
        await Task.Delay(wait * 1000);

        Message.text = message;
        
    }

    IList<ICard> IContext.FieldOfPlayer(int player)
    {
        return board.FieldOfPlayer(player);
    }

    IList<ICard> IContext.GraveYardOfPlayer(int player)
    {
        throw new NotImplementedException();
    }

    IList<ICard> IContext.HandOfPlayer(int player)
    {
        return players[player].Hand;
    }

    IList<ICard> IContext.DeckOfPlayer(int player)
    {
        return players[player].Deck;
    }
    #endregion
}
