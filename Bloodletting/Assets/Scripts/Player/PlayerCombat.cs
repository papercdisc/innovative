using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerInputSubscription getInput;
    bool abilityInput;

    [SerializeField] PlayerAbility equippedAbility;

    bool usedAbility = false;

    [SerializeField] GameObject bombPrefab;

    [SerializeField] float abilityCooldown = 2f;
    [SerializeField] float currentCooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerInputSubscription.Instance != null && getInput == null)
        {
            getInput = PlayerInputSubscription.Instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        abilityInput = getInput.AltAttackInput;

        if (!abilityInput)
        {
            usedAbility = false;
        }

        if (abilityInput && !usedAbility)
        {
            usedAbility = true;

            switch (equippedAbility)
            {
                case PlayerAbility.Bomb:
                    ExecuteBombAbility();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }
    }

    private void ExecuteBombAbility()
    {
        //Debug.Log("Bomb ability executed!");
        if (currentCooldown > 0)
        {
            //Debug.Log("Ability is on cooldown!");
            return;
        }


        Vector2 lookPosition = getInput.LookInput;
        Vector2 direction = (lookPosition - (Vector2)transform.position).normalized;

        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<BombBehavior>().velocity = direction; // Example velocity, you can set this based on player direction or input

        currentCooldown = abilityCooldown;
    }
}
