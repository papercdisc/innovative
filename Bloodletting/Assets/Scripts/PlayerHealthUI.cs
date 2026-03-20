using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider overhealthBar;
    [SerializeField] private Slider overhealthTimer;

    private PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = PlayerHealth.Instance;

        if (playerHealth != null)
        {
            // set max values
            healthBar.maxValue = playerHealth.maxHealth;
            overhealthBar.maxValue = playerHealth.maxHealth; // might change later, just for current UI configuration
            overhealthTimer.maxValue = playerHealth.overhealthSafeTime;

            // set initial values
            healthBar.value = playerHealth.currentHealth;
            overhealthBar.value = playerHealth.currentOverhealth;

            if (playerHealth.currentOverhealth > 0)
            {
                overhealthBar.gameObject.SetActive(true);
            }
            else
            {
                overhealthBar.gameObject.SetActive(false);
            }

            playerHealth.OnHealthChanged.AddListener(UpdateHealthBar);
            playerHealth.OnOverhealthChanged.AddListener(UpdateOverhealthBar);
        }
    }

    private void Update()
    {
        UpdateOverhealthTimer();
    }

    public void UpdateHealthBar(float newValue)
    {
        healthBar.value = newValue;
    }
    public void UpdateOverhealthBar(float newValue)
    {
        if (newValue > 0)
        {
            overhealthBar.value = newValue;
            overhealthBar.gameObject.SetActive(true);
        }
        else
        {
            overhealthBar.value = 0;
            overhealthBar.gameObject.SetActive(false);
        }
    }
    public void UpdateOverhealthTimer()
    {
        if (playerHealth.currentOverhealth > 0)
        {
            overhealthTimer.gameObject.SetActive(true);
            overhealthTimer.value = playerHealth.activeOverhealthSafeTime;
        }
        else
        {
            overhealthTimer.gameObject.SetActive(false);
        }
    }
}
