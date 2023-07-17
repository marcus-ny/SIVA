using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour, IObserver
{
    private AudioSource source;
    [SerializeField] public AudioClip damageSound;
    [SerializeField] private AudioClip lightSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip error;
    //private float delay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DamageManager.Instance.AddObserver(this);
        BattleSimulator.Instance.AddObserver(this);
        PlayerController.Instance.AddObserver(this);

        source = GetComponent<AudioSource>();
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if ((gameEvent == GameEvents.EnemyHealthAltered) || (gameEvent == GameEvents.PlayerHealthAltered) || (gameEvent == GameEvents.EnemyFriendlyFire))
        {
            // Play damage sound
            PlaySound(damageSound);
        }
        if (gameEvent == GameEvents.LightDamage)
        {
            // Play light sound
            PlaySound(lightSound);
        }
        if (gameEvent == GameEvents.EnemyHealed)
        {
            // Play heal sound
            PlaySound(healSound);
        }
        // Error Related
        if ((gameEvent == GameEvents.AOEunsuccessful) || (gameEvent == GameEvents.InteractableOFR) || (gameEvent == GameEvents.NoInteractable) || (gameEvent == GameEvents.TargetOFR) || (gameEvent == GameEvents.NoTarget))
        {
            PlaySound(error);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
