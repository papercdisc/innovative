using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public abstract void TakeDamage(float dmg);
    public abstract void Die();
}
