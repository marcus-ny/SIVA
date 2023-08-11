using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Publisher
{
    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }


    public GameObject character_prefab;

    public Vector2Int playerSpawn;

    public CharacterInfo character;

    public List<OverlayTile> path;
    public List<OverlayTile> reachableTiles;

    PathFinder pathFinder;
    Rangefinder rangeFinder;

    public OverlayTile destinationTile;

    [SerializeField] Firebolt firebolt;

    private PlayerAnimator animationController;
    public PathArrowGenerator pathArrowGenerator;

    public bool moving;

    private readonly int speed = 3;
    private readonly int reach = 4;

    public Enemy mostrecentEnemy;
    private void Start()
    {
        pathFinder = new();
        rangeFinder = new();
        path = new();
        reachableTiles = new();
        pathArrowGenerator = new();
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
    }

    private void Update()
    {
        if (character != null && character.hitpoints <= 0 && !BattleSimulator.Instance.levelComplete)
        {
            BattleSimulator.Instance.EnemyWin();
        }

        if (character == null)
        {
            character = Instantiate(character_prefab).GetComponent<CharacterInfo>();
            // Place the enemy on spawn
            PositionCharacterOnTile(MapController.Instance.map[playerSpawn]);
            destinationTile = character.activeTile;
            reachableTiles = rangeFinder.GetReachableTiles(character.activeTile, reach, 1);
            animationController = character.GetComponent<PlayerAnimator>();
        }

        if (BattleSimulator.Instance.State == BattleState.PLAYER_TURN)
        {
            var tileHit = MouseController.Instance.GetFocusedOnTile();
            OverlayTile overlayTile;
            if (tileHit.HasValue)
            {

                overlayTile = tileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

                if (Input.GetMouseButtonDown(0))
                {
                    destinationTile = overlayTile;

                }
            }
        }

    }

    private IEnumerator MoveAlongPath()
    {
        moving = true;
        while (path.Count > 0)
        {
            var step = speed * Time.deltaTime;

            var zIndex = path[0].transform.position.z;

            character.activeTile.isBlocked = false;

            // This animation code should be abstracted to somewhere else
            // Keep this file only for controlling the player and nothing else
            character.prev = path[0].previous.gridLocation;
            character.cur = path[0].gridLocation;

            character.transform.position = Vector2.MoveTowards(character.transform.position,
                path[0].transform.position, step);

            character.transform.position = new Vector3(character.transform.position.x,
                character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
            {
                PositionCharacterOnTile(path[0]);
                if (path.Count > 0 && path[0].light_level > 0 && path[0].previous.light_level == 0)
                {
                    path.RemoveAt(0);
                    animationController.status = PlayerAnimator.Status.WEAKENING;
                    yield return new WaitForSecondsRealtime(1.5f);
                    animationController.status = PlayerAnimator.Status.NIL;
                    StartCoroutine(MoveInLight());
                    break;
                }
                path.RemoveAt(0);
            }

            yield return null;
        }


        if (path.Count == 0)
        {
            character.prev = character.cur = character.activeTile.gridLocation;
            // here
            //GetMovementRange();
        }
        moving = false;
        // character.activeTile.isBlocked = true;
    }

    private IEnumerator MoveInLight()
    {
        moving = true;
        while (path.Count > 0)
        {
            var step = speed * Time.deltaTime;

            var zIndex = path[0].transform.position.z;

            character.activeTile.isBlocked = false;

            character.prev = path[0].previous.gridLocation;
            character.cur = path[0].gridLocation;

            character.transform.position = Vector2.MoveTowards(character.transform.position,
                path[0].transform.position, step);

            character.transform.position = new Vector3(character.transform.position.x,
                character.transform.position.y, zIndex);

            if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
            {
                PositionCharacterOnTile(path[0]);
                if (path.Count > 0 && path[0].light_level == 0 && path[0].previous.light_level > 0)
                {
                    path.RemoveAt(0);
                    animationController.status = PlayerAnimator.Status.STRONGER;
                    yield return new WaitForSecondsRealtime(1.5f);
                    animationController.status = PlayerAnimator.Status.NIL;
                    StartCoroutine(MoveAlongPath());
                    break;
                }
                path.RemoveAt(0);
            }

            yield return null;
        }

        if (path.Count == 0)
        {
            character.prev = character.cur = character.activeTile.gridLocation;
        }
        moving = false;
    }

    IEnumerator SuccessfulMelee()
    {
        moving = true;
        animationController.status = PlayerAnimator.Status.MELEEING;
        yield return new WaitForSecondsRealtime(0.3f);
        DamageManager.Instance.DealDamageToEnemy(30, destinationTile.enemy);
        yield return new WaitForSecondsRealtime(0.7f);

        animationController.status = PlayerAnimator.Status.NIL;
        moving = false;
    }

    public bool MeleeTrigger()
    {
        List<OverlayTile> meleeRange = MapController.Instance.Get3x3Grid(character.activeTile);

        foreach (OverlayTile tile in meleeRange)
        {
            tile.HideTile();
        }
        bool inRange = (Mathf.Abs(character.activeTile.gridLocation.x - destinationTile.gridLocation.x) < 2) && (Mathf.Abs(character.activeTile.gridLocation.y - destinationTile.gridLocation.y) < 2);
        if (destinationTile.enemy != null && inRange)
        {
            StartCoroutine(SuccessfulMelee());
            NotifyObservers(GameEvents.SuccessfulAttack);
            return true;
        }
        else if (destinationTile.enemy == null)
        {
            NotifyObservers(GameEvents.NoTarget);
        }
        else if (!inRange)
        {
            NotifyObservers(GameEvents.TargetOFR);
        }
        return false;
    }

    public void GetMeleeRange()
    {
        List<OverlayTile> meleeRange = MapController.Instance.Get3x3Grid(character.activeTile);

        foreach (OverlayTile tile in meleeRange)
        {
            tile.ShowGreenTile();
        }
    }

    public void GetInteractRange()
    {
        List<OverlayTile> meleeRange = MapController.Instance.Get3x3Grid(character.activeTile);

        foreach (OverlayTile tile in meleeRange)
        {
            tile.ShowBlueTile();
        }
    }

    public bool AoeAttackTrigger()
    {
        List<OverlayTile> aoeRange = new();
        if (MouseController.Instance.GetFocusedOnTile().HasValue)
        {
            aoeRange = MapController.Instance.GetAoeAttackTiles(MouseController.Instance.mouseOverTile, character.activeTile, 3);

        }
        if (!MouseController.Instance.GetFocusedOnTile().HasValue || !aoeRange.Contains(MouseController.Instance.mouseOverTile))
        {
            NotifyObservers(GameEvents.AOEunsuccessful);
            return false;
        }

        foreach (OverlayTile tile in aoeRange)
        {
            Vector2Int coordinates = new Vector2Int(tile.gridLocation.x, tile.gridLocation.y);

            if (EnemyManager.Instance.enemyMap.ContainsKey(coordinates))
            {
                Enemy target = EnemyManager.Instance.enemyMap[coordinates];

                DamageManager.Instance.DealDamageToEnemy(50, target);
            }
        }

        return true;
    }

    public void ShowAoeTiles()
    {
        List<OverlayTile> aoeRange = new();
        foreach (OverlayTile tile in MapController.Instance.GetAllAoeTiles(character.activeTile, 5))
        {
            tile.ShowTile();
        }
        if (MouseController.Instance.GetFocusedOnTile().HasValue)
        {
            aoeRange = MapController.Instance.GetAoeAttackTiles(MouseController.Instance.mouseOverTile, character.activeTile, 3);

        }

        foreach (OverlayTile tile in aoeRange)
        {
            tile.ShowGreenTile();
        }
    }

    public bool MoveTrigger()
    {
        foreach (var tile in reachableTiles)
        {
            tile.ShowTile();
        }


        path = pathFinder.FindPath(character.activeTile, MouseController.Instance.mouseOverTile, reachableTiles, 1);

        if (path.Count == 0) return false;

        if (character.activeTile.light_level > 0)
        {
            StartCoroutine(MoveInLight());
        }
        else
        {
            StartCoroutine(MoveAlongPath());
        }

        NotifyObservers(GameEvents.SuccessfulMove);
        return true;
    }

    public bool InteractTrigger()
    {
        bool inRange = (Mathf.Abs(character.activeTile.gridLocation.x - destinationTile.gridLocation.x) < 2) && (Mathf.Abs(character.activeTile.gridLocation.y - destinationTile.gridLocation.y) < 2);
        Vector2Int coordinates = new(destinationTile.gridLocation.x, destinationTile.gridLocation.y);

        if (inRange && WorldEntitiesManager.Instance.entityMap.ContainsKey(coordinates))
        {
            if (WorldEntitiesManager.Instance.Interact(destinationTile))
            {
                NotifyObservers(GameEvents.SuccessfulInteract);
                return true;
            }


        }
        else if (WorldEntitiesManager.Instance.entityMap.ContainsKey(PlayerController.Instance.character.activeTile.gridLocation2d))
        {
            if (WorldEntitiesManager.Instance.Interact(PlayerController.Instance.character.activeTile))
            {
                NotifyObservers(GameEvents.SuccessfulInteract);
                return true;
            }
        }
        else if (!inRange)
        {
            NotifyObservers(GameEvents.InteractableOFR);
        }
        else
        {

            NotifyObservers(GameEvents.NoInteractable);
        }
        return false;
    }

    public void GetMovementRange()
    {
        reachableTiles = rangeFinder.GetReachableTiles(character.activeTile, reach, 1);

        foreach (var tile in reachableTiles)
        {
            tile.ShowGreenTile();
        }
        List<OverlayTile> tempPath = pathFinder.FindPath(character.activeTile, MouseController.Instance.mouseOverTile, reachableTiles, 1);


        foreach (var item in reachableTiles)
        {
            item.SetPathDir(PathArrowGenerator.ArrowDir.None);
        }

        for (int i = 0; i < tempPath.Count; i++)
        {
            var prevTile = i > 0 ? tempPath[i - 1] : PlayerController.Instance.character.activeTile;
            var nextTile = i < tempPath.Count - 1 ? tempPath[i + 1] : null;

            var arrowDir = pathArrowGenerator.Translate(prevTile, tempPath[i], nextTile);
            tempPath[i].SetPathDir(arrowDir);

        }
    }


    public void ShowFireboltCastTile()
    {
        List<OverlayTile> fireboltCastRange = rangeFinder.GetReachableTiles(character.activeTile, 6, 1);
        foreach (OverlayTile tile in fireboltCastRange)
        {
            tile.ShowGreenTile();
        }
    }
    public bool CastFireballTrigger()
    {
        List<OverlayTile> fireboltCastRange = rangeFinder.GetReachableTiles(character.activeTile, 6, 1);
        if (fireboltCastRange.Contains(MouseController.Instance.mouseOverTile) && MouseController.Instance.mouseOverTile.enemy != null)
        {
            mostrecentEnemy = destinationTile.enemy;
            //NotifyObservers(GameEvents.EnemyHealthAltered);
            Instantiate(firebolt);
            firebolt.transform.position = character.transform.position;
            DamageManager.Instance.DealDamageToEnemy(30, destinationTile.enemy);
            return true;
        }
        else
        {
            return false;
        }

    }


    public void TransitionLTS()
    {
        StartCoroutine(TransitionLTSAnim());
    }

    public void TransitionSTL()
    {
        StartCoroutine(TransitionSTLAnim());
    }

    IEnumerator TransitionLTSAnim()
    {
        animationController.status = PlayerAnimator.Status.STRONGER;
        yield return new WaitForSecondsRealtime(1.5f);
        animationController.status = PlayerAnimator.Status.NIL;
    }

    IEnumerator TransitionSTLAnim()
    {
        animationController.status = PlayerAnimator.Status.WEAKENING;
        yield return new WaitForSecondsRealtime(1.5f);
        animationController.status = PlayerAnimator.Status.NIL;
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x,
            tile.transform.position.y, tile.transform.position.z);

        character.GetComponent<SpriteRenderer>().sortingOrder =
            tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
        character.activeTile.isBlocked = true;
    }
}
