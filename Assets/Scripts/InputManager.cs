using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


namespace Blackjack
{


    public class InputManager : MonoBehaviour
    {
        public Button hitButton;
        public Button stayButton;
        public Button resetButton;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame && hitButton.IsInteractable())
            {
                hitButton.onClick.Invoke();
            }
            
            if (Gamepad.current.buttonEast.wasPressedThisFrame && stayButton.IsInteractable())
            {
                stayButton.onClick.Invoke();
            }

            if (Gamepad.current.startButton.wasPressedThisFrame && resetButton.IsInteractable())
            {
                resetButton.onClick.Invoke();
            }
        }
    }
}