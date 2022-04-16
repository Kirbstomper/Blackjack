using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update

    public string Suit;
    public string Face;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCardSprite()
    {
        AsyncOperationHandle<Sprite> spriteHandle = 
            Addressables.LoadAssetAsync<Sprite>(string.Format("Assets/Sprites/black/{0}_{1}_black.png", Suit, Face));
        
        spriteHandle.Completed += UpdateSpriteWhenReady;
    }


    void UpdateSpriteWhenReady(AsyncOperationHandle<Sprite> handleToCheck)
    {

        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            var sprite = GetComponent<SpriteRenderer>();
            sprite.sprite = handleToCheck.Result;

        }
    }
}
