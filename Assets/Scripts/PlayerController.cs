using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.CanvasScaler;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed;

    [SerializeField] private LaserController laserPrefab;

    [SerializeField] private Transform atkRadiusUI;
    [SerializeField] private float enemyDmgRadius = 5f;
    private float distance;
    private bool dmgInEffect;
    private FindClosest findClosestEnemy;

    private InputManager input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    private SpriteRenderer rbSprite = null;
    private Vector2 spawnPoint;

    private LaserController activeLaser;

    private void Awake() {
        Instance = this;

        moveSpeed = 3;
        input = new InputManager();
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
        spawnPoint = rb.position;
        findClosestEnemy = GetComponent<FindClosest>();

        atkRadiusUI.localScale = new Vector3(enemyDmgRadius, enemyDmgRadius, 0f);
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsGameOver()) { 
            MonstersSpawnerControl.spawnAllowed = false;
        }
        if (GameManager.Instance.IsCountdownToStartActive()) {
            moveVector = Vector2.zero;
            rb.velocity = Vector2.zero;
            rb.position = spawnPoint;
            MonstersSpawnerControl.spawnAllowed = true;
            Debug.Log("Player positions is reset");
        }
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying()) {
            moveVector = Vector2.zero;
            return;
        }

        rb.velocity = moveVector * moveSpeed * 100f * Time.fixedDeltaTime;

        AiChase enemy = findClosestEnemy.closestEnemy;
        if (enemy) {
            distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < enemyDmgRadius && !dmgInEffect) {
                activeLaser?.gameObject.SetActive(false);
                StartCoroutine(DamageEnemy(enemy));
            }
        }

        if(activeLaser) {
            activeLaser.lineRenderer.SetPosition(0, rb.position);
            if (enemy && enemy.unitEnergy.curEnergy <= 0f) {
                activeLaser.gameObject.SetActive(false);
                activeLaser = null;
            }
        }
    }

    private IEnumerator DamageEnemy(AiChase enemy) {
        if (distance < enemyDmgRadius) {
            LaserController newLaser = Instantiate(laserPrefab);
            newLaser.AssignTarget(rb.position, enemy.transform);
            activeLaser = newLaser;
            newLaser.gameObject.SetActive(true);

            enemy.GetComponent<Energy>().DamageUnit(50);
            dmgInEffect = true;
            yield return new WaitForSeconds(0.7f);
        }
        dmgInEffect = false;
        yield return null;
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
