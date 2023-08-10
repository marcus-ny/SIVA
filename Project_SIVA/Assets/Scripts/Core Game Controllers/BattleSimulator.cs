using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, TRANSITION, PLAYER_WIN, ENEMY_WIN }
public class BattleSimulator : Publisher, IObserver
{
    private static BattleSimulator _instance;
    public static BattleSimulator Instance { get { return _instance; } }

    public BattleState State;

    public readonly int MAX_ACTIONS = 4;

    public int actionsPerformed;

    private Enemy currentEnemy;

    public List<Enemy> enemyList;

    public PlayerController player;

    public bool moving;

    public CutsceneRunner cutsceneRunner;

    public bool levelComplete;

    public Enemy GetCurrentEnemy()
    {
        return currentEnemy;
    }

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
        enemyList = new();
        levelComplete = false;
    }

    void Start()
    {
        State = BattleState.START;
        EnemyManager.Instance.AddObserver(this);
        StartCoroutine(AnnounceGameStart());

        actionsPerformed = 0;
        if (cutsceneRunner != null)
        {
            cutsceneRunner.RunCutscene();
        }

    }
    private void Update()
    {
        if (enemyList.Count != EnemyManager.Instance.transform.childCount)
        {
            for (int i = 0; i < EnemyManager.Instance.transform.childCount; i++)
            {
                enemyList.Add(EnemyManager.Instance.transform.GetChild(i).GetComponent<Enemy>());
            }
        }

        if (State == BattleState.ENEMY_TURN)
        {
            Coroutine EnemyCoroutine = StartCoroutine(EnemyTakeActions());
            State = BattleState.TRANSITION;
        }

    }
    IEnumerator AnnounceGameStart()
    {
        yield return new WaitForSecondsRealtime(1);
        NotifyObservers(GameEvents.GameStart);
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.PlayerWin)
        {
            StartGame();
            PlayerWin();
        }
    }
    public void StartGame()
    {
        if (State == BattleState.START)
        {
            State = BattleState.PLAYER_TURN;
            NotifyObservers(GameEvents.PlayerTurn);
        }
        else if (State == BattleState.PLAYER_TURN && levelComplete)
        {
            NotifyObservers(GameEvents.PlayerTurn);
        }
    }

    /*
     * All enemy actions sequentially executed
     */
    IEnumerator EnemyTakeActions()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (enemyList.Count <= 0) BattleSimulator.Instance.PlayerWin();

        Enemy bossEnemy = enemyList[0];

        foreach (Enemy enemy in enemyList)
        {
            if (enemy.GetType() == typeof(VampireBoss))
            {
                bossEnemy = enemy;
                continue;
            }
            currentEnemy = enemy;
            enemy.Action();
            while (enemy.state_moving)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1.0f);

        }
        if (bossEnemy.GetType() == typeof(VampireBoss))
        {
            bossEnemy.Action();
            currentEnemy = bossEnemy;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        
        SwitchTurns();

    }
    public void SwitchTurns()
    {

        if (State == BattleState.PLAYER_TURN)
        {
            // Feal tick damage from standing in light
            DamageManager.Instance.DealTickDamage();

            // Change to EnemyTurn and notify observers
            State = BattleState.ENEMY_TURN;
            NotifyObservers(GameEvents.EnemyTurn);


            foreach (Enemy enemy in enemyList)
            {
                // Reset AP
                enemy.actionsPerformed = 0;
            }
            // Reset player AP
            actionsPerformed = 0;
        }
        else if (State == BattleState.TRANSITION)
        {
            foreach (Enemy enemy in enemyList)
            {
                // Reset AP
                enemy.actionsPerformed = 0;
            }
            // Change to PlayerTurn and notify observers
            State = BattleState.PLAYER_TURN;
            NotifyObservers(GameEvents.PlayerTurn);

            actionsPerformed = 0;
        }
    }
    IEnumerator WaitForInteractInput()
    {
        WorldEntitiesManager.Instance.HightlightAll(true);

        while (!Input.GetMouseButtonDown(0))
        {
            player.GetInteractRange();
            yield return null;
        }

        if (player.InteractTrigger())
        {

            actionsPerformed += 1;
        }

        WorldEntitiesManager.Instance.HightlightAll(false);
    }
    public void InteractItem()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }
        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForInteractInput());
        }
    }
    public void MoveUnit()
    {

        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForPlayerMoveInput());

        }
    }
    IEnumerator WaitForPlayerMoveInput()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            player.GetMovementRange();
            yield return null;
        }
        if (player.MoveTrigger())
        {
            if (!levelComplete) actionsPerformed += 1;
        }
    }
    IEnumerator WaitForMeleeInput()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            // Show attack range tiles
            player.GetMeleeRange();
            yield return null;
        }
        if (!player.moving && player.MeleeTrigger())
        {
            actionsPerformed += 1;
        }
    }
    IEnumerator WaitForAoeInput()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            player.ShowAoeTiles();
            yield return null;
        }
        if (!player.moving && player.AoeAttackTrigger())
        {
            if (!levelComplete) actionsPerformed += 2;
        }
    }
    public void DealMeleeDamage()
    {
        if (actionsPerformed == MAX_ACTIONS)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForMeleeInput());
        }
    }
    public void DealAOEDamage()
    {
        if (actionsPerformed > MAX_ACTIONS - 2)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForAoeInput());
        }
    }

    public void CastFireball()
    {
        if (actionsPerformed > MAX_ACTIONS - 1)
        {
            return;
        }

        if (State == BattleState.PLAYER_TURN)
        {
            StartCoroutine(WaitForFireballCast());
        }
    }

    IEnumerator WaitForFireballCast()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            player.ShowFireboltCastTile();
            yield return null;
        }
        if (player.CastFireballTrigger())
        {
            actionsPerformed += 1;
        }
    }
    public void EnemyWin()
    {
        State = BattleState.ENEMY_WIN;
        levelComplete = true;
        NotifyObservers(GameEvents.PlayerLose);
    }

    public void PlayerWin()
    {

        {
            State = BattleState.PLAYER_WIN;
            if (levelComplete == false)
            {
                levelComplete = true;
                NotifyObservers(GameEvents.PlayerWin);
            }

            State = BattleState.PLAYER_TURN;

            actionsPerformed = 0;
        }
    }

    public void DisableBoss()
    {
        NotifyObservers(GameEvents.BossPowerDisabled);
    }
}
