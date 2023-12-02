
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public static PlayerScript Instance;
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected Transform destination;
    [SerializeField] protected LayerMask collisionMask;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    int frameCnt;

    int diagonalTolerance = 5; //in frames

    bool movingHori,movingVert;

    bool movementlockedIn;

    void Start() {
        destination.parent = null;
        movingHori = false;
        movingVert = false;
        movementlockedIn = false;
        targetPos = destination.position;
        frameCnt = diagonalTolerance;
    }

    Vector3 targetPos;
    public bool wallAtPos(Vector3 pos) {
        return Physics2D.OverlapCircle(pos, .1f, collisionMask);
    }
    void Update() {
        // Move Player to destination point
        transform.position = Vector3.MoveTowards(transform.position, destination.position, playerSpeed * Time.deltaTime);

        if ( Vector3.Distance(transform.position, destination.position) <= .05f || frameCnt<diagonalTolerance) {   // Check if Player is near destination or 
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {
                if (!wallAtPos(targetPos + new Vector3(Input.GetAxisRaw("Horizontal"),0,0)) && !movingHori) { // Check if new destination position will overlap with collisionMask
                    Vector3 horiDir =  new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    movingHori = true;
                    if (movingVert) {
                        targetPos += horiDir;
                        movementlockedIn = true;
                    } else {
                        targetPos = destination.position + horiDir;
                    }
                    frameCnt = 0;
                } 
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
                if (!wallAtPos(targetPos + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f)) && !movingVert) {
                    Vector3 vertDir = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    movingVert = true;
                    if (movingHori) {
                        targetPos += vertDir;
                        movementlockedIn = true;
                    } else {
                        targetPos = destination.position + vertDir;
                    }
                    frameCnt = 0;
                } 
            }
        }

        if (frameCnt>=diagonalTolerance || movementlockedIn) { // Player has diagonalTolerance # of frames to change their input to a diagonal
            if (movingHori || movingVert) {
                destination.position = targetPos;
                if (EnemyManagerScript.Instance!=null) {
                    EnemyManagerScript.Instance.EnemyTurn();
                }
                if (ProjectileManagerScript.Instance!=null) {
                    ProjectileManagerScript.Instance.ProjectileTurn();
                }
            }
            movingHori = false;
            movingVert = false;
            frameCnt = diagonalTolerance;
            movementlockedIn = false;
        } else {
            frameCnt++;
        }
    }

    

    // Move Point getter method
    public Transform getMovePoint() {
        return destination;
    }
}
