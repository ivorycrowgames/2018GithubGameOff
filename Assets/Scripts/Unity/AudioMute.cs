using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMute : MonoBehaviour {

    private bool _isMuted;
    public bool IsMuted
    {
        get
        {
            return _isMuted;
        }
        set
        {
            _isMuted = value;
            updateMuteState();
        }
    }

    public bool DefaultEnabled = true;
    public AudioSource[] ControlledSources;

    public GameObject MutedIndicator;
    public GameObject UnmutedIndicator;

    private const string mutedKeyName = "isUserVolumeMuted";

    // Use this for initialization
    void Start () {
        updateMuteState();
    }

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey(mutedKeyName))
        {
            PlayerPrefs.SetInt(mutedKeyName, DefaultEnabled ? 0 : 1);
        }

        IsMuted = (PlayerPrefs.GetInt(mutedKeyName) == 1);
    }

    public void ToggleVolume()
    {
        IsMuted = !IsMuted;
    }

    private void updateMuteState()
    {
        foreach(AudioSource audio in ControlledSources)
        {
            audio.mute = _isMuted;
        }

        PlayerPrefs.SetInt(mutedKeyName, _isMuted ? 1 : 0);

        if (MutedIndicator)
        {
            MutedIndicator.SetActive(_isMuted);
        }
        
        if (UnmutedIndicator)
        {
            UnmutedIndicator.SetActive(!_isMuted);
        }
    }
}
