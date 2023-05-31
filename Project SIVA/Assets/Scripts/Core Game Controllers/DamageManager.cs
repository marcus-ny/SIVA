using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (playerParty.Count <= 0)
        {
            playerParty.Add(mouseController.character);
        }
    }
    

  
    // This function is temporarily meant for enemy dealing damage to player
    // (not implemented yet)
    public void DealDamageToEnemy(float damage, Enemy target)
    {
        target.TakeDamage(damage);
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
            playerParty[0].SwitchColor();
        }
        
    }
}
