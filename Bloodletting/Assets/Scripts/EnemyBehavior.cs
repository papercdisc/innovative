using System.Collections;
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
    [SerializeField] float avoidanceDist = 4;

    [SerializeField] float damageAmount = 10f;
    [SerializeField] float damageInterval = 1f;
    Coroutine damageOverTimeCoroutine;

    float timeSinceLastDamage = 0f;

    [SerializeField] LayerMask mask;

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

        timeSinceLastDamage += Time.deltaTime;
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
                if (PlayerHealth.Instance.currentHealth >= PlayerHealth.Instance.maxHealth * 0.8f)
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

        //AvoidObstacles();
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

        rb.linearVelocity = direction * (moveSpeed);
    }

    void AvoidObstacles() // doesnt work :(
    {
        Vector2 direction = rb.linearVelocity.normalized;

        Ray2D ray = new Ray2D(transform.position, direction);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, avoidanceDist, mask);
        if (hit)
        {
            Vector2 perpRDir = Vector2.Perpendicular(direction);

            Ray2D perpR = new Ray2D(hit.point, perpRDir);
            Ray2D perpL = new Ray2D(hit.point, -perpRDir);

            Debug.DrawRay(hit.point, perpR.direction, Color.red);
            Debug.DrawRay(hit.point, perpL.direction, Color.green);

            // if ray hits something:
            RaycastHit2D hitR = Physics2D.Raycast(perpR.origin, perpR.direction, avoidanceDist);
            RaycastHit2D hitL = Physics2D.Raycast(perpL.origin, perpL.direction, avoidanceDist);


            Vector2 newDirection = direction;

            if (hitR)
            {
                if (hitL)
                {
                    // find which distance is longer
                    if (hitR.distance > hit.distance) // if right is longer
                    {
                        newDirection = perpRDir;
                    }
                    else // if left is longer
                    {
                        newDirection = -perpRDir;
                    }
                }
                else // r is shorter
                {
                    newDirection = -perpRDir;
                }
            }
            else if (hitL)
            {
                newDirection = perpRDir;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction);

        rb.linearVelocity = direction * (moveSpeed);
    }

    IEnumerator DamagePlayerOverTime(Collision2D collision)
    {
        while (collision.gameObject.GetComponent<PlayerHealth>() != null)
        { 
            timeSinceLastDamage = 0f;
            
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            yield return new WaitForSeconds(damageInterval); // Adjust the time interval as needed
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null )
        {
            if (timeSinceLastDamage >= damageInterval)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null)
        {
            if (damageOverTimeCoroutine != null)
                StopCoroutine(damageOverTimeCoroutine);
        }
    }
}
