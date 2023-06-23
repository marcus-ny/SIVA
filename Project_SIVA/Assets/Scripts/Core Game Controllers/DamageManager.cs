using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
// This one imports priority queue since unity .net version does not support this intrinsically
using Utils;

public class DamageManager : Publisher
{
    private static DamageManager _instance;

    public PlayerController playerController;

    public List<CharacterInfo> playerParty = new List<CharacterInfo>();

    public static DamageManager Instance { get { return _instance; } }

    public Enemy recentTarget;
    public float recentDamage;
    public float lightDamage = 5.0f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (playerController.character == null)
        {
            return;
        }
        if (playerParty.Count <= 0)
        {
            playerParty.Add(playerController.character);
        }
    }
    

    public void DealDamageToPlayer(float damage)
    {
        if (playerController.character.activeTile.light_level > 0)
        {
            damage = damage * 1.5f;
        }

        recentDamage = damage;

        playerParty[0].hitpoints -= damage;

        playerParty[0].DisplayDamageVisual();

        NotifyObservers(GameEvents.PlayerHealthAltered);
    }
    // This function is temporarily meant for enemy dealing damage to player
    // (not implemented yet)
    public void DealDamageToEnemy(float damage, Enemy target)
    {
        if (PlayerController.Instance.character.activeTile.light_level > 0)
        {
            damage /= 2;
        }
        recentTarget = target;
        recentDamage = damage;
        target.SwitchColor("Red");
        target.TakeDamage(damage);
        NotifyObservers(GameEvents.EnemyHealthAltered);       
    }
    
    public void FriendlyFireToEnemy(float damage, Enemy target)
    {
        target.TakeDamage(damage);
        target.SwitchColor("Red");
        recentTarget = target;
        recentDamage = damage;
        NotifyObservers(GameEvents.EnemyFriendlyFire);
    }

    public void HealEnemy(float healAmount, Enemy target)
    {
        recentTarget = target;
        recentDamage = healAmount;
        target.TakeDamage(-1 * healAmount);
        target.SwitchColor("Green");
        NotifyObservers(GameEvents.EnemyHealed);

    }

    public void changeColor(EnemyInfo target, Color color)
    {
        target.GetComponent<SpriteRenderer>().color = color;
    }

    // This function for tick damage from entering light tile
    public void tickDamage()
    {
        if (playerParty[0].activeTile.light_level > 0)
        { 
            playerParty[0].hitpoints -= lightDamage;
            playerParty[0].DisplayDamageVisual();
            NotifyObservers(GameEvents.LightDamage);
        }
        
    }
}
