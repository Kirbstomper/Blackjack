using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Blackjack
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update

        //Game manager should have list of players and the dealer (who is technically also a player)
        //Maybe set this as a queue?
        public List<Card> Deck = new List<Card>();
        public Card prefabCard;

        public Transform dealerFaceDown;
        public List<Card> PlayerHand = new List<Card>();
        int playerChips = 500;
        int playerBet = 0;

        public List<Card> DealerHand = new List<Card>();


        public enum GameState { PLAYERTURN, BETTING, DOUBLED_DOWN, BLACKJACK, END }

        GameState CurrentState;
        public bool canDoubleDown = false; // Can the player double down this hand?



        //UI elements
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI gameStatus;
        public TextMeshProUGUI playerBetText;
        public TextMeshProUGUI playerChipsText;

        List<string> suits = new List<string> { "Tiles", "Clovers", "Pikes", "Hearts" };
        List<string> faces = new List<string> { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "King", "Queen", "Jack" };


        public int GetHandValue(List<Card> cards)
        {

            //Because we have to account for aces, this gets wierd, but we can work around that
            var value = 0;
            var aceCount = 0;
            foreach (Card c in cards)
            {
                //If a 10 or royal
                if (faces.IndexOf(c.Face) >= 9)
                {
                    value += 10;
                    continue;
                }

                //If regular 
                if (faces.IndexOf(c.Face) > 0)
                {
                    value += int.Parse(c.Face);
                }
                //If an ace
                if (c.Face == "A")
                {
                    aceCount++;
                }

            }

            //After everything is added, we then can add the aces
            while (aceCount > 0)
            {
                if (aceCount == 1 && (value + 11 <= 21))
                {
                    //If last ace and safe to add
                    value += 11;
                }
                else
                {
                    //Otherwise need to use min value of aces 
                    value += 1;
                }
                aceCount--;
            }

            return value;
        }

        public void SetupNewGame()
        {
            //If game ended mid, return chips
            if (CurrentState != GameState.END)
            {
                playerChips += playerBet;
                playerBet = 0;
            }
            //Clear player hand
            foreach (Card c in PlayerHand)
            {
                Destroy(c.gameObject);
            }
            PlayerHand.Clear();

            //Same for dealer Hand
            foreach (Card c in DealerHand)
            {
                Destroy(c.gameObject);
            }
            DealerHand.Clear();

            //Clear UI elements
            scoreText.text = "";
            gameStatus.text = "";

            //Prompt player to set initial bet
            gameStatus.text = "Please Place your bet!";
            CurrentState = GameState.BETTING;



            //Populate players list with connected players
            //Deal 2 cards to each player

            //Start with player furthest right,
        }

        public bool IsGameState(GameState state)
        {
            return (CurrentState == state);
        }

        public void ChangeBet(int amt)
        {
            if ((playerChips >= 0) && !(playerBet + amt < 0))
            {
                playerBet += amt;
                playerChips -= amt;
            }
        }
        public void PlaceBet()
        {
            if (playerBet == 0)
            {
                gameStatus.text = "Your bet must be more than 0 coins to play!!!!";
                return;
            }
            //Deal cards after bet is placed
            gameStatus.text = "";
            CurrentState = GameState.PLAYERTURN;
            //Deal 2 cards to the dealer
            DealCardDealer(false);
            DealCardDealer();

            //Deal 2 Cards to the Player
            DealCardPlayer();
            DealCardPlayer();

            //Check if the player can double down
            canDoubleDown = (GetHandValue(PlayerHand) == 9 || GetHandValue(PlayerHand) == 10 || GetHandValue(PlayerHand) == 11);
            canDoubleDown = (canDoubleDown && ((playerChips / 2) >= playerBet));

        }
        public void Stay()
        {
            if (CurrentState == GameState.DOUBLED_DOWN)
            { //IF doubled down, we need to flip card and update value
                PlayerHand[2].UpdateCardSprite();
                scoreText.text = string.Format("PlayerHand Value:{0} ", GetHandValue(PlayerHand));

            }
            CurrentState = GameState.END;
            DealerHand[0].UpdateCardSprite();


            var dealerVal = GetHandValue(DealerHand);
            var playerVal = GetHandValue(PlayerHand);

            while (dealerVal < 17)
            {
                //Dealer must draw another card
                DealCardDealer();
                dealerVal = GetHandValue(DealerHand);
            }

            if (dealerVal > 21)
            {
                gameStatus.text = "Dealer Broke! You Win!";
                playerChips += (playerBet * 2);

            }
            else
            {
                //Player wins
                if (playerVal > dealerVal)
                {
                    gameStatus.text = "WINNER!";
                    //win back double
                    playerChips += (playerBet * 2);
                }

                if (playerVal < dealerVal)
                {
                    gameStatus.text = "Loser!";
                }

                if (playerVal == dealerVal)
                {
                    gameStatus.text = "TIE!";
                    //Get Bet back
                    playerChips += playerBet;
                }
            }
            //reset player bet 
            playerBet = 0;

        }

        public void DealCardPlayer()
        {
            //Hit the player with a new card

            var x = prefabCard.transform.position.x;
            var y = prefabCard.transform.position.y;

            PlayerHand.Add(Instantiate(prefabCard, new Vector3(x + 0.5f * PlayerHand.Count, y, -1 * PlayerHand.Count), Quaternion.identity));
            PlayerHand[PlayerHand.Count - 1].Suit = suits[Random.Range(0, suits.Count)];
            PlayerHand[PlayerHand.Count - 1].Face = faces[Random.Range(0, faces.Count)];
            PlayerHand[PlayerHand.Count - 1].UpdateCardSprite();

            var score = GetHandValue(PlayerHand);
            scoreText.text = string.Format("PlayerHand Value:{0} ", score);



            if (score == 21)
            {
                gameStatus.text = "BLACKJACK :D";
                CurrentState = GameState.BLACKJACK;
            }
            if (score > 21)
            {
                gameStatus.text = "BUST";
                CurrentState = GameState.END;
                playerBet = 0;
            }

        }

        public void DealCardDoubleDown()
        {
            //Hit the player with a new card

            var x = prefabCard.transform.position.x;
            var y = prefabCard.transform.position.y;

            PlayerHand.Add(Instantiate(prefabCard, new Vector3(x + 0.5f * PlayerHand.Count, y, -1 * PlayerHand.Count), Quaternion.identity));
            PlayerHand[PlayerHand.Count - 1].Suit = suits[Random.Range(0, suits.Count)];
            PlayerHand[PlayerHand.Count - 1].Face = faces[Random.Range(0, faces.Count)];
            PlayerHand[PlayerHand.Count - 1].ShowBack();
        }
        public void DoubleDown()
        {

            //Deal card to player facedown
            DealCardDoubleDown();
            //Remove their monies
            ChangeBet(playerBet);
            //Change gamestate
            CurrentState = GameState.DOUBLED_DOWN;

        }
        void DealCardDealer()
        {
            DealCardDealer(true);
        }
        void DealCardDealer(bool visable)
        {
            var x = dealerFaceDown.position.x;
            var y = dealerFaceDown.position.y;
            DealerHand.Add(Instantiate(prefabCard, new Vector3(x + 0.5f * DealerHand.Count, y, -1 * DealerHand.Count), Quaternion.identity));
            DealerHand[DealerHand.Count - 1].Suit = suits[Random.Range(0, suits.Count)];
            DealerHand[DealerHand.Count - 1].Face = faces[Random.Range(0, faces.Count)];
            if (!visable)
                DealerHand[DealerHand.Count - 1].ShowBack();
            else
            {
                DealerHand[DealerHand.Count - 1].UpdateCardSprite();
            }
        }



        void Start()
        {
            SetupNewGame();
        }

        void Update()
        {
            playerBetText.text = string.Format("{0}", playerBet);
            playerChipsText.text = string.Format("{0}", playerChips);

        }
    }
}