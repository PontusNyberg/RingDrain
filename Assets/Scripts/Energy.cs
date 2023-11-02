using System;
using System.Collections.Generic;
using UnityEngine;
public class Energy : MonoBehaviour {
    public int curEnergy = 0;
    public int maxEnergy = 100;
    public Transform unit;

    public event EventHandler OnEnergyEmpty;

    private void Start() {
        curEnergy = maxEnergy;
    }

    public void DamageUnit(int damage) {
        curEnergy -= damage;
        unit.localScale -= unit.localScale * (float) damage / 100;
        Debug.Log(unit.localScale * (float) damage / 100);

        if(curEnergy <= 0f || unit.localScale.x < 0.3) {
            Debug.Log("Enemy is dead");
            OnEnergyEmpty?.Invoke(this, new EventArgs());
        }
    }

    public void ResetHealth() {
        curEnergy = maxEnergy;
    }
}