using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosest : MonoBehaviour {
    public AiChase closestEnemy;

    private void Update() {
        FindClosestEnemy();
    }

    private void FindClosestEnemy() {
        float distanceToClosestEnemy = Mathf.Infinity;
        AiChase[] allEnemies = GameObject.FindObjectsOfType<AiChase>();

        for(int i = 0; i < allEnemies.Length; i++) {
            if (allEnemies[i] != null) {
                float distanceToEnemy = (allEnemies[i].transform.position - this.transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestEnemy) {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = allEnemies[i];
                }
            }
        }

        if (closestEnemy != null) {
            Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
        }
    }
}
