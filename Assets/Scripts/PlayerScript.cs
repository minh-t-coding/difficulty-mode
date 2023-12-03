using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTiming = 0.4f;
    
    private float currentSpeed;

    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;

    private float lastInitialDirectionalInputTime;
    private bool playerInAction = false;
    private Vector3 currentMoveDir;
    private bool hasDashed = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        destination.parent = null;
        currentSpeed = playerSpeed;
    }

    void Update() {
        // Move Player to destination point
        transform.position = Vector3.MoveTowards(transform.position, destination.position, currentSpeed * Time.deltaTime);

        // isHorizontalAxisInUse and isVerticalAxisInUse make GetAxisRaw behave like GetKey instead of GetKeyDown

        // Check if a second input has been inputted while player is mid move within the dash timing window
        // Dash can only be performed once per action
        if (playerInAction && Time.time - lastInitialDirectionalInputTime < dashTiming && !hasDashed) {
            if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && !isHorizontalAxisInUse) || (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && !isVerticalAxisInUse)) {
                isHorizontalAxisInUse = Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f;
                isVerticalAxisInUse = Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f;
                Vector3 dashDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
                if (dashDir == currentMoveDir) {
                    hasDashed = true;
                    if (!Physics2D.OverlapCircle(destination.position + dashDir, 0.1f, collisionMask)) {
                        Debug.Log("dashing! " + currentMoveDir);
                        destination.position += dashDir;
                        currentSpeed = dashSpeed;
                    }
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0) {
                isHorizontalAxisInUse = false;
            }
            if (Input.GetAxisRaw("Vertical") == 0) {
                isVerticalAxisInUse = false;
            }
        }

        // Check for new input if Player is close enough to destination
        if (Vector3.Distance(transform.position, destination.position) <= .05f) {
            // Player character has finished performing action
            // NPCs take turns, reset everything to listen for new move input
            if (playerInAction) {
                Debug.Log("arrived!");

                EnemyManagerScript.Instance.GetComponent<EnemyManagerScript>().EnemyTurn();
                ProjectileManagerScript.Instance.GetComponent<ProjectileManagerScript>().ProjectileTurn();

                lastInitialDirectionalInputTime = 0f;
                Debug.Log("lastInitialDirectionalIinputTime: " + lastInitialDirectionalInputTime);
                playerInAction = false;
                currentMoveDir = Vector3.zero;
                hasDashed = false;
                currentSpeed = playerSpeed;
            }

            if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && !isHorizontalAxisInUse) || (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && !isVerticalAxisInUse)) {
                isHorizontalAxisInUse = Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f;
                isVerticalAxisInUse = Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f;
                Vector3 moveDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
                // Ensure player doesn't move into wall
                if (!Physics2D.OverlapCircle(destination.position + moveDir, 0.1f, collisionMask)) {
                    destination.position += moveDir;
                    lastInitialDirectionalInputTime = Time.time;
                    Debug.Log("lastInitialDirectionalIinputTime: " + lastInitialDirectionalInputTime);
                    playerInAction = true;
                    currentMoveDir = moveDir;
                }
            }
            if (Input.GetAxisRaw("Horizontal") == 0) {
                isHorizontalAxisInUse = false;
            }
            if (Input.GetAxisRaw("Vertical") == 0) {
                isVerticalAxisInUse = false;
            }
        }

    }

    // Move Point getter method
    public Transform getMovePoint() {
        return destination;
    }
}
