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

        public List<Card> DealerHand = new List<Card>();



        //UI elements
        public TextMeshProUGUI scoreText;

        public TextMeshProUGUI gameStatus;

        public Button stayButton;
        public Button hitButton;
        List<string> suits = new List<string> { "Tiles", "Clovers", "Pikes", "Hearts" };
        List<string> faces = new List<string> { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "King", "Queen", "Jack" };

        /**
            Deals a card to the current player, then moves turn to next
        **/
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
                hitButton.interactable = false;
            }
            if (score > 21)
            {
                gameStatus.text = "BUST";
                stayButton.interactable = false;
                hitButton.interactable = false;

            }

        }

        /***
            Stop getting new cards
        **/
        public void Stay()
        {
            DealerHand[0].UpdateCardSprite();
            stayButton.interactable = false;
            hitButton.interactable = false;

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
            }
            else
            {
                //Player wins
                if (playerVal > dealerVal)
                {
                    gameStatus.text = "WINNER!";
                }

                if (playerVal < dealerVal)
                {
                    gameStatus.text = "Loser!";
                }

                if (playerVal == dealerVal)
                {
                    gameStatus.text = "TIE!";
                }
            }



        }
        void Start()
        {
            SetupNewGame();
        }

        //Calculate Player Hand Value

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
            hitButton.interactable = true;
            stayButton.interactable = true;

            //Deal 2 cards to the dealer
            DealCardDealer(false);
            DealCardDealer();

            //Deal 2 Cards to the Player
            DealCardPlayer();
            DealCardPlayer();


            //Populate players list with connected players
            //Deal 2 cards to each player

            //Start with player furthest right,
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
        // Update is called once per frame
        void Update()
        {

        }
    }
}