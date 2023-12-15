using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviorScript : MonoBehaviour {
    public static PlayerBehaviorScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;
    [SerializeField] protected LayerMask turretCollisionMask;
    [SerializeField] protected Animator playerSpriteAnimator;
    [SerializeField] protected GameObject deadPlayer;
    [SerializeField] protected float multiInputWindow = 0.05f;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTiming = 0.2f;
    
    private float currentSpeed;

    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;

    private float lastInitialDirectionalInputTime;
    private bool playerInAction = false;
    private Vector3 currActionDir;
    private bool hasDashed = false;
    private bool isDead = false;

    protected Vector3 lastPos;
    protected Vector3 lastDir;
    protected PlayerState.PlayerAction lastAction;

    // Tutorial variables
    // Variable for Tutorial tips
    private GameObject undoTip;

    // Animation state variables
    private string currentState;
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_MOVE = "PlayerMove";
    const string PLAYER_DASH = "PlayerDash";
    const string PLAYER_ATTACK = "PlayerAttack";
    const string PLAYER_DEFLECT = "PlayerDeflect";
    const string PLAYER_DIE = "PlayerDie";

    protected bool didOneAction;

    protected bool dashIsNext;

    public void setDashIsNext(bool b) {
        dashIsNext = b;
    }

    protected bool firstInputOfDash;

    protected bool liftedUp;

    public enum Direction {
        Down,
        DownLeft,
        Left,
        UpLeft,
        Up,
        UpRight,
        Right,
        DownRight
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } 

        undoTip = GameObject.Find("UndoTip");
        if (undoTip != null) {
            TipManagerScript.Instance.addToAllTips("UndoTip", undoTip);
            undoTip.SetActive(false);
        }
        
    }

    void Start() {
        destination.parent = null;
        currentSpeed = playerSpeed;
        lastPos = transform.position;
        lastDir = new Vector3(0, 0, 0);
    }
    public PlayerState GetPlayerState() {
        return new PlayerState(lastPos, lastDir, lastAction);
    }
    void Update() {
        if (isDead) {
            return;
        }
        // Move Player to destination point after input window closes
        if (Time.time - lastInitialDirectionalInputTime >= multiInputWindow) {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, currentSpeed * Time.deltaTime);

            if (playerInAction) {
                if (hasDashed) {
                    ChangePlayerAnimationState(PLAYER_DASH);
                } else {
                    lastAction = PlayerState.PlayerAction.Move;
                    ChangePlayerAnimationState(PLAYER_MOVE);
                }
            }
        }
        if (PlayerInputManager.Instance.getIsStickoMode()) {
            if ( !didOneAction) {
                processPlayerInputStickoMode();
            }
        } else {
            if (!ProjectileManagerScript.Instance.getAreProjectilesInAction() && !EnemyManagerScript.Instance.getAreEnemiesInAction()) {
                processPlayerInput();
            }
        }
    }

    private void playerAttack(Vector3 enemyPosition) {
        lastAction = PlayerState.PlayerAction.Attack;
        lastDir = currActionDir;
        if (GameStateManager.Instance!=null && !PlayerInputManager.Instance.getIsStickoMode()) {
            GameStateManager.Instance.captureGameState();
        }
        lastAction = PlayerState.PlayerAction.NONE;
        if (!PlayerInputManager.Instance.getIsStickoMode()) {
            SoundManager.Instance.playSound("staff_swish");
        }
        
        ChangePlayerAnimationState(PLAYER_ATTACK);

        EnemyManagerScript.Instance.EnemyAttacked(enemyPosition, playerAttackDamage);
        if (ProjectileManagerScript.Instance.ProjectileAttacked(enemyPosition,transform.position)) {
            ChangePlayerAnimationState(PLAYER_DEFLECT);
        };

    }

    private void processPlayerInputStickoMode() {
        List<PlayerInputManager.PlayerInputActions> allowedActions = PlayerInputManager.Instance.getAllowedActions();

        if (allowedActions.Contains(PlayerInputManager.PlayerInputActions.Attack)) { // Okay were only attacking
            Vector3 attackDir = PlayerInputManager.Instance.getDirectionalInput();
            if (PlayerInputManager.Instance.getAttackInput() && attackDir != new Vector3(0,0,0)) {

                playerAttack(destination.position + attackDir);
                processEnemyTurn(false);
                didOneAction = true;
                return;
            }
        } else if (allowedActions.Contains(PlayerInputManager.PlayerInputActions.Dash)) { // Okay were dashing
            if (PlayerInputManager.Instance.pressedAllDirectionals() && !firstInputOfDash) {
                firstInputOfDash = true;
            }

            if (firstInputOfDash && PlayerInputManager.Instance.pressedNoDirectionals() && !liftedUp) {
                liftedUp = true;
            }

            if (liftedUp && PlayerInputManager.Instance.pressedAllDirectionals() ) {
                Vector3 dir = PlayerInputManager.Instance.getDirectionalInput();
                ChangePlayerAnimationState(PLAYER_DASH);
                destination.position += dir*2;
                processEnemyTurn(false);
                didOneAction = true;
                currentSpeed = dashSpeed;
                return;
            }

        } else { // okay were moving
            
            if (PlayerInputManager.Instance.pressedAllDirectionals()) {
                
                Vector3 dir = PlayerInputManager.Instance.getDirectionalInput();
                ChangePlayerAnimationState(PLAYER_MOVE);
                destination.position += dir;
                processEnemyTurn(false);
                didOneAction = true;
            }
        }
    }

    private void processPlayerInput() {
        // isHorizontalAxisInUse and isVerticalAxisInUse make GetAxisRaw behave like GetKeyDown instead of GetKey
        Vector3 currInputDir = PlayerInputManager.Instance.getDirectionalInput();

        // Check if an input for a diagonal move was made
        // This is to add leniency to making a diagonal move so it doesn't have to be frame-perfect
        if (Time.time - lastInitialDirectionalInputTime < multiInputWindow) {
            if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;
                if(Mathf.Abs(currInputDir.x) == 1f && Mathf.Abs(currInputDir.y) == 1f) {
                    if (playerInAction) {   
                        // if original action was valid and had already been initiated
                        if (!willHitWall(destination.position + currInputDir - currActionDir)) {
                            
                            destination.position += currInputDir - currActionDir;
                            // for some reason, second input becomes (+-1, +-1, 0), so first input needs to be subtracted
                            currActionDir = currInputDir;
                            lastAction = PlayerState.PlayerAction.Move;
                            lastDir = currActionDir;
                        } else {
                            destination.position -= currActionDir;    // undo initiated action if new destination is invalid
                            playerInAction = false;
                        }
                    } else {    
                        // if original action was invalid and had never started
                        if (!willHitWall(destination.position + currInputDir)) {
                            destination.position += currInputDir;
                            currActionDir = currInputDir;
                            playerInAction = true;
                            lastAction = PlayerState.PlayerAction.Move;
                            lastDir = currActionDir;
                        }
                    }
                }
            }
        }

        // Check if a second identical input has been inputted while player is mid move within the dash timing window
        // Dash can only be performed once per action
        if (playerInAction && Time.time - lastInitialDirectionalInputTime < dashTiming && !hasDashed) {
            // Check if player has pressed the attack button and is not a diagonal input
            if ( PlayerInputManager.Instance.getAttackInput() && !(Mathf.Abs(currActionDir.x) == 1f && Mathf.Abs(currActionDir.y) == 1f)) {
                playerAttack(destination.position);

                // undo motion
                destination.position -= currActionDir;
                playerInAction = false;

                processEnemyTurn(false);
            } 

            else if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;

                // Ensure player doesn't move into wall
                if (currInputDir == currActionDir) {
                    
                    if (!willHitWall(destination.position + currInputDir)) {
                        hasDashed = true;
                        lastAction = PlayerState.PlayerAction.Dash;
                        lastDir = currInputDir;
                        destination.position += currInputDir;
                        currentSpeed = dashSpeed;
                    }
                }
            }
        }
        
        // Check for new input if Player is close enough to destination AND nothing is moving
        if (Vector3.Distance(transform.position, destination.position) <= Mathf.Epsilon && !ProjectileManagerScript.Instance.getAreProjectilesInAction()) {
            // Player character has finished performing action
            // NPCs take turns, reset everything to listen for new move input
            if (playerInAction) {
                processEnemyTurn(true);

                lastInitialDirectionalInputTime = 0f;
                playerInAction = false;
                if (!isDead) ChangePlayerAnimationState(PLAYER_IDLE);
                
                lastDir = currActionDir;
                currActionDir = Vector3.zero;
                hasDashed = false;
                currentSpeed = playerSpeed;
            }

            if (newInputReceived(currInputDir) && !playerInAction) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;
                lastInitialDirectionalInputTime = Time.time;
                currActionDir = currInputDir;
                //lastDir = currActionDir;
                if (!willHitWall(destination.position + currInputDir)) {
                    destination.position += currInputDir;
                    playerInAction = true;
                }
            }
        }
        

        if (currInputDir.x == 0) {
            isHorizontalAxisInUse = false;
        }
        if (currInputDir.y == 0) {
            isVerticalAxisInUse = false;
        }
    }

    public void LoadPlayerState(PlayerState p) {
        didOneAction = false;
        transform.position = p.getPos();
        lastPos = p.getPos();
        lastAction = p.getAction();
        playerInAction = false;
        dashIsNext = false;
        liftedUp = false;
        firstInputOfDash = false;
        destination.position = transform.position;
        isDead = false;
        currActionDir = p.getDirection();
        currentSpeed = playerSpeed;
        //PlayerInputManager.Instance.setAllowedActions(new List<KeyCode>(), false);
        ChangePlayerAnimationStateForce(PLAYER_IDLE);
    }

    private void processEnemyTurn(bool captureState) {
        if (captureState && lastAction == PlayerState.PlayerAction.NONE) {
            lastAction = PlayerState.PlayerAction.Move;
        }
        lastDir = currActionDir;
        if (PlayerInputManager.Instance.getIsStickoMode()){
            PlayerInputManager.Instance.setAllowedActions(new List<KeyCode>(), false);
        }
        if (GameStateManager.Instance!=null && captureState && !PlayerInputManager.Instance.getIsStickoMode()) {
            GameStateManager.Instance.captureGameState();
        }
        lastAction = PlayerState.PlayerAction.NONE;
        if (EnemyManagerScript.Instance != null) {
            EnemyManagerScript.Instance.EnemyTurn();
        }
        if (ProjectileManagerScript.Instance != null) {
            ProjectileManagerScript.Instance.ProjectileTurn();
        }
        if (TurretManagerScript.Instance != null) {
            TurretManagerScript.Instance.TurretTurn();
        }
        lastPos = destination.position;
    }

    /// <summary>
    /// Checks if a position is inside a wall
    /// </summary>
    /// <param name="position"></param>
    private bool willHitWall(Vector3 position) {
        if (Physics2D.OverlapCircle(position, 0.2f, turretCollisionMask)) {
            Debug.Log("swag");
        }
        if (Physics2D.OverlapCircle(position, 0.1f, collisionMask)) {
            Debug.Log("lois");
        }
        return Physics2D.OverlapCircle(position, 0.1f, collisionMask) || Physics2D.OverlapCircle(position, 0.1f, turretCollisionMask);
    }

    /// <summary>
    /// Checks whether an input is currently being given by the player
    /// </summary>
    /// <param name="input"></param>
    private bool newInputReceived(Vector3 input) {
        return Mathf.Abs(input.x) == 1f && !isHorizontalAxisInUse || Mathf.Abs(input.y) == 1f && !isVerticalAxisInUse;
    }

    /// <summary>
    /// Updates the player sprite
    /// </summary>
    /// <param name="newState"></param>
    private void ChangePlayerAnimationState(string newState) {
        if (currentState == newState) return;
        playerSpriteAnimator.SetFloat("MovementX", currActionDir.x);
        playerSpriteAnimator.SetFloat("MovementY", currActionDir.y);
        playerSpriteAnimator.Play(newState);
        currentState = newState;
    }

    /// <summary>
    /// Force sprite to change even if state is the same
    /// </summary>
    /// <param name="newState"></param>
    private void ChangePlayerAnimationStateForce(string newState) {
        playerSpriteAnimator.SetFloat("MovementX", currActionDir.x);
        playerSpriteAnimator.SetFloat("MovementY", currActionDir.y);
        playerSpriteAnimator.Play(newState);
        currentState = newState;
    }

    // Move Point getter method
    public Transform getMovePoint() {
        return destination;
    }

    public Transform getPlayerTransform() {
        return this.transform;
    }

    public void killPlayer() {
        if (PlayerInputManager.Instance.getIsStickoMode()) {
            return;
        }
        ChangePlayerAnimationState(PLAYER_DIE);
        isDead = true;

        // Specific case for level 0
        int hasDiedFirstTime = PlayerPrefs.GetInt("hasDiedFirstTime", 0);

        if (hasDiedFirstTime == 0) {
            // Show tool tip
            if (undoTip == null && TipManagerScript.Instance) {
                undoTip = TipManagerScript.Instance.GetTip("UndoTip");
            }
            if (undoTip != null) {
                TipManagerScript.Instance.EnqueueTip(undoTip);
            }
            PlayerPrefs.SetInt("hasDiedFirstTime", 1);  
        }
    }
}
