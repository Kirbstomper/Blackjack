using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blackjack
{
    public class Card : MonoBehaviour
{
    public string Face;
    public string Suit;
    
    // Start is called before the first frame update
    void Start()
    {
        var text = GetComponentInChildren<TextMesh>();
        text.text = string.Format("{0}\n of\n {1}", Face, Suit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

}
