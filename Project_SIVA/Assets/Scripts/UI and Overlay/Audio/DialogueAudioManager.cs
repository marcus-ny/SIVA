using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAudioManager : MonoBehaviour
{
    // If singleton is needed
    //public static DialogueAudioManager instance { get; private set; }

    private static DialogueAudioManager _instance;

    public static DialogueAudioManager Instance { get { return _instance; } }

    [Header ("sugondeeez")]
    private AudioSource source;
    
    private void Awake()
    {
        //instance = this;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
