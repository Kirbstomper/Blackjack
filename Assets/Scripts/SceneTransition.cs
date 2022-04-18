using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[CreateAssetMenu( menuName = "ScriptableObjects/SceneTransition") ]
public class SceneTransition : ScriptableObject 
{
    public string toTransition;


    public void TransitionScene()
    {
        SceneManager.LoadScene(toTransition);
    }
}
