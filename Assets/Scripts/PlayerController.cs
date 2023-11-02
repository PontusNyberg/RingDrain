using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed;

    // For testing out enemy dmg
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemyDmgRadius;
    private float distance;
    private bool dmgInEffect;

    private InputManager input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    private SpriteRenderer rbSprite = null;
    private Vector2 spawnPoint;

    private void Awake() {
        Instance = this;

        moveSpeed = 3;
        input = new InputManager();
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        spawnPoint = rb.position;
        Debug.Log(rb.position);
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.IsCountdownToStartActive()) {
            moveVector = Vector2.zero;
            rb.velocity = Vector2.zero;
            rb.position = spawnPoint;
            Debug.Log("Player positions is reset");
        }
    }

    private void OnEnable() {
        input.Enable();
        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
    }

    private void OnDisable() {
        input.Disable();
        input.Player.Move.performed -= Move_performed;
        input.Player.Move.canceled -= Move_canceled;
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying()) {
            moveVector = Vector2.zero;
            return;
        }

        rb.velocity = moveVector * moveSpeed * 100f * Time.fixedDeltaTime;


        if (enemy) {
            distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < enemyDmgRadius && !dmgInEffect) {
                StartCoroutine(DamageEnemy());
            }
        }
    }

    private IEnumerator DamageEnemy() {
        if (distance < enemyDmgRadius) {
            enemy.GetComponent<Energy>().DamageUnit(50);
            dmgInEffect = true;
            yield return new WaitForSeconds(1);
        }
        dmgInEffect = false;
        yield return null;
    }

    private void Move_performed(InputAction.CallbackContext ctx) {
        moveVector = ctx.ReadValue<Vector2>().normalized;

        if (moveVector.x < 0) {
            rbSprite.flipX = true;
        } else if (moveVector.x > 0){
            rbSprite.flipX = false;
        }
    }

    private void Move_canceled(InputAction.CallbackContext ctx) {
        moveVector = Vector2.zero;
    }
}
