using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private float delta;
    private float healthValue;
    private float currentHealth;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        healthValue = player.Health.CurrentHealth / 100.0f;
    }

    private void Update()
    {
        currentHealth = player.Health.CurrentHealth / 100.0f;
        if (currentHealth > healthValue)
            healthValue += delta;
        if(currentHealth < healthValue)
            healthValue -= delta;
        if (currentHealth < delta)
            healthValue = player.Health.CurrentHealth;

        health.fillAmount = healthValue;
    }
}
