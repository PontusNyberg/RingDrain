using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public static HealthBar Instance { get; private set; }

    public Slider healthBar;
    public Health playerHealth;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.maxHealth;
    }

    public void SetHealth(int hp) {
        healthBar.value = hp;
    }

    public void ResetHealthBar() {
        playerHealth.ResetHealth();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.maxHealth;
    }
}