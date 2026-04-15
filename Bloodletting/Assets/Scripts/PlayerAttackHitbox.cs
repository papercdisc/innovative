using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private Collider2D hb;

    private void Start()
    {
        hb = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<EnemyHealth>() != null)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
}
