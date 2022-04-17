using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


namespace Blackjack
{


    public class InputManager : MonoBehaviour
    {

        public GameManager gameManager;


        // Start is called before the first frame update
        void Start()
        {
        }


        public void PressStay()
        {
            if (gameManager.IsGameState(GameManager.GameState.PLAYERTURN)
            || gameManager.IsGameState(GameManager.GameState.BLACKJACK))
            {
                gameManager.Stay();
            }
        }

        public void PressDeal()
        {
            if (gameManager.IsGameState(GameManager.GameState.PLAYERTURN))
            {
                gameManager.DealCardPlayer();
            }
        }

        public void PressReset()
        {
            gameManager.SetupNewGame();
        }

        public void PressBet()
        {
            if (gameManager.IsGameState(GameManager.GameState.BETTING))
                gameManager.PlaceBet();
        }


        public void PressRaise()
        {
            if(gameManager.IsGameState(GameManager.GameState.BETTING))
                gameManager.ChangeBet(10);
        }

        public void PressLower()
        {
           if(gameManager.IsGameState(GameManager.GameState.BETTING))
                gameManager.ChangeBet(-10); 
        }

        void getControllerInput()
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                PressDeal();
            }

            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                PressStay();
            }

            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                PressReset();
            }
        }
        // Update is called once per frame
        void Update()
        {
            //getControllerInput();
        }

    }

}