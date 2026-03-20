using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    // on spawn, shoot out and slowly decay speed
    // when speed is under a certain threshold, signal it will explode, then explode after a short delay
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D bombCollider;

    [Header("Movement Settings")]
    public Vector2 velocity;
    public float initialSpeed;
    private float currentSpeed;
    public float decayRate;

    [Header("Explosion Settings")]
    public float explodeThreshold;
    public float explodeDelay;

    float colEnableBuffer = 0.08f;

    [SerializeField] GameObject explosionChild;

    void Start()
    {
        bombCollider.enabled = false;

        currentSpeed = initialSpeed;
        rb.linearVelocity = velocity.normalized * currentSpeed;

        if (explosionChild != null)
        {
            explosionChild.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        colEnableBuffer -= Time.fixedDeltaTime;
        if (colEnableBuffer <= 0 && !bombCollider.enabled)
        {
            bombCollider.enabled = true;
        }

        if (currentSpeed > 0)
        {
            currentSpeed -= decayRate * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = 0;
        }

        rb.linearVelocity = velocity.normalized * currentSpeed;

        if (rb.linearVelocity.magnitude <= explodeThreshold && explosionChild != null && !explosionChild.activeInHierarchy)
        {
            explosionChild.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyBehavior>()) return;

        if (collision.gameObject != null)
        {
            velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
            currentSpeed += 1f;
        }
    }
}
