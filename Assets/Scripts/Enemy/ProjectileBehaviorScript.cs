using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviorScript : StateEntity {
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileDistance;
    protected Transform projectileDestination;
    [SerializeField] protected LayerMask collisionMask;
    protected Transform playerPosition;
    private Vector3 direction;
    private bool isProjectileMoving;
    private GameObject projectileMovePoint;
    [SerializeField] protected SpriteRenderer projectileSpriteRenderer;
    [SerializeField] protected Sprite deflectedProjectileSprite;

    protected bool hitsEnemies;

    // Start is called before the first frame update
    public virtual void Start() {
        if (!createdAssociates) {
            CreateAssociates();
        }
    }

    public void setHitsEnemies(bool b) {
        hitsEnemies = b;
    }

    public bool getHitsEnemies() {
        return hitsEnemies;
    }

    public void copyDir(ProjectileBehaviorScript p) {
        p.setDirection(direction);
        p.setHitsEnemies(hitsEnemies);
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
        playerPosition = PlayerBehaviorScript.Instance.transform;
    }

    public override void DestroyAssociates() { 

        Destroy(projectileDestination.gameObject);
    }

    void Update() {
        isProjectileMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, projectileDestination.position, projectileSpeed * Time.deltaTime * SongTransitionerController.Instance.getBpmSpeedMultiplier());

        if (Vector3.Distance(transform.position, projectileDestination.position) <= Mathf.Epsilon) {
            isProjectileMoving = false;
        }

        // Check if projectile position will overlap with collisionMask
        if (Physics2D.OverlapCircle(transform.position, .1f, collisionMask)) {
            DestroyProjectile();
        }

        // Check if projectile position touches Player
        if (!hitsEnemies && Vector3.Distance(transform.position, playerPosition.position) < .05f) {
            DestroyProjectile();
            PlayerBehaviorScript.Instance.killPlayer();
        }

        // Check if projectile hits an Enemy
        if (hitsEnemies && EnemyManagerScript.Instance.EnemyAttacked(transform.position, 999, 0.1f)) {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile() {
        Destroy(this.projectileMovePoint);
        Destroy(this.gameObject);
    }

    public void ProjectileMove() {
        //Debug.Log("PROJ MOVE");
        projectileDestination.position += projectileDistance * direction;
    }

    public void projectileReflected(Vector3 newDir, bool hitFlag) {
        if (!hitFlag) {
            setHitsEnemies(true);
            this.setDirection(newDir);
            projectileSpriteRenderer.sprite = deflectedProjectileSprite;
        } else {
            DestroyProjectile();
        }
    }

    public bool getIsProjectileMoving() {
        return isProjectileMoving;
    }

    public void setDirection(Vector3 newDirection) {
        transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(newDirection, new Vector3(0, 1, 0), new Vector3(0, 0, -1)));
        this.direction = newDirection;
    }
}
