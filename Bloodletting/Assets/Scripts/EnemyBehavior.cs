using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // References
    EnemyHealth enemyHealth;
    Rigidbody2D rb;

    [SerializeField] EnemyType enemyType;
    [SerializeField] EnemyState behaviorState;

    [SerializeField] float moveSpeed;
    [SerializeField] float aggroRadius;
    [SerializeField] float damageAmount;

    GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerHealth.Instance != null)
        {
            target = PlayerHealth.Instance.gameObject;
        }
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType) { 
            case EnemyType.Chaser:
                    ChaserUpdate();
                    break;
            case EnemyType.Coward:
                    CowardUpdate();
                    break;
            case EnemyType.Saboteur:
                    SaboteurUpdate();
                    break;
        }
    }

    void ChaserUpdate()
    {
        // move towards player if in aggro range
        if (Vector2.Distance(transform.position, target.transform.position) <= aggroRadius)
        {
            behaviorState = EnemyState.Aggro;
        }
        else
        {
            behaviorState = EnemyState.Idle;
        }
    }
    void CowardUpdate()
    {
        // kind of like the chaser, but runs away after hitting the player or if its health is low

        if (Vector2.Distance(transform.position, target.transform.position) <= aggroRadius)
        {
            if(enemyHealth.currentHealth <= enemyHealth.maxHealth / 2)
            {
                behaviorState = EnemyState.Flee;
            }
            else
            {
                behaviorState = EnemyState.Aggro;
            }
        }
        else
        {
            behaviorState = EnemyState.Idle;
        }
    }
    void SaboteurUpdate()
    {
        // runs towards player, but keeps distance if the player has high health/overhealth
        if (Vector2.Distance(transform.position, target.transform.position) <= aggroRadius)
        {
            if(PlayerHealth.Instance != null)
            {
                if (PlayerHealth.Instance.currentHealth >= PlayerHealth.Instance.maxHealth / 2)
                {
                    behaviorState = EnemyState.Flee;
                }

                else
                {
                    behaviorState = EnemyState.Aggro;
                }
            }
        }
        else
        {
            behaviorState = EnemyState.Idle;
        }
    }

    private void FixedUpdate()
    {
        if (behaviorState == EnemyState.Aggro)
        {
            RunToTarget();
        }
        else if (behaviorState == EnemyState.Flee)
        {
            RunFromTarget();
        }
    }

    private void RunToTarget()
    {
        // move towards player
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
    private void RunFromTarget()
    {
        Vector2 direction = (transform.position - target.transform.position).normalized;

        Ray2D ray = new Ray2D(transform.position, direction);
        if (Physics2D.Raycast(ray.origin, ray.direction, aggroRadius))
        {
            // if there's an obstacle in the way, try to move in a different direction
            Vector2 perpendicularDirection = new Vector2(-direction.y, direction.x);
            if (!Physics2D.Raycast(transform.position, perpendicularDirection, aggroRadius))
            {
                direction = perpendicularDirection;
            }
            else if (!Physics2D.Raycast(transform.position, -perpendicularDirection, aggroRadius))
            {
                direction = -perpendicularDirection;
            }
        }

        rb.linearVelocity = direction * (moveSpeed * 0.75f);
    }
}
