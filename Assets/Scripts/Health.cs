using System;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour {
    public int curHealth = 0;
    public int maxHealth = 100;
    public HealthBar healthBar;

    public event EventHandler OnHealthEmpty;

    private void Start() {
        curHealth = maxHealth;
    }

    public void DamagePlayer(int damage) {
        curHealth -= damage;
        healthBar.SetHealth(curHealth);

        if(curHealth <= 0f) {
            OnHealthEmpty?.Invoke(this, new EventArgs());
        }
    }

    public void ResetHealth() {
        curHealth = maxHealth;
        healthBar.SetHealth(curHealth);
    }
}