using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private InputManager input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;

    private void Awake() {
        moveSpeed = 500f;
        rotateSpeed = 7f;
        input = new InputManager();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        input.Enable();
        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
    }

    private void FixedUpdate() {
        rb.velocity = moveVector * moveSpeed * Time.deltaTime;
    }

    private void Move_performed(InputAction.CallbackContext ctx) {
        moveVector = ctx.ReadValue<Vector2>().normalized;
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
