using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack
{
    public class PlayerHand : MonoBehaviour
    {
        public List<Card> cards = new List<Card>(); //The cards in this current hand
        public HandState handState; // The state of the current hand. Determines what can be done

        public bool canDoubleDown = false; //Can this player 
        public bool canSplit = false; //Can the player currently split this hand

        public int bet; // The current bet on this hand for the player

        //Don't care about access, this is my game anyways, lets mess with state everywhere!!!!!!



    void Update()
    {
        
        foreach (Card c in cards)
        {
            c.transform.SetParent(this.transform);
        }
    }

    }
}