using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamController : MonoBehaviour {

    public HAL hal;
    
    private bool follow_robot;

    public float move_speed;
    public float mouse_sensitivity;
    public DefaultInputActions FlyCamControls;

    private GameObject robot;

    private InputAction move;
    private InputAction look;
    private InputAction elev;

    private InputAction view;

    private Vector2 moveDir;
    private Vector2 lookDir;

    private float elevDir;

    private void Awake() {
        FlyCamControls = new DefaultInputActions();

        Cursor.lockState = CursorLockMode.Locked;

        robot = null;
    }

    

    private void OnEnable() {
        move = FlyCamControls.Player.Move;
        move.Enable();

        look = FlyCamControls.Player.Look;
        look.Enable();

        elev = FlyCamControls.Player.Elevate;
        elev.Enable();

        view = FlyCamControls.Player.ViewMode;
        view.Enable();
        view.performed += ToggleView;
    }

    private void ToggleView(InputAction.CallbackContext context) {
        follow_robot = !follow_robot;
    }

    void Update() {
        if (!follow_robot) {
            moveDir = move.ReadValue<Vector2>();
            lookDir += look.ReadValue<Vector2>() * mouse_sensitivity;
            elevDir = elev.ReadValue<float>();

            lookDir.y = Mathf.Clamp(lookDir.y, -90.0f, 90.0f);
        } else {
            if (!robot) {
                robot = hal.Robot;
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(robot.transform.position.x, 3.0f, robot.transform.position.z), 10.0f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(90.0f, 0.0f, -robot.transform.rotation.eulerAngles.y);
        }
    }

    private void OnDisable() {
        move.Disable();
        look.Disable();
        elev.Disable();
        view.Disable();
    }

    private void FixedUpdate() {
        transform.position += (transform.forward * moveDir.y + transform.right * moveDir.x + transform.up * elevDir).normalized * move_speed;

        transform.rotation = Quaternion.Euler(-lookDir.y, lookDir.x, 0.0f);
    }
}
