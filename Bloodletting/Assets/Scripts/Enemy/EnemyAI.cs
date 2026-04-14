using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private EnemyType enemyType;

    #region Steering Parameters
    [Header("Steering")]
    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private List<SteeringBehavior> steeringBehaviors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private ContextSolver moveDirSolver;
    #endregion

    [Header("Behavior Settings")]
    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDist = 0.5f;

    [SerializeField]
    EnemyState currentEnemyState;

    [SerializeField]
    private Vector2 movementInput;

    [Header("Events")]
    public UnityEvent<Transform> onAttack;
    public UnityEvent<Vector2> onMove;

    void Start()
    {
        InvokeRepeating("PerformDetection", 0f, detectionDelay); // more optimized for performance apparently, as it doesn't run this every frame
    }

    private void PerformDetection()
    {
        bool playerIsDanger = false; // just keeping it false for now. this will change with the implementation of more specific behaviors.
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData, playerIsDanger);
        }

        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehavior steeringBehavior in steeringBehaviors)
        {
            (danger, interest) = steeringBehavior.GetSteering(danger, interest, aiData);
        }
    }

    private void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Chaser:
                UpdateChaser();
                break;
            case EnemyType.Coward:
                UpdateCoward();
                break;
            case EnemyType.Saboteur:
                UpdateSaboteur();
                break;
        }
    }

    // Enemy type-specific update functions
    private void UpdateChaser() // most basic enemy. just chases player in range.
    {
        if (aiData.currentTarget != null) // if current target is in range, chase
        {
            if (currentEnemyState == EnemyState.Idle)
            {
                currentEnemyState = EnemyState.Aggro;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if(aiData.GetTargetsCount() > 0) // if there are valid targets in the list, find another
        {
            // choose closest target in list as current target
            aiData.currentTarget = aiData.targets[0];
        }

        onMove?.Invoke(movementInput);
    }
    private void UpdateCoward() // runs away from player if low health, otherwise chases player in range
    {
    }
    private void UpdateSaboteur() // chases player, but runs away if player has high health/overhealth
    {

    }

    private IEnumerator ChaseAndAttack()
    {
        if(aiData.currentTarget == null)
        {
            // stopping logic, return to idle
            movementInput = Vector2.zero;
            currentEnemyState = EnemyState.Idle;
            yield break;
        }
        else
        {
            float dist = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (dist < attackDist) // attack if in range
            {
                // attack logic
                movementInput = Vector2.zero;
                onAttack?.Invoke(aiData.currentTarget);
                yield return new WaitForSeconds(attackDelay); // wait to attack again
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                // chase logic
                movementInput = moveDirSolver.GetDirectionToMove(steeringBehaviors, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    public void OnDeath()
    {
        CancelInvoke("PerformDetection");
        StopAllCoroutines();
    }
}
