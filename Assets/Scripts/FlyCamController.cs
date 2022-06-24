using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamController : MonoBehaviour {

    public float move_speed;
    public float mouse_sensitivity;
    public DefaultInputActions FlyCamControls;

    private InputAction move;
    private InputAction look;

    private Vector2 moveDir;
    private Vector2 lookDir;

    private void Awake() {
        FlyCamControls = new DefaultInputActions();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() {
        move = FlyCamControls.Player.Move;
        move.Enable();

        look = FlyCamControls.Player.Look;
        look.Enable();
    }

    void Update() {
        moveDir = move.ReadValue<Vector2>();
        lookDir += look.ReadValue<Vector2>() * mouse_sensitivity;

        lookDir.y = Mathf.Clamp(lookDir.y, -90.0f, 90.0f);
    }

    private void OnDisable() {
        move.Disable();
        look.Disable();
    }

    private void FixedUpdate() {
        transform.position += (transform.forward * moveDir.y + transform.right * moveDir.x).normalized * move_speed;

        transform.rotation = Quaternion.Euler(-lookDir.y, lookDir.x, 0.0f);
    }
}
