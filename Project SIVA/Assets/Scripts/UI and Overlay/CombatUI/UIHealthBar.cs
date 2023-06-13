using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour, IObserver
{
    Slider _healthSlider;

    private void Start(){
        _healthSlider = GetComponent<Slider>();
        DamageManager.Instance.AddObserver(this);
    }

    public void SetMaxHealth(float maxHealth) {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
    }

    public void SetHealth(float health) {
        _healthSlider.value = health;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerHealthAltered)
        {
            SetHealth(PlayerController.Instance.character.hitpoints);
        }
    }
}
