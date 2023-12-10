using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : StateEntity {
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    protected Transform projectileDestination;
    [SerializeField] protected LayerMask collisionMask;
    protected Transform playerPosition;
    private int direction;
    private bool isProjectileMoving;
    private GameObject projectileMovePoint;
    private Dictionary<int, int> opposites = new Dictionary<int, int> {
        { (int) PlayerBehaviorScript.Direction.Up, (int) PlayerBehaviorScript.Direction.Down },
        { (int) PlayerBehaviorScript.Direction.Down, (int) PlayerBehaviorScript.Direction.Up },
        { (int) PlayerBehaviorScript.Direction.Left, (int) PlayerBehaviorScript.Direction.Right },
        { (int) PlayerBehaviorScript.Direction.Right, (int) PlayerBehaviorScript.Direction.Left }
    };

    // Start is called before the first frame update
    public virtual void Start() {
        if (!createdAssociates) {
            CreateAssociates();
        }
    }

    public void copyDir(ProjectileBehaviorScript p) {
        p.setDirection(direction);
    }

    public override void OnStateLoad() {
        createdAssociates = true;
        CreateAssociates();
    }

    public override void CreateAssociates() {
        transform.parent = ProjectileManagerScript.Instance.transform;
        projectileDestination = new GameObject("MovePoint"+gameObject.name).transform;
        projectileDestination.parent = null;
        projectileDestination.position = transform.position;
        //myActionIndicator = ActionIndicator.Create(transform);
        playerPosition = PlayerBehaviorScript.Instance.getMovePoint();
    }

    public override void DestroyAssociates() { 

        Destroy(projectileDestination.gameObject);
    }

    void Update() {
        isProjectileMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, projectileDestination.position) <= Mathf.Epsilon) {
            isProjectileMoving = false;
        }

        // Check if projectile position will overlap with collisionMask
        if (Physics2D.OverlapCircle(transform.position, .1f, collisionMask)) {
            DestroyProjectile();
        }

        // Check if projectile position touches Player
        if (Vector3.Distance(transform.position, playerPosition.position) < .05f) {
            DestroyProjectile();
            PlayerBehaviorScript.Instance.killPlayer();
        }
    }

    private void DestroyProjectile() {
        Destroy(this.gameObject);
        Destroy(this.projectileMovePoint);
    }

    public void ProjectileMove() {
        Debug.Log("PROJ MOVE");
        switch (direction) {
            case (int) PlayerBehaviorScript.Direction.Up:
                projectileDestination.position += new Vector3(0f, projectileDistance, 0f);
                break;
            case (int) PlayerBehaviorScript.Direction.Down:
                projectileDestination.position += new Vector3(0f, -projectileDistance, 0f);
                break;
            case (int) PlayerBehaviorScript.Direction.Left:
                projectileDestination.position += new Vector3(-projectileDistance, 0f, 0f);
                break;
            case (int) PlayerBehaviorScript.Direction.Right:
                projectileDestination.position += new Vector3(projectileDistance, 0f, 0f);
                break;
        }
    }

    public void projectileReflected() {
        Debug.Log("projectile reflected");
        this.setDirection(opposites[direction]);
    }

    public bool getIsProjectileMoving() {
        return isProjectileMoving;
    }

    public void setDirection(int newDirection) {
        this.direction = newDirection;
    }
}
