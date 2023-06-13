using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour, IObserver
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] TextMeshProUGUI HPCounter;

    private void Start(){
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
            HPCounter.text = (PlayerController.Instance.character.hitpoints).ToString() + "/" + (_healthSlider.maxValue).ToString();
        }
    }
}
