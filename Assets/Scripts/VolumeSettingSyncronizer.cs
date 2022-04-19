using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingSyncronizer : MonoBehaviour
{
    public AudioSetting setting; // The setting we wish to sync up with
    // Start is called before the first frame update
    void Start()
    {
        var slider = GetComponentInChildren<Slider>();
        var muteToggle = GetComponentInChildren<Toggle>();

        slider.value = setting.volume;
        muteToggle.isOn = setting.isMuted;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
