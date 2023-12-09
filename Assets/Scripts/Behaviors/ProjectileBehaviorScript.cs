using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : MonoBehaviour {
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileDestination;
    [SerializeField] protected LayerMask collisionMask;
    
    protected Transform playerPosition;
    private int direction = (int) PlayerBehaviorScript.Direction.Up;
    private bool isProjectileMoving;
    private GameObject projectileMovePoint;
    
    // Start is called before the first frame update
    void Start() {
        this.projectileMovePoint = projectileDestination.gameObject;
        playerPosition = PlayerBehaviorScript.Instance.getMovePoint();
        projectileDestination.parent = null;

        // create spawn command
        List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>();
        commandList.Add(new ProjectileSpawnCommand(this.gameObject, this.projectileMovePoint));
        CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);

        // will destroy the projectile by default after 20 seconds
        // Destroy(this.gameObject, 20);
        // Destroy(this.projectileMovePoint, 20);
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime);

        List<CommandManager.ICommand> commandList = new List<CommandManager.ICommand>();

        if (isProjectileMoving && Vector3.Distance(transform.position, projectileDestination.position) <= Mathf.Epsilon) {
            isProjectileMoving = false;
            commandList.Add(new ProjectileMoveCommand(getMovementDirection(), Vector3.zero, this));
        }

        // Check if projectile position will overlap with collisionMask
        if (Physics2D.OverlapCircle(transform.position, .1f, collisionMask)) {
            DestroyProjectile();
            commandList.Add(new ProjectileDeathCommand(projectileDestination.position, Vector3.zero, projectilePrefab));
        }

        // Check if projectile position touches Player
        if (Vector3.Distance(transform.position, playerPosition.position) < .05f) {
            DestroyProjectile();
            commandList.Add(new ProjectileDeathCommand(projectileDestination.position, Vector3.zero, projectilePrefab));
            PlayerBehaviorScript.Instance.killPlayer();
        }

        // only add commands if we have commands to add
        if (commandList.Count > 0) {
            CommandManager.Instance.AddCommand(this.GetInstanceID(), commandList);
        }
    }

    public void SetPrefab(GameObject projectilePrefab) {
        this.projectilePrefab = projectilePrefab;
    }

    private void DestroyProjectile() {
        Destroy(this.gameObject);
        Destroy(this.projectileMovePoint);
    }

    public virtual void MoveDestination(Vector3 direction) {
        this.projectileDestination.position += direction;
        isProjectileMoving = true;
    }

    private Vector3 getMovementDirection() {
        switch (direction) {
            case (int) PlayerBehaviorScript.Direction.Up:
                return new Vector3(0f, projectileDistance, 0f);
            case (int) PlayerBehaviorScript.Direction.Down:
                return new Vector3(0f, -projectileDistance, 0f);
            case (int) PlayerBehaviorScript.Direction.Left:
                return new Vector3(-projectileDistance, 0f, 0f);
            case (int) PlayerBehaviorScript.Direction.Right:
                return new Vector3(projectileDistance, 0f, 0f);
            default:
                return Vector3.zero;
        }
    }

    public void ProjectileMove() {
        projectileDestination.position += getMovementDirection();
        isProjectileMoving = true;
    }

    public bool getIsProjectileMoving() {
        return isProjectileMoving;
    }

    public void setDirection(int newDirection) {
        this.direction = newDirection;
    }
}
