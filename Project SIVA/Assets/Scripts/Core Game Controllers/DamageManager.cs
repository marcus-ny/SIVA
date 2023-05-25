using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private static DamageManager _instance;

    public MouseController mouseController;

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
        if (mouseController.character == null)
        {
            return;
        }
        
        playerParty.Add(mouseController.character);
        
    }

    // This function is temporarily meant for enemy dealing damage to player
    // (not implemented yet)
    public void DealDamageToEnemy(int damage, EnemyInfo target)
    {
        target.hitpoints -= damage;
    }

    // This function for tick damage from entering light tile
    public void tickDamage()
    {
        if (playerParty[0].activeTile.light_level > 0)
        { 
            playerParty[0].hitpoints -= 5;
        }
        
    }
}
