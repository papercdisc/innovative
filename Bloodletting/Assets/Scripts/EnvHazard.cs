using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnvHazard : MonoBehaviour
{
    // may change to scriptable object reference later
    [Header("Damage Settings")]
    public float damageAmount = 5f;

    public float baseDamage = 5f;
    public float damageMultiplier = 1.1f;

    public float damageInterval = 1f;
    Coroutine damageOverTimeCoroutine;

    [Header("Movement Settings")]
    public bool isMoveable = false;
    public float moveSpeed = 2f;
    public List<Vector2> movePoints;
    int currentMovePointIndex = 0;

    public bool canSpin = false;
    public float spinSpeed = 100f;

    private void Start()
    {
        if (movePoints.Count > 0)
        {
            transform.position = movePoints[0];
        }
    }

    private void FixedUpdate()
    {
        MoveHazard();
        SpinHazard();
    }

    IEnumerator DamageOverTime(Collider2D collision)
    {
        while (collision.GetComponent<PlayerHealth>() != null)
        {
            damageAmount *= damageMultiplier; // Increase damage over time

            collision.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval); // Adjust the time interval as needed
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>() != null)
        {
            damageAmount = baseDamage;
            damageOverTimeCoroutine = StartCoroutine(DamageOverTime(collision));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>() != null && damageOverTimeCoroutine != null)
        {
            StopCoroutine(damageOverTimeCoroutine);
            damageOverTimeCoroutine = null;
        }
    }

    private void MoveHazard()
    {
        if (isMoveable)
        {
            if (currentMovePointIndex >= movePoints.Count)
            {
                currentMovePointIndex = 0;
            }

            this.transform.position = Vector2.MoveTowards(this.transform.position, movePoints[currentMovePointIndex], Time.fixedDeltaTime * moveSpeed);
            if (Vector2.Distance(this.transform.position, movePoints[currentMovePointIndex]) < 0.1f)
            {
                currentMovePointIndex++;
            }
        }
    }
    private void SpinHazard()
    {
        if (canSpin)
        {
            this.transform.Rotate(Vector3.forward * spinSpeed * Time.fixedDeltaTime);
        }
    }
}
