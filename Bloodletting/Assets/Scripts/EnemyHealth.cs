using UnityEngine;

public class EnemyHealth : Health
{
    [Header("Health Stats")]
    [field: SerializeField] public float maxHealth { get; private set; } = 100f;
    [field: SerializeField] public float currentHealth { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0) {
            Die();
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
