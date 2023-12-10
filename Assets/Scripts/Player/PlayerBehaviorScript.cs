using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBehaviorScript : MonoBehaviour {
    public static PlayerBehaviorScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;
    [SerializeField] protected Animator playerSpriteAnimator;
    [SerializeField] protected GameObject deadPlayer;
    [SerializeField] protected float multiInputWindow = 0.05f;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTiming = 0.4f;
    
    private float currentSpeed;

    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;

    private float lastInitialDirectionalInputTime;
    private bool playerInAction = false;
    private Vector3 currActionDir;
    private bool hasDashed = false;
    private bool isDead = false;

    protected Vector3 lastPos;

    protected string lastAction;

    protected GameObject deadSpr;


    // Animation state variables
    private string currentState;
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_MOVE = "PlayerMove";
    const string PLAYER_DASH = "PlayerDash";
    const string PLAYER_ATTACK = "PlayerAttack";
    const string PLAYER_DEFLECT = "PlayerDeflect";
    const string PLAYER_DIE = "PlayerDie";

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
    }

    void Start() {
        destination.parent = null;
        currentSpeed = playerSpeed;
        lastPos = transform.position;
        lastAction = PLAYER_IDLE;
    }
    public PlayerState GetPlayerState() {
        return new PlayerState(lastPos, lastAction);
    }
    void Update() {
        // Move Player to destination point after input window closes
        if (Time.time - lastInitialDirectionalInputTime >= multiInputWindow) {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, currentSpeed * Time.deltaTime);

            if (playerInAction) {
                if (hasDashed) {
                    ChangePlayerAnimationState(PLAYER_DASH);
                } else {
                    ChangePlayerAnimationState(PLAYER_MOVE);
                }
            }
        }
        
        processPlayerInput();
    }

    private void playerAttack(Vector3 enemyPosition) {
        ChangePlayerAnimationState(PLAYER_ATTACK);
        EnemyManagerScript.Instance.EnemyAttacked(enemyPosition, playerAttackDamage);
    }

    private void processPlayerInput() {
        // isHorizontalAxisInUse and isVerticalAxisInUse make GetAxisRaw behave like GetKeyDown instead of GetKey
        Vector3 currInputDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

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
                            destination.position += currInputDir - currActionDir; // for some reason, second input becomes (+-1, +-1, 0), so first input needs to be subtracted
                            currActionDir = currInputDir;
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
                        }
                    }
                }
            }
        }

        // Check if a second identical input has been inputted while player is mid move within the dash timing window
        // Dash can only be performed once per action
        if (playerInAction && Time.time - lastInitialDirectionalInputTime < dashTiming && !hasDashed) {
            // Check if player has pressed the attack button and is not a diagonal input
            if (Input.GetKey(KeyCode.Return) && !(Mathf.Abs(currActionDir.x) == 1f && Mathf.Abs(currActionDir.y) == 1f)) {
                playerAttack(destination.position);

                // undo motion
                destination.position -= currActionDir;
                playerInAction = false;

                processEnemyTurn();
            } 

            else if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;

                // Ensure player doesn't move into wall
                if (currInputDir == currActionDir) {
                    hasDashed = true;
                    if (!willHitWall(destination.position + currInputDir)) {
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
                processEnemyTurn();

                lastInitialDirectionalInputTime = 0f;
                playerInAction = false;
                ChangePlayerAnimationState(PLAYER_IDLE);
                currActionDir = Vector3.zero;
                hasDashed = false;
                currentSpeed = playerSpeed;
            }

            if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;
                lastInitialDirectionalInputTime = Time.time;
                currActionDir = currInputDir;

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
        transform.position = p.getPos();
        lastPos = p.getPos();
        destination.position = transform.position;
        isDead = false;
        if (deadSpr!=null) {
            deadSpr.SetActive(false);
        }
        ChangePlayerAnimationState(p.getAction());
    }

    private void processEnemyTurn() {
        if (GameStateManager.Instance!=null) {
            GameStateManager.Instance.captureGameState();
        }
        if (EnemyManagerScript.Instance != null) {
            EnemyManagerScript.Instance.EnemyTurn();
        }
        if (ProjectileManagerScript.Instance != null) {
            ProjectileManagerScript.Instance.ProjectileTurn();
        }
        lastPos = transform.position;
        lastAction = currentState;
    }

    /// <summary>
    /// Checks if a position is inside a wall
    /// </summary>
    /// <param name="position"></param>
    private bool willHitWall(Vector3 position) {
        return Physics2D.OverlapCircle(position, 0.1f, collisionMask);
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
        if (currentState == newState || isDead) return;
        playerSpriteAnimator.SetFloat("MovementX", currActionDir.x);
        playerSpriteAnimator.SetFloat("MovementY", currActionDir.y);
        playerSpriteAnimator.Play(newState);
        currentState = newState;
    }

    // Move Point getter method
    public Transform getMovePoint() {
        return destination;
    }

    public void killPlayer() {
        if (deadSpr==null) {
            deadSpr = Instantiate(deadPlayer, gameObject.transform.position, Quaternion.identity);
        }
        deadSpr.transform.position = transform.position;
        deadSpr.SetActive(true);
        //this.gameObject.SetActive(false);
        Debug.Log("Player died. Press 'Esc' to restart.");
    }
}