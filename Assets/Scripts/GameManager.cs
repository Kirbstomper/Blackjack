using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace Blackjack
{
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Game manager should have list of players and the dealer (who is technically also a player)
    //Maybe set this as a queue?
    public  List<Card> Deck = new List<Card>();
    public Card prefabCard;
    public List<Card> PlayerHand = new List<Card>();
    string[] suits = {"Tiles", "Clovers", "Pikes","Hearts"};
    string[] faces = {"A","2", "3", "4", "5","6","7", "8", "9", "10", "King", "Queen", "Jack"};
    
/**
    Deals a card to the current player, then moves turn to next
**/
    public void DealCard()
    {
       //Hit the player with a new card

        var x = prefabCard.transform.position.x;
        var y = prefabCard.transform.position.y;
 
        PlayerHand.Add(Instantiate(prefabCard , new Vector3(x+0.5f*PlayerHand.Count,y,-1*PlayerHand.Count), Quaternion.identity));
        PlayerHand[PlayerHand.Count-1].Suit = suits[Random.Range(0,suits.Length)];
        PlayerHand[PlayerHand.Count-1].Face = faces[Random.Range(0, faces.Length)];
        PlayerHand[PlayerHand.Count-1].UpdateCardSprite();

    }

/***
    Stop getting new cards
**/
    public void Stay()
    {
        
    }
    void Start()
    {
 
        
    }

    //Setup the deck
   
    void setupNewGame(){
        //Deal 2 cards to the dealer


        //Populate players list with connected players
        //Deal 2 cards to each player

        //Start with player furthest right,


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}