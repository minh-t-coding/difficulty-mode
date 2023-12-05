using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;
    [SerializeField] protected float multiInputWindow = 0.05f;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashTiming = 0.4f;
    
    private float currentSpeed;

    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;

    private float lastInitialDirectionalInputTime;
    private bool playerInAction = false;
    private Vector3 currMoveDir;
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
        // Move Player to destination point after input window closes
        if (Time.time - lastInitialDirectionalInputTime >= multiInputWindow) {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, currentSpeed * Time.deltaTime);
        }

        Vector3 currInputDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

        // isHorizontalAxisInUse and isVerticalAxisInUse make GetAxisRaw behave like GetKeyDown instead of GetKey

        // Check if an input for a diagonal move was made
        // This is to add leniency to making a diagonal move so it doesn't have to be frame-perfect
        if (Time.time - lastInitialDirectionalInputTime < multiInputWindow) {
            if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;
                if(Mathf.Abs(currInputDir.x) == 1f && Mathf.Abs(currInputDir.y) == 1f) {
                    if (playerInAction) {   
                        // if original action was valid and had already been initiated
                        if (!willHitWall(destination.position + currInputDir - currMoveDir)) {
                            destination.position += currInputDir - currMoveDir; // for some reason, second input becomes (+-1, +-1, 0), so first input needs to be subtracted
                            currMoveDir = currInputDir;
                        } else {
                            destination.position -= currMoveDir;    // undo initiated action if new destination is invalid
                        }
                    } else {    
                        // if original action was invalid and had never started
                        if (!willHitWall(destination.position + currInputDir)) {
                            destination.position += currInputDir;
                            currMoveDir = currInputDir;
                            playerInAction = true;
                        }
                    }
                }
            }
        }

        // Check if a second identical input has been inputted while player is mid move within the dash timing window
        // Dash can only be performed once per action
        if (playerInAction && Time.time - lastInitialDirectionalInputTime < dashTiming && !hasDashed) {
            if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;

                // Ensure player doesn't move into wall
                if (currInputDir == currMoveDir) {
                    hasDashed = true;
                    if (!willHitWall(destination.position + currInputDir)) {
                        destination.position += currInputDir;
                        currentSpeed = dashSpeed;
                    }
                }
            }
        }

        // Check for new input if Player is close enough to destination
        if (Vector3.Distance(transform.position, destination.position) <= .05f) {
            // Player character has finished performing action
            // NPCs take turns, reset everything to listen for new move input
            if (playerInAction) {
                if (EnemyManagerScript.Instance!=null) {
                    EnemyManagerScript.Instance.EnemyTurn();
                }
                if (ProjectileManagerScript.Instance!=null) {
                    ProjectileManagerScript.Instance.ProjectileTurn();
                }

                lastInitialDirectionalInputTime = 0f;
                playerInAction = false;
                currMoveDir = Vector3.zero;
                hasDashed = false;
                currentSpeed = playerSpeed;
            }

            if (newInputReceived(currInputDir)) {
                isHorizontalAxisInUse = Mathf.Abs(currInputDir.x) == 1f;
                isVerticalAxisInUse = Mathf.Abs(currInputDir.y) == 1f;
                lastInitialDirectionalInputTime = Time.time;
                currMoveDir = currInputDir;

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

    // Move Point getter method
    public Transform getMovePoint() {
        return destination;
    }
}
