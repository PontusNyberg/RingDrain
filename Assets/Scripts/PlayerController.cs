using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed;

    private InputManager input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    private SpriteRenderer rbSprite = null;

    private void Awake() {        
        moveSpeed = 3;
        input = new InputManager();
        rb = GetComponent<Rigidbody2D>();
        rbSprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        input.Enable();
        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
    }

    private void FixedUpdate() {
        rb.velocity = moveVector * moveSpeed * 100f * Time.fixedDeltaTime;
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

    private void OnDisable() {
        input.Disable();
        input.Player.Move.performed -= Move_performed;
        input.Player.Move.canceled -= Move_canceled;
    }
}
