using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blackjack
{
    public enum GameState { PLAYERTURN, BETTING, SIDEBETTING, BLACKJACK, END }
    public enum HandState { OPEN, STAY, DOUBLED_DOWN, BLACKJACK, BUST }
}