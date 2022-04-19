using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioSource")]
public class AudioObject : ScriptableObject
{
    public string filePath; //Path to the given effects file
    public AudioSetting audioSetting; //The audio setting to derive volume from;
    AudioSource audioSource;
    public bool isLooping;  //Determines if the effect should loop forever
                            //or only play once


    //Plays the given sound effect
    public void Play()
    {
        if(audioSource == null)
        {
            var _obj = new GameObject("MusicAudioSource", typeof(AudioSource));
            audioSource = _obj.GetComponent<AudioSource>();
        }
        
        AsyncOperationHandle<AudioClip> audioClipHandle = Addressables.LoadAssetAsync<AudioClip>(filePath);

        audioClipHandle.Completed += PlaySoundWhenReady;

    }

    void PlaySoundWhenReady(AsyncOperationHandle<AudioClip> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            var newAudioClip = handleToCheck.Result;
            audioSource.clip = newAudioClip;
            audioSource.volume = audioSetting.volume;
            audioSource.mute = audioSetting.isMuted;
            audioSource.Play();
        }
    }
}
