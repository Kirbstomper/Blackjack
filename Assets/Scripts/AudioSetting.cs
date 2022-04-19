using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioSetting")]
public class AudioSetting : ScriptableObject
{

    public float volume; //The volume for this specific audio source
    public bool isMuted; //Determines if this audio is muted or not


    public void SetVolume(float vol)
    {
        volume = vol;
    }
    public void SetIsMuted(bool muted)
    {
        isMuted = muted;
    }

}
