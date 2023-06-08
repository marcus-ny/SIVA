using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
// This one imports priority queue since unity .net version does not support this intrinsically
using Utils;

public class DamageManager : MonoBehaviour
{
    private static DamageManager _instance;

    public PlayerController playerController;

    public List<CharacterInfo> playerParty = new List<CharacterInfo>();

    public static DamageManager Instance { get { return _instance; } }


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
        playerParty[0].hitpoints -= damage;
        playerParty[0].DisplayDamageVisual();
    }
    // This function is temporarily meant for enemy dealing damage to player
    // (not implemented yet)
    public void DealDamageToEnemy(float damage, Enemy target)
    {
        target.TakeDamage(damage);
        // EnemyManager.Instance.enemyHpStatus.Enqueue(target, target.hitpoints);
        target.SwitchColor();

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
            playerParty[0].hitpoints -= 5;
            playerParty[0].DisplayDamageVisual();
        }
        
    }
}
