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
        public const int PLAYER_STARTING_CHIPS = 500;
        //Game manager should have list of players and the dealer (who is technically also a player)
        //Maybe set this as a queue?
        public List<Card> Deck = new List<Card>();
        public Card prefabCard;

        public Transform dealerFaceDown;
        public List<PlayerHand> PlayerHands = new List<PlayerHand>();
        int playerChips;

        public int playerBet;
        int playerSideBet = 0;

        public List<Card> DealerHand = new List<Card>();

        GameState CurrentState;
        
        public PlayerHand currentHand;
        
    

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
            //If game ended mid, return half chips from all hands
            if (CurrentState != GameState.END)
            {
                foreach (PlayerHand hand in PlayerHands)
                {
                    playerChips += hand.bet / 2;
                }

            }

            //Destroy all cards on the field
            foreach (PlayerHand hand in PlayerHands) //Players cards
            {
                foreach (Card c in hand.cards)
                {
                    Destroy(c.gameObject);
                }
            }

            foreach (Card c in DealerHand)//Dealers cards
            {
                Destroy(c.gameObject);
            }


            PlayerHands.Clear();// Clear out the current hands
            PlayerHands.Add(new PlayerHand());//Create the first player hand
            currentHand = PlayerHands[0];
            DealerHand.Clear(); // Clear the dealers hand

            //Clear UI elements
            scoreText.text = "";
            gameStatus.text = "";

            //Prompt player to set initial bet
            gameStatus.text = "Please Place your bet!";
            CurrentState = GameState.BETTING;

        }

        public bool IsGameState(GameState state)
        {
            return (CurrentState == state);
        }

        public void ChangeBet(int amt)
        {
            if ((playerChips >= 0) && (playerBet + amt >= 0))
            {
                playerBet += amt;
                playerChips -= amt;
            }
        }
        public void PlaceBet() //Places bet and gets the player hand setup
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



            //Deal 2 Cards to the players current hand
            DealCardPlayer();
            DealCardPlayer();

            //Check if the player can double down
            currentHand.canDoubleDown = (GetHandValue(currentHand.cards) == 9 || GetHandValue(currentHand.cards) == 10 || GetHandValue(currentHand.cards) == 11);
            currentHand.canDoubleDown = (currentHand.canDoubleDown && ((playerChips / 2) >= playerBet));

            // If the dealer shown card is an ace, we can offer a side bet
            if (DealerHand[1].Face == "A")
            {
                gameStatus.text = "Side Betting Open!";
                CurrentState = GameState.SIDEBETTING;
            }

        }

        public void PlaceSideBet()
        {
            //IF a side bet is placed we should go ahead see if it pays out
            if (GetHandValue(DealerHand) == 21 && playerSideBet > 0)
            {
                gameStatus.text = "Side bet paid! Dealer has a blackjack";
                playerChips += (playerSideBet * 2);
            }
            CurrentState = GameState.PLAYERTURN;
        }

        public void ChangeSideBet(int amt)
        {
            print(string.Format("Current sidebet {0}", playerSideBet));
            if ((playerChips > 0) && (playerSideBet + amt <= (playerBet / 2)) && (playerSideBet + amt >= 0))
            {
                playerSideBet += amt;
                playerChips -= amt;
            }
        }
        public void Stay()
        {
            currentHand.handState = HandState.STAY;
            NextHand();
        }



        /***
            Whenever an action is taken, we need to move the current hand position to the next availible hand that can make a move, if no hands are open,
            we should initiate ending the game
        ***/
        public void NextHand()
        {

            var shouldEnd = true;
            foreach (PlayerHand hand in PlayerHands) //Determine if game sh ould end by checking all hand status
            {
                if (hand.handState == HandState.OPEN)
                    shouldEnd = false;

            }

            if (shouldEnd) //If there are no open hands end game
            {
                EndGame();
            }
            else
            {
                foreach (PlayerHand hand in PlayerHands) //Determine if game sh ould end by checking all hand status
                {
                    if (hand.handState == HandState.OPEN)
                    {
                        currentHand = hand;
                        return;
                    }
                }
            }

        }


        //It makes sense to me for all operations to effect the PlayerHand, not the overall game.
        //This means at the end the game has to be settled for each hand once they are all stay
        //Or busted, or blackjacked. 
        public void EndGame()
        {


            CurrentState = GameState.END;
            DealerHand[0].UpdateCardSprite(); //Flip the card for the dealer


            var dealerVal = GetHandValue(DealerHand);

            while (dealerVal < 17)  //Dealer must be at or above 17
            {
                //Dealer must draw another card
                DealCardDealer();
                dealerVal = GetHandValue(DealerHand);
            }

            //For each player hand we need to figure out where it lies

            foreach (PlayerHand hand in PlayerHands)
            {
                if (hand.handState == HandState.DOUBLED_DOWN)//IF doubled down, we need to flip card and update value 
                {
                    hand.cards[2].UpdateCardSprite();
                    scoreText.text = string.Format("PlayerHand Value:{0} ", GetHandValue(hand.cards));
                }
                

                if(hand.handState == HandState.BUST)//Handle a bust
                {
                    gameStatus.text = "You Broke! You Lost!";
 
                    continue;
                }
                var playerVal = GetHandValue(hand.cards); //Get the value of the hand
                
        
                if (dealerVal > 21)
                {
                    gameStatus.text = "Dealer Broke! You Win!";
                    playerChips += (hand.bet * 2);

                }

                else
                {
                    //Player wins
                    if (playerVal > dealerVal)
                    {
                        gameStatus.text = "WINNER!";
                        //win back double
                        playerChips += (hand.bet * 2);
                    }

                    if (playerVal < dealerVal)
                    {
                        gameStatus.text = "Loser!";
                    }

                    if (playerVal == dealerVal)
                    {
                        gameStatus.text = "TIE!";
                        //Get Bet back
                        playerChips += hand.bet;
                    }
                }
            }
        }


        public void Hit()
        {
            DealCardPlayer();
            NextHand();
        }
        public void DealCardPlayer()
        {
            //Hit the player with a new card

            var x = prefabCard.transform.position.x;
            var y = prefabCard.transform.position.y;

            currentHand.cards.Add(Instantiate(prefabCard, new Vector3(x + 0.5f * currentHand.cards.Count, y, -1 * currentHand.cards.Count), Quaternion.identity));
            currentHand.cards[currentHand.cards.Count - 1].Suit = suits[Random.Range(0, suits.Count)];
            currentHand.cards[currentHand.cards.Count - 1].Face = faces[Random.Range(0, faces.Count)];
            currentHand.cards[currentHand.cards.Count - 1].UpdateCardSprite();

            var score = GetHandValue(currentHand.cards);
            
            if (score == 21)
            {
                gameStatus.text = "BLACKJACK :D";
                PlayerHands[PlayerHands.IndexOf(currentHand)].handState = HandState.BLACKJACK;
            }
            if (score > 21)
            {
                gameStatus.text = "BUST";
                PlayerHands[PlayerHands.IndexOf(currentHand)].handState = HandState.BUST;
            }

        }
        public void DealCardDoubleDown()
        {
            //Hit the player with a new card

            var x = prefabCard.transform.position.x;
            var y = prefabCard.transform.position.y;

            currentHand.cards.Add(Instantiate(prefabCard, new Vector3(x + 0.5f * currentHand.cards.Count, y, -1 * currentHand.cards.Count), Quaternion.identity));
            currentHand.cards[currentHand.cards.Count - 1].Suit = suits[Random.Range(0, suits.Count)];
            currentHand.cards[currentHand.cards.Count - 1].Face = faces[Random.Range(0, faces.Count)];
            currentHand.cards[currentHand.cards.Count - 1].ShowBack();
        }
        public void DoubleDown()// TODO: COME BACK AND FIGURE THIS SHIT OUT
        {

            DealCardDoubleDown();//Deal card to player facedown


            ChangeBet(playerBet);//Remove their monies
            //Change gamestate
            currentHand.handState = HandState.DOUBLED_DOWN;

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
            playerChips = PLAYER_STARTING_CHIPS;
            SetupNewGame();
        }

        void Update()
        {
            scoreText.text = string.Format("PlayerHand Value:{0} ", GetHandValue(currentHand.cards));
            print(CurrentState);
            if (IsGameState(GameState.SIDEBETTING))
            {
                playerBetText.text = string.Format("{0}", playerSideBet);
            }
            else
            {
                playerBetText.text = string.Format("{0}", playerBet);
            }

            playerChipsText.text = string.Format("{0}", playerChips);

        }
    }
}