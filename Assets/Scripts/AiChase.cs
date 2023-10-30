using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float distanceForChase;
    [SerializeField] private ContactFilter2D movementFilter;
    [SerializeField] private float playerDmgRadius;

    private Rigidbody2D rb;
    private float distance;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private int damageFrameCounter;
    private bool dmgInEffect;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        distanceForChase = 12f;
        playerDmgRadius = 2f;
    }


    private void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying())
            return;

        distance = Vector2.Distance(transform.position, player.transform.position);


        if(distance < playerDmgRadius && !dmgInEffect) {
            StartCoroutine(DamagePlayer());
        }

        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        bool moveSucceded = MoveAi(direction);

        if (!moveSucceded) {
            // Try left / right movement
            moveSucceded = MoveAi(new Vector2(direction.x, 0));

            if (!moveSucceded) {
                moveSucceded = MoveAi(new Vector2(0, direction.y));
            }
        }
    }

    private IEnumerator DamagePlayer() {
        if (distance < playerDmgRadius) {
            player.GetComponent<Health>().DamagePlayer(50);
            dmgInEffect = true;
            yield return new WaitForSeconds(1);
        }
        dmgInEffect = false;
        yield return null;
    }

    private bool MoveAi(Vector2 direction) {
        // Find potential collisions
        int count = rb.Cast(
            direction,
            movementFilter, // Filters where collisions might occur
            castCollisions, // Store collisions after the cast has finished
            speed * Time.fixedDeltaTime
        );

        if (count == 0) {
            // No collisions, free to move
            if (distance < distanceForChase) {
                Vector2 newPosition = rb.position + (direction * speed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);

                return true;
            }

            // Outside range, do nothing
            return false;
        } else {
            // Collisions detected, do nothing
            return false;
        }
    }

}
