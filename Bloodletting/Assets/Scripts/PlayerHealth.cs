using UnityEngine;

/// <summary>
/// We're going to opperate under the assumption that this game is in real time, so NOT turn based. 
/// Just for adaptability, real time health functions will have the prefix "RT", and turn based health functions will have the prefix "TB".
/// </summary>
public class PlayerHealth : Health
{
    [Header("Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Overhealth")]
    [SerializeField] private float currentOverhealth; // health above max health
    [SerializeField] private float overhealthSafeTime; // amount of time it is safe to have overhealth before exploding (DO NOT CHANGE DURING RUNTIME)
    [SerializeField] private float activeOverhealthSafeTime;

    [Header("Health Regeneration")]
    [SerializeField] private float regenRate = 5f; // Health points regenerated per second (DO NOT CHANGE DURING RUNTIME)
    [SerializeField] private float regenBufferTime = 3f;
    [SerializeField] private float activeRegenBuffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RTHealthRegen();
        HandleOverhealth();
    }

    public void RTHealthRegen() // constant regen after not taking damage
    {
        if (activeRegenBuffer > 0)
        {
            activeRegenBuffer -= Time.deltaTime;
            return;
        }
        
        if (currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
        else
        {
            currentHealth = maxHealth; // Ensure health does not exceed max health
            currentOverhealth += regenRate * Time.deltaTime; // Regenerate overhealth
        }

        if (activeRegenBuffer != regenBufferTime)
        activeRegenBuffer = 3f; // Reset the buffer time after regeneration
    }

    public void HandleOverhealth()
    {
        if (currentOverhealth > 0)
        {
            activeOverhealthSafeTime -= Time.deltaTime;
            if (activeOverhealthSafeTime <= 0)
            {
                Die(); // Player dies if overhealth is not reduced in time
            }
        }
        else activeOverhealthSafeTime = overhealthSafeTime; // Reset the safe time when overhealth is reduced to 0
    }

    public override void TakeDamage(float dmg)
    {
        // Implement damage logic here
    }
    public override void Die()
    {
        // Implement death logic here
        Debug.Log("Player has died.");
    }
}
