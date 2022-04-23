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

        public void PressStay()
        {
            if (gameManager.IsGameState(GameState.PLAYERTURN))
            {
                gameManager.Stay();
            }

        }

        public void PressHit()
        {
            if (gameManager.IsGameState(GameState.PLAYERTURN))
            {
                gameManager.Hit();
            }
        }

        public void PressReset()
        {
            if (gameManager.IsGameState(GameState.PLAYERTURN) || gameManager.IsGameState(GameState.END))
            {
                gameManager.SetupNewGame();
            }
        }

        public void PressBet()
        {
            if (gameManager.IsGameState(GameState.BETTING))
            {
                gameManager.PlaceBet();
            }
            else if (gameManager.IsGameState(GameState.SIDEBETTING))
                gameManager.PlaceSideBet();

        }


        public void PressRaise()
        {
            if (gameManager.IsGameState(GameState.BETTING))
            {
                gameManager.ChangeBet(10);
            }
            if (gameManager.IsGameState(GameState.SIDEBETTING))
            {
                gameManager.ChangeSideBet(10);
            }
        }

        public void PressLower()
        {
            if (gameManager.IsGameState(GameState.BETTING))
            {
                gameManager.ChangeBet(-10);
            }
            else if (gameManager.IsGameState(GameState.SIDEBETTING))
            {
                gameManager.ChangeSideBet(-10);
            }
        }

        public void PressDoubleDown()
        {
            if (gameManager.currentHand.canDoubleDown && gameManager.IsGameState(GameState.PLAYERTURN))
            {
                gameManager.DoubleDown();
            }

            gameManager.NextHand();
        }

        void getControllerInput()
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                PressHit();
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