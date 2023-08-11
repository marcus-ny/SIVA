using System.Collections.Generic;
// This one imports priority queue since unity .net version does not support this intrinsically

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

    public void HealFromLifesteal(float healAmt)
    {
        if (PlayerController.Instance.character.hitpoints + healAmt >= 100)
        {
            PlayerController.Instance.character.hitpoints = 100;
        }
        else
        {
            PlayerController.Instance.character.hitpoints += healAmt;
        }
        NotifyObservers(GameEvents.PlayerLifesteal);
    }

    public void DealDamageToPlayer(float damage)
    {
        if (playerController.character.activeTile.light_level > 0)
        {
            damage *= 1.5f;
        }

        recentDamage = damage;

        playerParty[0].hitpoints -= damage;

        playerParty[0].DisplayDamageVisual();

        NotifyObservers(GameEvents.PlayerHealthAltered);
    }

    public void DealDamageToEnemy(float damage, Enemy target)
    {
        target.DisplayDamageVisual("Red");
        if (PlayerController.Instance.character.activeTile.light_level > 0)
        {
            damage /= 2;
        }
        recentTarget = target;
        recentDamage = damage;

        target.TakeDamage(damage);

    }

    public void FriendlyFireToEnemy(float damage, Enemy target)
    {
        target.TakeDamage(damage);
        target.DisplayDamageVisual("Red");
        recentTarget = target;
        recentDamage = damage;
        NotifyObservers(GameEvents.EnemyFriendlyFire);
    }

    public void HealEnemy(float healAmount, Enemy target)
    {
        recentTarget = target;
        recentDamage = healAmount;
        target.TakeDamage(-1 * healAmount);
        target.DisplayDamageVisual("Green");
        NotifyObservers(GameEvents.EnemyHealed);

    }

    // This function for tick damage from entering light tile
    public void DealTickDamage()
    {
        if (playerParty[0].activeTile.light_level > 0)
        {
            playerParty[0].hitpoints -= lightDamage;
            playerParty[0].DisplayDamageVisual();
            NotifyObservers(GameEvents.LightDamage);
        }

    }

    public void RaiseEventEnemyHealthAltered(float trueDamage, Enemy thisTarget)
    {
        recentDamage = trueDamage;
        recentTarget = thisTarget;
        NotifyObservers(GameEvents.EnemyHealthAltered);
    }
}
